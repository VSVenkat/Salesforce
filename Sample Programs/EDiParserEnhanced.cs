using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Indices.Edi;
using Indices.Edi.Utilities;

class Program
{
    static void Main(string[] args)
    {
        // Specify the path to the EDI file
        string ediFilePath = "your_edifile.edi";

        // Create collections to store member, organization, INS, DTP, and DBG details
        List<MemberDetail> members = new List<MemberDetail>();
        List<OrganizationDetail> organizations = new List<OrganizationDetail>();
        List<InsDetail> insList = new List<InsDetail>();
        List<DtpDetail> dtpList = new List<DtpDetail>();
        List<DbgDetail> dbgList = new List<DbgDetail>();

        // Read all lines from the EDI file
        var ediLines = File.ReadAllLines(ediFilePath);

        // Process each line of the EDI file
        foreach (var ediLine in ediLines)
        {
            // Split the line into segments
            var segments = ediLine.Split('*');

            // Get segment identifier
            var segmentIdentifier = segments.FirstOrDefault();

            // Process segment based on its identifier
            switch (segmentIdentifier)
            {
                case "NM1":
                    // Process NM1 segment
                    var memberId = segments.ElementAtOrDefault(9);
                    var firstName = segments.ElementAtOrDefault(3);
                    var lastName = segments.ElementAtOrDefault(4);
                    
                    // Determine if it's an individual or organization segment
                    var entityIdentifierCode = segments.ElementAtOrDefault(1);
                    if (entityIdentifierCode == "IL")
                    {
                        // Create member detail object and add to collection
                        members.Add(new MemberDetail
                        {
                            MemberId = memberId,
                            FirstName = firstName,
                            LastName = lastName
                        });
                    }
                    else if (entityIdentifierCode == "Names")
                    {
                        // Create organization detail object and add to collection
                        organizations.Add(new OrganizationDetail
                        {
                            MemberId = memberId,
                            FirstName = firstName,
                            LastName = lastName,
                            OrganizationName = segments.ElementAtOrDefault(3),
                            OrganizationId = segments.ElementAtOrDefault(9)
                        });
                    }
                    break;
                case "N3":
                    // Process N3 segment (Address Line 1)
                    var addressLine1 = segments.ElementAtOrDefault(1);
                    // Update the last member or organization detail with address line 1
                    if (members.Any())
                    {
                        members.Last().AddressLine1 = addressLine1;
                    }
                    else if (organizations.Any())
                    {
                        organizations.Last().AddressLine1 = addressLine1;
                    }
                    break;
                case "N4":
                    // Process N4 segment (City, State, ZIP)
                    var city = segments.ElementAtOrDefault(1);
                    var state = segments.ElementAtOrDefault(2);
                    var zip = segments.ElementAtOrDefault(3);
                    // Update the last member or organization detail with city, state, and ZIP
                    if (members.Any())
                    {
                        members.Last().City = city;
                        members.Last().State = state;
                        members.Last().ZIP = zip;
                    }
                    else if (organizations.Any())
                    {
                        organizations.Last().City = city;
                        organizations.Last().State = state;
                        organizations.Last().ZIP = zip;
                    }
                    break;
                case "INS":
                    // Process INS segment
                    insList.Add(new InsDetail
                    {
                        Indicator = segments.ElementAtOrDefault(1),
                        Code = segments.ElementAtOrDefault(2),
                        Description = segments.ElementAtOrDefault(3)
                    });
                    break;
                case "DTP":
                    // Process DTP segment
                    dtpList.Add(new DtpDetail
                    {
                        DateQualifier = segments.ElementAtOrDefault(1),
                        Date = segments.ElementAtOrDefault(2)
                    });
                    break;
                case "DBG":
                    // Process DBG segment
                    dbgList.Add(new DbgDetail
                    {
                        Code = segments.ElementAtOrDefault(0),
                        Description = segments.ElementAtOrDefault(1)
                    });
                    break;
            }
        }

        // Print member details
        Console.WriteLine("Members:");
        foreach (var member in members)
        {
            Console.WriteLine($"Member ID: {member.MemberId}, First Name: {member.FirstName}, Last Name: {member.LastName}, Address Line 1: {member.AddressLine1}, City: {member.City}, State: {member.State}, ZIP: {member.ZIP}");
        }

        // Print organization details
        Console.WriteLine("\nOrganizations:");
        foreach (var organization in organizations)
        {
            Console.WriteLine($"Member ID: {organization.MemberId}, First Name: {organization.FirstName}, Last Name: {organization.LastName}, Organization Name: {organization.OrganizationName}, Organization ID: {organization.OrganizationId}, Address Line 1: {organization.AddressLine1}, City: {organization.City}, State: {organization.State}, ZIP: {organization.ZIP}");
        }

        // Print INS details
        Console.WriteLine("\nINS Details:");
        foreach (var insDetail in insList)
        {
            Console.WriteLine($"Indicator: {insDetail.Indicator}, Code: {insDetail.Code}, Description: {insDetail.Description}");
        }

        // Print DTP details
        Console.WriteLine("\nDTP Details:");
        foreach (var dtpDetail in dtpList)
        {
            Console.WriteLine($"Date Qualifier: {dtpDetail.DateQualifier}, Date: {dtpDetail.Date}");
        }

        // Print DBG details
        Console.WriteLine("\nDBG Details:");
        foreach (var dbgDetail in dbgList)
        {
            Console.WriteLine($"Code: {dbgDetail.Code}, Description: {dbgDetail.Description}");
        }
    }
}

// Define classes to represent details of members, organizations, INS, DTP, and DBG segments
public class MemberDetail
{
    public string MemberId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string AddressLine1 { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string ZIP { get; set; }
}

public class OrganizationDetail
{
    public string MemberId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string OrganizationName { get; set; }
    public string OrganizationId { get; set; }
    public string AddressLine1 { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string ZIP { get; set; }
}

public class InsDetail
{
    public string Indicator { get; set; }
    public string Code { get; set; }
    public string Description { get; set; }
}

public class DtpDetail
{
    public string DateQualifier { get; set; }
    public string Date { get; set; }
}

public class DbgDetail
{
    public string Code { get; set; }
    public string Description { get; set; }
}
