using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using IronXL; // Import IronXL library for working with Excel files
using System.Data;
using System.Web.Mvc;
using finalProject.Models;

namespace finalProject.Controllers
{
    public class CsvController : Controller
    {
        // Action method to return the Upload Page view
        public ActionResult Upload()
        {
            return View();
        }

        // Action method to handle file upload via HTTP POST request
        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase UploadedFile)
        {
            // Check if a file has been uploaded and its content length is greater than 0
            if (UploadedFile != null && UploadedFile.ContentLength > 0)
            {
                // Get the stream of the uploaded file
                Stream File = UploadedFile.InputStream;

                // Load the uploaded file into a workbook using IronXL library
                WorkBook wb = WorkBook.Load(File);

                // Get the first worksheet from the workbook
                WorkSheet ws = wb.WorkSheets.First();

                // Get the value of the first cell in the worksheet
                string firstcell = (ws.GetCellAt(0, 0).Value).ToString();
                string second = (ws.GetCellAt(0, 1).Value).ToString();

                // Create a new DataForm object to store data
                DataForm data = new DataForm();

                // Retrieve data from Column A
                var ColumnA = ws["A2:A"];
                Cell[] cells = ColumnA.ToArray();

                // Retrieve data from Column B
                var ColumnB = ws["B2:B"];
                Cell[] cellsB = ColumnB.ToArray();

                // Check the content of the first cell to determine if it's income or expense
                if (firstcell == "Income" || firstcell == "Incomes")
                {
                    decimal[] inc = new decimal[cells.Length];

                    // Loop through cells in Column A and convert their values to decimal
                    for (int i = 0; i < cells.Length; i++)
                    {
                        inc[i] += Convert.ToDecimal(cells[i].Value);
                    }
                    data.Income_Data = inc;

                }
                else if (firstcell == "Expense" || firstcell == "Expenses")
                {
                    decimal[] exp = new decimal[cells.Length];

                    // Loop through cells in Column A and convert their values to decimal
                    for (int i = 0; i < cells.Length; i++)
                    {
                        exp[i] += Convert.ToDecimal(cells[i].Value);
                    }
                    data.Expense_Data = exp;
                }

                // Check the content of the second cell to determine if it's income or expense
                if (second == "Expense" || second == "Expenses")
                {
                    decimal[] exp = new decimal[cellsB.Length];

                    // Loop through cells in Column B and convert their values to decimal
                    for (int i = 0; i < cellsB.Length; i++)
                    {
                        exp[i] += Convert.ToDecimal(cellsB[i].Value);
                    }
                    data.Expense_Data = exp;
                }
                else if (second == "Income" || second == "Incomes")
                {
                    decimal[] inc = new decimal[cellsB.Length];

                    // Loop through cells in Column B and convert their values to decimal
                    for (int i = 0; i < cellsB.Length; i++)
                    {
                        inc[i] += Convert.ToDecimal(cells[i].Value);
                    }
                    data.Income_Data = inc;
                }

                // Create an instance of Analyzer class
                Analyzer an = new Analyzer();

                // Generate reports for income and expenses
                string inc_str = "Income: " + an.Report(data.Income_Data);
                string exp_str = "Expenses: " + an.Report(data.Expense_Data);

                // Combine income and expenses reports into a single array
                string[] str_list = { inc_str, exp_str };

                // Return the ReportResult view with the reports
                return View("ReportResult", str_list);
            }
            // If no file is uploaded, return the Upload Page view
            return View();
        }
    }
}
