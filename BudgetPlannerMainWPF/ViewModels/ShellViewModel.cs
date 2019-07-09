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
using BudgetPlannerMainWPF.EventModels;
using XMLParsingLibrary;

namespace BudgetPlannerMainWPF.ViewModels
{
    public class ShellViewModel : Conductor<object>, IHandle<NewBudgetEvent>, IHandle<SaveSubCategoryEvent>
    {
        #region - Fields
        private IEventAggregator _eventAggregator;
        private IFileBrowser _fileBrowser;

        private string _WindowTitle = "Budget Planner";
        private string _budgetName = String.Empty;
        private string _budgetDir = String.Empty;
        private string _SubCatDir = String.Empty;
        private string _subCatFileName = String.Empty;

        private bool isFileOpen = false;
        private bool lastScreenIsNF = false;

        private FileControl _saveControl;
        private FileControl _openControl;
        
        public string FileName { get; set; }

        private DataViewModel _dataViewModel;
        private SubCategoryViewModel _categoryViewModel = new SubCategoryViewModel();
        private NewBudgetViewModel _newBudgetViewModel = new NewBudgetViewModel();

        public static event EventHandler CancellingNewBudget;
        #endregion

        #region - Constructors
        public ShellViewModel()
        {
            // Adds basic test data:
            //DataViewModel.AddStaticCategories();
            InitializeAll();

            ActivateItem(NewBudgetViewModel);
            lastScreenIsNF = true;

            DataViewModel.SortCategories();
            
            SubCategoryViewModel.SubCatEventManager += this.SubCatEventManager_Event;
        }
        public ShellViewModel(IEventAggregator eventAggregator, IFileBrowser fileBrowser)
        {
            _fileBrowser = fileBrowser;
            _eventAggregator = eventAggregator;
            _eventAggregator.Subscribe(this);

            DataViewModel = new DataViewModel(eventAggregator);
            SubCategoryViewModel = new SubCategoryViewModel(eventAggregator);
            NewBudgetViewModel = new NewBudgetViewModel(eventAggregator, fileBrowser);

            InitializeAll();

            ActivateItem(NewBudgetViewModel);
            lastScreenIsNF = true;

            //DataViewModel.SortCategories();
            _eventAggregator.PublishOnUIThread(new UpdateDataListEvent());
        }
        #endregion

        #region - Methods

        #region -- Access FileBrowser

        #endregion
        private void SubCatEventManager_Event(Object sender, SubCategoryEventArgs e)
        {
            if (e.SaveCode == 0)
            {
                SaveSubsAs();
            }
            else if(e.SaveCode == 1)
            {
                SaveSubs();
            }
            else if(e.SaveCode == 2)
            {
                OpenSubs();
            }
            else if(e.SaveCode == 3)
            {
                SubCategoryDirectory = e.SubCatDirectory;
                SubCatFileName = e.SubCatFileName;
            }
        }

        /// <summary>
        /// Confirms the existance of the entered string
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool CheckDirectory(string input)
        {
            if (System.IO.Directory.Exists(input))
            {
                return true;
            }
            else return false;
        }

        #region -- Private Methods
        private void CreatingNewBudget_Event(object sender, FolderEventArgs e)
        {
            if (!e.OpenSubCategories)
            {
                CreateNewBudget(e.BudgetName, e.FolderPath, e.SubCatPath);
            }
            else if (e.OpenSubCategories)
            {
                CreateNewBudget(e.BudgetName, e.FolderPath, e.SubCatPath, e.OpenSubCategories);
            }
        }


        /// <summary>
        /// Creates a new budget, clears the old budget and directs a new path.
        /// </summary>
        /// <param name="budgetname">Name from CreatingNewBudget Event</param>
        /// <param name="budgetDir">Directory from CreatingNewBudget Event</param>
        private void CreateNewBudget(string budgetName, string budgetDir, string subDir)
        {
            if (!isFileOpen)
            {
                BudgetName = budgetName;
                WindowTitle = $"Budget Planner - {budgetName}";
                BudgetDirectory = budgetDir;
                SubCategoryDirectory = subDir;
                Activate_DataView();
            }
            else
            {
                MessageBoxResult contin = MessageBox.Show(
                        "Are you sure? This will erase any unsaved data.",
                        "New File", MessageBoxButton.OKCancel);

                if (contin == MessageBoxResult.OK)
                {
                    ClearData();
                    BudgetName = budgetName;
                    WindowTitle = $"Budget Planner - {budgetName}";
                    BudgetDirectory = budgetDir;
                    SubCategoryDirectory = subDir;
                    Activate_DataView();
                    isFileOpen = true;
                }
                else if (contin == MessageBoxResult.Cancel)
                {
                    //CancellingNewBudget?.Invoke(this, new EventArgs());
                    _eventAggregator.PublishOnUIThread(new CancelNewEvent());
                    Activate_DataView();
                }
            }
        }

        private void CreateNewBudget(string budgetName, string budgetDir, string subDir, bool openSubs)
        {
            if (!isFileOpen)
            {
                if (openSubs)
                {
                    // Need to fix. The IF statements drop without doing anything.
                    BudgetName = budgetName;
                    WindowTitle = $"Budget Planner - {budgetName}";
                    BudgetDirectory = budgetDir;
                    SubCategoryDirectory = subDir;
                    OpenSubs();
                    Activate_DataView();
                }
                else if (!openSubs)
                {
                    BudgetName = budgetName;
                    WindowTitle = $"Budget Planner - {budgetName}";
                    BudgetDirectory = budgetDir;
                    SubCategoryDirectory = subDir;
                    Activate_DataView();
                }
            }
            else
            {
                MessageBoxResult contin = MessageBox.Show(
                        "Are you sure? This will erase any unsaved data.",
                        "New File", MessageBoxButton.OKCancel);

                if (contin == MessageBoxResult.OK)
                {
                    ClearData();
                    BudgetName = budgetName;
                    WindowTitle = $"Budget Planner - {budgetName}";
                    BudgetDirectory = budgetDir;
                    SubCategoryDirectory = subDir;
                    Activate_DataView();
                    isFileOpen = true;
                }
                else if (contin == MessageBoxResult.Cancel)
                {
                    _eventAggregator.PublishOnUIThread(new CancelNewEvent());
                    Activate_DataView();
                }
            }

        }
        #endregion
        public void InitializeAll()
        {
            DataViewModel.Initialize();
            SubCategoryViewModel.Initialize();
        }

        /// <summary>
        /// Erases all current data to set up for a new file.
        /// </summary>
        public void NewFile()
        {
            ActivateItem(NewBudgetViewModel);
            lastScreenIsNF = true;

            if (isFileOpen)
            {
                MessageBoxResult save = MessageBox.Show("Save before creating new budget?", "Save??", MessageBoxButton.YesNo);
                if (save == MessageBoxResult.Yes)
                {
                    SaveFileAs();
                    SaveSubsAs();
                }
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
            WindowTitle = "Budget Planner";
            BudgetName = String.Empty;
            BudgetDirectory = String.Empty;
            SubCategoryDirectory = String.Empty;
        }

        #region Add/Remove Data Controls
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

        #region -- Views
        /// <summary>
        /// Switches to DataView.xaml. Updates the SubCategory Data.
        /// </summary>
        public void ViewData()
        {
            if (lastScreenIsNF)
            {
                _eventAggregator.PublishOnUIThread(new CancelNewEvent());
                //CancellingNewBudget?.Invoke(this, new EventArgs());
            }
            Activate_DataView();
            DataViewModel.SortCategories();
            SubCategoryViewModel.FinishCategories();
            DataViewModel.UpdateData();
        }

        /// <summary>
        /// Switches to SubCategoryView.xaml. Sends the SubCategories.
        /// </summary>
        public void ViewSubCategories()
        {
            if (lastScreenIsNF)
            {
                _eventAggregator.PublishOnUIThread(new CancelNewEvent());
                //CancellingNewBudget?.Invoke(this, new EventArgs());
            }
            Activate_SubCategoryView();
            if(SubCategoryDirectory != null)
            {
                SubCategoryViewModel.SubCategoryPath = SubCategoryDirectory;
            }
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

        #region -- ViewControl
        public void Activate_DataView()
        {
            _eventAggregator.PublishOnUIThread(new UpdateDataListEvent(
                SubCategoryViewModel.IncomeCategories,
                SubCategoryViewModel.ExpenseCategories
                ));

            ActivateItem(DataViewModel);
        }

        public void Activate_SubCategoryView()
        {
            _eventAggregator.PublishOnUIThread(new UpdateSubCatEvent(
                Income.AllIncomeCategories, 
                Expense.AllExpenseCategories
                ));

            ActivateItem(SubCategoryViewModel);
        }
        #endregion
        #endregion

        #region -- File Management
        /// <summary>
        /// Opens the OpenFileDialog. Pulls data from a full .bpn file.
        /// </summary>
        //public void OpenFile()
        //{
        //    string tempBudgetName = "";
        //    List<Income> tempIncomeData = new List<Income>();
        //    List<Expense> tempExpenseData = new List<Expense>();

        //    string selectedFile = _fileBrowser.OpenFileAccess(SubCategoryDirectory, "Open Budget Plan", true);

        //    FileControl_2.OpenMainFile(
        //            selectedFile, 
        //            out tempBudgetName,
        //            out tempIncomeData,
        //            out tempExpenseData
        //            );

        //    BudgetName = tempBudgetName;
        //    DataViewModel.IncomeDataList = new BindableCollection<Income>(tempIncomeData);
        //    DataViewModel.ExpenseDataList = new BindableCollection<Expense>(tempExpenseData);

        //    isFileOpen = true;
        //    FileName = selectedFile;
        //    Activate_DataView();
        //}

        public void OpenFile()
        {
            // Probably not needed...
            IXMLData data = new XMLData();

            string selectedFile = _fileBrowser.OpenFileAccess(SubCategoryDirectory, 
                "Open Budget Plan", true);

            XMLReader reader = new XMLReader(selectedFile, data);
            reader.ParseData(MessageManager.DisplayMessage);

            BudgetName = reader.Data.ProjectName;
            DataViewModel.IncomeDataList = new BindableCollection<Income>(reader.Data.IncomeData);
            DataViewModel.ExpenseDataList = new BindableCollection<Expense>(reader.Data.ExpenseData);

            isFileOpen = true;
            FileName = selectedFile;
            Activate_DataView();
        }

        private void AssignFile()
        {

        }

        /// <summary>
        /// Opens the OpenFileDialog. Pulls SubCategory Data from a .bps file.
        /// </summary>
        public void OpenSubs()
        {
            string selectedPath = _fileBrowser.OpenFileAccess(
                SubCategoryDirectory,
                "Open Category File",
                false);

            List<SubCategory> incSub = new List<SubCategory>();
            List<SubCategory> expSub = new List<SubCategory>();

            // Unproven Syntax... Handle with care!
            if(FileControl_2.OpenSubsFile(selectedPath, out incSub, out expSub))
            {
                Income.AllIncomeCategories = incSub;
                Expense.AllExpenseCategories = expSub;
            }

            // Old
            //if (isFileOpen)
            //{
            //    OpenFileDialog openFile = new OpenFileDialog
            //    {
            //        Multiselect = false,
            //        Title = "Open Budget Plan",
            //        DefaultExt = ".bpn",
            //        InitialDirectory = SubCategoryDirectory
            //    };

            //    if (openFile.ShowDialog() == true)
            //    {
            //        FileName = openFile.FileName;
            //        OpenController = new FileControl(FileName);
            //        OpenController.OpenSubCategories();

            //        Income.AllIncomeCategories = new List<SubCategory>(OpenController.IncomeSubCateories);
            //        Expense.AllExpenseCategories = new List<SubCategory>(OpenController.ExpenseSubCategories);
            //    }
            //}
        }

        /// <summary>
        /// Opens the SaveFileDialog and saves a complete .bpn file.
        /// </summary>
        //public void SaveFileAs()
        //{
        //    string selectedPath = _fileBrowser.SaveFileAccess(BudgetDirectory, "Save Budget Plan", true);

        //    FileControl_2.SaveMainFile(selectedPath, BudgetName,
        //            DataViewModel.IncomeDataList.ToList(),
        //            DataViewModel.ExpenseDataList.ToList());

        //    FileName = selectedPath;
        //}

        public void SaveFileAs()
        {
            string selectedFile = _fileBrowser.SaveFileAccess(BudgetDirectory, "Save Budget Plan", true);

            IXMLData data = new XMLData()
            {
                ProjectName = BudgetName,
                IncomeData = DataViewModel.IncomeDataList.ToList(),
                ExpenseData = DataViewModel.ExpenseDataList.ToList()
            };
            XMLWrtier writer = new XMLWrtier(selectedFile, data);
            writer.WriteBudgetFile(MessageManager.DisplayMessageWithOK, MessageManager.DisplayMessage, MessageManager.DisplayMessage, true, true);
        }

        public void SaveFile()
        {
            FileControl_2.SaveMainFile(FileName, BudgetName,
                    DataViewModel.IncomeDataList.ToList(),
                    DataViewModel.ExpenseDataList.ToList());
        }

        /// <summary>
        /// Opens the SaveFileDialog and saves SubCategory Data to a .bps file.
        /// </summary>
        public void SaveSubsAs()
        {
            string selectedFile = _fileBrowser.SaveFileAccess(
                SubCategoryDirectory,
                "Save Sub Categories",
                false);

            FileControl_2.SaveSubFile(selectedFile,
                    Income.AllIncomeCategories,
                    Expense.AllExpenseCategories);

            SubCatFileName = selectedFile;
        }

        public void SaveSubsAs(string subsPath)
        {
            string selectedFile = _fileBrowser.SaveFileAccess(
                SubCategoryDirectory,
                "Save Sub Categories",
                false);
            
            FileControl_2.SaveSubFile(selectedFile,
                    Income.AllIncomeCategories,
                    Expense.AllExpenseCategories);

            SubCatFileName = selectedFile;
        }

        public void SaveSubs()
        {
            FileControl_2.SaveSubFile(SubCatFileName,
                Income.AllIncomeCategories,
                Expense.AllExpenseCategories);
        }

        public void SaveSubs(string path)
        {
            FileControl_2.SaveSubFile(path,
                Income.AllIncomeCategories,
                Expense.AllExpenseCategories);
        }

        #region -- Event Handles
        public void Handle(SaveSubCategoryEvent message)
        {
            if(message.ActionCode == 1)
            {
                if (message.Path != String.Empty)
                {
                    SaveSubsAs(message.Path);
                }
                else
                {
                    SaveSubsAs();
                }
            }
            else if(message.ActionCode == 2)
            {
                SaveSubs(message.Path);
            }
            else if (message.ActionCode == 3)
            {
                OpenSubs();
            }
            else if (message.ActionCode == 4)
            {

            }
        }

        public void Handle(NewBudgetEvent message)
        {
            // This IF is useless. Need to rethink.
            if (message.OpenSubCategories_E)
            {
                CreateNewBudget(
                    message.BudgetName_E,
                    message.MainFolder_E,
                    message.MainFolder_E,
                    message.OpenSubCategories_E);
            }
            else if (!message.OpenSubCategories_E)
            {
                CreateNewBudget(
                    message.BudgetName_E,
                    message.MainFolder_E,
                    message.MainFolder_E,
                    message.OpenSubCategories_E);
            }
        }
        #endregion

        #endregion

        /// <summary>
        /// Called on close. Unsusbscribes from events.
        /// </summary>
        public static void Exit()
        {
            DataElement.Exit();
        }

        #endregion

        #region - Properties

        public string WindowTitle
        {
            get { return _WindowTitle; }
            set
            {
                _WindowTitle = value;
                NotifyOfPropertyChange(() => WindowTitle);
            }
        }
        public string BudgetName
        {
            get { return _budgetName; }
            set
            {
                _budgetName = value;
                NotifyOfPropertyChange(() => BudgetName);
            }
        }

        public string BudgetDirectory
        {
            get { return _budgetDir; }
            set
            {
                _budgetDir = value;
            }
        }

        public string SubCategoryDirectory
        {
            get { return _SubCatDir; }
            set
            {
                _SubCatDir = value;
                NotifyOfPropertyChange(() => SubCategoryViewModel.SubCategoryPath);
            }
        }

        public string SubCatFileName
        {
            get { return _subCatFileName; }
            set
            {
                _subCatFileName = value;
                NotifyOfPropertyChange(() => SubCategoryViewModel.SubCatFileName);
            }
        }

        public FileControl SaveController
        {
            get { return _saveControl; }
            set
            {
                _saveControl = value;
            }
        }

        public FileControl OpenController
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

        public NewBudgetViewModel NewBudgetViewModel
        {
            get { return _newBudgetViewModel; }
            set
            {
                _newBudgetViewModel = value;
            }
        }
        #endregion
    }
}
