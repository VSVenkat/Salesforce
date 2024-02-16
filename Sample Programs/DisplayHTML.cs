static string CompareDataTables(DataTable dt1, DataTable dt2)
{
    StringBuilder htmlReport = new StringBuilder();
    int mismatchCount = 0; // Variable to count the number of mismatches

    // Create HTML structure with two iframes divided horizontally
    htmlReport.AppendLine("<html><head><style>table, th, td { border: 1px solid black; border-collapse: collapse; }</style></head><body>");
    htmlReport.AppendLine("<h2>Mismatch Report</h2>");
    htmlReport.AppendLine("<div style='display: flex;'>");

    // Include total number of records in source and target
    htmlReport.AppendLine($"<div style='width: 50%;'><p>Total records in source: {dt1.Rows.Count}</p>");
    htmlReport.AppendLine("<iframe style='width: 100%; height: 500px; border: 1px solid black;' srcdoc='<table>");
    htmlReport.AppendLine("<tr><th>Row</th><th>Column</th><th>Expected Value</th><th>Actual Value</th></tr>");

    // Iterate through rows and columns
    for (int i = 0; i < dt1.Rows.Count; i++)
    {
        bool hasMismatch = false; // Flag to check if there is any mismatch in the current row

        for (int j = 0; j < dt1.Columns.Count; j++)
        {
            // Compare values
            if (!dt1.Rows[i][j].Equals(dt2.Rows[i][j]))
            {
                // Mismatch found, add to the HTML report and set the flag
                hasMismatch = true;
                htmlReport.AppendLine($"<tr style='background-color: yellow;'><td>{i + 1}</td><td>{dt1.Columns[j].ColumnName}</td><td>{dt1.Rows[i][j]}</td><td>{dt2.Rows[i][j]}</td></tr>");
            }
        }

        // If there is a mismatch in the current row, increment the mismatch count
        if (hasMismatch)
        {
            mismatchCount++;
        }
    }

    // Close the first iframe and the div
    htmlReport.AppendLine("</table>'></iframe></div>");

    // Include total number of records in target and mismatch count
    htmlReport.AppendLine($"<div style='width: 50%;'><p>Total records in target: {dt2.Rows.Count}</p>");
    htmlReport.AppendLine($"<p>Number of mismatched records: {mismatchCount}</p>");
    htmlReport.AppendLine("</div>");

    // Close the main div and body
    htmlReport.AppendLine("</div></body></html>");

    // Return the HTML report
    return htmlReport.ToString();
}
