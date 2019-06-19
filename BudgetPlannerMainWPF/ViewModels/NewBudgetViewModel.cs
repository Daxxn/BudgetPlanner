using System;
using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using Microsoft.Win32;
using BudgetPlannerLib;
using BudgetPlannerLib.Models;
using System.Windows.Forms;
using System.Windows;

namespace BudgetPlannerMainWPF.ViewModels
{
    public class NewBudgetViewModel : Caliburn.Micro.Screen
    {
        #region - Fields
        private string _budgetName;
        private string _directoryPath;
        private bool _goodFolderPath;

        private FolderBrowserDialog _folderDialog;

        /// <summary>
        /// Sends Data From the NewBudgetViewModel to the ShellViewModel
        /// </summary>
        public event EventHandler<FolderEventArgs> CreatingNewBudget;
        #endregion

        #region - Constructors
        public NewBudgetViewModel()
        {

        }
        #endregion

        #region - Methods
        /// <summary>
        /// NewSavePath Button
        /// </summary>
        public void NewSavePath()
        {
            FolderDialog = new FolderBrowserDialog()
            {
                Description = "Select a path for the new project:",
                RootFolder = Environment.SpecialFolder.MyComputer
            };

            DialogResult result = FolderDialog.ShowDialog();

            if(result == DialogResult.OK)
            {
                DirectoryPath = FolderDialog.SelectedPath;
            }

            FolderDialog.Dispose();
        }

        /// <summary>
        /// CreatNewBudget Button
        /// </summary>
        public void CreateNewBudget()
        {
            CreatingNewBudget?.Invoke(this, new FolderEventArgs(DirectoryPath, BudgetName));
        }

        #region Private Methods
        /// <summary>
        /// Confirms the existance of the entered string
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private bool CheckDirectory(string input)
        {
            if (System.IO.Directory.Exists(input))
            {
                return true;
            }
            else return false;
        }
        #endregion
        #endregion

        #region - Properties
        /// <summary>
        /// Connected to the DirectoryPath CheckBox
        /// </summary>
        public bool GoodFolderPath
        {
            get { return _goodFolderPath; }
            set
            {
                _goodFolderPath = value;
                NotifyOfPropertyChange(() => GoodFolderPath);
            }
        }

        /// <summary>
        /// Connected to the BudgetName TextBox
        /// </summary>
        public string BudgetName
        {
            get { return _budgetName; }
            set
            {
                _budgetName = value;
                NotifyOfPropertyChange(() => BudgetName);
            }
        }

        /// <summary>
        /// Connected to the DirecoryPath TextBox
        /// </summary>
        public string DirectoryPath
        {
            get { return _directoryPath; }
            set
            {
                _directoryPath = value;

                if (CheckDirectory(value))
                {
                    GoodFolderPath = true;
                }
                else GoodFolderPath = false;

                NotifyOfPropertyChange(() => DirectoryPath);
            }
        }

        /// <summary>
        /// Folder Path Dialog Box
        /// </summary>
        public FolderBrowserDialog FolderDialog
        {
            get { return _folderDialog; }
            set
            {
                _folderDialog = value;
            }
        }
        #endregion
    }
}
