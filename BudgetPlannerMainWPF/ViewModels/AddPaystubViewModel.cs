using BudgetPlannerMainWPF.EventModels;
using Caliburn.Micro;
using PaystubLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BudgetPlannerMainWPF.ViewModels
{
    public class AddPaystubViewModel : Caliburn.Micro.Screen
    {
        #region - Fields & Properties
        private IWindowManager _windowManager;
        private IEventAggregator _eventAggregator;

        private List<Paystub> _paystubDataList;

        private string _dataInput;
        private int _indexInput;
        private decimal _grossInput;
        private decimal _netInput;
        #endregion

        #region - Constructors
        public AddPaystubViewModel() { }
        public AddPaystubViewModel(IWindowManager windowManager, IEventAggregator eventAggregator)
        {
            _windowManager = windowManager;
            _eventAggregator = eventAggregator;
            _eventAggregator.Subscribe(this);

            PaystubDataList = new List<Paystub>();
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
            PaystubDataList.Add(
                new Paystub(DataInput, IndexInput, GrossInput, NetInput)
                );
            ClearInputFields();
        }
        public void SendDataTest()
        {
            _eventAggregator.PublishOnUIThread(new AddManyPaystubsEventModel(PaystubDataList));
            ClearInputFields();
            PaystubDataList = new List<Paystub>();
            this.TryClose(null);
        }

        private void ClearInputFields()
        {
            DataInput = String.Empty;
            IndexInput = 0;
            GrossInput = 0;
            NetInput = 0;
        }
        #endregion

        #region - Full Properties
        public List<Paystub> PaystubDataList
        {
            get { return _paystubDataList; }
            set
            {
                _paystubDataList = value;
                NotifyOfPropertyChange(() => PaystubDataList);
            }
        }
        public string DataInput
        {
            get { return _dataInput; }
            set
            {
                _dataInput = value;
                NotifyOfPropertyChange(() => DataInput);
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

        public decimal GrossInput
        {
            get { return _grossInput; }
            set
            {
                _grossInput = value;
                NotifyOfPropertyChange(() => GrossInput);
            }
        }

        public decimal NetInput
        {
            get { return _netInput; }
            set
            {
                _netInput = value;
                NotifyOfPropertyChange(() => NetInput);
            }
        }
        #endregion
    }
}
