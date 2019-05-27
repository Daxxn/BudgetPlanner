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
        /// <summary>
        /// Erases all current data to set up for a new file.
        /// </summary>
        public void NewFile()
        {
            MessageBoxResult contin = MessageBox.Show("Are you sure? This will erase all unsaved data.", "New File", MessageBoxButton.OKCancel);
            if (contin == MessageBoxResult.OK)
            {
                ClearData();
            }
        }

        /// <summary>
        /// Clears all data from the DataLists and SubCategory Lists.
        /// </summary>
        private void ClearData()
        {
            DataViewModel.ClearData();
            SubCategoryViewModel.ClearData();
            Income.ClearData();
            Expense.ClearData();
        }

        #region Add/Remove
        /// <summary>
        /// Adds a new Income Column with Default Data.
        /// </summary>
        public void AddIncomeColumn()
        {
            DataViewModel.IncomeDataList.Add(new Income("Default", "New Income", 0.0M, DataViewModel.IncomeDataList.Count + 1));
        }

        /// <summary>
        /// Adds a new Expense Column with Default Data.
        /// </summary>
        public void AddExpenseColumn()
        {
            DataViewModel.ExpenseDataList.Add(new Expense("default", "New Expense", 0.0M, DataViewModel.ExpenseDataList.Count + 1));
        }

        /// <summary>
        /// Removes the selected Income Column.
        /// </summary>
        public void RemoveIncome()
        {
            DataViewModel.IncomeDataList.Remove(DataViewModel.SelectedIncome);
            DataViewModel.SelectedIncome = null;
        }

        /// <summary>
        /// Removes the selected Expense Column.
        /// </summary>
        public void RemoveExpense()
        {
            DataViewModel.ExpenseDataList.Remove(DataViewModel.SelectedExpense);
            DataViewModel.SelectedExpense = null;
        }
        #endregion

        #region SubCategories
        /// <summary>
        /// Switches to DataView.xaml. Updates the SubCategory Data.
        /// </summary>
        public void ViewData()
        {
            ActivateItem(DataViewModel);
            DataViewModel.SortCategories();
            SubCategoryViewModel.FinishCategories();
            DataViewModel.UpdateData();
        }

        /// <summary>
        /// Switches to SubCategoryView.xaml. Sends the SubCategories.
        /// </summary>
        public void ViewSubCategories()
        {
            ActivateItem(SubCategoryViewModel);
            SendCategories();
        }

        /// <summary>
        /// Casts SubCategories to a BindableCollection for sending data.
        /// </summary>
        public void SendCategories()
        {
            SubCategoryViewModel.IncomeCategories = new BindableCollection<SubCategory>(Income.AllIncomeCategories);
            SubCategoryViewModel.ExpenseCategories = new BindableCollection<SubCategory>(Expense.AllExpenseCategories);
        }

        /// <summary>
        /// Casts SubCategories to a List for data return.
        /// </summary>
        public void RetrieveCategories()
        {
            Income.AllIncomeCategories = SubCategoryViewModel.IncomeCategories.ToList();
            Expense.AllExpenseCategories = SubCategoryViewModel.ExpenseCategories.ToList();
        }
        #endregion

        /// <summary>
        /// Opens the OpenFileDialog and creates a new FileControl Instance.
        /// </summary>
        public void OpenFile()
        {
            OpenFileDialog openFile = new OpenFileDialog
            {
                Multiselect = false,
                Title = "Open Budget Plan",
                DefaultExt = ".bpn"
            };

            if (openFile.ShowDialog() == true)
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

        /// <summary>
        /// Opens the SaveFileDialog and creates a new FileControl Instance.
        /// </summary>
        public void SaveFile()
        {
            SaveFileDialog saveFile = new SaveFileDialog
            {
                Title = "Save Budget Plan",
                OverwritePrompt = true,
                AddExtension = true,
                DefaultExt = ".bpn"
            };

            if (saveFile.ShowDialog() == true)
            {
                SaveController = new FileConrol(saveFile.FileName, DataViewModel.IncomeDataList.ToList(), DataViewModel.ExpenseDataList.ToList(), Income.AllIncomeCategories, Expense.AllExpenseCategories);
                SaveController.SaveFile();
            }
        }

        /// <summary>
        /// Called on close. Unsusbscribes from events.
        /// </summary>
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
