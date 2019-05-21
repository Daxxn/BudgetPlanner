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

        private decimal _incomeTotal;
        private decimal _expenseTotal;

        private decimal _netDifference;

        private BindableCollection<SubCategory> _incomeSubCategoryDisplay;
        private BindableCollection<SubCategory> _expenseSubCategoryDisplay;
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

        public void ClearData()
        {
            IncomeDataList.Clear();
            ExpenseDataList.Clear();

            SelectedIncome = new Income();
            SelectedExpense = new Expense();

            IncomeTotal = 0;
            ExpenseTotal = 0;
            NetDifference = 0;

            IncomeSubCategoryDisplay.Clear();
            ExpenseSubCategoryDisplay.Clear();
        }

        public void UpdateData()
        {
            if (ExpenseDataList != null)
            {
                IncomeTotal = IncomeDataList.Sum(x => x.Value);
                ExpenseTotal = ExpenseDataList.Sum(x => x.Value); 
                NetDifference = IncomeTotal - ExpenseTotal;

                if (Income.AllIncomeCategories != null && Expense.AllExpenseCategories != null)
                {
                    SortCategories();
                    IncomeSubCategoryDisplay = new BindableCollection<SubCategory>(Income.AllIncomeCategories);
                    ExpenseSubCategoryDisplay = new BindableCollection<SubCategory>(Expense.AllExpenseCategories);
                }
            }
        }

        #region Sorting Categories and summing Total
        /// <summary>
        /// Sums AllSubCategories. Replaces the old list with a new one.
        /// </summary>
        public void SortCategories()
        {
            Income.AllIncomeCategories = SortCategories(IncomeDataList.ToList(), Income.AllIncomeCategories);
            Expense.AllExpenseCategories = SortCategories(ExpenseDataList.ToList(), Expense.AllExpenseCategories);
        }
        
        /// <summary>
        /// Places all occurences of the SubCategory into a new list and returns the totals.
        /// </summary>
        /// <param name="dataList">Income DataList</param>
        /// <param name="AllsubCategories">Static Income SubCategories</param>
        /// <returns>New SubCategory List With Totals</returns>
        public List<SubCategory> SortCategories(List<Income> dataList, List<SubCategory> AllsubCategories)
        {
            List<SubCategory> tempCategories = new List<SubCategory>();

            foreach (var subCategory in AllsubCategories)
            {
                List<decimal> tempSum = new List<decimal>();

                foreach (var selected in dataList)
                {
                    if (subCategory.Name == selected.SelectedCategory.Name)
                    {
                        tempSum.Add(selected.Value);
                    }
                }

                if (tempSum.Count > 0)
                {
                    subCategory.Value = tempSum.Sum();
                }
                else { subCategory.Value = 0; }

                tempCategories.Add(subCategory);
            }

            return tempCategories;
        }

        /// <summary>
        /// Places all occurences of the SubCategory into a new list and returns the totals.
        /// </summary>
        /// <param name="dataList">Income DataList</param>
        /// <param name="AllsubCategories">Static Income SubCategories</param>
        /// <returns>New SubCategory List With Totals</returns>
        public List<SubCategory> SortCategories(List<Expense> dataList, List<SubCategory> AllsubCategories)
        {
            // Final list to replace the old static list.
            List<SubCategory> tempCategories = new List<SubCategory>();

            // Loops through the static SubCategory list.
            foreach (var subCategory in AllsubCategories)
            {
                // Sums all occurences of the SubCategory in the DataList.
                List<decimal> tempSum = new List<decimal>();

                // mathces all occurences of subCategory and adds to tempSum.
                foreach (var selected in dataList)
                {
                    if (subCategory.Name == selected.SelectedCategory.Name)
                    {
                        tempSum.Add(selected.Value);
                    }
                }

                // Checks for an empty List.
                // if not epmty, Sums the list and throws it into subCategory.Value.
                if (tempSum.Count > 0)
                {
                    subCategory.Value = tempSum.Sum();
                }
                else { subCategory.Value = 0; }

                // Finally adds the SubCategory to the main list, repeat.
                tempCategories.Add(subCategory);
            }

            return tempCategories;
        }
        #endregion
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
                UpdateData();
                NotifyOfPropertyChange(() => SelectedIncome);
            }
        }

        public Expense SelectedExpense
        {
            get { return _selectedExpense; }
            set
            {
                _selectedExpense = value;
                UpdateData();
                NotifyOfPropertyChange(() => SelectedExpense);
            }
        }

        public decimal IncomeTotal
        {
            get { return _incomeTotal; }
            set
            {
                _incomeTotal = value;
                NotifyOfPropertyChange(() => IncomeTotal);
            }
        }

        public decimal ExpenseTotal
        {
            get { return _expenseTotal; }
            set
            {
                _expenseTotal = value;
                NotifyOfPropertyChange(() => ExpenseTotal);
            }
        }

        public decimal NetDifference
        {
            get { return _netDifference; }
            set
            {
                _netDifference = value;
                NotifyOfPropertyChange(() => NetDifference);
            }
        }

        public BindableCollection<SubCategory> IncomeSubCategoryDisplay
        {
            get { return _incomeSubCategoryDisplay; }
            set
            {
                _incomeSubCategoryDisplay = value;
                NotifyOfPropertyChange(() => IncomeSubCategoryDisplay);
            }
        }

        public BindableCollection<SubCategory> ExpenseSubCategoryDisplay
        {
            get { return _expenseSubCategoryDisplay; }
            set
            {
                _expenseSubCategoryDisplay = value;
                NotifyOfPropertyChange(() => ExpenseSubCategoryDisplay);
            }
        }
        #endregion
    }
}
