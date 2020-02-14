using BudgetPlannerMainWPF.Enums;
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
        private const string MainFileFilter = "Complete Budget Planner Project Files|*.bpm";
        private const string BudgetFileFilter = "Budget Files|*.bpb";
        private const string CategoryFileFilter = "Category Files|*.bpc";
        private const string PaystubFileFilter = "Paystub Files|*.bpp";
        private const string AllBudgetFilesFilter = "All Budget Planner Files|*.bpm;*.bpb*.bpc*.bpp";
        private const string AllFilesFilter = "All Files|*.*";
        #endregion

        /// <summary>
        /// Opens the SaveFileDialog and returns a path uri.
        /// </summary>
        /// <param name="dir">Initial Directory to start at.</param>
        /// <param name="fileName">The File name to suggest.</param>
        /// <param name="ext">The Extension the file should have.</param>
        /// <param name="dialogTitle">The title to display at the to of the dialog.</param>
        /// <returns>Returns the uri and a bool indicating success.</returns>
        public Tuple<string, bool> SaveFileAccess(string dir, string fileName, ExtensionType ext, string dialogTitle)
        {
            Tuple<string, bool> output;
            string extension = "";
            string filter = "";
            if (ext == ExtensionType.Main)
            {
                extension = Properties.Resources.MainExtension;
                filter = $"{MainFileFilter}|{AllBudgetFilesFilter}|{AllFilesFilter}";
            }
            else if (ext == ExtensionType.Budget)
            {
                extension = Properties.Resources.BudgetExtension;
                filter = $"{BudgetFileFilter}|{AllBudgetFilesFilter}|{AllFilesFilter}";
            }
            else if (ext == ExtensionType.Category)
            {
                extension = Properties.Resources.CategoryExtension;
                filter = $"{CategoryFileFilter}|{AllBudgetFilesFilter}|{AllFilesFilter}";
            }
            else if (ext == ExtensionType.Paystub)
            {
                extension = Properties.Resources.PaystubExtension;
                filter = $"{PaystubFileFilter}|{AllBudgetFilesFilter}|{AllFilesFilter}";
            }
            else
            {
                throw new Exception("Extension type doesnt exist. How did you do that??");
            }

            Saver = new SaveFileDialog()
            {
                AddExtension = true,
                CheckFileExists = false,
                CheckPathExists = true,
                DefaultExt = extension,
                InitialDirectory = dir,
                OverwritePrompt = true,
                FileName = fileName,
                Filter = filter,
                Title = dialogTitle
            };

            if (Saver.ShowDialog() is true)
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
                filter = $"{CategoryFileFilter}|{AllBudgetFilesFilter}|{AllFilesFilter}";
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
                filter = $"{CategoryFileFilter}|{AllBudgetFilesFilter}|{AllFilesFilter}";
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
                filter = $"{CategoryFileFilter}|{AllBudgetFilesFilter}|{AllFilesFilter}";
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

            if(Opener.ShowDialog() is true)
            {
                output = Tuple.Create(Opener.FileName, true);
            }
            else
            {
                output = Tuple.Create("", false);
            }
            
            return output;
        }

        public Tuple<string, bool> OpenFileAccess(string dir, string fileName, ExtensionType ext, string dialogTitle)
        {
            Tuple<string, bool> output;
            string extension = "";
            string filter = "";

            if (ext == ExtensionType.Main)
            {
                extension = Properties.Resources.MainExtension;
                filter = $"{MainFileFilter}|{AllBudgetFilesFilter}|{AllFilesFilter}";
            }
            else if (ext == ExtensionType.Budget)
            {
                extension = Properties.Resources.BudgetExtension;
                filter = $"{BudgetFileFilter}|{AllBudgetFilesFilter}|{AllFilesFilter}";
            }
            else if (ext == ExtensionType.Category)
            {
                extension = Properties.Resources.CategoryExtension;
                filter = $"{CategoryFileFilter}|{AllBudgetFilesFilter}|{AllFilesFilter}";
            }
            else if (ext == ExtensionType.Paystub)
            {
                extension = Properties.Resources.PaystubExtension;
                filter = $"{PaystubFileFilter}|{AllBudgetFilesFilter}|{AllFilesFilter}";
            }
            else
            {
                throw new Exception("Extension type doesnt exist. How did you do that??");
            }

            Opener = new OpenFileDialog()
            {
                AddExtension = true,
                CheckFileExists = true,
                CheckPathExists = true,
                DefaultExt = extension,
                FileName = fileName,
                Filter = filter,
                InitialDirectory = dir,
                Multiselect = false,
                Title = dialogTitle
            };

            if (Opener.ShowDialog() is true)
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
