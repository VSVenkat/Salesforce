using System;
using System.IO;
using RestSharp;

class Program
{
    static void Main()
    {
        // ADO organization URL
        string adoUrl = "https://dev.azure.com/YourOrganization";

        // Personal Access Token (PAT)
        string pat = "YourPersonalAccessToken";

        // Project and Test Plan details
        string project = "YourProject";
        int testPlanId = 1; // Replace with your Test Plan ID
        int attachmentId = 1; // Replace with your Attachment ID

        // ADO REST API URLs
        string attachmentDownloadUrl = $"{adoUrl}/{project}/_apis/test/plans/{testPlanId}/attachments/{attachmentId}?api-version=6.0";

        // Create RestClient with base URL
        var client = new RestClient(attachmentDownloadUrl);

        // Create request
        var request = new RestRequest(Method.GET);

        // Add Personal Access Token to request header
        request.AddHeader("Authorization", $"Basic {Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($":{pat}")));

        // Execute request and get response
        var response = client.Execute(request);

        if (response.IsSuccessful)
        {
            // Save attachment content to a file
            string fileName = "DownloadedAttachment.txt"; // Provide the desired file name
            File.WriteAllText(fileName, response.Content);

            Console.WriteLine($"Attachment downloaded and saved to {fileName}");
        }
        else
        {
            Console.WriteLine($"Failed to download attachment. Status: {response.StatusCode}, Response: {response.Content}");
        }
    }
}
