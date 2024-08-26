using Cinema.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MailKit.Net.Smtp;
using MimeKit;
using System.Reflection.Metadata.Ecma335;
using QRCoder;
using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using Stripe.Checkout;
using Stripe;
using Account = Cinema.Models.Account;

namespace Cinema.Controllers
{
    public class OrderTicketController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Order(List<int> SeatIds, int MovieShowId)
        {
            CinemaContext context = new CinemaContext();
            if (HttpContext.Session.GetString("UserName") != null)
            {
                ViewBag.Session = HttpContext.Session.GetString("UserName").ToString();
                ViewBag.SessionID = HttpContext.Session.GetInt32("IDAcc");
                ViewBag.SessionRoleID = HttpContext.Session.GetInt32("RoleID");
                try
                {
                    ViewBag.SeccionCusId = HttpContext.Session.GetInt32("CusId");
                }

                catch (Exception ex)
                {
                    ViewBag.SeccionEmpId = HttpContext.Session.GetInt32("EmpId");
                }
            }

            Booking booking = new Booking(HttpContext.Session.GetInt32("CusId"), 0, DateTime.Today);
            context.Bookings.Add(booking);
            context.SaveChanges();
            int bookingId = context.Bookings
            .OrderByDescending(e => e.BookingId)
            .Select(e => e.BookingId)
            .FirstOrDefault();

            List<Seat> seats = new List<Seat>();
            foreach (var item in SeatIds)
            {
                Seat seat = context.Seats.Where(row => row.SeatId == item).FirstOrDefault();
                seats.Add(seat);
                Ticket ticket = context.Tickets.Where(row => row.MovieShowId == MovieShowId && row.SeatId == item).FirstOrDefault();
                ticket.Status = 1;
                context.Tickets.Update(ticket);
                context.SaveChanges();
                if (ticket != null)
                {
                    context.OrderTickets.Add(new OrderTicket((decimal)ticket.TotalPrice, bookingId, ticket.TicketId));
                    context.SaveChanges();
                }
            }
            ViewBag.Seat = seats;
            MovieShow movieShow = context.MovieShows.Where(row => row.MovieShowId == MovieShowId).FirstOrDefault();
            ViewBag.MovieShow = movieShow;
            Movie movie = context.Movies.Where(row => row.MovieId == movieShow.MovieId).FirstOrDefault();
            ViewBag.Movie = movie;
            List<OrderTicket> orders = context.OrderTickets.Where(row => row.BookingId == bookingId).ToList();
            decimal TotalPrice = orders.Sum(row => row.Price);
            ViewBag.TotalPrice = TotalPrice;
            ViewBag.BookingId = bookingId;
            return View(orders);
        }

        static byte[] GenerateQrCode(string data)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(data, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);

            using (Bitmap qrCodeImage = qrCode.GetGraphic(50)) // Change the size as needed
            using (MemoryStream stream = new MemoryStream())
            {
                qrCodeImage.Save(stream, ImageFormat.Png);
                return stream.ToArray();
            }
        }

        public ActionResult Payment(int bookingId)
        {
            CinemaContext context = new CinemaContext();
            var domain = "https://localhost:7143/";
            var options = new SessionCreateOptions
            {
                SuccessUrl = domain + $"OrderTicket/Successfull?bookingId={bookingId}",
                CancelUrl = domain + "Movie/Index",
                LineItems = new List<SessionLineItemOptions>(),              
                Mode = "payment",
            };

            List<OrderTicket> orderTickets = context.OrderTickets.Where(row => row.BookingId == bookingId).ToList();
            List<Ticket> tickets = new List<Ticket>();
            foreach (var orderTicket in orderTickets)
            {
                Ticket ticket = context.Tickets.Where(row => row.TicketId == orderTicket.TicketId).FirstOrDefault();
                var sessionLineItem = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(ticket.TotalPrice),
                        Currency = "vnd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = Convert.ToString(ticket.MovieShowId)
                        }
                    },
                    Quantity = 1,
                };
                options.LineItems.Add(sessionLineItem);

            }


            var service = new SessionService();
            Session session = service.Create(options);
            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);
        }

        public ActionResult Successfull(int bookingId)
        {

            if (HttpContext.Session.GetString("UserName") != null)
            {
                ViewBag.Session = HttpContext.Session.GetString("UserName").ToString();
                ViewBag.SessionID = HttpContext.Session.GetInt32("IDAcc");
                ViewBag.SessionRoleID = HttpContext.Session.GetInt32("RoleID");
                try
                {
                    ViewBag.SeccionCusId = HttpContext.Session.GetInt32("CusId");
                }

                catch (Exception ex)
                {
                    ViewBag.SeccionEmpId = HttpContext.Session.GetInt32("EmpId");
                }
            }

            if (HttpContext.Session.GetString("UserName") != null)
            {
                ViewBag.Session = HttpContext.Session.GetString("UserName").ToString();
                ViewBag.SessionID = HttpContext.Session.GetInt32("IDAcc");
                ViewBag.SessionRoleID = HttpContext.Session.GetInt32("RoleID");
                ViewBag.SessionCusId = HttpContext.Session.GetInt32("CusId");
            }
            CinemaContext context = new CinemaContext();
            Account account = context.Accounts.Where(row => row.AccId == HttpContext.Session.GetInt32("IDAcc")).FirstOrDefault();
            List<OrderTicket> orderTickets = context.OrderTickets.Where(row => row.BookingId == bookingId).ToList();
            List<Ticket> tickets = new List<Ticket>();
            decimal price = 0;
            foreach (OrderTicket orderTicket in orderTickets)
            {
                Ticket ticket = context.Tickets.Where(row => row.TicketId == orderTicket.TicketId).FirstOrDefault();
                price = price + orderTicket.Price;
                tickets.Add(ticket);
                ticket.Status = 1;
                context.Tickets.Update(ticket);
                context.SaveChanges();
            }
            Booking booking = context.Bookings.Where(row => row.BookingId == bookingId).FirstOrDefault();
            booking.Status = 1;
            booking.TotalPrice = price;
            context.Bookings.Update(booking);
            context.SaveChanges();

            //
            string id = Convert.ToString(bookingId);
            byte[] qrCodeBytes = GenerateQrCode(id);

            //


            using (var client = new SmtpClient())
            {
                client.Connect("smtp.gmail.com");
                client.Authenticate("anhtuan35670@gmail.com", "cddxxbxbmihjlmqm");
                var bodyBuider = new BodyBuilder();
                bodyBuider.TextBody = $"ĐƠN HÀNG";
                bodyBuider.Attachments.Add("BookingQRCode.png", qrCodeBytes, ContentType.Parse("image/png"));
                bodyBuider.TextBody = $"Vui lòng không tiết lộ mã này vì lý do bảo mật";
                var message = new MimeMessage
                {
                    Body = bodyBuider.ToMessageBody()
                };
                message.From.Add(new MailboxAddress("Admin", "anhtuan35670@gmail.com"));
                message.To.Add(new MailboxAddress("KhachHang", account.Email));
                message.Subject = "[MANDAN Cinema]";
                client.Send(message);
                client.Disconnect(true);
            }
            return View();
        }

        public ActionResult GetTiket(string error = "")
        {
            if (HttpContext.Session.GetString("UserName") != null)
            {
                ViewBag.Session = HttpContext.Session.GetString("UserName").ToString();
                ViewBag.SessionID = HttpContext.Session.GetInt32("IDAcc");
                ViewBag.SessionRoleID = HttpContext.Session.GetInt32("RoleID");
                try
                {
                    ViewBag.SeccionCusId = HttpContext.Session.GetInt32("CusId");
                }

                catch (Exception ex)
                {
                    ViewBag.SeccionEmpId = HttpContext.Session.GetInt32("EmpId");
                }
            }
            ViewBag.Error = error;
            return View();
        }

        [HttpPost]
        public ActionResult GetTiket(int bookingId)
        {
            if (HttpContext.Session.GetString("UserName") != null)
            {
                ViewBag.Session = HttpContext.Session.GetString("UserName").ToString();
                ViewBag.SessionID = HttpContext.Session.GetInt32("IDAcc");
                ViewBag.SessionRoleID = HttpContext.Session.GetInt32("RoleID");
                try
                {
                    ViewBag.SeccionCusId = HttpContext.Session.GetInt32("CusId");
                }

                catch (Exception ex)
                {
                    ViewBag.SeccionEmpId = HttpContext.Session.GetInt32("EmpId");
                }
            }
            CinemaContext context = new CinemaContext();
            Booking booking = context.Bookings.Where(row => row.BookingId == bookingId).FirstOrDefault();
            if (booking != null)
            {
                List<OrderTicket> orderTickets = context.OrderTickets.Where(row => row.BookingId == bookingId).ToList();
                List<Ticket> tickets = new List<Ticket>();
                List<Seat> seats = new List<Seat>();
                foreach(var item in orderTickets)
                {
                    Ticket ticket = context.Tickets.Where(row => row.TicketId == item.TicketId).FirstOrDefault();
                    tickets.Add(ticket);
                    
                    Seat seat = context.Seats.Where(row => row.SeatId == ticket.SeatId).FirstOrDefault();
                    seats.Add(seat);
                    MovieShow movieShow = context.MovieShows.Where(row => row.MovieShowId == ticket.MovieShowId).FirstOrDefault();
                    ViewBag.MovieShow = movieShow;
                    ViewBag.Movie = context.Movies.Where(row => row.MovieId == movieShow.MovieId).FirstOrDefault();
                    ViewBag.Room = context.Rooms.Where(row => row.RoomId == seat.RoomId).FirstOrDefault();
                }

                ViewBag.Seats = seats;
                ViewBag.Tickets = tickets;
                return View(booking);
            }
            else
            {
                ViewBag.Error = "Không tìm thấy vé !";
                return View();
            }
            
        }
    }
}
