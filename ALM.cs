using System;
using RestSharp;

class Program
{
    static void Main()
    {
        // ALM server details
        string almServer = "http://YourALMServer/qcbin";
        string domain = "YourDomain";
        string project = "YourProject";

        // User credentials
        string userName = "YourUserName";
        string password = "YourPassword";

        // Test set details
        string testSetFolder = "Root\\TestSetFolder";
        string testSetName = "YourTestSetName";

        // File to upload
        string attachmentFilePath = "C:\\Path\\To\\Your\\File.txt";

        // ALM REST API URLs
        string loginUrl = $"{almServer}/authentication-point/authenticate";
        string logoutUrl = $"{almServer}/authentication-point/logout";
        string testSetUrl = $"{almServer}/qcbin/rest/domains/{domain}/projects/{project}/test-sets";

        // Perform ALM login
        RestClient client = new RestClient(loginUrl);
        RestRequest request = new RestRequest(Method.POST);
        request.AddHeader("Content-Type", "application/json");
        request.AddParameter("application/json", $"{{\"username\": \"{userName}\", \"password\": \"{password}\"}}", ParameterType.RequestBody);
        IRestResponse response = client.Execute(request);

        if (response.StatusCode == System.Net.HttpStatusCode.OK)
        {
            Console.WriteLine("Login successful.");

            // Upload attachment to test set
            client = new RestClient(testSetUrl);
            request = new RestRequest(Method.GET);
            response = client.Execute(request);

            // Extract the test set ID from the response
            string testSetId = GetTestSetId(response.Content, testSetFolder, testSetName);

            if (!string.IsNullOrEmpty(testSetId))
            {
                Console.WriteLine($"Found Test Set: {testSetName} (ID: {testSetId})");

                // Upload attachment
                client = new RestClient($"{almServer}/qcbin/rest/domains/{domain}/projects/{project}/test-sets/{testSetId}/attachments");
                request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "multipart/form-data");
                request.AddHeader("Cookie", response.Headers.Get("Set-Cookie"));

                byte[] fileBytes = System.IO.File.ReadAllBytes(attachmentFilePath);
                request.AddFile("attachment", fileBytes, "File.txt", "application/octet-stream");

                response = client.Execute(request);

                if (response.StatusCode == System.Net.HttpStatusCode.Created)
                {
                    Console.WriteLine("Attachment uploaded.");

                    // Trigger execution
                    client = new RestClient($"{almServer}/qcbin/rest/domains/{domain}/projects/{project}/test-sets/{testSetId}/runs");
                    request = new RestRequest(Method.POST);
                    request.AddHeader("Content-Type", "application/json");
                    request.AddHeader("Cookie", response.Headers.Get("Set-Cookie"));
                    request.AddParameter("application/json", "{}", ParameterType.RequestBody);

                    response = client.Execute(request);

                    if (response.StatusCode == System.Net.HttpStatusCode.Created)
                    {
                        Console.WriteLine("Execution triggered.");
                    }
                    else
                    {
                        Console.WriteLine($"Failed to trigger execution. Status: {response.StatusCode}, Response: {response.Content}");
                    }
                }
                else
                {
                    Console.WriteLine($"Failed to upload attachment. Status: {response.StatusCode}, Response: {response.Content}");
                }
            }
            else
            {
                Console.WriteLine($"Test Set not found: {testSetName}");
            }

            // Logout
            client = new RestClient(logoutUrl);
            request = new RestRequest(Method.GET);
            client.Execute(request);

            Console.WriteLine("Logged out.");
        }
        else
        {
            Console.WriteLine($"Login failed. Status: {response.StatusCode}, Response: {response.Content}");
        }
    }

    // Helper method to extract Test Set ID from the ALM response
    static string GetTestSetId(string responseContent, string testSetFolder, string testSetName)
    {
        // Implement your logic to extract Test Set ID based on the response content
        // This example assumes a simple parsing, you may need to adjust it based on the actual response format

        // Example parsing logic
        string searchString = $"\"{testSetFolder}\\{testSetName}\"";
        int startIndex = responseContent.IndexOf(searchString);

        if (startIndex != -1)
        {
            int idStartIndex = responseContent.LastIndexOf("id\":", startIndex) + 4;
            int idEndIndex = responseContent.IndexOf(",", idStartIndex);

            if (idEndIndex != -1)
            {
                string testSetId = responseContent.Substring(idStartIndex, idEndIndex - idStartIndex).Trim();
                return testSetId;
            }
        }

        return null;
    }
}
