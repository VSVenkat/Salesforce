using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;

public class AttachmentUtility
{
    public static void DownloadAttachment(int testPlanId)
    {
        string personalAccessToken = ConfigurationManager.AppSettings["PersonalAccessToken"];
        string organization = ConfigurationManager.AppSettings["Organization"];
        string attachmentName = ConfigurationManager.AppSettings["AttachmentName"];

        string attachmentUrl = $"https://dev.azure.com/{organization}/_apis/test/Plans/{testPlanId}/Attachments/{attachmentName}?api-version=6.0";

        try
        {
            WebClient webClient = new WebClient();
            webClient.Headers.Add("Authorization", Convert.ToBase64String(Encoding.ASCII.GetBytes($":{personalAccessToken}")));

            byte[] data = webClient.DownloadData(attachmentUrl);

            // Save the attachment to a local file
            string localFilePath = Path.Combine(Path.GetTempPath(), attachmentName);
            File.WriteAllBytes(localFilePath, data);

            Console.WriteLine($"Attachment downloaded and saved to: {localFilePath}");
        }
        catch (WebException ex)
        {
            Console.WriteLine($"Failed to download attachment. Error: {ex.Message}");
        }
    }
}
