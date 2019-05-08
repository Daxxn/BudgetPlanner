using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BudgetPlannerMainWPF.ViewModels;
using Caliburn.Micro;

namespace BudgetPlannerMainWPF
{
    public class Bootstrapper : BootstrapperBase
    {
        public Bootstrapper()
        {
            Initialize();
        }

        protected override void OnStartup(Object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<ShellViewModel>();
        }

        protected override void OnExit(Object sender, EventArgs e)
        {
            ShellViewModel.Exit();
        }
    }
}
