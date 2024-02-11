using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

class MemberData
{
    public string MemberName { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string DateOfBirth { get; set; }
    public string InsurancePlan { get; set; }
    public string ReferenceNumber { get; set; }
    public DateTime EnrollmentDate { get; set; }
}

class Program
{
    static void Main(string[] args)
    {
        string connectionString = "your_connection_string"; // Update with your connection string
        List<MemberData> members = new List<MemberData>();

        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string sqlQuery = "SELECT MemberName, Address, City, State, DateOfBirth, InsurancePlan, ReferenceNumber, EnrollmentDate FROM YourTableName"; // Update with your table name
                SqlCommand command = new SqlCommand(sqlQuery, connection);

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    MemberData member = new MemberData
                    {
                        MemberName = reader["MemberName"].ToString(),
                        Address = reader["Address"].ToString(),
                        City = reader["City"].ToString(),
                        State = reader["State"].ToString(),
                        DateOfBirth = reader["DateOfBirth"].ToString(),
                        InsurancePlan = reader["InsurancePlan"].ToString(),
                        ReferenceNumber = reader["ReferenceNumber"].ToString(),
                        EnrollmentDate = Convert.ToDateTime(reader["EnrollmentDate"])
                    };

                    members.Add(member);
                }

                // Display member data
                foreach (var member in members)
                {
                    Console.WriteLine($"Member Name: {member.MemberName}");
                    Console.WriteLine($"Address: {member.Address}");
                    Console.WriteLine($"City: {member.City}, State: {member.State}");
                    Console.WriteLine($"Date of Birth: {member.DateOfBirth}");
                    Console.WriteLine($"Insurance Plan: {member.InsurancePlan}");
                    Console.WriteLine($"Reference Number: {member.ReferenceNumber}");
                    Console.WriteLine($"Enrollment Date: {member.EnrollmentDate}");
                    Console.WriteLine();
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred: " + ex.Message);
        }
    }
}
