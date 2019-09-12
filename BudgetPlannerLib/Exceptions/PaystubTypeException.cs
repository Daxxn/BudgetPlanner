using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BudgetPlannerLib.Exceptions
{
    public class PaystubTypeException : Exception
    {
        public string GivenTypeString { get; set; }

        public PaystubTypeException()
        {
        }

        public PaystubTypeException(String message) : base(message)
        {
        }

        public PaystubTypeException(String message, Exception innerException) : base(message, innerException)
        {
        }

        protected PaystubTypeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
