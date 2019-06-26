using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using BudgetPlannerLib;
using BudgetPlannerLib.Models;
using BudgetPlannerMainWPF.Views;
using Caliburn.Micro;

namespace BudgetPlannerMainWPF.ViewModels
{
    public class DataViewModel : Screen
    {
        #region - Fields
        private IEventAggregator _eventAggregator;

        private BindableCollection<Income> _incomeDataList;
        private BindableCollection<Expense> _expenseDataList;

        private Income _selectedIncome;
        private Expense _selectedExpense;

        private decimal _incomeTotal;
        private decimal _expenseTotal;

        private decimal _netDifference;
        private string _netNegative;

        private BindableCollection<SubCategory> _incomeSubCategoryDisplay;
        private BindableCollection<SubCategory> _expenseSubCategoryDisplay;
        
        #endregion

        #region - Constructors
        /// <summary>
        /// Base Constructor, Subscribes to needed Events.
        /// </summary>
        public DataViewModel()
        {
            DataElement.ValueChanged += this.DataElement_ValueChanged;
            DataView.SendEnter += this.DataView_SendEnter;
        }

        public DataViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.Subscribe(this);
        }

        public DataViewModel(BindableCollection<Income> incomes, BindableCollection<Expense> expenses)
        {
            IncomeDataList = incomes;
            ExpenseDataList = expenses;
        }
        #endregion

        #region - Methods
        /// <summary>
        /// Runs during OnStartUp()
        /// </summary>
        public void Initialize()
        {
            IncomeDataList = new BindableCollection<Income>();
            ExpenseDataList = new BindableCollection<Expense>();
        }

        /// <summary>
        /// Triggers the SendEnter Event for confirming changes to the DataList.
        /// </summary>
        private void DataView_SendEnter(Object sender, SimpleKeyEventAgrs e)
        {
            if(e.SenderId == 1)
            {
                UpdateIncome();
            }
            else if(e.SenderId == 2)
            {
                UpdateExpense();
            }
        }

        /// <summary>
        /// Pulls data from the TestDataAccesser. For Testing Purposes.
        /// </summary>
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

        /// <summary>
        /// Clears all the data from the DataLists and SubCategories.
        /// </summary>
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

        /// <summary>
        /// Sums both Income & Expense DataLists. Casts Income & Expense SubCategory Lists to BindableCollections.
        /// </summary>
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
                    IncomeSubCategoryDisplay = 
                        new BindableCollection<SubCategory>(Income.AllIncomeCategories);

                    ExpenseSubCategoryDisplay = 
                        new BindableCollection<SubCategory>(Expense.AllExpenseCategories);
                }
            }
        }

        /// <summary>
        /// Sums the Income DataList Values. Sorts all Income SubCategories.
        /// </summary>
        public void UpdateIncome()
        {
            IncomeTotal = IncomeDataList.Sum(x => x.Value);
            NetDifference = IncomeTotal - ExpenseTotal;

            if(Income.AllIncomeCategories != null)
            {
                Income.AllIncomeCategories = 
                    SortCategories(IncomeDataList.ToList(), Income.AllIncomeCategories);
            }
        }

        /// <summary>
        /// Sums the Expense DataList Values. Sorts all Expense SubCategories.
        /// </summary>
        public void UpdateExpense()
        {
            ExpenseTotal = ExpenseDataList.Sum(x => x.Value);
            NetDifference = IncomeTotal - ExpenseTotal;

            if(Expense.AllExpenseCategories != null)
            {
                Expense.AllExpenseCategories = 
                    SortCategories(ExpenseDataList.ToList(), Expense.AllExpenseCategories);
            }
        }

        /// <summary>
        /// Checks for a negative Net Difference, Sets NetNegative TextBox.
        /// </summary>
        /// <param name="valueIn">NetDifference Value</param>
        public void ConvertNetDifference(decimal valueIn)
        {
            if (valueIn < 0)
            {
                NetNegative = "-";
            }
            else
            {
                NetNegative = String.Empty;
            }
        }

        #region Sorting Categories and summing Total
        /// <summary>
        /// Sums AllSubCategories. Replaces the old list with a new one.
        /// </summary>
        public void SortCategories()
        {
            if (Income.AllIncomeCategories.Count > 0)
            {
                Income.AllIncomeCategories =
                        SortCategories(IncomeDataList.ToList(), Income.AllIncomeCategories);
            }
            
            if (Expense.AllExpenseCategories.Count > 0)
            {
                Expense.AllExpenseCategories =
                        SortCategories(ExpenseDataList.ToList(), Expense.AllExpenseCategories);
            }
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
        /// Places all occurences of a SubCategory into a new list and returns the totals.
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
        /// <summary>
        /// Connected to the Income DataGrid
        /// </summary>
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

        /// <summary>
        /// Connected to the Expense DataGrid
        /// </summary>
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

        /// <summary>
        /// Connected to the selected Income Item in the Income DataGrid
        /// </summary>
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

        /// <summary>
        /// Connected to the selected Expense Item in the Expense DataGrid
        /// </summary>
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

        /// <summary>
        /// Connected to the IncomeTotal TextBlock
        /// </summary>
        public decimal IncomeTotal
        {
            get { return _incomeTotal; }
            set
            {
                _incomeTotal = value;
                NotifyOfPropertyChange(() => IncomeTotal);
            }
        }

        /// <summary>
        /// Connected to the ExpenseTotal TextBlock
        /// </summary>
        public decimal ExpenseTotal
        {
            get { return _expenseTotal; }
            set
            {
                _expenseTotal = value;
                NotifyOfPropertyChange(() => ExpenseTotal);
            }
        }

        /// <summary>
        /// Connected to the NetDifference TextBlock
        /// </summary>
        public decimal NetDifference
        {
            get { return _netDifference; }
            set
            {
                ConvertNetDifference(value);
                _netDifference = Math.Abs(value);
                NotifyOfPropertyChange(() => NetDifference);
            }
        }

        /// <summary>
        /// Connected to the NetNegative TextBlock,
        /// Shows a Minus(-) if the NetDifference is negative
        /// </summary>
        public string NetNegative
        {
            get { return _netNegative; }
            set
            {
                _netNegative = value;
                NotifyOfPropertyChange(() => NetNegative);
            }
        }

        /// <summary>
        /// Connected to Income.AllSubCategories & the IncomeSubCategory DataGrid
        /// </summary>
        public BindableCollection<SubCategory> IncomeSubCategoryDisplay
        {
            get { return _incomeSubCategoryDisplay; }
            set
            {
                _incomeSubCategoryDisplay = value;
                NotifyOfPropertyChange(() => IncomeSubCategoryDisplay);
            }
        }

        /// <summary>
        /// Connected to Expense.AllSubCategories & the ExpenseSubCategory DataGrid
        /// </summary>
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
