using System;
using System.Collections.Generic;
using System.IO;

namespace BudgetPlannerMainWPF
{
    /// <summary>
    /// Static class used to check the validity of path strings.
    /// </summary>
    public static class FileCheck
    {
        /// <summary>
        /// Checks the path for null, length, and directory existance
        /// </summary>
        /// <param name="path">Full string path to check.</param>
        /// <returns>True if the path exists.</returns>
        public static bool CheckDirectory(string path)
        {
            if (path != null && path.Length > 3)
            {
                if (Directory.Exists(path))
                {
                    return true;
                }
                else return false;
            }
            else return false;
        }

        /// <summary>
        /// Checks the path for null, length, and File existance.
        /// </summary>
        /// <param name="path">Full string path to check.</param>
        /// <returns>True if the path exists.</returns>
        public static bool CheckFile(string path)
        {
            if (path != null && path.Length > 3)
            {
                if (File.Exists(path))
                {
                    return true;
                }
                else return false;
            }
            else return false;
        }
    }
}
