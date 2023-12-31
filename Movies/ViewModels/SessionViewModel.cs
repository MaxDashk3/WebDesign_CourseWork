﻿using Movies.Models;
using System.ComponentModel.DataAnnotations;

namespace Movies.ViewModels
{
    public class SessionViewModel
    {
        public SessionViewModel() { }

        public int Id { get; set; }
        public int HallId { get; set; }
        public string? Hall { get; set; }

        public string? Movie { get; set; }
        public int MovieId { get; set; }
        [Required(ErrorMessage = "This field is required")]
        [Display(Prompt = "Select session time")]
        public DateTime TimeDate { get; set; }
        [Display(Prompt = "Enter price in UAH")]
        [Required(ErrorMessage = "This field is required")]
        [Range(40, 500, ErrorMessage = "Price has to be between 40 and 500")]
        public int Price { get; set; }

        public List<string>? HallTechStrList { get; set; }
        public string? HallTechString { get; set; }

        public List<Ticket>? Tickets { get; set; }

        public SessionViewModel(Session session)
        {
            Id = session.Id;
            HallId = session.HallId;
            MovieId = session.MovieId;
            TimeDate = session.TimeDate;
            Price = session.Price;

            if (session.Tickets != null) 
            {
                Tickets = session.Tickets.ToList();
            }
            if (session.Hall != null)
            {
                Hall = session.Hall.Name;
                var Hmodel = new HallViewModel(session.Hall);
                HallTechStrList = Hmodel.TechnologiesStrList;
                HallTechString = Hmodel.TechnologiesString;
            }
            if (session.Movie != null)
            {
                Movie = session.Movie.Title;
            }
        }
    }
}
