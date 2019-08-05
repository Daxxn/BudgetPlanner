using System;
using Caliburn.Micro;
using System.Windows.Forms;
using BudgetPlannerMainWPF.EventModels;
using MessageBox = System.Windows.MessageBox;

namespace BudgetPlannerMainWPF.ViewModels
{
    public class NewBudgetViewModel : Caliburn.Micro.Screen, IHandle<CancelNewEvent>
    {
        #region - Fields
        private IEventAggregator _eventAggregator;
        private IFileBrowser _fileBrowser;

        private string _budgetName = String.Empty;
        private string _directoryPath = String.Empty;
        private string _subCategoryPath = String.Empty;
        private bool _goodFolderPath = false;
        private bool _goodSubCatPath = false;
        #endregion

        #region - Constructors
        public NewBudgetViewModel() { }
        public NewBudgetViewModel(IEventAggregator eventAggregator, IFileBrowser fileBrowser)
        {
            _fileBrowser = fileBrowser;
            _eventAggregator = eventAggregator;
            _eventAggregator.Subscribe(this);
        }
        #endregion

        #region - Methods
        #region -- Private Methods

        #endregion

        #region -- Buttons
        /// <summary>
        /// NewSavePath Button
        /// </summary>
        public void NewSavePath()
        {
            DirectoryPath = _fileBrowser.OpenFolderAccess("Select Save Folder");
        }

        public void GetSubCatPath()
        {
            // Wrong FileDialog
            //SubCategoryPath = _fileBrowser.OpenFolderAccess("Select Sub-Category File");

            Tuple<string, bool> tempPath = _fileBrowser.OpenFileAccess(DirectoryPath, "Open Category File", false);

            if (tempPath.Item2)
            {
                SubCategoryPath = tempPath.Item1;
            }
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
                        if (SubCategoryPath == String.Empty)
                        {
                            _eventAggregator.PublishOnUIThread(new NewBudgetEvent(BudgetName, DirectoryPath, DirectoryPath, false));
                        }
                        else
                        {
                            if (GoodSubCatPath)
                            {
                                _eventAggregator.PublishOnUIThread(new NewBudgetEvent(BudgetName, DirectoryPath, SubCategoryPath, true));
                            }
                            else
                            {
                                MessageManager.DisplayMessage("Bad Sub-Category Path. If there's a check in the box to the right, it's a good path.", Error);
                            }
                        }
                    }
                    else
                    {
                        MessageManager.DisplayMessage("Bad Save Location. If there's a check in the box to the right, it's a good path.", Error);
                    }
                }
                else
                {
                    MessageManager.DisplayMessage("No Path Given.", Error);
                }
            }
            else
            {
                MessageManager.DisplayMessage("No Name Given.", Error);
            }
        }

        public void Handle(CancelNewEvent message)
        {
            string empty = String.Empty;
            BudgetName = empty;
            DirectoryPath = empty;
            SubCategoryPath = empty;
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

                if (FileCheck.CheckDirectory(value))
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

                if (FileCheck.CheckFile(value))
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