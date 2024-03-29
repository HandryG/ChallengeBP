﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChallengeBP.Entities.ViewModels
{
    public class MetaViewModel
    {
        public int Id { get; set; } 
        public string Titulo { get; set; }
        public int Years { get; set; }
        public  double InversionInicial { get; set; }
        public double AporteMensual { get; set; }
        public double MontoObjetivo { get; set; }
        public string EntidadFinanciera { get; set; }
        public PortafolioViewModel Portafolio { get; set; }  
        public DateTime FechaCreacion { get; set; }
    }
}
