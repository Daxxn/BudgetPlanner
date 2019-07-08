namespace BudgetPlannerMainWPF
{
    public interface IFileBrowser
    {
        string SaveFileAccess(string initialDir, string title, bool isMainFile);
        string OpenFileAccess(string initialDir, string title, bool isMainFile);
        string OpenFolderAccess(string description);
    }
}