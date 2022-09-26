using ChallengeBP.Entities.Models;
using ChallengeBP.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChallengeBP.Repository
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> getUsers();
        UserViewModel getUserById(int userid);
        ResumenViewModel getResumenById(int userid);
        ResumenViewModel getResumenByIdAndDate(int userid, DateTime date);

    }
}
