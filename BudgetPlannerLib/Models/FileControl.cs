using System;
using System.Collections.Generic;
using System.Linq;

using System.IO;
using Microsoft.VisualBasic.FileIO;

namespace BudgetPlannerLib.Models
{
    public class FileControl
    {
        #region - Fields
        public string FilePath { get; set; }
        public string ProjectName { get; private set; }
        public bool IsFileSuccess { get; set; }

        private List<Income> _incomeData;
        private List<Expense> _expenseData;

        private List<SubCategory> _incomeSubCategories;
        private List<SubCategory> _expenseSubCategories;
        #endregion

        #region - Constructors
        /// <summary>
        /// Depriciated - Saves Income & Expense DataLists.
        /// </summary>
        /// <param name="filePath">Sent from SaveFileDialog</param>
        /// <param name="incomes">Save - IncomeDataList</param>
        /// <param name="expenses">Save - ExpenseDataList</param>
        public FileControl(string filePath, List<Income> incomes, List<Expense> expenses)
        {
            FilePath = filePath;
            IncomeData = incomes;
            ExpenseData = expenses;
        }

        /// <summary>
        /// Up to date Constructor for saving SubCategories.
        /// </summary>
        /// <param name="filePath">Sent from Open/SaveFileDialog Box</param>
        /// <param name="incomes">Save - IncomeDataList</param>
        /// <param name="expenses">Save - ExpenseDataList</param>
        /// <param name="incomeSubs">Save - Income.AllSubCategories</param>
        /// <param name="expenseSubs">Save - Expense.AllSubCategories</param>
        public FileControl(string filePath, List<Income> incomes, List<Expense> expenses, List<SubCategory> incomeSubs, List<SubCategory> expenseSubs)
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
        public FileControl(string filePath)
        {
            FilePath = filePath;
        }

        /// <summary>
        /// FOr Saving SubCategory Data to a seperate file.
        /// </summary>
        /// <param name="filePath">Sent from Open/SaveFileDialog Box</param>
        /// <param name="incomeSubs">Save - Income.AllSubCategories</param>
        /// <param name="expenseSubs">Save - Expense.AllSubCategories</param>
        public FileControl(string filePath, List<SubCategory> incomeSubs, List<SubCategory> expenseSubs)
        {
            FilePath = filePath;
            IncomeSubCateories = incomeSubs;
            ExpenseSubCategories = expenseSubs;
        }

        public FileControl(string projectName, string filePath, List<Income> incomes, List<Expense> expenses, List<SubCategory> incomeSubs, List<SubCategory> expenseSubs)
        {
            ProjectName = projectName;
            FilePath = filePath;
            IncomeData = incomes;
            ExpenseData = expenses;
            IncomeSubCateories = incomeSubs;
            ExpenseSubCategories = expenseSubs;
        }
        #endregion

        #region - Methods
        /// <summary>
        /// Opens a ".bpn" file thats been saved by the Budget Planner.
        /// </summary>
        public void OpenFile()
        {
            int index = 0;
            string dataDivider = "***";
            string nameMarker = "###";

            TextFieldParser parser = new TextFieldParser(FilePath);
            parser.SetDelimiters(new string[] { ":" });

            IncomeData = new List<Income>();
            ExpenseData = new List<Expense>();

            IncomeSubCateories = new List<SubCategory>();
            ExpenseSubCategories = new List<SubCategory>();

            while (!parser.EndOfData)
            {
                if (parser.PeekChars(3) == nameMarker)
                {
                    index++;
                    ProjectName = parser.ReadLine();
                }

                if (parser.PeekChars(3) == dataDivider)
                {
                    index++;
                    parser.ReadLine();
                }

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

            // Dispose of allocated memory.
            parser.Close();
            parser.Dispose();
        }

        /// <summary>
        /// Formats the data and saves to disk.
        /// </summary>
        public void SaveFile()
        {
            // Instansiates the Writer:
            string line = String.Empty;
            #pragma warning disable IDE0017 // Simplify object initialization
            StreamWriter writer = new StreamWriter(FilePath);
            #pragma warning restore IDE0017 // Simplify object initialization
            writer.AutoFlush = true;

            // Writes the name of the project:
            writer.WriteLine($"###{ProjectName}");

            // Writes the header and the Income DataList:
            writer.WriteLine("***Income Data");
            foreach (var item in IncomeData)
            {
                line = $"{item.IdNumber}:{item.Category}:{item.Value}:{item.SelectedCategory.Name}";
                writer.WriteLine(line);
            }

            // Writes the header and the Expense DataList:
            writer.WriteLine("***Expense Data");
            foreach (var item in ExpenseData)
            {
                line = $"{item.IdNumber}:{item.Category}:{item.Value}:{item.SelectedCategory.Name}";
                writer.WriteLine(line);
            }

            // Writes the header and the Income SubCategories:
            writer.WriteLine("***Income SubCategories");
            foreach (var item in IncomeSubCateories)
            {
                line = $"{item.Name}";
                writer.WriteLine(line);
            }

            // Writes the header and the Expense SubCategories:
            writer.WriteLine("***Expense SubCategories");
            foreach (var item in ExpenseSubCategories)
            {
                line = $"{item.Name}";
                writer.WriteLine(line);
            }

            // Disposes of the Writer Instance:
            writer.Close();
            writer.Dispose();
        }

        /// <summary>
        /// Formats the SubCategory data and saves to disk.
        /// </summary>
        public void SaveSubCategories()
        {
            // Instansiates the Writer:
            string line = String.Empty;
            #pragma warning disable IDE0017 // Simplify object initialization
            StreamWriter writer = new StreamWriter(FilePath);
            #pragma warning restore IDE0017 // Simplify object initialization
            writer.AutoFlush = true;

            // Writes the header and the Income SubCategories:
            writer.WriteLine("***Income SubCategories");
            foreach (var item in IncomeSubCateories)
            {
                line = $"{item.Name}";
                writer.WriteLine(line);
            }

            // Writes the header and the Expense SubCategories:
            writer.WriteLine("***Expense SubCategories");
            foreach (var item in ExpenseSubCategories)
            {
                line = $"{item.Name}";
                writer.WriteLine(line);
            }

            // Disposes of the Writer Instance:
            writer.Close();
            writer.Dispose();
        }

        /// <summary>
        /// Pulls SubCategory Data from a file.
        /// </summary>
        public void OpenSubCategories()
        {
            int index = 0;
            string dataDivider = "***";

            TextFieldParser parser = new TextFieldParser(FilePath);
            parser.SetDelimiters(new string[] { ":" });

            IncomeSubCateories = new List<SubCategory>();
            ExpenseSubCategories = new List<SubCategory>();

            // Parser reading loop.
            while (!parser.EndOfData)
            {
                if (parser.PeekChars(3) == dataDivider)
                {
                    index++;
                    parser.ReadLine();
                }

                switch (index)
                {
                    default:
                        throw new Exception("Index outside data bounds");
                    case 1:
                        IncomeSubCateories.Add(SubCategory.FromFields(parser.ReadFields()));
                        break;
                    case 2:
                        ExpenseSubCategories.Add(SubCategory.FromFields(parser.ReadFields()));
                        break;
                }
            }

            // Open allocated memory.
            parser.Close();
            parser.Dispose();
        }
        #endregion

        #region - Properties
        /// <summary>
        /// Saving - Income DataList from the DataViewModel
        /// </summary>
        public List<Income> IncomeData
        {
            get { return _incomeData; }
            set
            {
                _incomeData = value;
            }
        }

        /// <summary>
        /// Saving - Expense DataList from the DataViewModel
        /// </summary>
        public List<Expense> ExpenseData
        {
            get { return _expenseData; }
            set
            {
                _expenseData = value;
            }
        }

        /// <summary>
        /// Save - Static SubCategories from Income
        /// </summary>
        public List<SubCategory> IncomeSubCateories
        {
            get { return _incomeSubCategories; }
            set
            {
                _incomeSubCategories = value;
            }
        }

        /// <summary>
        /// Save - Static SubCategories from Expense
        /// </summary>
        public List<SubCategory> ExpenseSubCategories
        {
            get { return _expenseSubCategories; }
            set
            {
                _expenseSubCategories = value;
            }
        }
        #endregion

    }
}
