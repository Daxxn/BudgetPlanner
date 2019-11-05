using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Caliburn.Micro;
using PaystubLibrary;

namespace BudgetPlannerMainWPF.ViewModels
{
    public class PaystubViewModel : Screen
    {
        #region - Fields & Properties
        private IEventAggregator _eventAggregator;
        private IFileBrowser _fileBrowser;

        public BindableCollection<Paystub> _paystubDataList;
        public Paystub _selectedPaystub;

        public int _accuracyInputDisp;
        public double _accuracyInput;
        public int _compPaystubCountInputDisp = 1;

        public string _collectionName = "No Name";
        public string _collectionDesc = "<empty>";

        public decimal _averageGross;
        public decimal _averageNet;
        public decimal _averagePercent;

        public CalcType _decision = CalcType.NotEnoughInfo;
        public string _decisionDisp = String.Empty;
        public bool _decisionBackground = false;

        public Warning _warning = Warning.NoWarning;
        public string _warningMessage;
        public bool _warningBackground;


        public decimal _percentDiff;

        /// <summary>
        /// Hold on... probably gonna do this differently. use an int and just count it.
        /// </summary>
        public decimal _compPaystubOut;
        #endregion

        #region - Constructors
        public PaystubViewModel(IEventAggregator eventAggregator, IFileBrowser fileBrowser)
        {
            _eventAggregator = eventAggregator;
            _fileBrowser = fileBrowser;
            
            #region Testing ONLY
            #region Test 1
            //PaystubDataList = new BindableCollection<Paystub>()
            //{
            //    new Paystub(1, 200, 20),
            //    new Paystub(2, 400, 40),
            //    new Paystub(3, 300, 30),
            //    new Paystub(4, 100, 10)
            //};
            #endregion

            #region Test 2
            //PaystubDataList = new BindableCollection<Paystub>()
            //{
            //    new Paystub(1, 200, true),
            //    new Paystub(1, 400, true),
            //    new Paystub(1, 200, true),
            //    new Paystub(1, 800, true),
            //};
            #endregion

            #region Test 3
            //PaystubDataList = new BindableCollection<Paystub>()
            //{
            //    new Paystub(1, 200, true),
            //    new Paystub(1, 0, true),
            //    new Paystub(1, 0, true),
            //    new Paystub(1, 0, true),
            //};
            #endregion

            #region Test 4
            PaystubDataList = new BindableCollection<Paystub>()
            {
                new Paystub(1, 200, 50),
                new Paystub(1, 400, true),
                new Paystub(1, 100, 20),
                new Paystub(1, 800, true),
            };
            #endregion
            #endregion
        }
        #endregion

        #region - Methods
        #region -- Buttons
        public void AddPaystub()
        {
            PaystubDataList.Add(Paystub.Default(PaystubDataList.Count));
        }

        public void RemovePaystub()
        {
            PaystubDataList.Remove(SelectedPaystub);
        }

        public void CalculatePaystubs()
        {
            Tuple<List<Paystub>, Tuple<decimal, decimal, decimal>, Tuple<decimal, decimal>> calcOut = 
                PaystubCalculator.BeginCalc(
                    MessageManager.DisplayMessage, 
                    ConvertWarning,
                    PaystubDataList.ToList(), 
                    (decimal)AccuracyInput
                    );
            
            Decision = PaystubCalculator.Decision;

            //Tuple 1
            PaystubDataList = new BindableCollection<Paystub>(calcOut.Item1);

            // Tuple 2
            AverageGross = calcOut.Item2.Item1;
            AverageNet = calcOut.Item2.Item2;
            AveragePercent = calcOut.Item2.Item3;

            // Tuple 3
            PercentDifference = calcOut.Item3.Item1;
            CompletePaystubOut = calcOut.Item3.Item2;
        }

        public void CalculatePercentages()
        {
            PaystubDataList = new BindableCollection<Paystub>(Paystub.GetPercentages(PaystubDataList.ToList()));
        }

        public void ResetTest()
        {
            PaystubDataList = new BindableCollection<Paystub>()
            {
                new Paystub(1, 200, 50),
                new Paystub(1, 400, true),
                new Paystub(1, 100, 2),
                new Paystub(1, 800, true),
            };
        }
        #endregion

        public void ConvertWarning(Warning warning)
        {
            Warning = warning;
            switch (warning)
            {
                case Warning.NoWarning:
                    //MessageManager.DisplayMessage("All accuracy within tolerance.");
                    break;
                case Warning.LowAccuracy:
                    MessageManager.DisplayMessage("The accuracy is below the desired tolerance.");
                    break;
                case Warning.LowCompletePaystubs:
                    MessageManager.DisplayMessage("The number of completed paystubs is low.");
                    break;
                default:
                    throw new Exception("Warning message is not recognized.");
            }
        }

        #endregion

        #region - Full Properties
        public string CollectionName
        {
            get { return _collectionName; }
            set
            {
                _collectionName = value;
                NotifyOfPropertyChange(() => CollectionName);
            }
        }

        public string CollectionDescription
        {
            get { return _collectionDesc; }
            set
            {
                _collectionDesc = value;
                NotifyOfPropertyChange(() => CollectionDescription);
            }
        }
        public BindableCollection<Paystub> PaystubDataList
        {
            get
            {
                return _paystubDataList;
            }
            set
            {
                _paystubDataList = value;
                NotifyOfPropertyChange(() => PaystubDataList);
            }
        }

        public Paystub SelectedPaystub
        {
            get { return _selectedPaystub; }
            set
            {
                _selectedPaystub = value;
                NotifyOfPropertyChange(() => SelectedPaystub);
            }
        }

        public int AccuracyInputDisplay
        {
            get { return _accuracyInputDisp; }
            set
            {
                _accuracyInputDisp = value;
                NotifyOfPropertyChange(() => AccuracyInputDisplay);
                NotifyOfPropertyChange(() => AccuracyInput);
            }
        }

        public double AccuracyInput
        {
            get
            {
                if (AccuracyInputDisplay != 0)
                {
                    return 1 - ((double)AccuracyInputDisplay / 100);
                }
                else { return 0; }
            }
        }

        public int CompletePaystubCountInputDisplay
        {
            get { return _compPaystubCountInputDisp; }
            set
            {
                _compPaystubCountInputDisp = value;
                NotifyOfPropertyChange(() => CompletePaystubCountInputDisplay);
            }
        }

        #region Calculator Properties
        public decimal AverageGross
        {
            get { return _averageGross; }
            set
            {
                _averageGross = value;
                NotifyOfPropertyChange(() => AverageGross);
            }
        }

        public decimal AverageNet
        {
            get { return _averageNet; }
            set
            {
                _averageNet = value;
                NotifyOfPropertyChange(() => AverageNet);
            }
        }

        public decimal AveragePercent
        {
            get { return _averagePercent; }
            set
            {
                _averagePercent = value;
                NotifyOfPropertyChange(() => AveragePercent);
            }
        }

        public CalcType Decision
        {
            get { return _decision; }
            set
            {
                _decision = value;
                DecisionDisp = value.ToString();

                bool decisionBackground = false;

                if(value == CalcType.CalcGross || value == CalcType.CalcNet)
                {
                    decisionBackground = true;
                }

                DecisionBackground = decisionBackground;

                NotifyOfPropertyChange(() => Decision);
            }
        }

        public string DecisionDisp
        {
            get { return _decisionDisp; }
            set
            {
                _decisionDisp = value;
                NotifyOfPropertyChange(() => DecisionDisp);
            }
        }

        public bool DecisionBackground
        {
            get { return _decisionBackground; }
            set
            {
                _decisionBackground = value;
                NotifyOfPropertyChange(() => DecisionBackground);
            }
        }

        public Warning Warning
        {
            get { return _warning; }
            set
            {
                _warning = value;
                WarningMessage = Warning.ToString();
                bool warnBackground = true;

                if (value != Warning.NoWarning)
                {
                    warnBackground = false;
                }

                WarningBackground = warnBackground;
                NotifyOfPropertyChange(() => Warning);
            }
        }

        public string WarningMessage
        {
            get { return _warningMessage; }
            set
            {
                _warningMessage = value;
                NotifyOfPropertyChange(() => WarningMessage);
            }
        }

        public bool WarningBackground
        {
            get { return _warningBackground; }
            set
            {
                _warningBackground = value;
                NotifyOfPropertyChange(() => WarningBackground);
            }
        }

        public decimal PercentDifference
        {
            get { return _percentDiff; }
            set
            {
                _percentDiff = value;
                NotifyOfPropertyChange(() => PercentDifference);
            }
        }

        public decimal CompletePaystubOut
        {
            get { return _compPaystubOut; }
            set
            {
                _compPaystubOut = value;
                NotifyOfPropertyChange(() => CompletePaystubOut);
            }
        }
        #endregion
        #endregion
    }
}
