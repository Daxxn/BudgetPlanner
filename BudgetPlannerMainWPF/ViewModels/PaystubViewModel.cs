using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BudgetPlannerLib.Exceptions;
using BudgetPlannerLib.Models;
using Caliburn.Micro;

namespace BudgetPlannerMainWPF.ViewModels
{
    public class PaystubViewModel : Screen
    {
        #region - Fields
        private IEventAggregator _eventAggregator;

        private BindableCollection<PaystubModel> _paystubDataList = new BindableCollection<PaystubModel>();
        private PaystubModel _selectedPaystub;

        private PaystubType _newPaystubType;
        private decimal _paystubAmount;

        private bool _findMissingSelect = true;

        private decimal _grossAverage;
        private decimal _netAverage;
        private double _grossPercentage/* = 42.424244242 Testing */;
        private double _netPercentage;
        private double _percentageDiff;
        #endregion

        #region - Constructors
        public PaystubViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.Subscribe(this);

            #region Testing Data 1 (With Both gross & net for one paystub)
            PaystubDataList = new BindableCollection<PaystubModel>()
            {
                new PaystubModel(0, 500M, 250M),
                new PaystubModel(1, 33.4M),
                new PaystubModel(2, 400M),
                new PaystubModel(3, 286.64M),
                new PaystubModel(4, 22.54M)
            };
            #endregion
        }
        #endregion

        #region - Methods
        public void AddPaystub()
        {
            PaystubModel newPaystub = new PaystubModel();

            if(NewPaystubType == PaystubType.Gross)
            {
                newPaystub.Index = PaystubDataList.Count;
                newPaystub.Gross = PaystubAmount;
                PaystubDataList.Add(newPaystub);
                PaystubAmount = 0;
            }
            else if (NewPaystubType == PaystubType.Net)
            {
                newPaystub.Index = PaystubDataList.Count;
                newPaystub.Net = PaystubAmount;
                PaystubDataList.Add(newPaystub);
                PaystubAmount = 0;
            }
            else
            {
                MessageManager.DisplayMessage("You need to select gross or net from the drop-down menu");
            }
        }

        public void RemoveSelected()
        {
            PaystubDataList.Remove(SelectedPaystub);
        }

        public void CalculateAverage()
        {
            Tuple<decimal, decimal> output = PaystubModel.Average(PaystubDataList.ToArray(), NewPaystubType, false);

            GrossAverage = output.Item1;
            NetAverage = output.Item2;

            if (FindMissingSelect)
            {
                
            }
        }
        #endregion

        #region - Properties
        public BindableCollection<PaystubModel> PaystubDataList
        {
            get { return _paystubDataList; }
            set
            {
                _paystubDataList = value;
                NotifyOfPropertyChange(() => PaystubDataList);
            }
        }

        public PaystubModel SelectedPaystub
        {
            get { return _selectedPaystub; }
            set
            {
                _selectedPaystub = value;
                NotifyOfPropertyChange(() => SelectedPaystub);
            }
        }

        public PaystubType NewPaystubType
        {
            get { return _newPaystubType; }
            set
            {
                _newPaystubType = value;
                NotifyOfPropertyChange(() => NewPaystubType);
            }
        }

        public decimal PaystubAmount
        {
            get { return _paystubAmount; }
            set
            {
                _paystubAmount = value;
                NotifyOfPropertyChange(() => PaystubAmount);
            }
        }

        public IEnumerable<PaystubType> NewPaystubTypeDisplay
        {
            get
            {
                return Enum.GetValues(typeof(PaystubType)).Cast<PaystubType>();
            }
        }

        public decimal GrossAverage
        {
            get { return _grossAverage; }
            set
            {
                _grossAverage = value;
                NotifyOfPropertyChange(() => GrossAverage);
            }
        }

        public decimal NetAverage
        {
            get { return _netAverage; }
            set
            {
                _netAverage = value;
                NotifyOfPropertyChange(() => NetAverage);
            }
        }

        public double GrossPercentage
        {
            get { return _grossPercentage; }
            set
            {
                _grossPercentage = value;
                NotifyOfPropertyChange(() => GrossPercentage);
            }
        }

        public double NetPercentage
        {
            get { return _netPercentage; }
            set
            {
                _netPercentage = value;
                NotifyOfPropertyChange(() => NetPercentage);
            }
        }

        public double PercentageDifference
        {
            get { return _percentageDiff; }
            set
            {
                _percentageDiff = value;
                NotifyOfPropertyChange(() => PercentageDifference);
            }
        }

        public bool FindMissingSelect
        {
            get { return _findMissingSelect; }
            set
            {
                _findMissingSelect = value;
                NotifyOfPropertyChange(() => FindMissingSelect);
            }
        }
        #endregion
    }
}
