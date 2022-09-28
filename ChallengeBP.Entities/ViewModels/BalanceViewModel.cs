using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChallengeBP.Entities.ViewModels
{
    public class BalanceViewModel
    {
        public double Cambio { get; set; }
        public double Quotas { get; set; }
        public double ShareValue { get; set; }
        public double MontoFinal { get; set; }
        public int GoalId { get; set; }
        public int FundingId { get; set; }
        public DateOnly Fecha { get; set; }
    }
}
