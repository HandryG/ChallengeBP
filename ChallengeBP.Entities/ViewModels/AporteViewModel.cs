using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChallengeBP.Entities.ViewModels
{
    public class AporteViewModel
    {
        public int GoalId { get; set; }
        public double MontoOriginal { get; set; }
        public string Tipo { get; set; }
        public double Cambio { get; set; }  
        public double MontoFinal { get; set; }
        public DateOnly Fecha { get; set; }

        public enum KDTipos
        {
            [Description("buy")]
            KDBuy = 0,
            [Description("sale")]
            KDSale = 1,
        }

        
    }
}
