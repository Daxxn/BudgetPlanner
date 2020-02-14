using BudgetPlannerMainWPF.EventModels;
using Caliburn.Micro;
using PaystubLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using BudgetPlannerMainWPF.Structs;

namespace BudgetPlannerMainWPF.ViewModels
{
    public class AddPaystubViewModel : Caliburn.Micro.Screen
    {
        #region - Fields & Properties
        private IWindowManager _windowManager;
        private IEventAggregator _eventAggregator;

        private BindableCollection<Paystub> _paystubDataList;

        private string _nameInput;
        private int _indexInput;
        private decimal? _grossInput;
        private decimal? _netInput;

        private AddToSelector[] _addToSelectorList;

        private AddToSelector _addToSelection;
        #endregion

        #region - Constructors
        public AddPaystubViewModel() { }
        public AddPaystubViewModel(IWindowManager windowManager, IEventAggregator eventAggregator)
        {
            _windowManager = windowManager;
            _eventAggregator = eventAggregator;
            _eventAggregator.Subscribe(this);

            PaystubDataList = new BindableCollection<Paystub>();
            InitComboBox();
        }
        #endregion

        #region - Methods
        public void AddEnter(ActionExecutionContext context)
        {
            KeyEventArgs keyArgs = null;

            if (context != null)
            {
                keyArgs = context.EventArgs as KeyEventArgs;
            }

            if (keyArgs != null && keyArgs.Key == Key.Enter)
            {
                Add();
            }
        }

        public void Add()
        {
            Paystub temp = new Paystub()
            {
                Index = (uint)PaystubDataList.Count + 1
            };

            if (NameInput != null)
            {
                temp.Name = NameInput;
            }

            if (GrossInput != null)
            {
                temp.Gross = (decimal)GrossInput;
            }

            if (NetInput != null)
            {
                temp.Net = (decimal)NetInput;
            }

            PaystubDataList.Add(temp);
            ClearInputFields();
        }

        public void SendData()
        {
            _eventAggregator.PublishOnUIThread(new AddManyPaystubsEventModel(PaystubDataList.ToList(), AddToSelection.Code));
            ClearInputFields();
            PaystubDataList = new BindableCollection<Paystub>();
            this.TryClose(null);
        }

        private void ClearInputFields()
        {
            NameInput = null;
            IndexInput = 0;
            GrossInput = null;
            NetInput = null;
        }

        private void InitComboBox()
        {
            AddToSelectorList = new AddToSelector[]
            {
                new AddToSelector(0, "Add New"),
                new AddToSelector(1, "Add To Front"),
                new AddToSelector(2, "Add To End")
            };

            AddToSelection = AddToSelectorList[2];
        }
        #endregion

        #region - Full Properties
        public BindableCollection<Paystub> PaystubDataList
        {
            get { return _paystubDataList; }
            set
            {
                _paystubDataList = value;
                NotifyOfPropertyChange(() => PaystubDataList);
            }
        }

        public string NameInput
        {
            get { return _nameInput; }
            set
            {
                _nameInput = value;
                NotifyOfPropertyChange(() => NameInput);
            }
        }

        public int IndexInput
        {
            get { return _indexInput; }
            set
            {
                _indexInput = value;
                NotifyOfPropertyChange(() => IndexInput);
            }
        }

        public decimal? GrossInput
        {
            get { return _grossInput; }
            set
            {
                _grossInput = value;
                NotifyOfPropertyChange(() => GrossInput);
            }
        }

        public decimal? NetInput
        {
            get { return _netInput; }
            set
            {
                _netInput = value;
                NotifyOfPropertyChange(() => NetInput);
            }
        }

        public AddToSelector[] AddToSelectorList
        {
            get { return _addToSelectorList; }
            set
            {
                _addToSelectorList = value;
                NotifyOfPropertyChange(() => AddToSelectorList);
            }
        }

        public AddToSelector AddToSelection
        {
            get { return _addToSelection; }
            set
            {
                _addToSelection = value;
                NotifyOfPropertyChange(() => AddToSelection);
            }
        }
        #endregion
    }
}
