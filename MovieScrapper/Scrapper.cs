using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MovieScrapper
{
	public static class Scrapper
	{
		private static async Task<string> ScrapMovieUrl(Movie movie)
		{
			var movieTitle = movie.Title.Replace(' ', '+');
			var url = "https://www.themoviedb.org/search?query=" + movieTitle;

			var httpClient = new HttpClient();
			var html = await httpClient.GetStringAsync(url);

			var htmlDocument = new HtmlDocument();
			htmlDocument.LoadHtml(html);

			var movieHtml = htmlDocument.DocumentNode.Descendants("div")
				.Where(node => node.GetAttributeValue("class", "")
				.Equals("results flex")).ToList();

			movieHtml = movieHtml[0].Descendants("div")
				.Where(node => node.GetAttributeValue("class", "")
				.Equals("card v4 tight")).ToList();

			foreach (var item in movieHtml)
			{
				var itemBuff = item.Descendants("span")
				.Where(node => node.GetAttributeValue("class", "")
				.Equals("release_date")).ToList();

				if (itemBuff[0].InnerText.Replace(" ", string.Empty) == movie.ReleaseDateString.Replace(" ", string.Empty))
				{
					var movieUrl = String.Concat(item.Descendants("a")
						.First().GetAttributeValue("href", null));
					return movieUrl;
				}
			}
			return null;
		}
		public static string GetMovieUrlFromHtml(Movie movie)
		{
			var getMovieUrlTask = ScrapMovieUrl(movie);
			getMovieUrlTask.Wait();
			return getMovieUrlTask.Result;
		}
		private static async Task<string> ScrapMovieTrailerUrl(Movie movie)
		{
			var url = "https://www.themoviedb.org" + movie.MovieUrl;

			var httpClient = new HttpClient();
			var html = await httpClient.GetStringAsync(url);

			var htmlDocument = new HtmlDocument();
			htmlDocument.LoadHtml(html);

			var movieHtml = htmlDocument.DocumentNode.Descendants("div")
				.Where(node => node.GetAttributeValue("class", "")
				.Equals("video card no_border")).ToList();

			if (movieHtml.Count > 0)
			{
				var trailerUrl = String.Concat(movieHtml[0].Descendants("a")
			.First().GetAttributeValue("href", null));
				return trailerUrl;
			}
			return null;
		}
		public static string GetMovieTrailerUrl(Movie movie)
		{
			var getTrailerUrlTask = ScrapMovieTrailerUrl(movie);
			getTrailerUrlTask.Wait();
			return getTrailerUrlTask.Result;
		}
	}
}
