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

        public int _accuracyDisp;
        public double _accuracy;
        public int _compPaystubCountDisp = 1;
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
            PaystubCollection.Accuracy = Accuracy;
            PaystubCollection.PassCount = CompletePaystubCountDisplay;

            PaystubCollection test = new PaystubCollection()
            {
                Name = "Test",
                Paystubs = PaystubDataList.ToList()
            };

            test.BeginCalc(MessageManager.DisplayMessage, ConvertWarning);

            PaystubDataList = new BindableCollection<Paystub>(test.Paystubs);
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
        public BindableCollection<Paystub> PaystubDataList
        {
            get { return _paystubDataList; }
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

        public int AccuracyDisplay
        {
            get { return _accuracyDisp; }
            set
            {
                _accuracyDisp = value;
                NotifyOfPropertyChange(() => AccuracyDisplay);
                NotifyOfPropertyChange(() => Accuracy);
            }
        }

        public double Accuracy
        {
            get
            {
                if (AccuracyDisplay != 0)
                {
                    return 1 - ((double)AccuracyDisplay / 100);
                }
                else { return 0; }
            }
        }

        public double Accuracy_old
        {
            get { return _accuracy; }
            set
            {
                _accuracy = value;
                NotifyOfPropertyChange(() => Accuracy);
            }
        }

        public int CompletePaystubCountDisplay
        {
            get { return _compPaystubCountDisp; }
            set
            {
                _compPaystubCountDisp = value;
                NotifyOfPropertyChange(() => CompletePaystubCountDisplay);
            }
        }
        #endregion
    }
}
