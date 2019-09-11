//using System;
//using System.Collections.Generic;
//using System.Reflection;
//using System.IO;
//using System.Text;
//using BudgetPlannerLib.Models;

//namespace BudgetPlannerMainWPF
//{
//    /// <summary>
//    /// Need to work out how to implement.
//    /// </summary>
//    public class ExceptionLogger : IExceptionLogger
//    {
//        #region - Fields
//        private List<Exception> SessionExceptions { get; set; } = new List<Exception>();
//        private List<string> SessionInfo { get; set; } = new List<string>();
//        private List<string> SessionWarnings { get; set; } = new List<string>();

//        public static DateTime SessionDateTime { get; set; }
//        public static string ExceptionFolderPath { get; private set; }
//        public static string ExceptionLogFilePath { get; private set; }
//        #endregion

//        #region - Constructors
//        public ExceptionLogger() { }
//        #endregion

//        #region - Methods

//        private void SaveToFile()
//        {
//            StreamWriter writer = new StreamWriter(ExceptionLogFilePath) { AutoFlush = true };
//        }

//        public void Info(string message, Income[] incomeData, Expense[] expenseData, bool isFileOpen, bool isSubOpen)
//        {
//            string output = message + " | ";

//            output += $"Income Count: {incomeData.Length} | Expense Count: {expenseData.Length} | Is file Open: {isFileOpen.ToString()} | Is Sub File Open: {isSubOpen.ToString()}";

//            SessionInfo.Add(output);
//        }

//        public void Warn(string message, Income[] incomeData, Expense[] expenseData, bool isFileOpen, bool isSubFileOpen)
//        {
//            StringBuilder builder = new StringBuilder();
            
//            builder.AppendLine(message);
            
//            builder.AppendLine($"Income Count: {incomeData.Length} | Expense Count: {expenseData.Length} | Is File Open: {isFileOpen.ToString()} | Is Sub File Open: {isSubFileOpen.ToString()}");

//            foreach (var inc in incomeData)
//            {
//                builder.AppendLine($"\t{inc.Category} / {inc.Value} / {inc.SelectedCategory.Name}");
//            }
//            builder.AppendLine();

//            foreach (var exp in expenseData)
//            {
//                builder.AppendLine($"\t{exp.Category} / {exp.Value} / {exp.SelectedCategory.Name}");
//            }
//            builder.AppendLine();

//            SessionWarnings.Add(builder.ToString());
//        }

//        public void Error(Exception exception)
//        {
//            SessionExceptions.Add(exception);
//        }

//        private string ToString(Exception exception)
//        {
//            string delimiter = " | ";
//            string output = "ToString() Failed: ";

//            if(exception != null)
//            {
//                output = $"Exception Type : {exception.GetType().ToString()}{delimiter}";

//                // Ok for now. need to collect the time when the exception occurs.
//                output += $"Time : {DateTime.Now.ToShortDateString()} {DateTime.Now.ToShortTimeString()}{delimiter}";

//                if(exception.Message != null && exception.Message.Length > 0)
//                {
//                    output += $"Message : {exception.Message}{delimiter}";
//                }
//                else
//                {
//                    output += $"Message : No message{delimiter}";
//                }

//                if(exception.StackTrace != null && exception.StackTrace.Length > 0)
//                {
//                    output += $"Stack Trace : {exception.StackTrace}{delimiter}";
//                }
//            }
//            else
//            {
//                output += "Exception was null.";
//            }

//            return output;
//        }

//        #region -- Static Methods
//        public static void Initialize()
//        {
//            Assembly thisAssembly = Assembly.GetEntryAssembly();
//            DirectoryInfo assemblyDir_1 = Directory.GetParent(thisAssembly.Location);
//            DirectoryInfo assemblyDir_2 = Directory.GetParent(assemblyDir_1.FullName);
//            DirectoryInfo assemblyDir_3 = Directory.GetParent(assemblyDir_2.FullName);
//            DirectoryInfo assemblyDir_4 = Directory.GetParent(assemblyDir_3.FullName);

//            if (assemblyDir_4.Name == "BudgetPlanner")
//            {
//                string exceptionFolder = $"{assemblyDir_4.FullName}\\ExceptionLogFolder";

//                if (exceptionFolder != null && Directory.Exists(exceptionFolder))
//                {
//                    ExceptionFolderPath = exceptionFolder;
//                    ExceptionLogFilePath = CreateFileName(exceptionFolder);
//                }
//                else
//                {
//                    Directory.CreateDirectory(exceptionFolder);
//                    ExceptionLogFilePath = CreateFileName(exceptionFolder);
//                }
//            }
//        }

//        public static void Exit(ExceptionLogger logger)
//        {
//            if (Directory.Exists(ExceptionFolderPath))
//            {
                
//            }
//        }

//        private static string CreateFileName(string directoryPath)
//        {
//            if (Directory.Exists(directoryPath))
//            {
//                SessionDateTime = DateTime.Now;
                
//                string name = $"Exception Log : {SessionDateTime.ToLongDateString()} {SessionDateTime.ToLongTimeString()}";
//                return $"{directoryPath}\\{name}.log";
//            }
//            else return null;
//        }
//        #endregion

//        #endregion

//        #region - Properties

//        #endregion
//    }
//}
