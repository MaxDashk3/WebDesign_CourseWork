﻿using Microsoft.AspNetCore.Mvc;
using Movies.Models;
using System.ComponentModel.DataAnnotations;

namespace Movies.ViewModels
{
    public class MovieViewModel
    {
        public MovieViewModel() { }

        public int Id { get; set; }
        public string? Genre { get; set; }

        [Remote(action: "MoviesValidation", controller: "Data", ErrorMessage = "Title already exists!!!", AdditionalFields = "Title,Id")]
        [Required(ErrorMessage = "This field is required")]
        [Display(Prompt = "Enter title")]
        public string Title { get; set; }

        [Display(Prompt = "Enter country of production", Name = "Country of production")]
        [Required(ErrorMessage = "This field is required")]
        public string Country { get; set; }
        [Display(Name = "Genre")]
        public int GenreId { get; set; }

        [Display(Prompt = "Enter year of production")]
        [Required(ErrorMessage = "This field is required")]
        [Range(1900, 2024, ErrorMessage = "The value has to be between 1900 and 2024")]
        public int Year { get; set; }

        [Required]
        [Display(Prompt = "Enter description")]
        public string Description { get; set; }
        [Required]
        [Display(Prompt = "Upload poster image here")]
        public byte[] Poster { get; set; }
        [Required]
        [Display(Prompt = "Example: xKJmEC5ieOk")]
        public string VideoPath { get; set; }

        public List<SessionViewModel>? Sessions { get; set; }

        public MovieViewModel(Movie movie)
        {
            Id = movie.Id;
            Title = movie.Title;
            Country = movie.Country;
            Year = movie.Year;
            GenreId = movie.GenreId;
            Description = movie.Description;
            Poster = movie.Poster;
            VideoPath = movie.VideoPath;

            if (movie.Sessions != null)
            {
                Sessions = movie.Sessions.Select(s => new SessionViewModel(s)).ToList();
            }

            if (movie.Genre != null)
            {
                Genre = movie.Genre.Name;
            }
        }
    }
}
