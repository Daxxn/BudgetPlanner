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

        /// <summary>
        /// Connects the ShellViewModel & ShellView together and displays them.
        /// </summary>
        /// <param name="sender">Application startup sender</param>
        /// <param name="e">Application startup event arguments</param>
        protected override void OnStartup(Object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<ShellViewModel>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnExit(Object sender, EventArgs e)
        {
            ShellViewModel.Exit();
            base.OnExit(sender, e);
        }
    }
}
