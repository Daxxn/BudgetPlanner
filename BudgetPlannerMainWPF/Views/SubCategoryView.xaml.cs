using System;
using System.Windows.Controls;
using System.Windows.Input;
using BudgetPlannerMainWPF.ViewModels;

namespace BudgetPlannerMainWPF.Views
{
    /// <summary>
    /// Interaction logic for SubCategoryView.xaml Only Forwards Events.
    /// </summary>
    public partial class SubCategoryView : UserControl
    {
        public static event EventHandler<SimpleKeyEventAgrs> SendEnter;

        public SubCategoryView()
        {
            InitializeComponent();
        }

        private void NewIncomeName_KeyDown(Object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SendEnter?.Invoke(sender, new SimpleKeyEventAgrs(1));
            }
        }

        private void NewExpenseName_KeyDown(Object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SendEnter?.Invoke(sender, new SimpleKeyEventAgrs(2));
            }
        }
    }
}