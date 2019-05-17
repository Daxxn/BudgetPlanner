using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BudgetPlannerLib.Models;
using Caliburn.Micro;

namespace BudgetPlannerMainWPF.ViewModels
{
    public class SubCategoryViewModel : Screen
    {
        #region - Fields
        private BindableCollection<SubCategory> _incomeCategories = new BindableCollection<SubCategory>();
        private BindableCollection<SubCategory> _expenseCategories = new BindableCollection<SubCategory>();

        private SubCategory _selectedIncomeCategory;
        private SubCategory _selectedExpenseCategory;

        private string _newIncomeName = String.Empty;
        private string _newExpenseName = String.Empty;
        #endregion

        #region - Constructors
        public SubCategoryViewModel()
        {
            IncomeCategories.Add(new SubCategory("Temp Income Category 1"));
            IncomeCategories.Add(new SubCategory("Temp Income Category 2"));
            IncomeCategories.Add(new SubCategory("Temp Income Category 3"));
            IncomeCategories.Add(new SubCategory("Should not be shown."));

            ExpenseCategories.Add(new SubCategory("Temp Expense Category 1"));
            ExpenseCategories.Add(new SubCategory("Temp Expense Category 2"));
            ExpenseCategories.Add(new SubCategory("Temp Expense Category 3"));
            ExpenseCategories.Add(new SubCategory("Should not be shown."));
        }
        #endregion

        #region - Methods

        #region -- Buttons
        public void AddIncomeCategory()
        {
            if(NewIncomeName == String.Empty)
            {
                IncomeCategories.Add(new SubCategory("Default"));
            }
            else
            {
                IncomeCategories.Add(new SubCategory(NewIncomeName));
            }
        }

        public void RemoveIncomeCategory()
        {
            IncomeCategories.Remove(SelectedIncomeCategory);
        }

        public void AddExpenseCategory()
        {
            if(NewExpenseName == String.Empty)
            {
                ExpenseCategories.Add(new SubCategory("Default"));
            }
            else
            {
                ExpenseCategories.Add(new SubCategory(NewExpenseName));
            }
        }

        public void RemoveExpenseCategory()
        {
            ExpenseCategories.Remove(SelectedExpenseCategory);
        }

        public void FinishCategories()
        {
            Income.AllIncomeCategories = IncomeCategories.ToList();
            Expense.AllExpenseCategories = ExpenseCategories.ToList();
        }
        #endregion
        #endregion

        #region - Properties
        public BindableCollection<SubCategory> IncomeCategories
        {
            get { return _incomeCategories; }
            set
            {
                _incomeCategories = value;
                NotifyOfPropertyChange(() => IncomeCategories);
            }
        }

        public BindableCollection<SubCategory> ExpenseCategories
        {
            get { return _expenseCategories; }
            set
            {
                _expenseCategories = value;
                NotifyOfPropertyChange(() => ExpenseCategories);
            }
        }

        public SubCategory SelectedIncomeCategory
        {
            get { return _selectedIncomeCategory; }
            set
            {
                _selectedIncomeCategory = value;
                NotifyOfPropertyChange(() => SelectedIncomeCategory);
            }
        }

        public SubCategory SelectedExpenseCategory
        {
            get { return _selectedExpenseCategory; }
            set
            {
                _selectedExpenseCategory = value;
                NotifyOfPropertyChange(() => SelectedExpenseCategory);
            }
        }

        public string NewIncomeName
        {
            get { return _newIncomeName; }
            set
            {
                _newIncomeName = value;
            }
        }

        public string NewExpenseName
        {
            get { return _newExpenseName; }
            set
            {
                _newExpenseName = value;
            }
        }
        #endregion
    }
}
