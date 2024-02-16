using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace CompareDataTables
{
    class Program
    {
        static void Main(string[] args)
        {
            DataTable dtMaths = new DataTable("Maths");
            dtMaths.Columns.Add("StudID", typeof(int));
            dtMaths.Columns.Add("StudName", typeof(string));
            dtMaths.Rows.Add(1, "Mike");
            dtMaths.Rows.Add(2, "Mukesh");
            dtMaths.Rows.Add(3, "Erin");
            dtMaths.Rows.Add(4, "Roshni");
            dtMaths.Rows.Add(5, "Ajay");

            DataTable dtEnglish = new DataTable("English");
            dtEnglish.Columns.Add("StudID", typeof(int));
            dtEnglish.Columns.Add("StudName", typeof(string));
            dtEnglish.Rows.Add(6, "Arjun");
            dtEnglish.Rows.Add(2, "Mukesh");
            dtEnglish.Rows.Add(7, "John");
            dtEnglish.Rows.Add(4, "Roshni");
            dtEnglish.Rows.Add(8, "Kumar");

            DataTable dtOnlyMaths = dtMaths.AsEnumerable().Except(dtEnglish.AsEnumerable(), DataRowComparer.Default).CopyToDataTable();
            DataTable dtOnlyEnglish = dtEnglish.AsEnumerable().Except(dtMaths.AsEnumerable(), DataRowComparer.Default).CopyToDataTable();
            DataTable dtIntersect = dtMaths.AsEnumerable().Intersect(dtEnglish.AsEnumerable(), DataRowComparer.Default).CopyToDataTable();
            DataTable dtBoth = dtMaths.AsEnumerable().Union(dtEnglish.AsEnumerable(), DataRowComparer.Default).CopyToDataTable();

            // Generate HTML report for mismatches
            StringBuilder htmlReport = new StringBuilder();
            htmlReport.AppendLine("<html><head><style>table, th, td { border: 1px solid black; border-collapse: collapse; }</style></head><body>");
            htmlReport.AppendLine("<h2>Mismatch Report</h2>");

            // Helper function to generate HTML table rows
            Action<DataTable, string> addTableRows = (DataTable dt, string header) =>
            {
                htmlReport.AppendLine($"<h3>{header}</h3>");
                htmlReport.AppendLine("<table>");
                htmlReport.AppendLine("<tr><th>StudentID</th><th>StudentName</th></tr>");
                foreach (DataRow dr in dt.Rows)
                {
                    htmlReport.AppendLine($"<tr><td>{dr["StudID"]}</td><td>{dr["StudName"]}</td></tr>");
                }
                htmlReport.AppendLine("</table>");
            };

            addTableRows(dtOnlyMaths, "Students enrolled only for Maths");
            addTableRows(dtOnlyEnglish, "Students enrolled only for English");
            addTableRows(dtIntersect, "Students enrolled for both Math and English");
            addTableRows(dtBoth, "List of all students");

            htmlReport.AppendLine("</body></html>");

            // Save HTML report to file
            string reportFilePath = "MismatchReport.html";
            File.WriteAllText(reportFilePath, htmlReport.ToString());

            Console.WriteLine($"Mismatch report generated and saved to: {reportFilePath}");
            Console.Read();
        }
    }
}
