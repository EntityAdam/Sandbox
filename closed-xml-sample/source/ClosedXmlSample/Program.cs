using System.Data;
using ClosedXML.Excel;

string filename = @"C:\temp\file.xlsx";
Demo.CreateSample(filename);

internal static class Demo 
{
    internal static void CreateSample(string filename)
    {
        // create a new workbook
        using XLWorkbook workbook = new();

            // add a worksheet
            var worksheet = workbook.Worksheets.Add("Sample Sheet");

                // manipulate the cells
                worksheet.Cell("A1").Value = "Hello World!";
                worksheet.Cell("A2").FormulaA1 = "=MID(A1, 7, 5)";

            // add second worksheet with data table
            AddSheetWithDataTable(workbook, Data.SampleData());
        
        // save the workbook
        workbook.SaveAs(filename);
    }

    private static string AddSheetWithDataTable(XLWorkbook workbook, DataTable dataTable)
    {
        string worksheetName = $"_{DateTimeAsString(DateTime.Now)}";
        
        // add sheet with datatable
        IXLWorksheet worksheet = workbook.Worksheets.Add(dataTable, worksheetName);

        // insert a row for the table title
        worksheet.Row(1).InsertRowsAbove(1);
        
        // set the title
        worksheet.Cell("A1").Value = "Employees";

        // formatting
        worksheet.Range("A1:B1")
            .Merge()
            .Style.Font.SetBold()
            .Font.FontSize = 16;
            
        return worksheetName;
    }

    internal static string DateTimeAsString(DateTime dateTime) => dateTime.ToString("yyyy_MM_dd_HHmmss");
}

internal static class Data 
{
    public static DataTable SampleData()
    {
        var DataTable = new DataTable()
        {
            Columns = { "EmployeeId", "Name" }
        };
        DataTable.Rows.Add("EMP-001", "Johnson, Jack");
        DataTable.Rows.Add("EMP-002", "Jackson, John");
        return DataTable;
    }
}