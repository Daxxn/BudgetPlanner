using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaystubLibrary
{
    public class PaystubCollection
    {
        #region Fields
        public static double Accuracy { get; set; } = 0.1;
        public static double DesiredAccuracyRatio { get; set; } = 0.1;
        public static int PassCount { get; set; } = 1;

        public List<Paystub> Paystubs { get; set; }
        public decimal AverageGross { get; set; }
        public decimal AverageNet { get; set; }
        public decimal AveragePercent { get; set; }
        public CalcType Decision { get; private set; }

        public double PercentageDifference { get; set; }
        public double OutputRatio { get; set; } = 0;
        public int CompletePaystubCount { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public delegate void Message(string message);
        public delegate void WarningMessage(Warning warning);
        #endregion

        #region Constructors
        public PaystubCollection() { }
        public PaystubCollection(List<Paystub> paystubs)
        {
            Paystubs = paystubs;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Starts full calculation proccess.
        /// </summary>
        /// <param name="message">Delegate for UI message.</param>
        /// <returns>Returns the calculation decision made.</returns>
        public Warning BeginCalc(Message message)
        {
            decimal averagePercentage = 0;
            bool paystubRatio = true;
            bool accuracyOutput = true;

            Decision = CalculationDecision();

            // Maybe add a name to the paystub collection??
            message(Decision.ToString());

            if (Decision == CalcType.CalcGross)
            {
                averagePercentage = CalculatePercentage();
                SetPercentage(averagePercentage);
                paystubRatio = CheckCompletedPaystubRatio();
                Paystub.GrossFromPercentageList(Paystubs);
                accuracyOutput = CheckPercentageAccuracy();
                RunAverages();
            }
            else if (Decision == CalcType.CalcNet)
            {
                averagePercentage = CalculatePercentage();
                SetPercentage(averagePercentage);
                paystubRatio = CheckCompletedPaystubRatio();
                Paystub.NetFromPercentageList(Paystubs);
                accuracyOutput = CheckPercentageAccuracy();
                RunAverages();
            }

            return CheckWarning(paystubRatio, accuracyOutput);
        }

        /// <summary>
        /// Starts full calculation proccess.
        /// </summary>
        /// <param name="message">String to be printed through the UI.</param>
        /// <param name="warning">Warning Enum to display to the user.</param>
        /// <param name="passCount">The desired number of completed paystubs.</param>
        public void BeginCalc(Message message, WarningMessage warning, int passCount = 1)
        {
            decimal averagePercentage = 0;
            bool paystubRatio = true;
            bool accuracyOutput = true;

            Decision = CalculationDecision();

            // Maybe add a name to the paystub collection??
            message(Decision.ToString());

            if (Decision == CalcType.CalcGross)
            {
                averagePercentage = CalculatePercentage();
                SetPercentage(averagePercentage);
                paystubRatio = CheckCompletedPaystubRatio();
                Paystub.GrossFromPercentageList(Paystubs);
                accuracyOutput = CheckPercentageAccuracy();
                RunAverages();
                warning(CheckWarning(paystubRatio, accuracyOutput));
            }
            else if (Decision == CalcType.CalcNet)
            {
                averagePercentage = CalculatePercentage();
                SetPercentage(averagePercentage);
                paystubRatio = CheckCompletedPaystubRatio();
                Paystub.NetFromPercentageList(Paystubs);
                accuracyOutput = CheckPercentageAccuracy();
                RunAverages();
                warning(CheckWarning(paystubRatio, accuracyOutput));
            }
        }

        #region Average Methods
        /// <summary>
        /// Averages GROSS enries.
        /// </summary>
        public void AverageGrossCalc()
        {
            var temp = from data in Paystubs where data.Gross != 0 select data.Gross;

            //AverageGross = CalcAverage(temp.ToArray());
            AverageGross = temp.ToArray().Average();
        }

        /// <summary>
        /// Averages NET entries.
        /// </summary>
        public void AverageNetCalc()
        {
            var temp = from data in Paystubs where data.Net != 0 select data.Net;

            //AverageNet = CalcAverage(temp.ToArray());
            AverageNet = temp.ToArray().Average();
        }

        /// <summary>
        /// Averages PERCENTAGE entries.
        /// </summary>
        public void AveragePercentCalc()
        {
            var temp = from data in Paystubs where data.Percent != 0 select data.Percent;

            //AveragePercent = CalcAverage(temp.ToArray());
            AveragePercent = temp.ToArray().Average();
        }

        /// <summary>
        /// Could probably get rid of this one.
        /// Generic average method.
        /// </summary>
        /// <param name="data">Array of decimal data to average.</param>
        /// <returns>Returns the average.</returns>
        private decimal CalcAverage(decimal[] data)
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

        /// <summary>
        /// Finds empty entires and decides what calc to run, if any.
        /// </summary>
        /// <param name="accuracyRatio">How accurate should the calculation be.</param>
        /// <returns>Returns the decision as a CalcType Enum.</returns>
        private CalcType CalculationDecision()
        {
            CalcType output = CalcType.NotEnoughInfo;

            int paystubCount = Paystubs.Count;

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
            Tuple<double, double, double> zeros = CountZeros();
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
        private bool CheckPercentageAccuracy()
        {
            bool pass = true;

            decimal calcAccuracy = 0;
            decimal percentDiff = Paystubs.Max(x => x.Percent) - Paystubs.Min(x => x.Percent);

            if (percentDiff != 0)
            {
                calcAccuracy = percentDiff / 100;
            }

            if (calcAccuracy > (decimal)Accuracy)
            {
                pass = false;
            }

            return pass;
        }

        /// <summary>
        /// Checks the number of completed paystubs to the user defined ratio.
        /// </summary>
        /// <returns>Returns true if check passed.</returns>
        private bool CheckCompletedPaystubRatio()
        {
            bool pass = true;

            if (PassCount != 0)
            {
                DesiredAccuracyRatio = (double)PassCount / (double)Paystubs.Count;
            }

            if (CompletePaystubCount != 0)
            {
                OutputRatio = (double)CompletePaystubCount / (double)Paystubs.Count;
            }

            if (DesiredAccuracyRatio > OutputRatio)
            {
                pass = false;
            }

            return pass;
        }

        /// <summary>
        /// Counts all empty entries in the paystubs.
        /// </summary>
        /// <returns>Item1: Gross, Item2: Net, Item3: Empty</returns>
        private Tuple<double, double, double> CountZeros()
        {
            double totalGrossZeros = 0;
            double totalNetZeros = 0;
            double totalEmptyEntries = 0;

            foreach (var stub in Paystubs)
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
        private decimal CalculatePercentage()
        {
            decimal averagePercent = 0;
            int passCount = 0;
            decimal tempAverage = 0;

            foreach (Paystub stub in Paystubs)
            {
                if (stub.Gross != 0 && stub.Net != 0)
                {
                    tempAverage += stub.GetPercentage();
                    passCount++;
                }
            }

            if (tempAverage != 0 && passCount != 0)
            {
                averagePercent = tempAverage / (decimal)passCount;
                CompletePaystubCount = passCount;
            }

            return averagePercent;
        }
        
        /// <summary>
        /// Simply loops through each paystub and stores the percentage.
        /// </summary>
        /// <param name="percent">The percentage to store.</param>
        private void SetPercentage(decimal percent)
        {
            if (percent != 0)
            {
                foreach (Paystub paystub in Paystubs)
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
        private Warning CheckWarning(bool paystubRatio, bool accuracy)
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
        public void RunAverages()
        {
            AverageGrossCalc();
            AverageNetCalc();
            AveragePercentCalc();
        }

        /// <summary>
        /// Creates a display string for the overall averages.
        /// </summary>
        /// <returns>String to be printed.</returns>
        public string AveragesToString()
        {
            StringBuilder output = new StringBuilder("All Averages:\n");

            output.AppendLine($"Average Gross: {AverageGross}");
            output.AppendLine($"Average Net: {AverageNet}");
            output.AppendLine($"Average Percentage: {AveragePercent}");

            return output.ToString();
        }

        /// <summary>
        /// Combines the accuracy data to a formatted string.
        /// </summary>
        /// <returns>Returns a 2-line string for accuracy display.</returns>
        public string AccuracyToString()
        {
            return $"Calculation Accuracy\nComplete Paystubs: {CompletePaystubCount}, Percentage Accuracy: {PercentageDifference:N3}";
        }

        /// <summary>
        /// Creates a concatenated string to display the name and description of the collection.
        /// </summary>
        /// <returns>Returns a 2-line string to diplay.</returns>
        public string TitleToString()
        {
            return $"{Name}\nDescription: {Description}";
        }

        /// <summary>
        /// Combines the name, averages, and accuracy data a multi-line string.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder(TitleToString());
            builder.AppendLine();

            builder.AppendLine(AveragesToString());
            builder.AppendLine();

            builder.AppendLine(AccuracyToString());

            return builder.ToString();
        }
        #endregion

        #region Properties

        #endregion
    }
}
