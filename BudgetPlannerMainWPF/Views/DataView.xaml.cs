using System;
using System.Windows.Controls;
using System.Windows.Input;
using BudgetPlannerMainWPF.ViewModels;

namespace BudgetPlannerMainWPF.Views
{
    /// <summary>
    /// Interaction logic for DataView.xaml : Only Forwards Events :
    /// </summary>
    public partial class DataView : UserControl
    {
        public static event EventHandler<SimpleKeyEventAgrs> SendEnter;

        public DataView()
        {
            InitializeComponent();
        }

        private void ExpenseDataList_KeyDown(Object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SendEnter?.Invoke(sender, new SimpleKeyEventAgrs(1));
            }
        }

        private void IncomeDataList_KeyDown(Object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SendEnter?.Invoke(sender, new SimpleKeyEventAgrs(2));
            }
        }
    }
}