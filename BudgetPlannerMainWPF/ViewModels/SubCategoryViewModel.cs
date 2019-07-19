using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using BudgetPlannerLib.Models;
using BudgetPlannerMainWPF.EventModels;
using BudgetPlannerMainWPF.Views;
using Caliburn.Micro;

namespace BudgetPlannerMainWPF.ViewModels
{
    public class SubCategoryViewModel : Screen, IHandle<UpdateDataListEvent>, IHandle<UpdateSubCatEvent>
    {
        #region - Fields
        private IEventAggregator _eventAggregator;

        private BindableCollection<SubCategory> _incomeCategories = new BindableCollection<SubCategory>();
        private BindableCollection<SubCategory> _expenseCategories = new BindableCollection<SubCategory>();

        private SubCategory _selectedIncomeCategory;
        private SubCategory _selectedExpenseCategory;

        private string _newIncomeName = String.Empty;
        private string _newExpenseName = String.Empty;

        private string _subCategoryPath = String.Empty;
        private bool _goodSubCatPath = false;

        private string _subCatFileName;

        public event EventHandler<SubCategoryEventArgs> SubCatEventManager;
        #endregion

        #region - Constructors
        /// <summary>
        /// Base Constructor, Subscribes to the SendEnter Event.
        /// </summary>
        public SubCategoryViewModel()
        {
            #region Test Data. Should get replaced on startup.
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
        public SubCategoryViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.Subscribe(this);
        }
        #endregion

        #region - Methods
        /// <summary>
        /// Runs on startup only.
        /// </summary>
        public void Initialize()
        {
            IncomeCategories = new BindableCollection<SubCategory>();
            ExpenseCategories = new BindableCollection<SubCategory>();
        }

        /// <summary>
        /// Triggeres the KeyPress Event from the SubCategoryView Backend.
        /// </summary>
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

        /// <summary>
        /// Clears data from the SubCategories.
        /// </summary>
        public void ClearData()
        {
            IncomeCategories.Clear();
            ExpenseCategories.Clear();

            SelectedIncomeCategory = new SubCategory();
            SelectedExpenseCategory = new SubCategory();
        }

        #region -- Buttons
        /// <summary>
        /// Adds a new Income SubCategory with the name in the NewIncomeName TextBox.
        /// </summary>
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

        /// <summary>
        /// Removes the selected Income SubCategory from the SubCategory List.
        /// </summary>
        public void RemoveIncomeCategory()
        {
            IncomeCategories.Remove(SelectedIncomeCategory);
        }

        /// <summary>
        /// Adds a new Expense SubCategory with the name in the NewExpenseName TextBox.
        /// </summary>
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

        /// <summary>
        /// Removes the selected Expense SubCategory from the SubCategory List.
        /// </summary>
        public void RemoveExpenseCategory()
        {
            ExpenseCategories.Remove(SelectedExpenseCategory);
        }

        /// <summary>
        /// Sends all edits to the Income & Expense SubCategory Lists.
        /// </summary>
        public void FinishCategories()
        {
            Income.AllIncomeCategories = IncomeCategories.ToList();
            Expense.AllExpenseCategories = ExpenseCategories.ToList();
        }

        public void NewSubCatPath()
        {
            // Old Version
            SubCategoryPath = NewBudgetViewModel.OpenFolderDialog(
                "Select Sub-Category Save Location"
                );

            _eventAggregator.PublishOnUIThread(new SaveSubCategoryEvent(4, SubCategoryPath));
            //SubCatEventManager?.Invoke(this, new SubCategoryEventArgs(3, SubCategoryPath, SubCatFileName));
        }

        public void OpenSubCats()
        {
            //SubCatEventManager?.Invoke(this, new SubCategoryEventArgs(2));
            _eventAggregator.PublishOnUIThread(new SaveSubCategoryEvent(3, SubCategoryPath));
        }

        public void SaveSubCats()
        {
            FinishCategories();
            //SubCatEventManager?.Invoke(this, new SubCategoryEventArgs(1));
            _eventAggregator.PublishOnUIThread(new SaveSubCategoryEvent(2, SubCategoryPath));
        }

        public void SaveSubCatsAs()
        {
            FinishCategories();
            //SubCatEventManager?.Invoke(this, new SubCategoryEventArgs(0));
            _eventAggregator.PublishOnUIThread(new SaveSubCategoryEvent(1, SubCategoryPath));
        }
        #endregion

        #region -- Event Handlers
        public void Handle(UpdateDataListEvent message)
        {
            Income.AllIncomeCategories = IncomeCategories.ToList();
            Expense.AllExpenseCategories = ExpenseCategories.ToList();
        }

        public void Handle(UpdateSubCatEvent message)
        {
            IncomeCategories = new BindableCollection<SubCategory>(message.IncomeSubs);
            ExpenseCategories = new BindableCollection<SubCategory>(message.ExpenseSubs);
        }
        #endregion
        #endregion

        #region - Properties
        public string SubCategoryPath
        {
            get { return _subCategoryPath; }
            set
            {
                _subCategoryPath = value;

                if (FileCheck.CheckDirectory(value))
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

        public string SubCatFileName
        {
            get { return _subCatFileName; }
            set
            {
                _subCatFileName = value;
                NotifyOfPropertyChange(() => SubCatFileName);
            }
        }

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