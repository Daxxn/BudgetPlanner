using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BudgetPlannerMainWPF.ViewModels;

namespace BudgetPlannerMainWPF.Views
{
    /// <summary>
    /// Interaction logic for DataView.xaml Only Forwards Events.
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
