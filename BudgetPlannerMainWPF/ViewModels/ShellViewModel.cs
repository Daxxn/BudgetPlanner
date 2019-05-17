﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows;
using BudgetPlannerLib;
using BudgetPlannerLib.Models;
using Caliburn.Micro;
using Microsoft.Win32;
using BudgetPlannerMainWPF.Views;

namespace BudgetPlannerMainWPF.ViewModels
{
    public class ShellViewModel : Conductor<object>
    {
        #region - Fields
        private FileConrol _saveControl;
        private FileConrol _openControl;

        public string FilePath { get; set; }

        private DataViewModel _dataViewModel = new DataViewModel();
        private SubCategoryViewModel _categoryViewModel = new SubCategoryViewModel();
        #endregion

        #region - Constructors
        public ShellViewModel()
        {
            DataViewModel.AddStaticCategories();

            ActivateItem(DataViewModel);
        }
        #endregion

        #region - Methods
        #region Add/Remove
        public void AddIncomeColumn()
        {
            DataViewModel.IncomeDataList.Add(new Income("", 0, DataViewModel.IncomeDataList.Count));
        }

        public void AddExpenseColumn()
        {
            DataViewModel.ExpenseDataList.Add(new Expense("New Expense", 0.0, DataViewModel.ExpenseDataList.Count));
        }

        public void RemoveIncome()
        {
            DataViewModel.IncomeDataList.Remove(DataViewModel.SelectedIncome);
            DataViewModel.SelectedIncome = null;
        }

        public void RemoveExpense()
        {
            DataViewModel.ExpenseDataList.Remove(DataViewModel.SelectedExpense);
            DataViewModel.SelectedExpense = null;
        }
        #endregion

        #region SubCategories
        public void ViewData()
        {
            ActivateItem(DataViewModel);
        }

        public void ViewSubCategories()
        {
            ActivateItem(SubCategoryViewModel);
            SendCategories();
        }

        public void SendCategories()
        {
            SubCategoryViewModel.IncomeCategories = new BindableCollection<SubCategory>(Income.AllIncomeCategories);
            SubCategoryViewModel.ExpenseCategories = new BindableCollection<SubCategory>(Expense.AllExpenseCategories);
        }

        public void RetrieveCategories()
        {
            Income.AllIncomeCategories = SubCategoryViewModel.IncomeCategories.ToList();
            Expense.AllExpenseCategories = SubCategoryViewModel.ExpenseCategories.ToList();
        }
        #endregion

        public void OpenFile()
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Multiselect = false;
            openFile.Title = "Open Budget Plan";
            if(openFile.ShowDialog() == true)
            {
                FilePath = openFile.FileName;
                OpenController = new FileConrol(openFile.FileName);
                OpenController.OpenFile();

                //IncomeData = new BindableCollection<Income>(OpenController.IncomeData);
                //ExpenseData = new BindableCollection<Expense>(OpenController.ExpenseData);
            }
        }

        public void SaveFile()
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Title = "Save Budget Plan";
            saveFile.OverwritePrompt = true;
            saveFile.AddExtension = true;
            saveFile.DefaultExt = ".txt";

            if(saveFile.ShowDialog() == true)
            {
                //SaveController = new FileConrol(saveFile.FileName, IncomeData.ToList(), ExpenseData.ToList());
                SaveController.SaveFile();
            }
        }

        public static void Exit()
        {
            DataElement.Exit();
        }
        #endregion

        #region - Properties
        public FileConrol SaveController
        {
            get { return _saveControl; }
            set
            {
                _saveControl = value;
            }
        }

        public FileConrol OpenController
        {
            get { return _openControl; }
            set
            {
                _openControl = value;
            }
        }

        public DataViewModel DataViewModel
        {
            get { return _dataViewModel; }
            set
            {
                _dataViewModel = value;
            }
        }

        public SubCategoryViewModel SubCategoryViewModel
        {
            get { return _categoryViewModel; }
            set
            {
                _categoryViewModel = value;
            }
        }
        #endregion
    }
}
