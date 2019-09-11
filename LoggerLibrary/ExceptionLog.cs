using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;

namespace LoggerLibrary
{
    public static class ExceptionLog
    {
        #region - Fields
        public static List<Log> Exceptions { get; set; }
        public static string LogFolderPath { get; set; }
        private static string LogFilePath { get; set; }

        //public static string TestFolderPath { get; set; } = @"C:\Users\Daxxn\Documents\GitHub Repositories\BudgetPlanner\ExceptionLogFolder";
        #endregion

        #region - Methods
        public static void LogException(Log e)
        {
            if(Exceptions is null)
            {
                Exceptions = new List<Log>();
            }

            Exceptions.Add(e);
        }

        public static void LogException(Exception e, DateTime time)
        {
            if (Exceptions is null)
            {
                Exceptions = new List<Log>();
            }

            Exceptions.Add(new Log(e, time));
            
        }

        public static string[] WriteLogFileString()
        {
            List<string> logData = new List<string>();

            if (Exceptions != null && Exceptions.Count > 0)
            {
                DateTime logTime = DateTime.Now;

                logData.Add($"Exception Log File Created : {logTime.ToLongDateString()} | {logTime.ToLongTimeString()}\n");

                foreach (var log in Exceptions)
                {
                    logData.Add($"{log.ToString()}\n\n");
                }
            }

            return logData.ToArray();
        }

        /// <summary>
        /// Finds the ExceptionLogFolder using System.Reflection and sets the LogFolderPath property.
        /// </summary>
        public static void OnStartup()
        {
            Assembly thisAssm = Assembly.GetAssembly(typeof(ExceptionLog));
            DirectoryInfo nextDir = Directory.GetParent(thisAssm.Location);

            bool success = false;

            while (!success)
            {
                string debugDisplay = nextDir.Name;

                if (nextDir.Name != "BudgetPlanner")
                {
                    nextDir = Directory.GetParent(nextDir.FullName);
                    success = false;
                }
                else
                {
                    if(Directory.Exists(nextDir.FullName + "\\ExceptionLogFolder"))
                    {
                        LogFolderPath = nextDir.FullName + "\\ExceptionLogFolder";
                        success = true;
                    }
                    else
                    {
                        Directory.CreateDirectory(nextDir.FullName + "\\ExceptionLogFolder");
                        success = true;
                    }
                }
            }
        }

        public static void CreateLogFilePath()
        {

        }

        private static void CheckLogFileLength()
        {
            string[] logFilePaths = Directory.GetFiles(LogFolderPath, "*.log", SearchOption.TopDirectoryOnly);

            if (logFilePaths.Length >= 20)
            {
                Dictionary<string, DateTime> logFiles = new Dictionary<string, DateTime>();

                foreach (string logFile in logFilePaths)
                {
                    logFiles.Add(logFile, Directory.GetCreationTime(logFile));
                }

                // Need to relearn the linq expression to sort the DateTimes AGAIN!!
                var sortedLogs = logFiles.OrderBy(x => x.Value).ToArray();
            }
        }

        /// <summary>
        /// Saves logs to a .txt file.
        /// ExitCode: 0 = No Errors Saved, 1 = Errors Saved, 2 = Log Folder not found, 3 = Bad Log Path, Exception thrown during log save.
        /// </summary>
        /// <returns>Returns an exit code.</returns>
        public static uint OnExit()
        {
            uint exitCode;
            if(Exceptions != null)
            {
                if (Exceptions.Count > 0)
                {
                    if (LogFolderPath != null)
                    {
                        if(LogFilePath != null)
                        {
                            try
                            {
                                File.WriteAllLines(LogFilePath, WriteLogFileString());
                                exitCode = 1;
                            }
                            catch (Exception)
                            {
                                exitCode = 4;
                            }
                        }
                        else
                        {
                            exitCode = 3;
                        }
                    }
                    else
                    {
                        exitCode = 2;
                    }
                }
                else
                {
                    exitCode = 0;
                }
            }
            else
            {
                exitCode = 0;
            }

            return exitCode;
        }
        #endregion

        #region - Properties

        #endregion
    }
}
