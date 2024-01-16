using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;

public class AttachmentUtility
{
    private static string personalAccessToken = ConfigurationManager.AppSettings["PersonalAccessToken"];
    private static string organization = ConfigurationManager.AppSettings["Organization"];
    private static string projectName = ConfigurationManager.AppSettings["ProjectName"];
    private static string attachmentDownloadPath = ConfigurationManager.AppSettings["AttachmentDownloadPath"];

    public static void DownloadAttachment(int testCaseId)
    {
        string testCaseUrl = $"https://dev.azure.com/{organization}/{projectName}/_apis/testplan/testcases/{testCaseId}?api-version=6.0";

        try
        {
            WebClient webClient = new WebClient();
            webClient.Headers.Add("Authorization", Convert.ToBase64String(Encoding.ASCII.GetBytes($":{personalAccessToken}")));

            // Get test case details
            byte[] testCaseData = webClient.DownloadData(testCaseUrl);
            string testCaseDetails = Encoding.ASCII.GetString(testCaseData);

            // Parse the JSON to get the attachment details
            // Note: You might need a JSON parsing library or use JSON.NET for this task

            // Assuming you have extracted attachment details (e.g., attachmentUrl, attachmentName) from testCaseDetails

            // Download the attachment
            byte[] attachmentData = webClient.DownloadData(attachmentUrl);
            string attachmentFilePath = Path.Combine(attachmentDownloadPath, attachmentName);
            File.WriteAllBytes(attachmentFilePath, attachmentData);

            Console.WriteLine($"Attachment downloaded and saved to: {attachmentFilePath}");
        }
        catch (WebException ex)
        {
            Console.WriteLine($"Failed to download attachment. Error: {ex.Message}");
        }
    }
}