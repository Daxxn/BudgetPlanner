using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaystubLibrary
{
    public static class PaystubCalculator
    {
        #region Properties
        public delegate void Message(string message);
        public delegate void WarningMessage(Warning warning);
        public static CalcType Decision { get; set; }
        #endregion

        #region Methods
        public static  Tuple<Warning, decimal, decimal, decimal> BeginCalc(Message message, List<Paystub> paystubs, decimal accuracy, int passCount = 1)
        {
            int completePaystubs = 0;
            decimal averagePercentage = 0;
            decimal outputRatio = 0;
            decimal percentAccuracy = 0;
            bool paystubRatio = true;
            bool accuracyOutput = true;

            Decision = CalculationDecision(paystubs);

            // Maybe add a name to the paystub collection??
            message(Decision.ToString());

            Tuple<decimal, int> calcPercentageOutput = CalculatePercentage(paystubs);
            averagePercentage = calcPercentageOutput.Item1;
            completePaystubs = calcPercentageOutput.Item2;

            SetPercentage(paystubs, averagePercentage);
            Tuple<bool, decimal> checkCompleteOutput = CheckCompletedPaystubRatio(paystubs, passCount, completePaystubs);
            paystubRatio = checkCompleteOutput.Item1;
            outputRatio = checkCompleteOutput.Item2;

            if (Decision == CalcType.CalcGross)
            {
                Paystub.GrossFromPercentageList(paystubs);
            }
            else if (Decision == CalcType.CalcNet)
            {
                Paystub.NetFromPercentageList(paystubs);
            }

            Tuple<bool, decimal> checkPercentOutput = CheckPercentageAccuracy(paystubs, accuracy);
            accuracyOutput = checkPercentOutput.Item1;
            percentAccuracy = checkPercentOutput.Item2;

            Tuple<decimal, decimal, decimal> averages = RunAverages(paystubs);

            return Tuple.Create(CheckWarning(paystubRatio, accuracyOutput), averages.Item1, averages.Item2, averages.Item3);
        }

        /// <summary>
        /// Starts full calculation proccess.
        /// </summary>
        /// <param name="message">String to be printed through the UI.</param>
        /// <param name="warning">Warning Enum to display to the user.</param>
        /// <param name="passCount">The desired number of completed paystubs.</param>
        public static Tuple<List<Paystub>, Tuple<decimal, decimal, decimal>, Tuple<decimal, decimal>> BeginCalc(Message message, WarningMessage warning, List<Paystub> paystubs, decimal accuracy, int passCount = 1)
        {
            int completePaystubs = 0;

            decimal averagePercentage = 0;
            decimal percentAccuracy = 0;
            decimal outputRatio = 0;
            bool paystubRatio = true;
            bool accuracyOutput = true;

            Decision = CalculationDecision(paystubs);

            // Maybe add a name to the paystub collection??
            message(Decision.ToString());

            Tuple<decimal, int> calcPercentageOutput = CalculatePercentage(paystubs);
            averagePercentage = calcPercentageOutput.Item1;
            completePaystubs = calcPercentageOutput.Item2;

            SetPercentage(paystubs, averagePercentage);
            Tuple<bool, decimal> checkCompleteOutput = CheckCompletedPaystubRatio(paystubs, passCount, completePaystubs);
            paystubRatio = checkCompleteOutput.Item1;
            outputRatio = checkCompleteOutput.Item2;

            if (Decision == CalcType.CalcGross)
            {
                Paystub.GrossFromPercentageList(paystubs);
            }
            else if (Decision == CalcType.CalcNet)
            {
                Paystub.NetFromPercentageList(paystubs);
            }

            Tuple<bool, decimal> checkPercentOutput = CheckPercentageAccuracy(paystubs, accuracy);
            accuracyOutput = checkPercentOutput.Item1;
            percentAccuracy = checkPercentOutput.Item2;

            warning(CheckWarning(paystubRatio, accuracyOutput));

            Tuple<decimal, decimal> calcAccuracy = Tuple.Create(percentAccuracy, outputRatio);

            Tuple<decimal, decimal, decimal> averagesOut = RunAverages(paystubs);
            return Tuple.Create(paystubs, averagesOut, calcAccuracy);
        }
        
        #region Average Methods
        /// <summary>
        /// Averages GROSS enries.
        /// </summary>
        public static decimal AverageGrossCalc(List<Paystub> paystubs)
        {
            var temp = from data in paystubs where data.Gross != 0 select data.Gross;

            //AverageGross = CalcAverage(temp.ToArray());
            if (temp.Count() != 0)
            {
                return temp.ToArray().Average();
            }
            else return 0;
        }

        /// <summary>
        /// Averages NET entries.
        /// </summary>
        public static decimal AverageNetCalc(List<Paystub> paystubs)
        {
            var temp = from data in paystubs where data.Net != 0 select data.Net;

            //AverageNet = CalcAverage(temp.ToArray());
            if (temp.Count() != 0)
            {
                return temp.ToArray().Average();
            }
            else return 0;
        }

        /// <summary>
        /// Averages PERCENTAGE entries.
        /// </summary>
        public static decimal AveragePercentCalc(List<Paystub> paystubs)
        {
            var temp = from data in paystubs where data.Percent != 0 select data.Percent;

            //AveragePercent = CalcAverage(temp.ToArray());
            if (temp.Count() != 0)
            {
                return temp.ToArray().Average();
            }
            else return 0;
        }

        /// <summary>
        /// Could probably get rid of this one.
        /// Generic average method.
        /// </summary>
        /// <param name="data">Array of decimal data to average.</param>
        /// <returns>Returns the average.</returns>
        private static decimal CalcAverage(decimal[] data)
        {
            decimal sum = 0M;

            foreach (var d in data)
            {
                if (d != 0)
                {
                    sum += d;
                }
            }

            return sum / data.Length;
        }
        #endregion

        public static void CalculatePercentages(List<Paystub> paystubs)
        {
            foreach (Paystub paystub in paystubs)
            {
                paystub.Percentage();
            }
        }
        
        /// <summary>
        /// Finds empty entires and decides what calc to run, if any.
        /// </summary>
        /// <param name="accuracyRatio">How accurate should the calculation be.</param>
        /// <returns>Returns the decision as a CalcType Enum.</returns>
        private static CalcType CalculationDecision(List<Paystub> paystubs)
        {
            CalcType output = CalcType.NotEnoughInfo;

            int paystubCount = paystubs.Count;

            // Not needed??
            double totalGrossZeros = 0;
            double totalNetZeros = 0;
            double totalEmptyEntries = 0;

            double grossRatio = 0;
            double netRatio = 0;
            double emptyRatio = 0;

            // Item1 = Total Gross Zeros
            // Item2 = Total Net Zeros
            // Item3 = Total Empty Zeros
            Tuple<double, double, double> zeros = CountZeros(paystubs);
            totalGrossZeros = zeros.Item1;
            totalNetZeros = zeros.Item2;
            totalEmptyEntries = zeros.Item3;

            if (totalEmptyEntries == 0)
            {
                // Gets the ratios for each category.
                // The ifs only protect from dividing by 0.
                if (totalGrossZeros != 0)
                {
                    grossRatio = totalGrossZeros / paystubCount;
                }
                if (totalNetZeros != 0)
                {
                    netRatio = totalNetZeros / paystubCount;
                }
                // Not needed??
                if (totalEmptyEntries != 0)
                {
                    emptyRatio = totalEmptyEntries / paystubCount;
                }

                if (grossRatio != 1 && netRatio != 1)
                {
                    if (grossRatio >= netRatio)
                    {
                        output = CalcType.CalcGross;
                    }
                    else if (grossRatio < netRatio)
                    {
                        output = CalcType.CalcNet;
                    }
                }
                else
                {
                    output = CalcType.NeedOneCompletePaystub;
                }
            }
            else
            {
                output = CalcType.NoEmptyPaystubs;
            }

            if (totalEmptyEntries + totalGrossZeros + totalNetZeros == paystubCount)
            {
                output = CalcType.NeedOneCompletePaystub;
            }

            return output;
        }
        
        /// <summary>
        /// Finds the difference of the min and max percentages and compares that to the users desired accuracy tolerance.
        /// </summary>
        /// <returns>Returns true if the check passes.</returns>
        private static Tuple<bool, decimal> CheckConvertedPercentageAccuracy(List<Paystub> paystubs, decimal accuracy)
        {
            bool pass = true;

            decimal calcAccuracy = 0;
            decimal percentDiff = paystubs.Max(x => x.Percent) - paystubs.Min(x => x.Percent);

            if (percentDiff != 0)
            {
                calcAccuracy = percentDiff / 100;
            }

            if (calcAccuracy > (decimal)accuracy)
            {
                pass = false;
            }

            return Tuple.Create(pass, percentDiff);
        }
        
        public static Tuple<bool, decimal> CheckPercentageAccuracy(List<Paystub> paystubs, decimal accuracy)
        {
            bool pass = true;

            decimal percentDiff = paystubs.Max(x => x.Percent) - paystubs.Min(x => x.Percent);

            if (percentDiff > (decimal)accuracy)
            {
                pass = false;
            }

            return Tuple.Create(pass, percentDiff);
        }

        /// <summary>
        /// Checks the number of completed paystubs to the user defined ratio.
        /// </summary>
        /// <returns>Returns true if check passed.</returns>
        private static Tuple<bool, decimal> CheckCompletedPaystubRatio(List<Paystub> paystubs, int passCount, int completePaystubs)
        {
            bool pass = true;
            decimal desiredAccuracyratio = 0;
            decimal outputRatio = 0;

            if (passCount != 0)
            {
                desiredAccuracyratio = (decimal)passCount / (decimal)paystubs.Count;
            }

            if (completePaystubs != 0)
            {
                outputRatio = (decimal)completePaystubs / (decimal)paystubs.Count;
            }

            if (desiredAccuracyratio > outputRatio)
            {
                pass = false;
            }

            return Tuple.Create(pass, outputRatio);
        }

        /// <summary>
        /// Counts all empty entries in the paystubs.
        /// </summary>
        /// <returns>Item1: Gross, Item2: Net, Item3: Empty</returns>
        private static  Tuple<double, double, double> CountZeros(List<Paystub> paystubs)
        {
            double totalGrossZeros = 0;
            double totalNetZeros = 0;
            double totalEmptyEntries = 0;

            foreach (var stub in paystubs)
            {
                if (stub.Gross == 0 && stub.Net != 0)
                {
                    totalGrossZeros++;
                }
                else if (stub.Gross != 0 && stub.Net == 0)
                {
                    totalNetZeros++;
                }
                else if (stub.Gross == 0 && stub.Net == 0)
                {
                    totalEmptyEntries++;
                }
            }

            return Tuple.Create(totalGrossZeros, totalNetZeros, totalEmptyEntries);
        }

        /// <summary>
        /// Calculates the percentages of completed paystubs and returns the average.
        /// </summary>
        /// <returns>Returns a decimal of the percentage average.</returns>
        private static Tuple<decimal, int> CalculatePercentage(List<Paystub> paystubs)
        {
            decimal averagePercent = 0;
            int completePaystubs = 0;
            decimal tempAverage = 0;

            foreach (Paystub stub in paystubs)
            {
                if (stub.Gross != 0 && stub.Net != 0)
                {
                    //tempAverage += stub.GetConvertedPercentage();
                    tempAverage += stub.GetPercentage();
                    completePaystubs++;
                }
            }

            if (tempAverage != 0 && completePaystubs != 0)
            {
                averagePercent = tempAverage / (decimal)completePaystubs;
            }

            return Tuple.Create(averagePercent, completePaystubs);
        }

        /// <summary>
        /// Simply loops through each paystub and stores the percentage.
        /// </summary>
        /// <param name="percent">The percentage to store.</param>
        private static void SetPercentage(List<Paystub> paystubs, decimal percent)
        {
            if (percent != 0)
            {
                foreach (Paystub paystub in paystubs)
                {
                    if (paystub.Percent == 0)
                    {
                        paystub.Percent = percent;
                        paystub.Complete = false;
                    }
                    else
                    {
                        paystub.Complete = true;
                    }
                }
            }
        }

        /// <summary>
        /// Combines both acuracy checks to a warning enum.
        /// </summary>
        /// <param name="paystubRatio">True if passed.</param>
        /// <param name="accuracy">True if passed.</param>
        /// <returns>Warning enum to be displayed.</returns>
        private static Warning CheckWarning(bool paystubRatio, bool accuracy)
        {
            Warning output = Warning.NoWarning;

            if (!paystubRatio)
            {
                output = Warning.LowCompletePaystubs;
            }

            if (!accuracy)
            {
                output = Warning.LowAccuracy;
            }

            return output;
        }

        /// <summary>
        /// Calculates all three averages.
        /// </summary>
        public static Tuple<decimal, decimal, decimal> RunAverages(List<Paystub> paystubs)
        {
            return Tuple.Create(AverageGrossCalc(paystubs), AverageNetCalc(paystubs), AveragePercentCalc(paystubs));
        }
        #endregion
    }
}
