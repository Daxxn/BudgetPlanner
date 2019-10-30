using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaystubLibrary
{
    public class Paystub
    {
        #region Properties & Variables
        public int Index { get; set; }
        public decimal Gross { get; set; }
        public decimal Net { get; set; }
        public decimal Percent { get; set; }
        public bool Complete { get; set; }
        #endregion

        #region Constructors
        public Paystub() { }
        public Paystub(int index, decimal amount, bool isGross)
        {
            Index = index;

            if (isGross)
            {
                Gross = amount;
            }
            else
            {
                Net = amount;
            }
        }
        public Paystub(int index, decimal gross, decimal net)
        {
            Index = index;
            Gross = gross;
            Net = net;
        }
        public Paystub(int index, decimal gross, decimal net, decimal percent)
        {
            Index = index;
            Gross = gross;
            Net = net;
            Percent = percent;
        }
        #endregion

        #region Methods
        public void Percentage()
        {
            Percent = Net / Gross * 100;
        }

        /// <summary>
        /// Calculates the Percentage from the gross and net values, also places the result in the Percent prop.
        /// </summary>
        /// <returns>Returns a decimal of the calculated percentage.</returns>
        public decimal GetPercentage()
        {
            if (Net != 0 && Gross != 0)
            {
                Percent = Net / Gross * 100;
                Complete = true;
                return Percent;
            }
            else
            {
                Complete = false;
                return 0;
            }
        }

        public void GrossFromPercentage()
        {
            Gross = Net / Percent * 100;
        }

        public void NetFromPercentage()
        {
            Net = Percent / 100 * Gross;
        }

        public override string ToString()
        {
            return $"Paystub {Index}: Gross {Gross:N3} , Net {Net:N3} , Percentage {Percent}";
        }

        public static void GrossFromPercentageList(List<Paystub> paystubs)
        {
            foreach (Paystub paystub in paystubs)
            {
                if (paystub.Gross == 0
                    && paystub.Net != 0
                    && paystub.Percent != 0)
                {
                    paystub.GrossFromPercentage();
                }
            }
        }

        public static void NetFromPercentageList(List<Paystub> paystubs)
        {
            foreach (Paystub paystub in paystubs)
            {
                if (paystub.Net == 0
                    && paystub.Gross != 0
                    && paystub.Percent != 0)
                {
                    paystub.NetFromPercentage();
                }
            }
        }
        #endregion

        #region Full Properties

        #endregion
    }
}
