static string CompareDataTables(DataTable dt1, DataTable dt2)
{
    StringBuilder htmlReport = new StringBuilder();
    int mismatchCount = 0; // Variable to count the number of mismatches

    // Create HTML headers
    htmlReport.AppendLine("<html><head><style>table, th, td { border: 1px solid black; border-collapse: collapse; }</style></head><body>");
    htmlReport.AppendLine("<h2>Mismatch Report</h2>");

    // Include total number of records in source and target
    htmlReport.AppendLine($"<p>Total records in source: {dt1.Rows.Count}</p>");
    htmlReport.AppendLine($"<p>Total records in target: {dt2.Rows.Count}</p>");

    // First frame for mismatches from source to target
    htmlReport.AppendLine("<h3>Source to Target Mismatches</h3>");
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
                mismatchCount++; // Increment mismatch count
            }
        }
    }

    htmlReport.AppendLine("</table><br>");

    // Second frame for mismatches from target to source
    htmlReport.AppendLine("<h3>Target to Source Mismatches</h3>");
    htmlReport.AppendLine("<table style='width:100%'>");
    htmlReport.AppendLine("<tr><th>Row</th><th>Column</th><th>Expected Value</th><th>Actual Value</th></tr>");

    // Reset mismatch count for the second frame
    mismatchCount = 0;

    // Iterate through rows and columns
    for (int i = 0; i < dt2.Rows.Count; i++)
    {
        for (int j = 0; j < dt2.Columns.Count; j++)
        {
            // Compare values
            if (!dt2.Rows[i][j].Equals(dt1.Rows[i][j]))
            {
                // Mismatch found, add to the HTML report
                htmlReport.AppendLine($"<tr><td>{i + 1}</td><td>{dt2.Columns[j].ColumnName}</td><td>{dt2.Rows[i][j]}</td><td>{dt1.Rows[i][j]}</td></tr>");
                mismatchCount++; // Increment mismatch count
            }
        }
    }

    htmlReport.AppendLine("</table></body></html>");

    // Return the HTML report
    return htmlReport.ToString();
}
