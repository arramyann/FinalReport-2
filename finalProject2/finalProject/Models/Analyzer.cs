using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using finalProject.Models;
using finalProject.Controllers;
namespace finalProject.Models
{

    public class Analyzer
    {
        public string Report(decimal[] dec) // Gets the IncomeData or the ExpenseData
        {
            decimal sum = 0;
            string all_str = ""; // This will be the whole data in a line
            for (int i = 0; i < dec.Length; i++)
            {
                sum += dec[i]; // Sum of the Income/Expense Data
                all_str += (dec[i]).ToString() + " ";
            }
            all_str = all_str + $"\nThe sum is: {sum}";
            return all_str;
        } 

    }
}
