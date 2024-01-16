' VBScript to generate test data with different formulas for each column and update Excel columns

' Excel file details
ExcelFilePath = "C:\Path\To\Your\File.xlsx"
SheetName = "Sheet1"

' Columns to update with their corresponding formulas
ColumnFormulas = Array("=TEXT(RAND()*1000000000,""000-00-0000"")", "=TEXT(TODAY(),""yyDDD"")", "=TEXT(NOW(),""yyyy-mm-dd hh:mm:ss"")")

' Number of rows to generate
NumberOfRows = 10

' Create Excel application object
Set objExcel = CreateObject("Excel.Application")

' Make Excel visible (for debugging purposes)
objExcel.Visible = True

' Open the Excel file
Set objWorkbook = objExcel.Workbooks.Open(ExcelFilePath)

' Activate the specified sheet
Set objSheet = objWorkbook.Sheets(SheetName)
objSheet.Activate

' Generate test data with different formulas for each column and update columns
For i = 1 To NumberOfRows
    ' Generate test data for each column based on the specified formula
    For j = LBound(ColumnFormulas) To UBound(ColumnFormulas)
        ' Update the cell with the generated data using the formula
        objSheet.Cells(i, j + 1).Formula = ColumnFormulas(j)
    Next
Next

' Save changes and close Excel
objWorkbook.Save
objWorkbook.Close
objExcel.Quit

' Release Excel objects
Set objSheet = Nothing
Set objWorkbook = Nothing
Set objExcel = Nothing
