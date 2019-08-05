using System;
using System.Windows.Forms;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
using SaveFileDialog = Microsoft.Win32.SaveFileDialog;

namespace BudgetPlannerMainWPF
{
    public class FileBrowser : IFileBrowser
    {
        private SaveFileDialog Saver { get; set; }
        private OpenFileDialog Opener { get; set; }
        private FolderBrowserDialog Folder { get; set; }

        #region Filter Strings
        private const string MainFileFilter = "Budget Planner Project Files|*.bpn";
        private const string SubFileFilter = "Budget Planner Sub Category Files|*.bps";
        private const string AllBudgetFilesFilter = "All Budget Planner Files|*.bpn;*.bps";
        private const string AllFilesFilter = "All Files|*.*";
        #endregion


        /// <summary>
        /// Opens the WPF save file dialog window.
        /// </summary>
        /// <param name="initialDir">Default dir to start.</param>
        /// <param name="title">Window Title.</param>
        /// <param name="projName">Sets the FileName to the project name by default.</param>
        /// <param name="isMainFile">Specifies what file extension to look for.</param>
        /// <returns>Returns the path to the selected file.</returns>
        public Tuple<string, bool> SaveFileAccess(string initialDir, string title, string projName, bool isMainFile)
        {
            Tuple<string, bool> output;
            string defExt = "";
            string filter = AllFilesFilter;

            if (isMainFile)
            {
                defExt = ".bpn";
                filter = $"{MainFileFilter}|{AllBudgetFilesFilter}|{AllFilesFilter}";
            }
            else
            {
                defExt = ".bps";
                filter = $"{SubFileFilter}|{AllBudgetFilesFilter}|{AllFilesFilter}";
            }

            Saver = new SaveFileDialog()
            {
                InitialDirectory = initialDir,
                Title = title,
                OverwritePrompt = true,
                AddExtension = true,
                DefaultExt = defExt,
                FileName = projName,
                Filter = filter
            };

            if(Saver.ShowDialog() == true)
            {
                output = Tuple.Create(Saver.FileName, true);
            }
            else
            {
                output = Tuple.Create("", false);
            }

            return output;
        }

        /// <summary>
        /// Opens the WPF save file dialog window.
        /// </summary>
        /// <param name="initialDir">Default dir to start.</param>
        /// <param name="title">Window Title.</param>
        /// <param name="isMainFile">Specifies what file extension to look for.</param>
        /// <returns>Returns the path to the selected file.</returns>
        public Tuple<string, bool> SaveFileAccess(string initialDir, string title, bool isMainFile)
        {
            Tuple<string, bool> output;
            string defExt = "";
            string filter = AllFilesFilter;

            if (isMainFile)
            {
                defExt = ".bpn";
                filter = $"{MainFileFilter}|{AllBudgetFilesFilter}|{AllFilesFilter}";
            }
            else
            {
                defExt = ".bps";
                filter = $"{SubFileFilter}|{AllBudgetFilesFilter}|{AllFilesFilter}";
            }

            Saver = new SaveFileDialog()
            {
                InitialDirectory = initialDir,
                Title = title,
                OverwritePrompt = true,
                AddExtension = true,
                DefaultExt = defExt,
                Filter = filter
            };

            if (Saver.ShowDialog() == true)
            {
                output = Tuple.Create(Saver.FileName, true);
            }
            else
            {
                output = Tuple.Create("", false);
            }

            return output;
        }

        /// <summary>
        /// Opens the WPF open file dialog window.
        /// </summary>
        /// <param name="initialDir">Default dir to start.</param>
        /// <param name="title">Window Title.</param>
        /// <param name="isMainFile">Specifies the file extension to look for.</param>
        /// <returns>Returns the path to the selected file.</returns>
        public Tuple<string, bool> OpenFileAccess(string initialDir, string title, bool isMainFile)
        {
            Tuple<string, bool> output;
            string defExt = ".bps";
            string filter = AllFilesFilter;

            if (isMainFile)
            {
                defExt = ".bpn";
                filter = $"{MainFileFilter}|{AllBudgetFilesFilter}|{AllFilesFilter}";
            }
            else
            {
                defExt = ".bps";
                filter = $"{SubFileFilter}|{AllBudgetFilesFilter}|{AllFilesFilter}";
            }

            Opener = new OpenFileDialog()
            {
                Multiselect = false,
                InitialDirectory = initialDir,
                Title = title,
                DefaultExt = defExt,
                AddExtension = true,
                Filter = filter
            };

            if(Opener.ShowDialog() == true)
            {
                output = Tuple.Create(Opener.FileName, true);
            }
            else
            {
                output = Tuple.Create("", false);
            }
            
            return output;
        }

        /// <summary>
        /// Opens the WinForm open folder dialog window.
        /// </summary>
        /// <param name="description">Window Title</param>
        /// <returns>Returns the path to the selected file.</returns>
        public string OpenFolderAccess(string description)
        {
            string output = "";
            FolderBrowserDialog folderDialog = new FolderBrowserDialog()
            {
                Description = description,
                RootFolder = Environment.SpecialFolder.MyComputer
            };

            DialogResult result = folderDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                output = folderDialog.SelectedPath;
            }
            else
            {
                output = "Something went wrong!!";
            }

            folderDialog.Dispose();
            return output;
        }
    }
}
