﻿using Movies.ViewModels;

namespace Movies.Models
{
    public class Movie
    {
        public Movie() { }
        public int Id { get; set; }
        public string Title { get; set; }
        public string Country { get; set; }
        public int GenreId { get; set; }
        public int Year { get; set; } 
        public string Description { get; set; }
        public byte[] Poster { get; set; }
        public string VideoPath { get; set; }

        public Genre? Genre { get; set;}
        public IEnumerable<Session>? Sessions { get; set;}

        public Movie(MovieViewModel movieviewmodel)
        {
            Id = movieviewmodel.Id;
            Title = movieviewmodel.Title;
            Country = movieviewmodel.Country;
            GenreId = movieviewmodel.GenreId;
            Year = movieviewmodel.Year;
            Description = movieviewmodel.Description;
            Poster = movieviewmodel.Poster;
            VideoPath = movieviewmodel.VideoPath;
        }
    }
}
