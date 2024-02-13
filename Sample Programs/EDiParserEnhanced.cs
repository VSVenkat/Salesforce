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

        // Load the EDI file
        var ediFile = EdiFile.Load(new FileInfo(ediFilePath), new EdiOptions { ReadAhead = true });

        // Process each transaction in the EDI file
        foreach (var transaction in ediFile.Transactions)
        {
            string memberId = string.Empty;
            string firstName = string.Empty;
            string lastName = string.Empty;

            foreach (var segment in transaction.Segments)
            {
                // Get segment data elements
                var elements = segment.Elements;

                switch (segment.Identifier)
                {
                    case "NM1":
                        memberId = elements.ElementAtOrDefault(9)?.Value;
                        firstName = elements.ElementAtOrDefault(3)?.Value;
                        lastName = elements.ElementAtOrDefault(4)?.Value;
                        break;
                    case "IL":
                        // Create member detail object and add to collection
                        members.Add(new MemberDetail
                        {
                            MemberId = memberId,
                            FirstName = firstName,
                            LastName = lastName,
                            Address = elements.ElementAtOrDefault(6)?.Value
                        });
                        break;
                    case "Names":
                        // Create organization detail object and add to collection
                        organizations.Add(new OrganizationDetail
                        {
                            MemberId = memberId,
                            FirstName = firstName,
                            LastName = lastName,
                            OrganizationName = elements.ElementAtOrDefault(3)?.Value,
                            OrganizationId = elements.ElementAtOrDefault(9)?.Value,
                            Address = elements.ElementAtOrDefault(6)?.Value
                        });
                        break;
                    case "INS":
                        // Create INS detail object and add to collection
                        insList.Add(new InsDetail
                        {
                            MemberId = memberId,
                            FirstName = firstName,
                            LastName = lastName,
                            Indicator = elements.ElementAtOrDefault(1)?.Value,
                            Code = elements.ElementAtOrDefault(2)?.Value,
                            Description = elements.ElementAtOrDefault(3)?.Value
                        });
                        break;
                    case "DTP":
                        // Create DTP detail object and add to collection
                        dtpList.Add(new DtpDetail
                        {
                            MemberId = memberId,
                            FirstName = firstName,
                            LastName = lastName,
                            DateQualifier = elements.ElementAtOrDefault(1)?.Value,
                            Date = elements.ElementAtOrDefault(2)?.Value
                        });
                        break;
                    case "DBG":
                        // Create DBG detail object and add to collection
                        dbgList.Add(new DbgDetail
                        {
                            MemberId = memberId,
                            FirstName = firstName,
                            LastName = lastName,
                            Code = elements.ElementAtOrDefault(0)?.Value,
                            Description = elements.ElementAtOrDefault(1)?.Value
                        });
                        break;
                }
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
            Console.WriteLine($"Member ID: {insDetail.MemberId}, First Name: {insDetail.FirstName}, Last Name: {insDetail.LastName}, Indicator: {insDetail.Indicator}, Code: {insDetail.Code}, Description: {insDetail.Description}");
        }

        // Print DTP details
        Console.WriteLine("\nDTP Details:");
        foreach (var dtpDetail in dtpList)
        {
            Console.WriteLine($"Member ID: {dtpDetail.MemberId}, First Name: {dtpDetail.FirstName}, Last Name: {dtpDetail.LastName}, Date Qualifier: {dtpDetail.DateQualifier}, Date: {dtpDetail.Date}");
        }

        // Print DBG details
        Console.WriteLine("\nDBG Details:");
        foreach (var dbgDetail in dbgList)
        {
            Console.WriteLine($"Member ID: {dbgDetail.MemberId}, First Name: {dbgDetail.FirstName}, Last Name: {dbgDetail.LastName}, Code: {dbgDetail.Code}, Description: {dbgDetail.Description}");
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
    public string MemberId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Indicator { get; set; }
    public string Code { get; set; }
    public string Description { get; set; }
}

public class DtpDetail
{
    public string MemberId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string DateQualifier { get; set; }
    public string Date { get; set; }
}

public class DbgDetail
{
    public string MemberId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Code { get; set; }
    public string Description { get; set; }
}
