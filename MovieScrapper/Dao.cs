using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace MovieScrapper
{
	public class Dao
	{
		public static bool TableExist(string tableName)
		{
			bool exists;
			try
			{
				var connectionString = "Server=tcp:dw-team.database.windows.net,1433;Database=dw-team;Uid=dw-team;Pwd=datawarehouse-test1;Encrypt=yes;TrustServerCertificate=no;Connection Timeout=30;";
				var queryString = "select case when exists((select * from information_schema.tables where table_name = '" + tableName + "')) then 1 else 0 end";
				using (SqlConnection connection = new SqlConnection(connectionString))
				{
					SqlCommand command = new SqlCommand(queryString, connection);
					command.Connection.Open();
					exists = (int)command.ExecuteScalar() == 1;
					command.Connection.Close();
					return exists;
				}
			}
			catch
			{
				exists = false;
				return exists;
			}
		}
		private static void CreateTable()
		{
			var connectionString = "Server=tcp:dw-team.database.windows.net,1433;Database=dw-team;Uid=dw-team;Pwd=datawarehouse-test1;Encrypt=yes;TrustServerCertificate=no;Connection Timeout=30;";
			var queryString = "CREATE TABLE movies_external_content (movie_id bigint, poster_path varchar(MAX), trailer_patch varchar(MAX));";
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				SqlCommand command = new SqlCommand(queryString, connection);
				command.Connection.Open();
				command.ExecuteScalar();
				command.Connection.Close();
			}
		}
		private static void DeleteTable()
		{
			var connectionString = "Server=tcp:dw-team.database.windows.net,1433;Database=dw-team;Uid=dw-team;Pwd=datawarehouse-test1;Encrypt=yes;TrustServerCertificate=no;Connection Timeout=30;";
			var queryString = "DROP TABLE movies_external_content;";
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				SqlCommand command = new SqlCommand(queryString, connection);
				command.Connection.Open();
				command.ExecuteScalar();
				command.Connection.Close();
			}
		}
		private static void InsertRecord(Movie movie)
		{

			var connectionString = "Server=tcp:dw-team.database.windows.net,1433;Database=dw-team;Uid=dw-team;Pwd=datawarehouse-test1;Encrypt=yes;TrustServerCertificate=no;Connection Timeout=30;";
			var queryString = "INSERT INTO movies_external_content (movie_id, trailer_patch)VALUES(" + movie.Id + ", '" + movie.TrailerUrl + "');";
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				SqlCommand command = new SqlCommand(queryString, connection);
				command.Connection.Open();
				command.ExecuteScalar();
				command.Connection.Close();
			}
		}
		public static void InitializeData(List<Movie> movieList)
		{
			if (TableExist("movies_external_content"))
			{
				DeleteTable();
				CreateTable();
				foreach (var item in movieList)
				{
					InsertRecord(item);
				}
			}
			else
			{
				CreateTable();
				foreach (var item in movieList)
				{
					InsertRecord(item);
				}
			}
		}
	}
}
