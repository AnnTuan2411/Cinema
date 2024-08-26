

namespace Cinema.Helpers
{
    public static class JSONListHelper
    {
        public static string GetEventListForOneEmpJSONString(IEnumerable<Models.Calendar> events, int targetEmpId)
        {
            var eventList = events.Where(calendar => calendar.EmloyeeId == targetEmpId)
            .Select(calendar => new Event
            {
                id = calendar.CalendarId,
                title = calendar.Title,
                start = GetStarDate(calendar),
                end = GetEndDate(calendar),
                empId = calendar.Emloyee.EmployeeId,
                description = calendar.Description,
                empName = calendar.Emloyee.Acc.FullName
            }).ToList();

            return System.Text.Json.JsonSerializer.Serialize(eventList);
        }

        public static string GetEventListJSONString(IEnumerable<Models.Calendar> events)
        {
            var eventList = new List<Event>();
            foreach (var calendar in events)
            {
                var myEvent = new Event()
                {
                    id = calendar.CalendarId,
                    title = calendar.Title,
                    start = GetStarDate(calendar),
                    end = GetEndDate(calendar),
                    empId = calendar.Emloyee.EmployeeId,
                    description = calendar.Description,
                    empName = calendar.Emloyee.Acc.FullName
                };            
                eventList.Add(myEvent);
            }
            return System.Text.Json.JsonSerializer.Serialize(eventList);
        }

        public static string GetResourceListJSONString(IEnumerable<Models.Employee> employees)
        {
            var resourceList = new List<Resource>();
            foreach (var employee in employees)
            {
                var resource = new Resource()
                {
                    id = employee.EmployeeId,
                    title = employee.Position
                };
                resourceList.Add(resource);
            }
            return System.Text.Json.JsonSerializer.Serialize(resourceList);
        }




        //thoi gian di lam: 9h-14h | 14h - 18h | 18h - 22h
        public static DateTime GetStarDate(Models.Calendar calendar)
        {
            if (calendar.Shift == 1)
            {
                return new DateTime(calendar.Date.Year, calendar.Date.Month, calendar.Date.Day, 9, 0, 0);
            }
            else if (calendar.Shift == 2)
            {
                return new DateTime(calendar.Date.Year, calendar.Date.Month, calendar.Date.Day, 14, 0, 0);
            }
            else
            {
                return new DateTime(calendar.Date.Year, calendar.Date.Month, calendar.Date.Day, 18, 0, 0);
            }
        }

        public static DateTime GetEndDate(Models.Calendar calendar)
        {
            if (calendar.Shift == 1)
            {
                return new DateTime(calendar.Date.Year, calendar.Date.Month, calendar.Date.Day, 14, 0, 0);
            }
            else if (calendar.Shift == 2)
            {
                return new DateTime(calendar.Date.Year, calendar.Date.Month, calendar.Date.Day, 18, 0, 0);
            }
            else
            {
                return new DateTime(calendar.Date.Year, calendar.Date.Month, calendar.Date.Day, 22, 0, 0);
            }
        }
    }

    public class Event
    {
        public int id { get; set; }
        public string title { get; set; }
        public DateTime start { get; set; }
        public DateTime end { get; set; }
        public int empId { get; set; }
        public string description { get; set; }
        public string empName { get; set; }
    }

    public class Resource
    {
        public int id { get; set; }
        public string title { get; set; }
    }

}
