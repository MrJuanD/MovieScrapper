using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Net.Http;
using System.Threading;

namespace MovieScrapper
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Starting...");
			var movieList = new List<Movie>();
			// Get Data requied for scrap
			// ----------
			var connectionString = "Server=tcp:dw-team.database.windows.net,1433;Database=dw-team;Uid=dw-team;Pwd=datawarehouse-test1;Encrypt=yes;TrustServerCertificate=no;Connection Timeout=30;";
			var queryString = "SELECT TOP 10 * FROM movies ORDER BY vote_count DESC";
			using (SqlConnection connection = new SqlConnection(
						  connectionString))
			{
				SqlCommand command = new SqlCommand(queryString, connection);
				command.Connection.Open();
				SqlDataReader reader = command.ExecuteReader();
				while(reader.Read())
				{
					var movie = new Movie() { Id = (long)reader[0], Title = reader[1].ToString(), ReleaseDate = DateTime.Parse(reader[11].ToString()) };
					movie.ReleaseDateString = movie.ReleaseDate.Value.ToString("MMMM", CultureInfo.CreateSpecificCulture("en")) + " " + movie.ReleaseDate.Value.Day + ", " + movie.ReleaseDate.Value.Year;
					movieList.Add(movie);
				}
				command.Connection.Close();
			}
			// ----------

			// Start Scraping
			// ----------
			foreach (var item in movieList)
			{
				item.MovieUrl = Scrapper.GetMovieUrlFromHtml(item);
				Thread.Sleep(200);
				item.TrailerUrl = Scrapper.GetMovieTrailerUrl(item);
				Thread.Sleep(200);
			}
			Dao.InitializeData(movieList);
			Console.WriteLine("Finished.");
			// ----------
		}
	}
}