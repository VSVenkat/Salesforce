using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System.Data.SqlClient;

class Program
{
    static void Main(string[] args)
    {
        // Specify the path to the JSON file containing SQL queries
        string jsonFilePath = "queries.json";

        // Read the JSON file and parse the queries
        List<Query> queries = ReadQueriesFromJson(jsonFilePath);

        // Prompt the user to enter the name of the query to execute
        Console.Write("Enter the name of the query to execute: ");
        string queryName = Console.ReadLine();

        // Find the query with the provided name
        Query query = queries.Find(q => q.Name.Equals(queryName, StringComparison.OrdinalIgnoreCase));

        if (query != null)
        {
            // Execute the SQL query
            ExecuteSqlQuery(query.Sql);
        }
        else
        {
            Console.WriteLine($"Query with name '{queryName}' not found.");
        }

        Console.ReadLine(); // Keep console open
    }

    static List<Query> ReadQueriesFromJson(string jsonFilePath)
    {
        // Read the JSON file and deserialize it into a list of Query objects
        string json = File.ReadAllText(jsonFilePath);
        return JsonConvert.DeserializeObject<List<Query>>(json);
    }

    static void ExecuteSqlQuery(string sqlQuery)
    {
        // Connection string for the SQL Server
        string connectionString = "Data Source=myServerAddress;Initial Catalog=myDatabase;User Id=myUsername;Password=myPassword;";

        // Execute the SQL query
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            SqlCommand command = new SqlCommand(sqlQuery, connection);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();

            // Process the results if needed
            while (reader.Read())
            {
                // Process each row returned by the query
            }

            reader.Close();
        }
    }

    // Define a class to represent a SQL query
    class Query
    {
        public string Name { get; set; }
        public string Sql { get; set; }
    }
}
