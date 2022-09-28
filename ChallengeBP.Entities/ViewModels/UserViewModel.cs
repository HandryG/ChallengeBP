using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChallengeBP.Entities.ViewModels
{
    public  class UserViewModel
    {
        public int id { get; set; }
        public string nombrecompleto { get; set; }
        public string? nombrecompletoadvisor { get; set; }
        public DateTime fechacreacion { get; set; }

        public UserViewModel(int id, string nombrecompleto, DateTime fechacreacion)
        {
            this.id = id;
            this.nombrecompleto = nombrecompleto;
            this.fechacreacion = fechacreacion;
        }
    }
}
