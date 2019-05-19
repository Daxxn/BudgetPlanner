using System;
using System.Collections.Generic;
using System.Linq;

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

        private List<SubCategory> _incomeSubCategories;
        private List<SubCategory> _expenseSubCategories;

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

        public FileConrol(string filePath, List<Income> incomes, List<Expense> expenses, List<SubCategory> incomeSubs, List<SubCategory> expenseSubs)
        {
            FilePath = filePath;
            IncomeData = incomes;
            ExpenseData = expenses;
            IncomeSubCateories = incomeSubs;
            ExpenseSubCategories = expenseSubs;
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
            int index = 0;
            string dataDivider = "***";

            TextFieldParser parser = new TextFieldParser(FilePath);
            parser.SetDelimiters(new string[] { ":" });

            IncomeData = new List<Income>();
            ExpenseData = new List<Expense>();

            while (!parser.EndOfData)
            {
                /*
                if(index == parser.LineNumber)
                {
                    IncomeData.Add(Income.FromFields(parser.ReadFields()));
                    index++;
                }
                else
                {
                    ExpenseData.Add(Expense.FromFields(parser.ReadFields()));
                    index++;
                }*/

                if(parser.PeekChars(3) == dataDivider)
                {
                    index++;
                    switch (index)
                    {
                        default:
                            throw new Exception("Index outside data bounds");
                        case 1:
                            IncomeData.Add(Income.FromFields(parser.ReadFields()));
                            break;
                        case 2:
                            ExpenseData.Add(Expense.FromFields(parser.ReadFields()));
                            break;
                        case 3:
                            IncomeSubCateories.Add(SubCategory.FromFields(parser.ReadFields()));
                            break;
                        case 4:
                            ExpenseSubCategories.Add(SubCategory.FromFields(parser.ReadFields()));
                            break;
                    }
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

            writer.WriteLine("***Income Data");
            foreach (var item in IncomeData)
            {
                line = $"{item.IdNumber}:{item.Category}:{item.Value}:{item.SelectedCategory.Name}";
                writer.WriteLine(line);
            }

            writer.WriteLine("***Expense Data");
            foreach (var item in ExpenseData)
            {
                line = $"{item.IdNumber}:{item.Category}:{item.Value}:{item.SelectedCategory.Name}";
                writer.WriteLine(line);
            }

            writer.WriteLine("***Income SubCategories");
            foreach (var item in IncomeSubCateories)
            {
                line = $"{item.Name}";
                writer.WriteLine(line);
            }

            writer.WriteLine("***Expense SubCategories");
            foreach (var item in ExpenseSubCategories)
            {
                line = $"{item.Name}";
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

        public List<SubCategory> IncomeSubCateories
        {
            get { return _incomeSubCategories; }
            set
            {
                _incomeSubCategories = value;
            }
        }

        public List<SubCategory> ExpenseSubCategories
        {
            get { return _expenseSubCategories; }
            set
            {
                _expenseSubCategories = value;
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
