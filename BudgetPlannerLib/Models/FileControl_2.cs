using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;

namespace BudgetPlannerLib.Models
{
    public static class FileControl_2
    {
        #region Fields
        public static string DataDivider { get; set; } = "***";
        public static string NameMarker { get; set; } = "###";
        public static string DirectoryMarker { get; set; } = "///";
        public static string[] Delimiter { get; set; } = { ":" };

        public delegate void OpenMainFileOutput(out string name, out List<Income> incomeData, out List<Expense> expenseData);
        public delegate void OpenSubCategoryOutput(out List<SubCategory> incomeSubCategories, out List<SubCategory> expenseSubCategories);
        #endregion

        #region - Methods
        #region -- Private Methods
        private static string CleanName(string input)
        {
            string[] tempSplit = input.Split(NameMarker.ToArray(), StringSplitOptions.RemoveEmptyEntries);

            if (tempSplit[0] != String.Empty)
            {
                return tempSplit[0];
            }
            else return "";
        }
        #endregion
        public static bool SaveMainFile(string fileName, string projectName, List<Income> incomeData, List<Expense> expenseData)
        {
            if (File.Exists(fileName))
            {
                // Instansiates the Writer:
                string line = String.Empty;
                StreamWriter writer = new StreamWriter(fileName) { AutoFlush = true };

                // Writes the name of the project:
                writer.WriteLine($"{NameMarker}{projectName}");

                // Writes the header and the Income DataList:
                writer.WriteLine($"{DataDivider}Income Data");
                foreach (var item in incomeData)
                {
                    line = $"{item.IdNumber}:{item.Category}:{item.Value}:{item.SelectedCategory.Name}";
                    writer.WriteLine(line);
                }

                // Writes the header and the Expense DataList:
                writer.WriteLine($"{DataDivider}Expense Data");
                foreach (var item in expenseData)
                {
                    line = $"{item.IdNumber}:{item.Category}:{item.Value}:{item.SelectedCategory.Name}";
                    writer.WriteLine(line);
                }

                // Disposes of the Writer Instance:
                writer.Close();
                writer.Dispose();

                return true;
            }
            else return false;
        }

        public static bool SaveSubFile(string folder, List<SubCategory> incomeSubCat, List<SubCategory> expenseSubCat)
        {
            if (Directory.Exists(folder))
            {
                // Instansiates the Writer:
                StreamWriter writer = new StreamWriter(folder) { AutoFlush = true };

                writer.WriteLine($"{DataDivider}IncomeSubCategories");
                foreach (var item in incomeSubCat)
                {
                    writer.WriteLine(item.Name);
                }

                writer.WriteLine($"{DataDivider}ExpenseSubCategories");
                foreach (var item in expenseSubCat)
                {
                    writer.WriteLine(item.Name);
                }

                writer.Close();
                writer.Dispose();
                return true;
            }
            else return false;
        }

        /// <summary>
        /// NOT IN USE!!!
        /// </summary>
        /// <param name="path"></param>
        /// <param name="openMainFileOutput"></param>
        /// <returns></returns>
        public static bool OpenMainFile(string path, OpenMainFileOutput openMainFileOutput)
        {
            int index = 0;

            string projectNameTemp = "";
            List<Income> incomeDataTemp = new List<Income>();
            List<Expense> expenseDataTemp = new List<Expense>();

            if (Directory.Exists(path))
            {
                TextFieldParser parser = new TextFieldParser(path);
                parser.SetDelimiters(Delimiter);
                
                while (!parser.EndOfData)
                {
                    if (parser.PeekChars(3) == NameMarker)
                    {
                        index++;
                        projectNameTemp = CleanName(parser.ReadLine());

                    }

                    if (parser.PeekChars(3) == DataDivider)
                    {
                        index++;
                        parser.ReadLine();
                    }

                    switch (index)
                    {
                        default:
                            throw new Exception("Index outside data bounds");
                        case 1:
                            incomeDataTemp.Add(Income.FromFields(parser.ReadFields()));
                            break;
                        case 2:
                            expenseDataTemp.Add(Expense.FromFields(parser.ReadFields()));
                            break;
                    }
                }

                // Dispose of allocated memory, if needed.
                parser?.Close();
                parser?.Dispose();

                bool dataCheck = false;
                if(projectNameTemp != null && incomeDataTemp != null && expenseDataTemp != null)
                {
                    dataCheck = true;
                }

                if (dataCheck)
                {
                    openMainFileOutput(out projectNameTemp, out incomeDataTemp, out expenseDataTemp);
                    return true;
                }
                else return false;
            }
            else
            {
                return false;
            }
        }

        public static void OpenMainFile(string path, out string projectName, out List<Income> incomeData, out List<Expense> expenseData)
        {
            int index = 0;

            projectName = "";
            incomeData = new List<Income>();
            expenseData = new List<Expense>();

            if (File.Exists(path))
            {
                TextFieldParser parser = new TextFieldParser(path);
                parser.SetDelimiters(Delimiter);

                while (!parser.EndOfData)
                {
                    if (parser.PeekChars(3) == NameMarker)
                    {
                        projectName = CleanName(parser.ReadLine());
                    }
                    if (parser.PeekChars(3) == DataDivider)
                    {
                        index++;
                        parser.ReadLine();
                    }

                    switch (index)
                    {
                        default:
                            break;
                        case 1:
                            incomeData.Add(Income.FromFields(parser.ReadFields()));
                            break;
                        case 2:
                            expenseData.Add(Expense.FromFields(parser.ReadFields()));
                            break;
                    }
                }

                // Dispose of allocated memory, if needed.
                parser?.Close();
                parser?.Dispose();
            }
        }

        public static bool OpenSubsFile(string path, out List<SubCategory> incomeSubs, out List<SubCategory> expenseSubs)
        {
            int index = 0;
            bool success = true;

            incomeSubs = new List<SubCategory>();
            expenseSubs = new List<SubCategory>();

            if (File.Exists(path))
            {
                TextFieldParser parser = new TextFieldParser(path);

                while (!parser.EndOfData)
                {
                    if(parser.PeekChars(3) == DataDivider)
                    {
                        index++;
                        parser.ReadLine();
                    }

                    switch (index)
                    {
                        default:
                            success = false;
                            break;
                        case 1:
                            incomeSubs.Add(new SubCategory(parser.ReadLine()));
                            break;
                        case 2:
                            expenseSubs.Add(new SubCategory(parser.ReadLine()));
                            break;
                    }
                }

                // Dispose of allocated memory, if needed.
                parser?.Close();
                parser?.Dispose();
            }
            
            return success;
        }
        #endregion
    }
}
