using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
                            LastName = lastName,
                            Address = segments.ElementAtOrDefault(6)
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
                            OrganizationId = segments.ElementAtOrDefault(9),
                            Address = segments.ElementAtOrDefault(6)
                        });
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
            Console.WriteLine($"Member ID: {member.MemberId}, First Name: {member.FirstName}, Last Name: {member.LastName}, Address: {member.Address}");
        }

        // Print organization details
        Console.WriteLine("\nOrganizations:");
        foreach (var organization in organizations)
        {
            Console.WriteLine($"Member ID: {organization.MemberId}, First Name: {organization.FirstName}, Last Name: {organization.LastName}, Organization Name: {organization.OrganizationName}, Organization ID: {organization.OrganizationId}, Address: {organization.Address}");
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
    public string Address { get; set; }
}

public class OrganizationDetail
{
    public string MemberId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string OrganizationName { get; set; }
    public string OrganizationId { get; set; }
    public string Address { get; set; }
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
