using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        #endregion

        #region - Constructors
        public PaystubViewModel(IEventAggregator eventAggregator, IFileBrowser fileBrowser)
        {
            _eventAggregator = eventAggregator;
            _fileBrowser = fileBrowser;

            #region Testing ONLY
            PaystubDataList = new BindableCollection<Paystub>()
            {
                new Paystub(1, 200, 20),
                new Paystub(2, 400, 40),
                new Paystub(3, 300, 30),
                new Paystub(4, 100, 10)
            };
            #endregion
        }
        #endregion

        #region - Methods

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
        #endregion
    }
}
