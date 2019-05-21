using System;
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

            DataViewModel.SortCategories();
        }

        #endregion

        #region - Methods
        public void NewFile()
        {
            MessageBoxResult contin = MessageBox.Show("Are you sure? This will erase all unsaved data.", "New File", MessageBoxButton.OKCancel);
            if (contin == MessageBoxResult.OK)
            {
                ClearData();
            }
        }

        private void ClearData()
        {
            DataViewModel.ClearData();
            SubCategoryViewModel.ClearData();
            Income.ClearData();
            Expense.ClearData();
        }

        #region Add/Remove
        public void AddIncomeColumn()
        {
            DataViewModel.IncomeDataList.Add(new Income("Default", "New Income", 0.0, DataViewModel.IncomeDataList.Count + 1));
        }

        public void AddExpenseColumn()
        {
            DataViewModel.ExpenseDataList.Add(new Expense("default", "New Expense", 0.0, DataViewModel.ExpenseDataList.Count + 1));
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
            DataViewModel.SortCategories();
            SubCategoryViewModel.FinishCategories();
            DataViewModel.UpdateData();
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
            openFile.DefaultExt = ".bpn";
            openFile.Filter = ".bpn";
            if(openFile.ShowDialog() == true)
            {
                FilePath = openFile.FileName;
                OpenController = new FileConrol(openFile.FileName);
                OpenController.OpenFile();

                DataViewModel.IncomeDataList = new BindableCollection<Income>(OpenController.IncomeData);
                DataViewModel.ExpenseDataList = new BindableCollection<Expense>(OpenController.ExpenseData);
                Income.AllIncomeCategories = new List<SubCategory>(OpenController.IncomeSubCateories);
                Expense.AllExpenseCategories = new List<SubCategory>(OpenController.ExpenseSubCategories);
            }
        }

        public void SaveFile()
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Title = "Save Budget Plan";
            saveFile.OverwritePrompt = true;
            saveFile.AddExtension = true;
            saveFile.DefaultExt = ".bpn";
            saveFile.Filter = ".bpn";

            if(saveFile.ShowDialog() == true)
            {
                SaveController = new FileConrol(saveFile.FileName, DataViewModel.IncomeDataList.ToList(), DataViewModel.ExpenseDataList.ToList(), Income.AllIncomeCategories, Expense.AllExpenseCategories);
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
