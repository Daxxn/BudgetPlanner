using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.IO;
using BudgetPlannerLib.Models;
using PaystubLibrary;

namespace FileManagerLibrary
{
    public static class OpenController
    {
        #region - Fields & Properties
        public delegate void Message(string message);
        #endregion

        #region - Methods
        public static MainFile BeginOpen(Message message, string path)
        {
            MainFile output = new MainFile();

            if (File.Exists(path))
            {
                output.FullPath = path;
                XDocument doc = XDocument.Load(path);

                XElement main = doc.Element(Element.Root);
                string fileName = main.Attribute(Element.FileName).Value;
                string budgetPath = main.Element(Element.BudgetFile).Value;
                string categoryPath = main.Element(Element.CategoryFile).Value;
                string paystubPath = main.Element(Element.PaystubFile).Value;

                output.FileName = fileName;

                if (budgetPath != Element.Null)
                {
                    if (File.Exists(budgetPath))
                    {
                        output.Budget = OpenBudget(message, budgetPath);
                    }
                    else
                    {
                        message("Budget File could not be found.");
                    }
                }
                else
                {
                    message("budget file is null");
                }

                if (categoryPath != Element.Null)
                {
                    if (File.Exists(categoryPath))
                    {
                        output.Category = OpenCategory(message, categoryPath);
                    }
                    else
                    {
                        message("Category File could not be found.");
                    }
                }
                else
                {
                    message("Category File is Null");
                }

                if (paystubPath != Element.Null)
                {
                    if (File.Exists(paystubPath))
                    {
                        output.Paystub = OpenPaystub(message, paystubPath);
                    }
                    else
                    {
                        message("Paystub File could not be found.");
                    }
                }
                else
                {
                    message("Paystub File is null");
                }
            }

            return output;
        }

        private static BudgetFile OpenBudget(Message message, string path)
        {
            BudgetFile output = new BudgetFile();

            XDocument doc = XDocument.Load(path);
            XElement root = doc.Element(Element.Root);
            output.BudgetName = root.Attribute(Element.BudgetName).Value;
            XElement[] incomeData = doc.Descendants(Element.Income).ToArray();
            XElement[] expenseData = doc.Descendants(Element.Expense).ToArray();

            if (incomeData.Length > 0)
            {
                output.IncomeData = IterateIncomeData(incomeData);
            }
            else
            {
                message("Income Data is empty.");
            }

            if (expenseData.Length > 0)
            {
                output.ExpenseData = IterateExpenseData(expenseData);
            }
            else
            {
                message("Expense Data is empty.");
            }

            return output;
        }

        private static CategoryFile OpenCategory(Message message, string path)
        {
            CategoryFile output = new CategoryFile();

            XDocument doc = XDocument.Load(path);
            XElement root = doc.Element(Element.Root);
            output.CategoryName = root.Attribute(Element.CategoryName).Value;

            XElement incomeRoot = root.Element(Element.IncomeCategoryData);
            XElement[] incomeElements = incomeRoot.Descendants(Element.Category).ToArray();

            XElement expenseRoot = root.Element(Element.ExpenseCategoryData);
            XElement[] expenseElements = expenseRoot.Descendants(Element.Category).ToArray();

            if (incomeElements.Length > 0)
            {
                output.IncomeCategories = IterateCategoryData(incomeElements);
            }
            else
            {
                message("Income Categories are empty.");
            }

            if (expenseElements.Length > 0)
            {
                output.ExpenseCategories = IterateCategoryData(expenseElements);
            }

            return output;
        }

        private static PaystubFile OpenPaystub(Message message, string path)
        {
            PaystubFile output = new PaystubFile();

            XDocument doc = XDocument.Load(path);
            XElement root = doc.Element(Element.Root);

            output.Name = root.Attribute(Element.PaystubName).Value;
            output.Description = root.Attribute(Element.PaystubDescription).Value;

            XElement[] paystubElements = root.Descendants(Element.Paystub).ToArray();

            if (paystubElements.Length > 0)
            {
                output.Paystubs = IteratePaystubData(paystubElements);
            }
            else
            {
                message("Paystub data is empty.");
            }

            return output;
        }

        #region Iterate Data
        private static Income[] IterateIncomeData(XElement[] data)
        {
            List<Income> dataOut = new List<Income>();

            foreach (var d in data)
            {
                uint IDTemp = ParseUInt(d.Attribute(Element.ID).Value);
                string nameTemp = d.Attribute(Element.Name).Value;
                decimal amountTemp = ParseDecimal(d.Attribute(Element.Amount).Value);
                string categoryNameTemp = d.Attribute(Element.Category).Value;

                dataOut.Add(new Income(categoryNameTemp, nameTemp, amountTemp, IDTemp));
            }

            return dataOut.ToArray();
        }

        private static Expense[] IterateExpenseData(XElement[] data)
        {
            List<Expense> dataOut = new List<Expense>();

            foreach (var d in data)
            {
                uint IDTemp = ParseUInt(d.Attribute(Element.ID).Value);
                string nameTemp = d.Attribute(Element.Name).Value;
                decimal amountTemp = ParseDecimal(d.Attribute(Element.Amount).Value);
                string categoryNameTemp = d.Attribute(Element.Category).Value;

                dataOut.Add(new Expense(categoryNameTemp, nameTemp, amountTemp, IDTemp));
            }

            return dataOut.ToArray();
        }

        private static Category[] IterateCategoryData(XElement[] data)
        {
            List<Category> dataOut = new List<Category>();

            foreach (var d in data)
            {
                dataOut.Add(new Category(d.Attribute(Element.Name).Value));
            }

            return dataOut.ToArray();
        }

        private static Paystub[] IteratePaystubData(XElement[] data)
        {
            List<Paystub> dataOut = new List<Paystub>();

            foreach (var d in data)
            {
                uint indexTemp = ParseUInt(d.Attribute(Element.ID).Value);
                string nameTemp = d.Attribute(Element.Name).Value;
                decimal grossTemp = ParseDecimal(d.Attribute(Element.Gross).Value);
                decimal netTemp = ParseDecimal(d.Attribute(Element.Net).Value);
                decimal percentTemp = ParseDecimal(d.Attribute(Element.Percent).Value);

                dataOut.Add(new Paystub(indexTemp, nameTemp, grossTemp, netTemp, percentTemp));
            }

            return dataOut.ToArray();
        }
        #endregion

        #region Parsing
        private static int ParseInt(string input)
        {
            Int32.TryParse(input, out int output);

            return output;
        }

        private static uint ParseUInt(string input)
        {
            UInt32.TryParse(input, out uint output);

            return output;
        }

        private static decimal ParseDecimal(string input)
        {
            Decimal.TryParse(input, out decimal output);

            return output;
        }

        private static double ParseDouble(string input)
        {
            Double.TryParse(input, out double output);

            return output;
        }
        #endregion
        #endregion

        #region - Full Properties

        #endregion
    }
}
