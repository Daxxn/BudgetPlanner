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
        public string Name { get; set; } = "No Name";
        public uint Index { get; set; }
        public decimal Gross { get; set; }
        public decimal Net { get; set; }
        public decimal Percent { get; set; }
        public bool Complete { get; set; }
        #endregion

        #region Constructors
        public Paystub() { }
        public Paystub(string name, uint index, decimal gross, decimal net)
        {
            Name = name;
            Index = index;
            Gross = gross;
            Net = net;
        }

        public Paystub(string name, uint index, decimal amount, bool isGross)
        {
            Name = name;
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

        public Paystub(uint index, decimal amount, bool isGross)
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

        public Paystub(uint index, decimal gross, decimal net)
        {
            Index = index;
            Gross = gross;
            Net = net;
        }

        public Paystub(uint index, decimal gross, decimal net, decimal percent)
        {
            Index = index;
            Gross = gross;
            Net = net;
            Percent = percent;
        }
        public Paystub(uint index, string name, decimal gross, decimal net, decimal percent)
        {
            Index = index;
            Name = name;
            Gross = gross;
            Net = net;
            Percent = percent;
        }
        #endregion

        #region Methods
        public void ConvertedPercentage()
        {
            if (Gross != 0)
            {
                Percent = Net / Gross * 100;
            }
            else Percent = 0;
        }

        public void Percentage()
        {
            if (Gross != 0)
            {
                Percent = Net / Gross;
            }
            else Percent = 0;
        }

        /// <summary>
        /// Calculates the Percentage from the gross and net values, also places the result in the Percent prop.
        /// </summary>
        /// <returns>Returns a decimal of the calculated percentage.</returns>
        public decimal GetConvertedPercentage()
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

        public decimal GetPercentage()
        {
            if (Net != 0 && Gross != 0)
            {
                Percent = Net / Gross;
                Complete = true;
                return Percent;
            }
            else
            {
                Complete = false;
                return 0;
            }
        }

        public void ConvertedGrossFromPercentage()
        {
            Gross = Net / Percent * 100;
        }

        public void GrossFromPercentage()
        {
            Gross = Net / Percent;
        }

        public void ConvertedNetFromPercentage()
        {
            Net = Percent / 100 * Gross;
        }

        public void NetFromPercentage()
        {
            Net = Percent * Gross;
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
                    //paystub.ConvertedGrossFromPercentage();
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
                    //paystub.ConvertedNetFromPercentage();
                    paystub.NetFromPercentage();
                }
            }
        }

        public static List<Paystub> GetPercentages(List<Paystub> paystubs)
        {
            List<Paystub> output = paystubs;

            foreach (Paystub paystub in output)
            {
                //paystub.ConvertedPercentage();
                paystub.Percentage();
            }

            return output;
        }

        /// <summary>
        /// Creates a blank paystub. Probably not necessary.
        /// </summary>
        /// <param name="currentIndex"></param>
        /// <returns></returns>
        public static Paystub Default(uint currentIndex)
        {
            return new Paystub()
            {
                Index = currentIndex++
            };
        }
        #endregion

        #region Full Properties

        #endregion
    }
}
