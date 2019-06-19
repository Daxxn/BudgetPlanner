using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BudgetPlannerMainWPF.ViewModels;
using System.IO;

namespace BudgetPlannerMainWPF.Views
{
    /// <summary>
    /// Interaction logic for NewBudgetView.xaml
    /// </summary>
    public partial class NewBudgetView : System.Windows.Controls.UserControl
    {
        //private bool fileOpened = false;
        //private string FolderName { get; set; }

        //public event EventHandler<FolderEventArgs> FolderSelected;

        public NewBudgetView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Needs to be tested!!!
        /// </summary>
        //private void NewSavePath_Click(Object sender, RoutedEventArgs e)
        //{
        //    FolderBrowserDialog folderBrowser = new FolderBrowserDialog
        //    {
        //        ShowNewFolderButton = true,
        //        Description = "Select a path for the new project:"
        //    };

        //    DialogResult result = folderBrowser.ShowDialog();

        //    if (result == DialogResult.OK)
        //    {
        //        FolderName = folderBrowser.SelectedPath;
        //        if (Directory.Exists(FolderName))
        //        {
        //            FolderSelected?.Invoke(this, new FolderEventArgs(FolderName));
        //        }
        //    }
        //}
    }
}
