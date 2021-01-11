using System;
using System.Collections.Generic;
using System.Text;

namespace MovieScrapper
{
	public class Movie
	{
		public long Id { get; set; }
		public string Title { get; set; }
		public DateTime? ReleaseDate { get; set; }
		public string ReleaseDateString { get; set; }
		public string MovieUrl { get; set; }
		public string TrailerUrl { get; set; }
	}
}
