using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChallengeBP.Entities.ViewModels
{
    public class PortafolioViewModel
    {
        public double RangoFechaMaximo { get; set; }
        public double RangoFechaMinimo { get; set; }
        public string Descripcion { get; set; }
        public string NivelRiesgo { get; set; }
        public string Rentabilidad { get; set; }
        public string EstrategiaInversion { get; set; }
        public string Version { get; set; }
        public double RentabilidadEstimada { get; set; }
        public double BpComision { get; set; }

    }
}
