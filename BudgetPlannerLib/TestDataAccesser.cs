using System;
using System.Collections.Generic;
using System.Linq;
using BudgetPlannerLib.Models;

namespace BudgetPlannerLib
{
    public class TestDataAccesser
    {
        #region - Fields
        private Random roll = new Random();

        private string[] incomeCategories = { "Wages", "commisions", "tax refund", "intrest", "asset sales", "money from others", "child support", "other" };

        private string[] expenseCategories = { "Federal income tax", "State income tax", "social security", "medicare", "savings", "investments",
            "heath insurance", "auto insurance", "renters insurance", "disability insurance", "life insurance", "property taxes", "housing payment", "utilities",
            "houshold furnishings", "car payment", "fuel", "car repairs/maintenance", "food & groceries", "snacks", "non-food products", "child care", "personal allowances",
            "phone bill", "cell phone", "internet", "cable/satelite", "computer expenses", "medical care", "dental care", "eye care", "medications" };

        public string[] incomeSubCategories = { "Earned Income", "Unearned Income", "Other" };
        public string[] expenseSubCategories = { "Deductions", "Heathcare", "Food", "Insurance", "Transportation" };

        private List<SubCategory> _incomeSubCats;

        private List<string> _incomeCategories;
        private List<decimal> _incomeValues;
        private List<Income> _incomes;

        private List<string> _expenseCategories;
        private List<decimal> _expenseValues;
        private List<Expense> _expenses;
        #endregion

        #region - Constructors
        public TestDataAccesser()
        {
            SetRandomData();
        }

        public TestDataAccesser(int incomeCategories, int expenseCategories)
        {
            SetRandomData(incomeCategories, expenseCategories);
        }

        public TestDataAccesser(int selector)
        {
            switch (selector)
            {
                default:
                    SetRandomData();
                    break;
                case 1:
                    SetStaticData();
                    break;
            }
        }
        #endregion

        #region - Methods
        private void SetStaticData()
        {
            IncomeCategories = incomeCategories.ToList();
            IncomeValues = SetSampleNumbers(incomeCategories.Length);

            ExpenseCategories = expenseCategories.ToList();
            ExpenseValues = SetSampleNumbers(expenseCategories.Length);

            IncomeList = new List<Income>();
            ExpenseList = new List<Expense>();

            Income.AllIncomeCategories = SetSubCategories(incomeSubCategories);
            Expense.AllExpenseCategories = SetSubCategories(expenseSubCategories);

            for (int i = 0; i < IncomeCategories.Count; i++)
            {
                IncomeList.Add(new Income(GetRandomItem(incomeSubCategories), IncomeCategories[i], IncomeValues[i], i + 1));
            }

            for (int i = 0; i < ExpenseCategories.Count; i++)
            {
                ExpenseList.Add(new Expense(GetRandomItem(expenseSubCategories), ExpenseCategories[i], ExpenseValues[i], i + 1));
            }
        }

        private void SetRandomData(int incAmount = 8, int expAmount = 12)
        {
            IncomeCategories = SetCategories(incomeCategories, incAmount);
            ExpenseCategories = SetCategories(expenseCategories, expAmount);

            IncomeValues = SetSampleNumbers(IncomeCategories.Count);
            ExpenseValues = SetSampleNumbers(ExpenseCategories.Count);
        }

        private T GetRandomItem<T>(T[] data)
        {
            return data[roll.Next(0, data.Length)];
        }

        private List<decimal> SetSampleNumbers(int length)
        {
            List<decimal> tempNums = new List<decimal>();

            // A silly way to get positive doubles.
            for (int i = 0; i < length; i++)
            {
                int whole = roll.Next(1, 10 * 10);
                int dec = roll.Next(0, 99);
                string tempNum = $"{whole}.{dec}";
                tempNums.Add(Decimal.Parse(tempNum, System.Globalization.NumberStyles.Currency));
            }

            return tempNums;
        }

        private List<string> SetCategories(string[] array, int amount)
        {
            List<string> tempStr = new List<string>();

            for (int i = 0; i < amount; i++)
            {
                tempStr.Add(GetRandomItem(array));
            }

            return tempStr;
        }

        private List<SubCategory> SetSubCategories(string[] array)
        {
            List<SubCategory> subCats = new List<SubCategory>();

            foreach (var item in array)
            {
                subCats.Add(new SubCategory(item));
            }

            return subCats;
        }
        #endregion

        #region - Properties
        public List<string> IncomeCategories
        {
            get { return _incomeCategories; }
            set
            {
                _incomeCategories = value;
            }
        }

        public List<decimal> IncomeValues
        {
            get { return _incomeValues; }
            set
            {
                _incomeValues = value;
            }
        }

        public List<string> ExpenseCategories
        {
            get { return _expenseCategories; }
            set
            {
                _expenseCategories = value;
            }
        }

        public List<decimal> ExpenseValues
        {
            get { return _expenseValues; }
            set
            {
                _expenseValues = value;
            }
        }

        public List<Income> IncomeList
        {
            get { return _incomes; }
            set
            {
                _incomes = value;
            }
        }

        public List<Expense> ExpenseList
        {
            get { return _expenses; }
            set
            {
                _expenses = value;
            }
        }

        public List<SubCategory> IncomeSubCategories
        {
            get { return _incomeSubCats; }
            set
            {
                _incomeSubCats = value;
            }
        }
        #endregion
    }
}
