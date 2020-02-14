using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.IO;
using BudgetPlannerLib.Models;
using PaystubLibrary;
using BudgetPlannerLib;

namespace FileManagerLibrary
{
    public static class SaveController
    {
        #region - Fields & Properties
        public static MainFile Main { get; set; } = new MainFile();
        public delegate void Message(string message);
        #endregion

        #region - Methods
        #region -- Pulling Data Methods
        public static void PullMainFileData(string mainFileName, string mainFilePath)
        {
            Main = new MainFile()
            {
                FileName = mainFileName,
                FullPath = mainFilePath
            };
        }

        public static void PullBudgetFileData(string name, string path, Income[] income, Expense[] expense)
        {
            Main.Budget = new BudgetFile()
            {
                BudgetName = name,
                FullPath = path,
                IncomeData = income,
                ExpenseData = expense
            };
        }

        public static void PullCategoryData(string name, string path, Category[] income, Category[] expense)
        {
            Main.Category = new CategoryFile()
            {
                CategoryName = name,
                FullPath = path,
                IncomeCategories = income,
                ExpenseCategories = expense
            };
        }

        public static void PullPaystubData(string name, string path, string desc, Paystub[] paystubs)
        {
            Main.Paystub = new PaystubFile()
            {
                Name = name,
                FullPath = path,
                Description = desc,
                Paystubs = paystubs
            };
        }
        #endregion

        #region -- Save Methods
        public static void BeginSave(Message message, MainFile mainFile)
        {
            // Testing
            Main = mainFile;
            // Testing

            XDocument mainDoc = WriteMainDoc(mainFile);
            XDocument budgetDoc = WriteBudgetDoc(mainFile.Budget);
            XDocument categoryDoc = WriteCategoryDoc(mainFile.Category);
            XDocument paystubDoc = WritePaystubDoc(mainFile.Paystub);

            message($"{mainDoc.ToString()} \n\n {budgetDoc.ToString()} \n\n {categoryDoc.ToString()} \n\n {paystubDoc.ToString()}");

            mainDoc.Save(mainFile.FullPath);
            budgetDoc.Save(mainFile.Budget.FullPath);
            categoryDoc.Save(mainFile.Category.FullPath);
            paystubDoc.Save(mainFile.Paystub.FullPath);
        }

        public static bool BeginSave(Message message)
        {
            bool success;
            XDocument mainDoc = WriteMainDoc(Main);
            XDocument budgetDoc = WriteBudgetDoc(Main.Budget);
            XDocument categoryDoc = WriteCategoryDoc(Main.Category);
            XDocument paystubDoc = WritePaystubDoc(Main.Paystub);

            message($"{mainDoc.ToString()} \n\n {budgetDoc.ToString()} \n\n {categoryDoc.ToString()} \n\n {paystubDoc.ToString()}");

            // THIS IS BAD! but i just need something for now...
            // Change later.
            try
            {
                mainDoc.Save(Main.FullPath);
                budgetDoc.Save(Main.Budget.FullPath);
                categoryDoc.Save(Main.Category.FullPath);
                paystubDoc.Save(Main.Paystub.FullPath);

                success = true;
            }
            catch
            {
                success = false;
            }

            return success;
        }

        private static XDocument WriteMainDoc(MainFile main)
        {
            if (main.Budget != null && main.Category != null && main.Paystub != null)
            {
                return new XDocument(
                        CreateDeclaration(),
                        new XElement(Element.Root, new XAttribute(Element.FileName, main.FileName),
                            new XElement(Element.BudgetFile, main.Budget.FullPath),
                            new XElement(Element.CategoryFile, main.Category.FullPath),
                            new XElement(Element.PaystubFile, main.Paystub.FullPath)
                            )
                        );
            }
            else
        }

        private static XDocument WriteBudgetDoc(BudgetFile budget)
        {
            return new XDocument(CreateDeclaration(),
                new XElement(Element.Root, new XAttribute(Element.BudgetName, budget.BudgetName),
                EnumerateColumnData(budget.IncomeData, true),
                EnumerateColumnData(budget.ExpenseData, false)
                )
            );
        }

        private static XDocument WriteCategoryDoc(CategoryFile category)
        {
            return new XDocument(CreateDeclaration(),
                new XElement(Element.Root, new XAttribute(Element.CategoryName, category.CategoryName),
                EnumerateCategoryData(category.IncomeCategories, true),
                EnumerateCategoryData(category.ExpenseCategories, false)
                )
            );
        }

        private static XDocument WritePaystubDoc(PaystubFile paystub)
        {
            return new XDocument(CreateDeclaration(),
                new XElement(Element.Root, new XAttribute(Element.PaystubName, paystub.Name),
                new XAttribute(Element.PaystubDescription, paystub.Description),
                EnumeratePaystubData(paystub.Paystubs)
                )
            );
        }

        private static XElement EnumerateColumnData(BaseColumn[] data, bool isIncome)
        {
            List<XElement> allElements = new List<XElement>();

            foreach (var d in data)
            {
                allElements.Add(CreateColmnElement(d, isIncome));
            }

            if (isIncome)
            {
                return new XElement(Element.IncomeData, allElements);
            }
            else
            {
                return new XElement(Element.ExpenseData, allElements);
            }
        }

        private static XElement EnumerateCategoryData(Category[] data, bool isIncome)
        {
            List<XElement> allElements = new List<XElement>();

            foreach (var d in data)
            {
                allElements.Add(CreateCategoryElement(d));
            }

            if (isIncome)
            {
                return new XElement(Element.IncomeCategoryData, allElements);
            }
            else
            {
                return new XElement(Element.ExpenseCategoryData, allElements);
            }
        }

        private static XElement EnumeratePaystubData(Paystub[] data)
        {
            List<XElement> allElements = new List<XElement>();

            foreach (var d in data)
            {
                allElements.Add(CreatePaystubElement(d));
            }

            return new XElement(Element.PaystubData, allElements);
        }

        private static XElement CreateFilePathElement(Element type, FileModel file)
        {
            if(type == Element.BudgetFile)
        }

        private static XElement CreateColmnElement(BaseColumn d, bool isIncome)
        {
            string type = Element.Expense;

            if (isIncome)
            {
                type = Element.Income;
            }

            return new XElement(type,
                new XAttribute(Element.ID, d.IDNumber),
                new XAttribute(Element.Name, d.Name),
                new XAttribute(Element.Amount, d.Amount),
                new XAttribute(Element.Category, d.Category.Name)
                );
        }

        private static XElement CreateCategoryElement(Category c)
        {
            return new XElement(Element.Category,
                new XAttribute(Element.Name, c.Name)
                );
        }

        private static XElement CreatePaystubElement(Paystub p)
        {
            return new XElement(Element.Paystub,
                new XAttribute(Element.ID, p.Index),
                new XAttribute(Element.Name, p.Name),
                new XAttribute(Element.Gross, p.Gross),
                new XAttribute(Element.Net, p.Net),
                new XAttribute(Element.Percent, p.Percent)
                );
        }
        #endregion

        #region -- Declaration Methods
        private static XDeclaration CreateDeclaration()
        {
            return new XDeclaration("1.0", "utf-8", "yes");
        }

        public static XDeclaration CreateDeclaration(int version)
        {
            return new XDeclaration($"{version}", "utf-8", "yes");
        }
        #endregion
        #endregion

        #region - Full Properties

        #endregion
    }
}
