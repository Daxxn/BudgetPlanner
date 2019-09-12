using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BudgetPlannerLib.Exceptions;

namespace BudgetPlannerLib.Models
{
    public class PaystubModel
    {
        #region - Fields
        public int Index { get; set; }
        public decimal Gross { get; set; }
        public decimal Net { get; set; }
        #endregion

        #region - Constructors
        public PaystubModel() { }
        public PaystubModel(int index, decimal gross)
        {
            Index = index;
            Gross = gross;
        }
        public PaystubModel(int index, decimal gross, decimal net)
        {
            Index = index;
            Gross = gross;
            Net = net;
        }
        #endregion

        #region - Methods
        /// <summary>
        /// Averages gross & net paystubs and returns a tuple with the data and if both were calculated.
        /// </summary>
        /// <param name="paystubs">An array of Paystubs to average.</param>
        /// <param name="type">The requested value to average.</param>
        /// <param name="both">Specifies wether to calc both gross & net.</param>
        /// <returns>Tuple of the average. 1 = gross, 2 = net</returns>
        public static Tuple<decimal, decimal> Average(PaystubModel[] paystubs, PaystubType type, bool both)
        {
            if (!both)
            {
                if (type == PaystubType.Gross)
                {
                    decimal temp = paystubs.Average(x => x.Gross);
                    return Tuple.Create(temp, 0M);
                }
                else if (type == PaystubType.Net)
                {
                    decimal temp = paystubs.Average(x => x.Net);
                    return Tuple.Create(0M, temp);
                }
                else
                {
                    throw new PaystubTypeException("The given paystub type to average was not valid.") { GivenTypeString = type.ToString() };
                }
            }
            else
            {
                decimal gross = paystubs.Average(x => x.Gross);
                decimal net = paystubs.Average(x => x.Net);

                return Tuple.Create(gross, net);
            }
        }

        public static double AveragePercentage(PaystubModel[] paystubs)
        {
            double[] allPercents = new double[paystubs.Length];

            for (int i = 0; i < paystubs.Length; i++)
            {
                if (paystubs[i].Gross > 0M && paystubs[i].Net > 0M)
                {
                    allPercents[i] = Convert.ToDouble((paystubs[i].Net * 100) / paystubs[i].Gross);
                }
            }

            return allPercents.Average();
        }

        public static IEnumerable<PaystubModel> GrossFromPercentage(IEnumerable<PaystubModel> paystubs, double avgPercentage)
        {
            foreach (var stub in paystubs)
            {
                if (stub.Gross == 0M)
                {
                    stub.Gross = (stub.Net * 100) / Convert.ToDecimal(avgPercentage);
                }
            }

            return paystubs;
        }

        public static IEnumerable<PaystubModel> NetFromPercentage(IEnumerable<PaystubModel> paystubs, double avgPercentage)
        {
            foreach (var stub in paystubs)
            {
                if(stub.Net == 0M)
                {
                    stub.Net = (stub.Gross * Convert.ToDecimal(avgPercentage)) / 100;
                }
            }

            return paystubs;
        }
        #endregion

        #region - Properties

        #endregion
    }
}
