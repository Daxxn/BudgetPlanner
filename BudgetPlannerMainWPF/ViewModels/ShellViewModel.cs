using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using BudgetPlannerLib;
using BudgetPlannerLib.Models;
using Caliburn.Micro;

namespace BudgetPlannerMainWPF.ViewModels
{
    public class ShellViewModel : Screen
    {
        #region - Fields
        private BindableCollection<Income> _incomeDataList = new BindableCollection<Income>();
        private BindableCollection<Expense> _expenseDataList = new BindableCollection<Expense>();

        private Income _selecetedIncome;

        private double _incomeTotal;
        private double _expenseTotal;

        private double _netDifference;
        
        #endregion

        #region - Constructors
        public ShellViewModel()
        {
            TestDataAccesser testData = new TestDataAccesser(8, 20);
            AddCategories(testData.IncomeCategories, testData.IncomeValues, 1);
            AddCategories(testData.ExpenseCategories, testData.ExpenseValues, 2);
            UpdateData();

            DataElement.ValueChanged += this.DataElement_ValueChanged;
        }

        // Testing needed
        private void DataElement_ValueChanged(object sender, EventArgs e)
        {
            UpdateData();
        }
        #endregion

        #region - Methods
        /// <summary>
        /// Initializes a random list from TestDataAccesser
        /// </summary>
        public void AddCategories(List<string> cats, List<double> vals, int type)
        {
            if(type == 1)
            {
                for (int i = 0; i < cats.Count; i++)
                {
                    IncomeData.Add(new Income(cats[i], vals[i], i));
                }
            }
            else if (type == 2)
            {
                for (int i = 0; i < cats.Count; i++)
                {
                    ExpenseData.Add(new Expense(cats[i], vals[i], i));
                }
            }
        }

        public void AddIncomeColumn()
        {
            IncomeData.Add(new Income("", 0, IncomeData.Count));
        }

        public void AddExpenseColumn()
        {
            ExpenseData.Add(new Expense("New Expense", 0.0, ExpenseData.Count));
        }

        public static void Exit()
        {
            DataElement.Exit();
        }

        public void UpdateData()
        {
            IncomeTotal = IncomeData.Sum(x => x.Value);
            ExpenseTotal = ExpenseData.Sum(x => x.Value);
            NetDifference = IncomeTotal - ExpenseTotal;
        }
        #endregion

        #region - Properties
        public BindableCollection<Income> IncomeData
        {
            get { return _incomeDataList; }
            set
            {
                _incomeDataList = value;
                UpdateData();
                NotifyOfPropertyChange(() => IncomeData);
            }
        }

        public BindableCollection<Expense> ExpenseData
        {
            get { return _expenseDataList; }
            set
            {
                _expenseDataList = value;
                NotifyOfPropertyChange(() => ExpenseData);
            }
        }

        public Income SelectedIncome
        {
            get { return _selecetedIncome; }
            set
            {
                _selecetedIncome = value;
            }
        }

        public double NetDifference
        {
            get { return _netDifference; }
            set
            {
                _netDifference = value;
                NotifyOfPropertyChange(() => NetDifference);
            }
        }

        public double IncomeTotal
        {
            get { return _incomeTotal; }
            set
            {
                _incomeTotal = value;
                NotifyOfPropertyChange(() => IncomeTotal);
                NotifyOfPropertyChange(() => IncomeData);
            }
        }

        public double ExpenseTotal
        {
            get { return _expenseTotal; }
            set
            {
                _expenseTotal = value;
                NotifyOfPropertyChange(() => ExpenseTotal);
                NotifyOfPropertyChange(() => ExpenseData);
            }
        }
        #endregion
    }
}
