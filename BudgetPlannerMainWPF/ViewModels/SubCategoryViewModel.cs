﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using BudgetPlannerLib.Models;
using BudgetPlannerMainWPF.Views;
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
            #region Test Data. Should get replaced.
            IncomeCategories.Add(new SubCategory("Temp Income Category 1"));
            IncomeCategories.Add(new SubCategory("Temp Income Category 2"));
            IncomeCategories.Add(new SubCategory("Temp Income Category 3"));
            IncomeCategories.Add(new SubCategory("Should not be shown."));

            ExpenseCategories.Add(new SubCategory("Temp Expense Category 1"));
            ExpenseCategories.Add(new SubCategory("Temp Expense Category 2"));
            ExpenseCategories.Add(new SubCategory("Temp Expense Category 3"));
            ExpenseCategories.Add(new SubCategory("Should not be shown."));
            #endregion

            SubCategoryView.SendEnter += this.SubCategoryView_SendKeyPress;
        }

        #endregion

        #region - Methods
        private void SubCategoryView_SendKeyPress(Object sender, SimpleKeyEventAgrs e)
        {
            if (e.SenderId == 1)
            {
                AddIncomeCategory();
            }
            else if(e.SenderId == 2)
            {
                AddExpenseCategory();
            }
        }

        public void ClearData()
        {
            IncomeCategories.Clear();
            ExpenseCategories.Clear();

            SelectedIncomeCategory = new SubCategory();
            SelectedExpenseCategory = new SubCategory();
        }

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
                NewIncomeName = String.Empty;
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
                NewExpenseName = String.Empty;
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
                NotifyOfPropertyChange(() => NewIncomeName);
            }
        }

        public string NewExpenseName
        {
            get { return _newExpenseName; }
            set
            {
                _newExpenseName = value;
                NotifyOfPropertyChange(() => NewExpenseName);
            }
        }
        #endregion
    }
}
