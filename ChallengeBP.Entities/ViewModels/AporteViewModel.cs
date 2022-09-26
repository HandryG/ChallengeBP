using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChallengeBP.Entities.ViewModels
{
    public class MontoViewModel
    {
        public double MontoOriginal { get; set; }  
        public double Cambio { get; set; }  
        public double MontoFinal { get; set; }

        public MontoViewModel(double aporteOriginal, double cambio)
        {
            MontoOriginal = aporteOriginal;
            Cambio = cambio;
        }
    }
}
