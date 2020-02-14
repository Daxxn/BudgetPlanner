using System;
using BudgetPlannerMainWPF.Enums;

namespace BudgetPlannerMainWPF
{
    public interface IFileBrowser
    {
        Tuple<string, bool> SaveFileAccess(string directory, string fileName, ExtensionType extension, string dialogTitle);

        /// <summary>
        /// Opens the WPF save file dialog window.
        /// </summary>
        /// <param name="initDir">Default dir to start.</param>
        /// <param name="title">Window Title.</param>
        /// <param name="isMainFile">Specifies what file extension to look for.</param>
        /// <returns>Returns the path to the selected file.</returns>
        Tuple<string, bool> SaveFileAccess(string initDir, string title, bool isMainFile);

        /// <summary>
        /// Opens the WPF save file dialog window.
        /// </summary>
        /// <param name="initDir">Default dir to start.</param>
        /// <param name="title">Window Title.</param>
        /// <param name="projName">Sets the FileName to the project name by default.</param>
        /// <param name="isMainFile">Specifies what file extension to look for.</param>
        /// <returns>Returns the path to the selected file.</returns>
        Tuple<string, bool> SaveFileAccess(string initDir, string title, string projName, bool isMainFile);

        Tuple<string, bool> OpenFileAccess(string directory, string fileName, ExtensionType extension, string dialogTitle);

        /// <summary>
        /// Opens the WPF open file dialog window.
        /// </summary>
        /// <param name="initDir">Default dir to start.</param>
        /// <param name="title">Window Title.</param>
        /// <param name="isMainFile">Specifies the file extension to look for.</param>
        /// <returns>Returns the path to the selected file.</returns>
        Tuple<string, bool> OpenFileAccess(string initDir, string title, bool isMainFile);

        /// <summary>
        /// Opens the WinForm open folder dialog window.
        /// </summary>
        /// <param name="description">Window Title</param>
        /// <returns>Returns the path to the selected file.</returns>
        string OpenFolderAccess(string description);
    }
}