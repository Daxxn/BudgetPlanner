using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public string SaveFileAccess(string initialDir, string title, bool isMainFile)
        {
            string output = "";

            string defExt = ".bps";
            if (isMainFile)
            {
                defExt = ".bpn";
            }

            Saver = new SaveFileDialog()
            {
                InitialDirectory = initialDir,
                Title = title,
                OverwritePrompt = true,
                AddExtension = true,
                DefaultExt = defExt
            };

            if(Saver.ShowDialog() == true)
            {
                output = Saver.FileName;
            }

            return output;
        }

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
                DefaultExt = defExt
            };

            if(Opener.ShowDialog() == true)
            {
                output = Opener.FileName;
            }

            return output;
        }
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
