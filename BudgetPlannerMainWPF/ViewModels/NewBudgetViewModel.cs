using System;
using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using Microsoft.Win32;
using BudgetPlannerLib;
using BudgetPlannerLib.Models;
using System.Windows.Forms;
using System.Windows;
using MessageBox = System.Windows.MessageBox;

namespace BudgetPlannerMainWPF.ViewModels
{
    public class NewBudgetViewModel : Caliburn.Micro.Screen
    {
        #region - Fields
        private IEventAggregator _eventAggregator;

        private string _budgetName = String.Empty;
        private string _directoryPath = String.Empty;
        private string _subCategoryPath = String.Empty;
        private bool _goodFolderPath = false;
        private bool _goodSubCatPath = false;

        /// <summary>
        /// Sends Data From the NewBudgetViewModel to the ShellViewModel
        /// </summary>
        public event EventHandler<FolderEventArgs> CreatingNewBudget;
        #endregion

        #region - Constructors
        public NewBudgetViewModel()
        {
            ShellViewModel.CancellingNewBudget += this.CancelNewBudget;
        }
        public NewBudgetViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.Subscribe(this);
        }

        private void CancelNewBudget(Object sender, EventArgs e)
        {
            string empty = String.Empty;
            BudgetName = empty;
            DirectoryPath = empty;
            SubCategoryPath = empty;
        }
        #endregion

        #region - Methods
        #region -- Private Methods
        public static string OpenFolderDialog(string description)
        {
            string output = "";
            FolderBrowserDialog folderDialog = new FolderBrowserDialog()
            {
                Description = description,
                RootFolder = Environment.SpecialFolder.MyComputer
            };

            DialogResult result = folderDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                output = folderDialog.SelectedPath;
            }
            else
            {
                output = "Something went wrong!!";
            }

            folderDialog.Dispose();
            return output;
        }
        #endregion
        #region -- Buttons
        /// <summary>
        /// NewSavePath Button
        /// </summary>
        public void NewSavePath()
        {
            DirectoryPath = OpenFolderDialog("Select Save Folder");
        }

        public void GetSubCatPath()
        {
            SubCategoryPath = OpenFolderDialog("Select Sub-Category File");
        }

        /// <summary>
        /// CreatNewBudget Button
        /// </summary>
        public void CreateNewBudget()
        {
            string Error = "Oops!";
            if(BudgetName != String.Empty)
            {
                if(DirectoryPath != String.Empty)
                {
                    if (GoodFolderPath)
                    {
                        if (SubCategoryPath == "")
                        {
                            CreatingNewBudget?.Invoke(this, new FolderEventArgs(DirectoryPath, BudgetName, DirectoryPath));
                        }
                        else
                        {
                            if (GoodSubCatPath)
                            {
                                CreatingNewBudget?.Invoke(this, new FolderEventArgs(DirectoryPath, BudgetName, SubCategoryPath, true));
                            }
                            else
                            {
                                MessageBox.Show("Bad Sub-Category Path. If there's a check in the box to the right, it's a good path.", Error);
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Bad Save Location. If there's a check in the box to the right, it's a good path.", Error);
                    }
                }
                else
                {
                    MessageBox.Show("No Path Given.", Error);
                }
            }
            else
            {
                MessageBox.Show("No Name Given.", Error);
            }
        }
        #endregion

        #region -- Private Methods

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

                if (ShellViewModel.CheckDirectory(value))
                {
                    GoodFolderPath = true;
                }
                else GoodFolderPath = false;

                NotifyOfPropertyChange(() => DirectoryPath);
            }
        }

        public string SubCategoryPath
        {
            get { return _subCategoryPath; }
            set
            {
                _subCategoryPath = value;

                if (ShellViewModel.CheckDirectory(value))
                {
                    GoodSubCatPath = true;
                }
                else GoodSubCatPath = false;

                NotifyOfPropertyChange(() => SubCategoryPath);
            }
        }

        public bool GoodSubCatPath
        {
            get { return _goodSubCatPath; }
            set
            {
                _goodSubCatPath = value;
                NotifyOfPropertyChange(() => GoodSubCatPath);
            }
        }
        #endregion
    }
}
