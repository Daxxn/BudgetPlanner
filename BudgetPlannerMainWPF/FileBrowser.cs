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

        /// <summary>
        /// Opens the WPF save file dialog window.
        /// </summary>
        /// <param name="initialDir">Default dir to start.</param>
        /// <param name="title">Window Title.</param>
        /// <param name="isMainFile">Specifies what file extension to look for.</param>
        /// <returns>Returns the path to the selected file.</returns>
        public string SaveFileAccess(string initialDir, string title, bool isMainFile)
        {
            string output = "";
            string tempName = "";
            string budgetFilter = "Budget Files (*.bpn, *.bps)|*.bpn;*.bps|All Files (*.*)|*.*";

            string defExt = ".bps";
            if (isMainFile)
            {
                defExt = ".bpn";
                tempName = "Suggested Name Test.bpn";
            }

            Saver = new SaveFileDialog()
            {
                InitialDirectory = initialDir,
                Title = title,
                OverwritePrompt = true,
                AddExtension = true,
                DefaultExt = defExt,
                FileName = tempName,
                Filter = budgetFilter
            };

            if(Saver.ShowDialog() == true)
            {
                output = Saver.FileName;
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
        public string OpenFileAccess(string initialDir, string title, bool isMainFile)
        {
            string output = "";
            string defExt = ".bps";
            if (isMainFile)
            {
                defExt = ".bpn";
            }

            Opener = new OpenFileDialog()
            {
                Multiselect = false,
                InitialDirectory = initialDir,
                Title = title,
                DefaultExt = defExt,
                AddExtension = true
            };

            if(Opener.ShowDialog() == true)
            {
                output = Opener.FileName;
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
