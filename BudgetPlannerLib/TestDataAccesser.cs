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

        private string[] incomeSubCategories = { "Earned Income", "Unearned Income", "Other" };
        private string[] expenseSubCategories = { "Deductions", "Heathcare", "Food", "Insurance", "Transportation" };

        private List<string> _incomeCategories;
        private List<double> _incomeValues;
        private List<Income> _incomes;

        private List<string> _expenseCategories;
        private List<double> _expenseValues;
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
        #endregion

        #region - Methods
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

        private List<double> SetSampleNumbers(int length)
        {
            List<double> tempNums = new List<double>();

            // A silly way to get positive doubles.
            for (int i = 0; i < length; i++)
            {
                int whole = roll.Next(1, 10 * 10);
                int dec = roll.Next(0, 1000);
                string tempNum = $"{whole}.{dec}";
                tempNums.Add(Double.Parse(tempNum));
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

        public List<double> IncomeValues
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

        public List<double> ExpenseValues
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
        #endregion
    }
}
