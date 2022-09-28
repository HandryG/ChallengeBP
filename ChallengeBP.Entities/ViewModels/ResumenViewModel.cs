using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChallengeBP.Entities.ViewModels
{
    public class ResumenViewModel
    {
        public double aportestotales { get; set; }
        public double balance { get; set; }

        public ResumenViewModel(double aportestotales, double balance)
        {
            this.aportestotales = aportestotales;
            this.balance = balance;
        }
    }
}
