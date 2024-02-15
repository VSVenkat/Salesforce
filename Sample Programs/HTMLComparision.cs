using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data;
using System.IO;
using System.Text;

[TestClass]
public class DataTableComparisonTest
{
    [TestMethod]
    public void TestDataTableComparison()
    {
        // Example DataTables (replace with your actual DataTables)
        DataTable dt1 = GetFirstDataTable();
        DataTable dt2 = GetSecondDataTable();

        // Compare the DataTables and create HTML report
        string htmlReport = CompareDataTables(dt1, dt2);

        // Save the HTML report to a file
        string reportFilePath = "DataTableComparisonReport.html";
        File.WriteAllText(reportFilePath, htmlReport);

        // Assert that there are no mismatches
        Assert.IsTrue(string.IsNullOrEmpty(htmlReport), "There are mismatches. See the HTML report for details.");

        Console.WriteLine($"HTML report saved to: {reportFilePath}");
    }

    static string CompareDataTables(DataTable dt1, DataTable dt2)
    {
        StringBuilder htmlReport = new StringBuilder();

        // Create HTML table headers
        htmlReport.AppendLine("<html><head><style>table, th, td { border: 1px solid black; border-collapse: collapse; }</style></head><body>");
        htmlReport.AppendLine("<h2>Mismatch Report</h2>");
        htmlReport.AppendLine("<table style='width:100%'>");
        htmlReport.AppendLine("<tr><th>Row</th><th>Column</th><th>Expected Value</th><th>Actual Value</th></tr>");

        // Iterate through rows and columns
        for (int i = 0; i < dt1.Rows.Count; i++)
        {
            for (int j = 0; j < dt1.Columns.Count; j++)
            {
                // Compare values
                if (!dt1.Rows[i][j].Equals(dt2.Rows[i][j]))
                {
                    // Mismatch found, add to the HTML report
                    htmlReport.AppendLine($"<tr><td>{i + 1}</td><td>{dt1.Columns[j].ColumnName}</td><td>{dt1.Rows[i][j]}</td><td>{dt2.Rows[i][j]}</td></tr>");
                }
            }
        }

        // Close HTML tags
        htmlReport.AppendLine("</table></body></html>");

        // Return the HTML report
        return htmlReport.ToString();
    }

    // Example DataTable methods (replace with your actual DataTables)
    static DataTable GetFirstDataTable() { /* Your code */ }
    static DataTable GetSecondDataTable() { /* Your code */ }
}
