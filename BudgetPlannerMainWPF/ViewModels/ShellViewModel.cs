using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows;
using BudgetPlannerLib;
using BudgetPlannerLib.Models;
using Caliburn.Micro;
using BudgetPlannerMainWPF.EventModels;
using XMLParserLibrary;
using XMLParserLibrary.Interfaces;
using XMLParserLibrary.Exceptions;
using BudgetPlannerMainWPF.Enums;
using FileManagerLibrary;

namespace BudgetPlannerMainWPF.ViewModels
{
    // BAD Need to move to another file!!!
    public enum FileType
    {
        All,
        Budget,
        Category,
        Paystub
    }

    public class ShellViewModel : Conductor<object>, IHandle<NewBudgetEvent>, IHandle<SaveSubCategoryEvent>
    {
        #region - Fields
        private IWindowManager _windowManager;
        private IEventAggregator _eventAggregator;
        private IFileBrowser _fileBrowser;
        private IExceptionLogger _exceptionLogger;

        /// <summary>
        /// Name added to the Window Bar.
        /// </summary>
        private string _WindowTitle = "Budget Planner";

        /// <summary>
        /// Need to phase out.
        /// </summary>
        private string _mainFileDir = String.Empty;

        /// <summary>
        /// File name used for saving the main project.
        /// </summary>
        public string _mainFileName = String.Empty;

        /// <summary>
        /// Project name added to the save file and the Window Title.
        /// </summary>
        private string _budgetFileName = String.Empty;

        private string _budgetDir = String.Empty;

        /// <summary>
        /// Need to phase out.
        /// </summary>
        private string _CategoryDir = String.Empty;

        /// <summary>
        /// Stores the Sub Category File Path.
        /// </summary>
        private string _categoryFileName = String.Empty;

        private string _paystubDir = String.Empty;

        private string _paystubFileName = String.Empty;

        /// <summary>
        /// Set when a project is opened.
        /// </summary>
        private bool IsFileOpen { get; set; } = false;

        /// <summary>
        /// Set when a sub category file is open.
        /// </summary>
        private bool IsSubFileOpen { get; set; } = false;

        #region Save Check Properties
        /// <summary>
        /// Probably not needed. New Save Check. Set when the MAIN file is saved.
        /// </summary>
        private bool IsMainFileSaved { get; set; } = false;

        /// <summary>
        /// Probably not needed. New save check. Set when the SUB file is saved.
        /// </summary>
        private bool IsSubFileSaved { get; set; } = false;

        /// <summary>
        /// Budget FIle Name Save State.
        /// </summary>
        private string SaveBudgetNameState { get; set; } = "";

        /// <summary>
        /// Income DataList Save State.
        /// </summary>
        private Income[] SaveIncomeState { get; set; } = new Income[0];

        /// <summary>
        /// Expense DataList Save State.
        /// </summary>
        private Expense[] SaveExpenseState { get; set; } = new Expense[0];

        /// <summary>
        /// Income SubCategory Save State.
        /// </summary>
        private Category[] SaveSubIncomeState { get; set; } = new Category[0];

        /// <summary>
        /// Expense SubCategory Save State.
        /// </summary>
        private Category[] SaveSubExpenseState { get; set; } = new Category[0];
        #endregion

        /// <summary>
        /// Stores whether the last screen was the NewBudgetView.
        /// </summary>
        private bool lastScreenIsNF = false;

        /// <summary>
        /// Need to phase out.
        /// </summary>
        //private FileControl _saveControl;
        /// <summary>
        /// Need to phase out.
        /// </summary>
        //private FileControl _openControl;
        

        private DataViewModel _dataViewModel;
        private SubCategoryViewModel _categoryViewModel = new SubCategoryViewModel();
        private NewBudgetViewModel _newBudgetViewModel = new NewBudgetViewModel();
        private PaystubViewModel _paystubViewModel;
        #endregion

        #region - Constructors
        /// <summary>
        /// Not used anymore because of Dependancy Injection.
        /// </summary>
        public ShellViewModel()
        {
            InitializeAll();

            ActivateItem(NewBudgetViewModel);
            lastScreenIsNF = true;

            DataViewModel.SortCategories();
            
            //SubCategoryViewModel.SubCatEventManager += this.SubCatEventManager_Event;
        }

        /// <summary>
        /// Constructes the shell view and activates the Dependancy Injection.
        /// </summary>
        /// <param name="eventAggregator">Caliburns Event Manager.</param>
        /// <param name="fileBrowser">Custom Save/Open/Folder FileDialog Manager.</param>
        public ShellViewModel(IEventAggregator eventAggregator, IFileBrowser fileBrowser, IExceptionLogger exceptionLogger, IWindowManager windowManager)
        {
            // Testing only.
            _windowManager = windowManager;

            _exceptionLogger = exceptionLogger;
            _fileBrowser = fileBrowser;
            _eventAggregator = eventAggregator;
            _eventAggregator.Subscribe(this);

            DataViewModel = new DataViewModel(eventAggregator);
            SubCategoryViewModel = new SubCategoryViewModel(eventAggregator, fileBrowser);
            NewBudgetViewModel = new NewBudgetViewModel(eventAggregator, fileBrowser);
            PaystubViewModel = new PaystubViewModel(eventAggregator, fileBrowser, windowManager);

            InitializeAll();

            ActivateItem(NewBudgetViewModel);
            lastScreenIsNF = true;

            //DataViewModel.SortCategories();
            _eventAggregator.PublishOnUIThread(new UpdateDataListEvent());

            #region Testing ONLY:
            BudgetFileName = "Test ONLY";
            TestDataAccesser testData = new TestDataAccesser(2);
            DataViewModel.IncomeDataList = new BindableCollection<Income>(testData.IncomeList);
            DataViewModel.ExpenseDataList = new BindableCollection<Expense>(testData.ExpenseList);
            #endregion
        }
        #endregion

        #region - Methods

        #region -- Save Menu Button Testing
        public void Save(FileType file)
        {
            if(file == FileType.All)
            {
                Tuple<string, bool> mainOutput = _fileBrowser.SaveFileAccess(MainFileDirectory, MainFileName, ExtensionType.Main, "Save Main File");
                Tuple<string, bool> budgetOutput = _fileBrowser.SaveFileAccess(BudgetDirectory, BudgetFileName, ExtensionType.Budget, "Save Budget File");
                Tuple<string, bool> categoryOutput = _fileBrowser.SaveFileAccess(CategoryDirectory, CategoryFileName, ExtensionType.Category, "Save Category File");
                Tuple<string, bool> paystubOutput = _fileBrowser.SaveFileAccess(PaystubDirectory, PaystubFileName, ExtensionType.Paystub, "Save Paystub File");

                if (mainOutput.Item2 is true)
                {
                    MainFileDirectory = mainOutput.Item1;
                    SaveMainFile();
                }

                if (budgetOutput.Item2 is true)
                {
                    BudgetDirectory = budgetOutput.Item1;
                    SaveBudgetFile();
                }

                if (categoryOutput.Item2 is true)
                {
                    CategoryDirectory = categoryOutput.Item1;
                    SaveCategoryFile();
                }

                if (paystubOutput.Item2 is true)
                {
                    PaystubDirectory = paystubOutput.Item1;
                    SavePaystubFile();
                }
            }
            else if (file == FileType.Budget)
            {
                
            }
            else if (file == FileType.Category)
            {
                
            }
            else if (file == FileType.Paystub)
            {
                
            }
        }

        public void SaveAs(FileType file)
        {
            if (file == FileType.All)
            {
                Tuple<string, bool> mainOut = _fileBrowser.SaveFileAccess(MainFileDirectory, MainFileName, ExtensionType.Main, "Save Main File As...");

                if (mainOut.Item2 is true)
                {
                    SaveController.PullMainFileData(MainFileName, mainOut.Item1);

                    Tuple<string, bool> budgetOut = _fileBrowser.SaveFileAccess(BudgetDirectory, BudgetFileName, ExtensionType.Budget, "Save Budget As...");
                    
                    if (budgetOut.Item2 is true)
                    {
                        SaveController.PullBudgetFileData(BudgetFileName, budgetOut.Item1, DataViewModel.IncomeDataList.ToArray(), DataViewModel.ExpenseDataList.ToArray());
                    }
                    else
                    {
                        SaveController.Main.Budget = null;
                    }

                    Tuple<string, bool> categoryOut = _fileBrowser.SaveFileAccess(CategoryDirectory, CategoryFileName, ExtensionType.Category, "Save Categories As...");

                    if (categoryOut.Item2 is true)
                    {
                        SaveController.PullCategoryData(CategoryFileName, categoryOut.Item1, Income.AllIncomeCategories.ToArray(), Expense.AllExpenseCategories.ToArray());
                    }
                    else
                    {
                        SaveController.Main.Category = null;
                    }

                    Tuple<string, bool> paystubOut = _fileBrowser.SaveFileAccess(PaystubDirectory, PaystubFileName, ExtensionType.Paystub, "Save Paystubs As...");

                    if (paystubOut.Item2 is true)
                    {
                        SaveController.PullPaystubData(PaystubFileName, paystubOut.Item1, PaystubViewModel.CollectionDescription, PaystubViewModel.PaystubDataList.ToArray());
                    }
                    else
                    {
                        SaveController.Main.Paystub = null;
                    }

                    SaveController.BeginSave(MessageManager.DisplayMessage);
                }
            }
            else if (file == FileType.Budget)
            {
                Tuple<string, bool> budgetOut = _fileBrowser.SaveFileAccess(BudgetDirectory, BudgetFileName, ExtensionType.Budget, "Save Budget As...");
            }
            else if (file == FileType.Category)
            {
                Tuple<string, bool> categoryOut = _fileBrowser.SaveFileAccess(CategoryDirectory, CategoryFileName, ExtensionType.Category, "Save Categories As...");
            }
            else if (file == FileType.Paystub)
            {
                Tuple<string, bool> paystubOut = _fileBrowser.SaveFileAccess(PaystubDirectory, PaystubFileName, ExtensionType.Paystub, "Save Paystubs As...");
            }
        }

        public void Open(FileType button)
        {
            if (button == FileType.All)
            {
                
            }
            else if (button == FileType.Budget)
            {
                
            }
            else if (button == FileType.Category)
            {
                
            }
            else if (button == FileType.Paystub)
            {
                
            }
        }

        public void OpenAs(FileType button)
        {
            if (button == FileType.All)
            {
                
            }
            else if (button == FileType.Budget)
            {
                
            }
            else if (button == FileType.Category)
            {
                
            }
            else if (button == FileType.Paystub)
            {
                
            }
        }

        #region Save/Open File helper Methods
        public bool SaveMainFile()
        {
            bool success = false;

            SaveController.Main.FullPath = MainFileDirectory;
            SaveController.Main.FileName = MainFileName;

            if (BudgetDirectory is null)
            {
                SaveController.Main.Budget.FullPath = "Null";
            }
            else
            {
                SaveController.Main.Budget.BudgetName = BudgetFileName;
                SaveController.Main.Budget.IncomeData = DataViewModel.IncomeDataList.ToArray();
                SaveController.Main.Budget.ExpenseData = DataViewModel.ExpenseDataList.ToArray();
                SaveController.Main.Budget.FullPath = BudgetDirectory;
            }

            success = SaveController.BeginSave(MessageManager.DisplayMessage);

            return success;
        }

        public bool SaveBudgetFile()
        {
            bool success = false;


            return success;
        }

        public bool SaveCategoryFile()
        {
            bool success = false;


            return success;
        }

        public bool SavePaystubFile()
        {
            bool success = false;


            return success;
        }
        #endregion
        #endregion
        /// <summary>
        /// Need to phase out.
        /// </summary>
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
                CategoryDirectory = e.SubCatDirectory;
                CategoryFileName = e.SubCatFileName;
            }
        }
        
        /// <summary>
        /// Sets up empty data lists.
        /// </summary>
        public void InitializeAll()
        {
            DataViewModel.Initialize();
            SubCategoryViewModel.Initialize();
        }

        /// <summary>
        /// Checks the main file save state. 
        /// </summary>
        /// <returns>If either Income or Expense datalists dont match, returns false.</returns>
        public bool CheckMainFileSaveState()
        {
            bool A = CheckSaveState(DataViewModel.IncomeDataList.ToArray(), SaveIncomeState);
            bool B = CheckSaveState(DataViewModel.ExpenseDataList.ToArray(), SaveExpenseState);

            if (A && B)
            {
                return true;
            }
            else return false;
        }

        /// <summary>
        /// Checks the sub category file save state.
        /// </summary>
        /// <returns>If the Income or Expense SubCategory lists are different, returns false.</returns>
        public bool CheckSubFileSaveState()
        {
            bool A = CheckSaveState(Income.AllIncomeCategories.ToArray(), SaveSubIncomeState);
            bool B = CheckSaveState(Expense.AllExpenseCategories.ToArray(), SaveSubExpenseState);

            if (A && B)
            {
                return true;
            }
            else return false;
        }

        #region -- Private Methods
        /// <summary>
        /// OLD, May need to be reWritten.
        /// Needs full testing.
        /// Creates a new budget, clears the old budget and directs a new path.
        /// </summary>
        /// <param name="budgetname">Name from CreatingNewBudget Event</param>
        /// <param name="budgetDir">Directory from CreatingNewBudget Event</param>
        /// <param name="subDir">SubCategory Directory</param>
        private void CreateNewBudget(string budgetName, string budgetDir, string subDir)
        {
            if (!IsFileOpen)
            {
                BudgetFileName = budgetName;
                WindowTitle = $"Budget Planner - {budgetName}";
                MainFileDirectory = budgetDir;
                CategoryDirectory = subDir;

                IsMainFileSaved = false;
                IsFileOpen = true;

                Activate_DataView();
            }
            else
            {
                bool contin = MessageManager.DisplayMessageWithOK(
                    "Are you sure? This will erase any unsaved data.", 
                    "New File");

                if (contin)
                {
                    ClearData();
                    BudgetFileName = budgetName;
                    WindowTitle = $"Budget Planner - {budgetName}";
                    MainFileDirectory = budgetDir;
                    CategoryDirectory = subDir;

                    IsFileOpen = true;
                    IsMainFileSaved = false;

                    Activate_DataView();
                }
                else
                {
                    _eventAggregator.PublishOnUIThread(new CancelNewEvent());
                    Activate_DataView();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="budgetName"></param>
        /// <param name="budgetDir"></param>
        /// <param name="subDir"></param>
        /// <param name="openSubs"></param>
        private void CreateNewBudget(string budgetName, string budgetDir, string subDir, bool openSubs)
        {
            if (!IsFileOpen)
            {
                if (openSubs)
                {
                    // Need to fix. The IF statements drop without doing anything.
                    BudgetFileName = budgetName;
                    WindowTitle = $"Budget Planner - {budgetName}";
                    MainFileDirectory = budgetDir;
                    CategoryDirectory = subDir;
                    OpenSubs();

                    IsMainFileSaved = false;
                    IsFileOpen = true;

                    Activate_DataView();
                }
                else if (!openSubs)
                {
                    BudgetFileName = budgetName;
                    WindowTitle = $"Budget Planner - {budgetName}";
                    MainFileDirectory = budgetDir;
                    CategoryDirectory = subDir;

                    IsMainFileSaved = false;
                    IsFileOpen = true;

                    Activate_DataView();
                }
            }
            else
            {
                bool contin = MessageManager.DisplayMessageWithOK(
                    "Are you sure? This will erase any unsaved data.", 
                    "New File");

                if (contin)
                {
                    ClearData();
                    BudgetFileName = budgetName;
                    WindowTitle = $"Budget Planner - {budgetName}";
                    MainFileDirectory = budgetDir;
                    CategoryDirectory = subDir;

                    IsMainFileSaved = false;
                    IsFileOpen = true;
                    Activate_DataView();
                }
                else
                {
                    _eventAggregator.PublishOnUIThread(new CancelNewEvent());
                    Activate_DataView();
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
            BudgetFileName = String.Empty;
            MainFileDirectory = String.Empty;
            CategoryDirectory = String.Empty;
        }

        /// <summary>
        /// Checks on the status of the loaded data from a file.
        /// </summary>
        /// <typeparam name="T">Type of data in the list.</typeparam>
        /// <param name="data">List to check.</param>
        /// <returns>returns a bool on whether null or empty.</returns>
        private bool CheckData<T>(List<T> data) where T : class
        {
            if (data != null)
            {
                if (data.Count > 0)
                {
                    // Need to remove
                    MessageManager.DisplayMessage("Check Passed", "Info");
                    return true;
                }
                else
                {
                    MessageManager.DisplayMessage("Income Sub Categories where empty.", "Info");
                    return false;
                }
            }
            else
            {
                MessageManager.DisplayMessage("Income Sub Categories where not found.", "Error");
                return false;
            }
        }

        /// <summary>
        /// Checks on the status of the loaded data from a file.
        /// </summary>
        /// <typeparam name="T">Type of data in the list.</typeparam>
        /// <param name="data">List to check.</param>
        /// <returns>returns a bool on whether null or empty.</returns>
        private bool CheckData<T>(BindableCollection<T> data) where T : class
        {
            if (data != null)
            {
                if (data.Count > 0)
                {
                    MessageManager.DisplayMessage("Check Passed");
                    return true;
                }
                else
                {
                    MessageManager.DisplayMessage("Income Sub Categories where empty.", "Info");
                    return false;
                }
            }
            else
            {
                MessageManager.DisplayMessage("Income Sub Categories where not found.", "Error");
                return false;
            }
        }

        /// <summary>
        /// Copys the current projects data to the save state properties.
        /// </summary>
        private void SetMainFileSaveState()
        {
            SaveBudgetNameState = BudgetFileName;
            SaveIncomeState = DataViewModel.IncomeDataList.ToArray();
            SaveExpenseState = DataViewModel.ExpenseDataList.ToArray();
        }

        /// <summary>
        /// Copies the current sub category data to the save state properties.
        /// </summary>
        private void SetSubFileSaveState()
        {
            SaveSubIncomeState = Income.AllIncomeCategories.ToArray();
            SaveSubExpenseState = Expense.AllExpenseCategories.ToArray();
        }
        
        /// <summary>
        /// Not Needed. Creates a memory pointer, not a new list.
        /// </summary>
        private bool CheckSaveState<T>(List<T> input, List<T> saveData) where T : IEquals
        {
            bool pass = false;

            if (input.Count == saveData.Count)
            {
                for (int i = 0; i < input.Count; i++)
                {
                    if (!input[i].Equals(saveData[i]))
                    {
                        pass = false;
                        break;
                    }
                    else pass = true;
                }
            }
            else pass = false;

            return pass;
        }

        /// <summary>
        /// BUGGED. Creates a memory pointer, not a new BindableCollection.
        /// </summary>
        private bool CheckSaveState<T>(BindableCollection<T> input, BindableCollection<T> saveData) where T : IEquals
        {
            bool pass = false;

            if (input.Count == saveData.Count)
            {
                for (int i = 0; i < input.Count; i++)
                {
                    if (!input[i].Equals(saveData[i]))
                    {
                        pass = false;
                        break;
                    }
                    else pass = true;
                }
            }
            else pass = false;

            return pass;
        }

        /// <summary>
        /// Takes the current data collection and compares it to the Saved State
        /// collection.
        /// </summary>
        /// <typeparam name="T">Any class that implements the IEquals interface.</typeparam>
        /// <param name="input">Current collection to check.</param>
        /// <param name="saveData">Saved collection to compare with.</param>
        /// <returns>Returns true if the collections match.</returns>
        private bool CheckSaveState<T>(T[] input, T[] saveData) where T : IEquals
        {
            bool pass = false;

            if (input != null && saveData != null)
            {
                if (input.Length == saveData.Length)
                {
                    for (int i = 0; i < input.Length; i++)
                    {
                        if (!input[i].Equals(saveData[i]))
                        {
                            pass = false;
                            break;
                        }
                        else pass = true;
                    }
                }
                else pass = false;
            }
            else pass = false;

            return pass;
        }
        #endregion

        #region Add/Remove Data Controls
        /// <summary>
        /// Adds a new Income Column with Default Data.
        /// </summary>
        public void AddIncomeColumn()
        {
            DataViewModel.IncomeDataList.Add(new Income("Default", "New Income", 0.0M, (uint)DataViewModel.IncomeDataList.Count + 1));
        }

        /// <summary>
        /// Adds a new Expense Column with Default Data.
        /// </summary>
        public void AddExpenseColumn()
        {
            DataViewModel.ExpenseDataList.Add(new Expense("default", "New Expense", 0.0M, (uint)DataViewModel.ExpenseDataList.Count + 1));
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
            if(CategoryDirectory != null)
            {
                SubCategoryViewModel.SubCategoryPath = CategoryDirectory;
            }
            SendCategories();
        }

        /// <summary>
        /// Casts SubCategories to a BindableCollection for sending data.
        /// </summary>
        public void SendCategories()
        {
            SubCategoryViewModel.IncomeCategories = new BindableCollection<Category>(Income.AllIncomeCategories);
            SubCategoryViewModel.ExpenseCategories = new BindableCollection<Category>(Expense.AllExpenseCategories);
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

        public void Activate_PaystubView()
        {
            ActivateItem(PaystubViewModel);
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
            Tuple<string, bool> selectedFile = _fileBrowser.OpenFileAccess(CategoryDirectory,
                "Open Budget Plan", true);

            if (selectedFile.Item2)
            {
                XMLReader reader = new XMLReader(selectedFile.Item1);
                reader.ParseData(MessageManager.DisplayMessage);

                BudgetFileName = reader.Data.ProjectName;
                DataViewModel.IncomeDataList = new BindableCollection<Income>(reader.Data.IncomeData);
                DataViewModel.ExpenseDataList = new BindableCollection<Expense>(reader.Data.ExpenseData);

                IsFileOpen = true;
                MainFileName = selectedFile.Item1;

                SetMainFileSaveState();
                IsMainFileSaved = true;
                Activate_DataView();
            }
        }

        #region --- File Management Button Methods
        /// <summary>
        /// Erases all current data to set up for a new file.
        /// </summary>
        public void NewFile_Button()
        {
            lastScreenIsNF = true;

            if (IsFileOpen)
            {
                if (!CheckMainFileSaveState())
                {
                    if (MessageManager.DisplayMessageWithOK("Save before creating new budget?", "Save??"))
                    {
                        SaveFile_Button();
                    }
                }
            }
            InitializeAll();
            ActivateItem(NewBudgetViewModel);
        }

        /// <summary>
        /// When Menu Open Button is pressed.
        /// </summary>
        public void OpenFile_Button()
        {
            if (IsFileOpen)
            {
                if (!CheckMainFileSaveState())
                {
                    int save = MessageManager.DisplayMessageWithYesNo(
                                "This file hasnt been saved. Save First?",
                                "Careful!");

                    if (save == 1)
                    {
                        SaveFile_Button();
                        OpenFile();
                    }
                    else if (save == 2)
                    {
                        OpenFile();
                    } 
                }
                else
                {
                    OpenFile();
                }
            }
            else
            {
                OpenFile();
            }
        }

        public void OpenSubs_Button()
        {
            if (IsSubFileOpen)
            {
                if (!CheckSubFileSaveState())
                {
                    int save = MessageManager.DisplayMessageWithYesNo(
                        "Category data isnt saved. Save now??", "Careful");

                    if(save == 1)
                    {
                        SaveSubs_Button();
                        OpenSubs();
                    }
                    else
                    {
                        OpenSubs();
                    }
                }
                else
                {
                    OpenSubs();
                }
            }
            else
            {
                OpenSubs();
            }
        }
        
        /// <summary>
        /// Checks for a started file and a valid save path.
        /// </summary>
        public void SaveFile_Button()
        {
            try
            {
                if (IsFileOpen)
                {
                    if (FileCheck.CheckFile(MainFileName))
                    {
                        SaveFile();
                    }
                    else
                    {
                        SaveFileAs();
                    }
                }
                else
                {
                    // Testing the XMLWriter Exception handling:
                    SaveFileBad(false);

                    //SaveFileAs();
                }
            }
            catch (XMLParseException e)
            {
                MessageManager.DisplayMessage($"TESTING** Was not able to save. {e.Message}", "ERROR!");
                e.CompileErrorData();
            }
        }

        public void SaveSubs_Button()
        {
            if (IsSubFileOpen)
            {
                if (FileCheck.CheckFile(CategoryFileName))
                {
                    SaveSubs();
                }
                else
                {
                    SaveSubsAs();
                }
            }
            else
            {
                SaveSubsAs();
            }
        }

        public void SaveAll_Button()
        {
            SaveFile_Button();
            SaveSubs_Button();
        }

        public void SaveAllAs_Button()
        {
            SaveFileAs();
            SaveSubsAs();
        }
        #endregion
        
        /// <summary>
        /// Test Version ONLY!
        /// </summary>
        public void OpenSubs()
        {
            Tuple<string, bool> selectedPath = _fileBrowser.OpenFileAccess(
                        CategoryDirectory,
                        "Open Sub Category File",
                        false);

            if (selectedPath.Item2)
            {
                XMLReader reader = new XMLReader(selectedPath.Item1);
                reader.ParseSubFile(MessageManager.DisplayMessage);

                // Need to test.
                // Should work. Checks for null or empty using a generic method.
                if (CheckData(reader.SubData.IncomeSubCategories))
                {
                    Income.AllIncomeCategories = reader.SubData.IncomeSubCategories;
                }

                if (CheckData(reader.SubData.ExpenseSubCategories))
                {
                    Expense.AllExpenseCategories = reader.SubData.ExpenseSubCategories;
                }
            }

            SetSubFileSaveState();
            
            Activate_SubCategoryView();
        }

        /// <summary>
        /// Opens the SaveFileDialog, saves the current file, then stores the save state.
        /// </summary>
        public void SaveFileAs()
        {
            Tuple<string, bool> selectedFile = _fileBrowser.SaveFileAccess(
                MainFileDirectory, 
                "Save Budget Plan", 
                BudgetFileName, 
                true);

            if (selectedFile.Item2)
            {
                IXMLData data = new XMLData()
                {
                    ProjectName = BudgetFileName,
                    IncomeData = DataViewModel.IncomeDataList.ToList(),
                    ExpenseData = DataViewModel.ExpenseDataList.ToList()
                };

                XMLWrtier writer = new XMLWrtier(selectedFile.Item1, data);

                //writer.WriteBudgetFile(
                //    MessageManager.DisplayMessage,
                //    MessageManager.DisplayMessage);

                try
                {
                    writer.WriteBudgetFile();

                    MainFileName = selectedFile.Item1;
                    IsFileOpen = true;
                    IsMainFileSaved = true;

                    SetMainFileSaveState();
                }
                catch (Exception)
                {
                    MessageManager.DisplayMessage("An error occured while saving..");
                }
            }
        }

        /// <summary>
        /// For TESTING. Used for stress testing.
        /// ! bad path.
        /// </summary>
        public void SaveFileBad(bool path = true)
        {
            XMLWrtier writer = new XMLWrtier();
            string badPath = @"C:\Users\Daxxn\Daxxn\TestFile.bpn";
            string goodPath = @"C:\Users\Daxxn\Desktop\ErrorTest.bpn";

            IXMLData goodData = new XMLData()
            {
                ProjectName = BudgetFileName,
                IncomeData = DataViewModel.IncomeDataList.ToList(),
                ExpenseData = DataViewModel.ExpenseDataList.ToList()
            };

            IXMLData badData = new XMLData()
            {
                ProjectName = BudgetFileName,
                IncomeData = null,
                ExpenseData = DataViewModel.ExpenseDataList.ToList()
            };

            if (path)
            {
                writer = new XMLWrtier(badPath, goodData); 
            }
            else
            {
                writer = new XMLWrtier(goodPath, badData);
            }

            try
            {
                writer.WriteBudgetFile();

                MainFileName = badPath;
                IsFileOpen = true;
                IsMainFileSaved = true;

                SetMainFileSaveState();
            }
            catch (Exception e)
            {
                MessageManager.DisplayMessage($"{e.HResult} : {e.Message}", "ERROR!!");
            }
        }

        /// <summary>
        /// Saves the project and sets the save state.
        /// </summary>
        public void SaveFile()
        {
            IXMLData data = new XMLData()
            {
                ProjectName = BudgetFileName,
                IncomeData = DataViewModel.IncomeDataList.ToList(),
                ExpenseData = DataViewModel.ExpenseDataList.ToList()
            };

            // Shouldnt get to this point if its false.
            if (IsMainFileSaved)
            {
                XMLWrtier wrtier = new XMLWrtier(MainFileName, data);
                wrtier.WriteBudgetFile(
                    MessageManager.DisplayMessage,
                    MessageManager.DisplayMessage);
            }
            else
            {
                SaveFileAs();
            }

            IsMainFileSaved = true;
            SetMainFileSaveState();
        }

        /// <summary>
        /// Testing still. Saves the subcategories to an XML file.
        /// </summary>
        public void SaveSubsAs()
        {
            Tuple<string, bool> selectedFile = _fileBrowser.SaveFileAccess(
                CategoryDirectory,
                "Save Sub Categories",
                false);

            if (selectedFile.Item2)
            {
                IXMLDataSub subData = new XMLData()
                {
                    IncomeSubCategories = Income.AllIncomeCategories,
                    ExpenseSubCategories = Expense.AllExpenseCategories
                };

                XMLWrtier wrtier = new XMLWrtier(selectedFile.Item1, subData);
                wrtier.WriteSubFile();

                // Update the Shell
                CategoryFileName = selectedFile.Item1;
                IsSubFileSaved = true;

                SetSubFileSaveState();
            }
        }

        public void SaveSubs()
        {
            if (FileCheck.CheckDirectory(CategoryFileName))
            {
                IXMLDataSub data = new XMLData()
                {
                    IncomeSubCategories = Income.AllIncomeCategories,
                    ExpenseSubCategories = Expense.AllExpenseCategories
                };

                XMLWrtier wrtier = new XMLWrtier(CategoryFileName, data);
                wrtier.WriteSubFile(MessageManager.DisplayMessage);

                IsSubFileSaved = true;

                SetSubFileSaveState();
            }
            else
            {
                SaveSubsAs();
            }
        }

        // Need to remove. Error prone.
        public void SaveSubs(string path)
        {
            
        }

        #region -- Event Handles
        public void Handle(SaveSubCategoryEvent message)
        {
            if(message.ActionCode == 1)
            {
                // Not going to use the Writable Path.
                // Too many possible bugs.
                if (message.Path != String.Empty)
                {
                    SaveSubsAs();
                }
                else
                {
                    SaveSubsAs();
                }
            }
            else if(message.ActionCode == 2)
            {
                SaveSubs();
            }
            else if (message.ActionCode == 3)
            {
                OpenSubs();
            }
            else if (message.ActionCode == 4)
            {
                // Not needed.
            }
        }

        public void Handle(NewBudgetEvent message)
        {
            CreateNewBudget(
                    message.BudgetName,
                    message.MainFolder,
                    message.MainFolder,
                    message.OpenSubCategories);
        }
        #endregion

        #endregion

        /// <summary>
        /// Called on close. Unsusbscribes from events.
        /// </summary>
        public static void Exit()
        {
            BaseColumn.Exit();
        }

        public string NameWindowTitle(string name)
        {
            return $"Budget Planner - {name}";
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
                NotifyOfPropertyChange(() => BudgetFileName);
            }
        }

        public string MainFileDirectory
        {
            get { return _mainFileDir; }
            set
            {
                _mainFileDir = value;
            }
        }

        public string MainFileName
        {
            get { return _mainFileName; }
            set
            {
                _mainFileName = value;
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

        public string BudgetFileName
        {
            get { return _budgetFileName; }
            set
            {
                _budgetFileName = value;
                WindowTitle = NameWindowTitle(value);
                NotifyOfPropertyChange(() => BudgetFileName);
                NotifyOfPropertyChange(() => WindowTitle);
            }
        }

        public string CategoryDirectory
        {
            get { return _CategoryDir; }
            set
            {
                _CategoryDir = value;
                NotifyOfPropertyChange(() => SubCategoryViewModel.SubCategoryPath);
            }
        }

        public string CategoryFileName
        {
            get { return _categoryFileName; }
            set
            {
                _categoryFileName = value;
                NotifyOfPropertyChange(() => SubCategoryViewModel.SubCatFileName);
            }
        }

        public string PaystubDirectory
        {
            get { return _paystubDir; }
            set
            {
                _paystubDir = value;
            }
        }

        public string PaystubFileName
        {
            get { return _paystubFileName; }
            set
            {
                _paystubFileName = value;
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

        public PaystubViewModel PaystubViewModel
        {
            get { return _paystubViewModel; }
            set
            {
                _paystubViewModel = value;
            }
        }
        #endregion
    }
}