using System.Windows.Forms;

namespace BudgetPlannerMainWPF
{
    public static class MessageManager
    {
        #region - Methods

        /// <summary>
        /// Simple Message Box Display.
        /// </summary>
        /// <param name="message">Main body of message box.</param>
        public static void DisplayMessage(string message)
        {
            MessageBox.Show(message);
        }

        /// <summary>
        /// Message Box Diplay with title.
        /// </summary>
        /// <param name="message">Main body of message box.</param>
        /// <param name="title">Title of Message</param>
        public static void DisplayMessage(string message, string title)
        {
            MessageBox.Show(message, title);
        }

        /// <summary>
        /// Message Box diplay function: OK/Cancel
        /// </summary>
        /// <param name="message">Main body of message box.</param>
        /// <param name="title">Title for the text box</param>
        /// <param name="buttonType">Button type to display</param>
        /// <returns>Returns the result as a bool. OK = true / Cancel = false</returns>
        public static bool DisplayMessageWithOK(string message, string title)
        {
            DialogResult result = MessageBox.Show(message, title, MessageBoxButtons.OKCancel);

            if (result == DialogResult.OK) return true;
            else return false;
        }

        /// <summary>
        /// Message Box Display function: Yes/No/Cancel
        /// </summary>
        /// <param name="message">Main body of message box.</param>
        /// <param name="title">Window Title.</param>
        /// <returns>Returns an int. Yes = 1 / No = 2 / Cancel = 3 / Error = 0</returns>
        public static int DisplayMessageWithYesNo(string message, string title)
        {
            DialogResult result = MessageBox.Show(message, title, MessageBoxButtons.YesNoCancel);

            if (result == DialogResult.Yes) return 1;
            else if (result == DialogResult.No) return 2;
            else if (result == DialogResult.Cancel) return 3;
            else return 0;
        }

        /// <summary>
        /// Message Box Display Function: Abort/Retry/Ignore
        /// </summary>
        /// <param name="message">Main body of message box.</param>
        /// <param name="title">Window Title.</param>
        /// <returns>Returns an int. Abort = 1 / Retry = 2 / Ignore = 3 / Error = 0</returns>
        public static int DisplayMessgaeWithRetry(string message, string title)
        {
            DialogResult result = MessageBox.Show(message, title, MessageBoxButtons.AbortRetryIgnore);

            if (result == DialogResult.Abort) return 1;
            else if (result == DialogResult.Retry) return 2;
            else if (result == DialogResult.Ignore) return 3;
            else return 0;
        }
        #endregion

        #region - Properties

        #endregion
    }
}
