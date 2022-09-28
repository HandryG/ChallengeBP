﻿using ChallengeBP.Entities.Models;
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
        Task<User> GetUserById(int userid);
        Task<List<AporteViewModel>> GetAportesById(int userid, int? goalid = null);
        Task<List<BalanceViewModel>> GetBalanceById(int userid);
        ResumenViewModel getResumenById(int userid);
        ResumenViewModel getResumenByIdAndDate(int userid, DateTime date);
        Task<List<MetaViewModel>> getMetas(int userid, int? goalid = null);
        MetaViewModel getMetaDetail(int userid,int goalid);
    }
}
