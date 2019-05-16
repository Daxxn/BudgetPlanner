using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BudgetPlannerLib;
using BudgetPlannerLib.Models;
using Caliburn.Micro;

namespace BudgetPlannerMainWPF.ViewModels
{
    public class DataViewModel : Screen
    {
        #region - Fields
        private BindableCollection<Income> _incomeDataList;
        private BindableCollection<Expense> _expenseDataList;

        private Income _selectedIncome;
        private Expense _selectedExpense;

        private double _incomeTotal;
        private double _expenseTotal;

        private double _netDifference;
        #endregion

        #region - Constructors
        public DataViewModel()
        {
            DataElement.ValueChanged += this.DataElement_ValueChanged;
        }
        public DataViewModel(BindableCollection<Income> incomes, BindableCollection<Expense> expenses)
        {
            IncomeDataList = incomes;
            ExpenseDataList = expenses;
        }
        #endregion

        #region - Methods
        public void AddStaticCategories()
        {
            TestDataAccesser testData = new TestDataAccesser(1);
            IncomeDataList = new BindableCollection<Income>(testData.IncomeList);
            ExpenseDataList = new BindableCollection<Expense>(testData.ExpenseList);
        }

        private void DataElement_ValueChanged(object sender, EventArgs e)
        {
            UpdateData();
        }

        public void UpdateData()
        {
            if (ExpenseDataList != null)
            {
                IncomeTotal = IncomeDataList.Sum(x => x.Value);
                ExpenseTotal = ExpenseDataList.Sum(x => x.Value); 
                NetDifference = IncomeTotal - ExpenseTotal;
            }
        }
        #endregion

        #region - Properties

        public BindableCollection<Income> IncomeDataList
        {
            get { return _incomeDataList; }
            set
            {
                _incomeDataList = value;
                UpdateData();
                NotifyOfPropertyChange(() => IncomeDataList);
            }
        }

        public BindableCollection<Expense> ExpenseDataList
        {
            get { return _expenseDataList; }
            set
            {
                _expenseDataList = value;
                UpdateData();
                NotifyOfPropertyChange(() => ExpenseDataList);
            }
        }

        public Income SelectedIncome
        {
            get { return _selectedIncome; }
            set
            {
                _selectedIncome = value;
                NotifyOfPropertyChange(() => SelectedIncome);
            }
        }

        public Expense SelectedExpense
        {
            get { return _selectedExpense; }
            set
            {
                _selectedExpense = value;
                NotifyOfPropertyChange(() => SelectedExpense);
            }
        }

        public double IncomeTotal
        {
            get { return _incomeTotal; }
            set
            {
                _incomeTotal = value;
                NotifyOfPropertyChange(() => IncomeTotal);
            }
        }

        public double ExpenseTotal
        {
            get { return _expenseTotal; }
            set
            {
                _expenseTotal = value;
                NotifyOfPropertyChange(() => ExpenseTotal);
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
        #endregion
    }
}
