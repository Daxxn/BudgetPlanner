using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BudgetPlannerMainWPF
{
    public static class MessageManager
    {
        #region - Methods

        /// <summary>
        /// Simple Message Box Display.
        /// </summary>
        /// <param name="message">Text to display</param>
        public static void DisplayMessage(string message)
        {
            MessageBox.Show(message);
        }

        /// <summary>
        /// Simple Message Box Diplay, with title.
        /// </summary>
        /// <param name="message">Text to diplay</param>
        /// <param name="title">Title of Message</param>
        public static void DisplayMessage(string message, string title)
        {
            MessageBox.Show(message, title);
        }

        /// <summary>
        /// Message Box diplay function.
        /// Only handles OK or OK/Cancel.
        /// </summary>
        /// <param name="message">Text to display</param>
        /// <param name="title">Title for the text box</param>
        /// <param name="buttonType">Button type to display</param>
        /// <returns>Returns the result of OK as a bool.</returns>
        public static bool DisplayMessageWithOK(string message, string title)
        {
            DialogResult result = MessageBox.Show(message, title, MessageBoxButtons.OKCancel);

            if (result == DialogResult.OK)
            {
                return true;
            }
            else return false;
        }
        #endregion

        #region - Properties

        #endregion
    }
}
