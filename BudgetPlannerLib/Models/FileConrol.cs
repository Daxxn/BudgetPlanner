using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using Microsoft.VisualBasic.FileIO;

namespace BudgetPlannerLib.Models
{
    public class FileConrol
    {
        #region - Fields
        public string FilePath { get; set; }
        public bool IsFileSuccess { get; set; }

        private List<Income> _incomeData;
        private List<Expense> _expenseData;

        private List<int> _ids;
        private List<string> _categories;
        private List<double> _values;
        #endregion

        #region - Constructors
        /// <summary>
        /// For Saving Income & Expense Data.
        /// </summary>
        public FileConrol(string filePath, List<Income> incomes, List<Expense> expenses)
        {
            FilePath = filePath;
            IncomeData = incomes;
            ExpenseData = expenses;
        }

        /// <summary>
        /// For Loading Income & Expense Data.
        /// </summary>
        public FileConrol(string filePath)
        {
            FilePath = filePath;
        }
        #endregion

        #region - Methods
        public void OpenFile()
        {
            int index = 1;

            TextFieldParser parser = new TextFieldParser(FilePath);
            parser.SetDelimiters(new string[] { ":" });

            IncomeData = new List<Income>();
            ExpenseData = new List<Expense>();

            while (!parser.EndOfData)
            {
                if(index == parser.LineNumber)
                {
                    IncomeData.Add(Income.FromFields(parser.ReadFields()));
                    index++;
                }
                else
                {
                    ExpenseData.Add(Expense.FromFields(parser.ReadFields()));
                    index++;
                }
            }

            parser.Close();
            parser.Dispose();
            
        }

        public void SaveFile()
        {
            string line = String.Empty;
            StreamWriter writer = new StreamWriter(FilePath);
            writer.AutoFlush = true;

            foreach (var item in IncomeData)
            {
                line = $"{item.IdNumber}:{item.Category}:{item.Value}";
                writer.WriteLine(line);
            }

            writer.WriteLine();

            foreach (var item in ExpenseData)
            {
                line = $"{item.IdNumber}:{item.Category}:{item.Value}";
                writer.WriteLine(line);
            }

            writer.Close();
            writer.Dispose();
        }

        #endregion

        #region - Properties
        public List<Income> IncomeData
        {
            get { return _incomeData; }
            set
            {
                _incomeData = value;
            }
        }

        public List<Expense> ExpenseData
        {
            get { return _expenseData; }
            set
            {
                _expenseData = value;
            }
        }

        public List<string> Categories
        {
            get { return _categories; }
            set
            {
                _categories = value;
            }
        }

        public List<double> Values
        {
            get { return _values; }
            set
            {
                _values = value;
            }
        }

        public List<int> IDs
        {
            get { return _ids; }
            set
            {
                _ids = value;
            }
        }
        #endregion

    }
}
