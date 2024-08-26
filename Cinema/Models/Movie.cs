using System;
using System.Collections.Generic;

namespace Cinema.Models;

public partial class Movie
{
    public int MovieId { get; set; }

    public string Title { get; set; } = null!;

    public int Duration { get; set; }

    public string Poster { get; set; } = null!;

    public string Language { get; set; } = null!;

    public string Country { get; set; } = null!;

    public DateTime ReleaseDate { get; set; }

    public string Describe { get; set; } = null!;

    public string TrailerUrl { get; set; } = null!;

    public int? Status { get; set; }

    public string License { get; set; } = null!;

    public DateTime StartDate { get; set; }

    public DateTime FinishDate { get; set; }

    public string Background { get; set; } = null!;

    public virtual ICollection<ActorAndDirector> ActorAndDirectors { get; set; } = new List<ActorAndDirector>();

    public virtual ICollection<MovieCategoryName> MovieCategoryNames { get; set; } = new List<MovieCategoryName>();

    public virtual ICollection<MovieRating> MovieRatings { get; set; } = new List<MovieRating>();

    public virtual ICollection<MovieShow> MovieShows { get; set; } = new List<MovieShow>();
}
