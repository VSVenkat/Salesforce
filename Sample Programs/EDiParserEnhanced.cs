using System;
using System.Collections.Generic;
using System.IO;
using EdiFabric.Framework;
using EdiFabric.Framework.Readers;
using EdiFabric.Models.X12;
using EdiFabric.Templates.X12004010;

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

        // Create a reader for X12 format (version 4010)
        using (var stream = File.OpenRead(ediFilePath))
        {
            using (var ediReader = new X12Reader(stream, "EdiFabric.Templates.X12"))
            {
                while (ediReader.Read())
                {
                    var transaction = ediReader.CurrentTransaction;

                    // Check if the transaction is of type 834
                    if (transaction is TS834 ts834)
                    {
                        // Process segments
                        foreach (var segment in ts834.AllSegments)
                        {
                            string memberId = string.Empty;
                            string firstName = string.Empty;
                            string lastName = string.Empty;

                            if (segment is NM1 nm1Segment)
                            {
                                memberId = nm1Segment.MemberIdentificationNumber;
                                firstName = nm1Segment.FirstName;
                                lastName = nm1Segment.LastName;
                            }

                            switch (segment)
                            {
                                case NM1 nm1Segment when nm1Segment.EntityIdentifierCode == "IL":
                                    // Create member detail object and add to collection
                                    members.Add(new MemberDetail
                                    {
                                        MemberId = memberId,
                                        FirstName = firstName,
                                        LastName = lastName,
                                        Address = nm1Segment.AddressLine
                                    });
                                    break;
                                case NM1 nm1Segment when nm1Segment.EntityIdentifierCode == "Names":
                                    // Create organization detail object and add to collection
                                    organizations.Add(new OrganizationDetail
                                    {
                                        MemberId = memberId,
                                        FirstName = firstName,
                                        LastName = lastName,
                                        OrganizationName = nm1Segment.OrganizationName,
                                        OrganizationId = nm1Segment.OrganizationIdentificationNumber,
                                        Address = nm1Segment.AddressLine
                                    });
                                    break;
                                case INS insSegment:
                                    // Create INS detail object and add to collection
                                    insList.Add(new InsDetail
                                    {
                                        MemberId = memberId,
                                        FirstName = firstName,
                                        LastName = lastName,
                                        Indicator = insSegment.YesNoConditionOrResponseCode,
                                        Code = insSegment.YesNoConditionOrResponseCode,
                                        Description = insSegment.Description
                                    });
                                    break;
                                case DTP dtpSegment:
                                    // Create DTP detail object and add to collection
                                    dtpList.Add(new DtpDetail
                                    {
                                        MemberId = memberId,
                                        FirstName = firstName,
                                        LastName = lastName,
                                        DateQualifier = dtpSegment.DateQualifier,
                                        Date = dtpSegment.Date
                                    });
                                    break;
                                case DBG dbgSegment:
                                    // Create DBG detail object and add to collection
                                    dbgList.Add(new DbgDetail
                                    {
                                        MemberId = memberId,
                                        FirstName = firstName,
                                        LastName = lastName,
                                        Code = dbgSegment.DataElementReferenceNumber,
                                        Description = dbgSegment.Description
                                    });
                                    break;
                            }
                        }
                    }
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
    public string Code { get
