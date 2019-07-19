namespace BudgetPlannerMainWPF
{
    public interface IFileBrowser
    {
        /// <summary>
        /// Opens the WPF save file dialog window.
        /// </summary>
        /// <param name="initialDir">Default dir to start.</param>
        /// <param name="title">Window Title.</param>
        /// <param name="isMainFile">Specifies what file extension to look for.</param>
        /// <returns>Returns the path to the selected file.</returns>
        string SaveFileAccess(string initialDir, string title, bool isMainFile);

        /// <summary>
        /// Opens the WPF open file dialog window.
        /// </summary>
        /// <param name="initialDir">Default dir to start.</param>
        /// <param name="title">Window Title.</param>
        /// <param name="isMainFile">Specifies the file extension to look for.</param>
        /// <returns>Returns the path to the selected file.</returns>
        string OpenFileAccess(string initialDir, string title, bool isMainFile);

        /// <summary>
        /// Opens the WinForm open folder dialog window.
        /// </summary>
        /// <param name="description">Window Title</param>
        /// <returns>Returns the path to the selected file.</returns>
        string OpenFolderAccess(string description);
    }
}