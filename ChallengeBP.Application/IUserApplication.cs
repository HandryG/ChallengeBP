using ChallengeBP.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChallengeBP.Application
{
    public interface IUserApplication
    {
        Task<UserViewModel> GetUserById(int userid);
        Task<ResumenViewModel> GetResumenById(int userid);
        Task<ResumenViewModel> GetResumenByIdAndDate(int userid, DateOnly date);
        Task<List<MetaViewModel>> GetMetasById(int userid);
        Task<MetaDetalleViewModel> GetMetaDetail(int userid,int goalid);
    }
}
