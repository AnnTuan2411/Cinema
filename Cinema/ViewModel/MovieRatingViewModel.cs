using Cinema.Models;

namespace Cinema.ViewModel
{
    public class MovieRatingViewModel
    {
        public int MovieRatingId { get; set; }

        public int? MovieId { get; set; }

        public int? CustomerId { get; set; }

        public string FullName { get; set; }

        public string? Comment { get; set; }

        public int Star { get; set; }

        public DateTime? MovieRatingDate { get; set; }

        public int? ParentId { get; set; }

        public virtual Customer? Customer { get; set; }

        public virtual Movie? Movie { get; set; }
    }
}
