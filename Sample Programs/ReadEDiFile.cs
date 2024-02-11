using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
        string filePath = "your_file_path.edi"; // Update with your file path

        List<MemberData> members = new List<MemberData>();

        try
        {
            using (StreamReader sr = new StreamReader(filePath))
            {
                string line;
                MemberData currentMember = null;

                while ((line = sr.ReadLine()) != null)
                {
                    string[] segments = line.Split('*');

                    switch (segments[0])
                    {
                        case "N1":
                            if (segments[1] == "IV")
                            {
                                currentMember = new MemberData();
                                currentMember.MemberName = segments[2];
                            }
                            break;

                        case "N3":
                            if (currentMember != null)
                            {
                                currentMember.Address = segments[1];
                            }
                            break;

                        case "N4":
                            if (currentMember != null)
                            {
                                currentMember.City = segments[1];
                                currentMember.State = segments[2];
                            }
                            break;

                        case "DMG":
                            if (currentMember != null)
                            {
                                currentMember.DateOfBirth = segments[3];
                            }
                            break;

                        case "INS":
                            if (currentMember != null)
                            {
                                currentMember.InsurancePlan = segments[4];
                            }
                            break;

                        case "REF":
                            if (currentMember != null && segments[1] == "0F")
                            {
                                currentMember.ReferenceNumber = segments[2];
                            }
                            break;

                        case "DTP":
                            if (currentMember != null && segments[1] == "356")
                            {
                                currentMember.EnrollmentDate = DateTime.ParseExact(segments[3], "yyyyMMdd", null);
                                members.Add(currentMember);
                                currentMember = null; // Reset current member
                            }
                            break;

                        default:
                            break;
                    }
                }
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
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred: " + ex.Message);
        }
    }
}
