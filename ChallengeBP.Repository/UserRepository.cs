using ChallengeBP.DataAccess;
using ChallengeBP.Entities.Models;
using ChallengeBP.Entities.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace ChallengeBP.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly challengeContext _context;

        public UserRepository(challengeContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> getUsers()
        {
            using(var db = _context)
            {
                var list = from user in db.Users
                           select user;

                return await list.ToListAsync();
            }
        }

        public UserViewModel getUserById(int userid)
        {
            using (var db = _context)
            {
                var userlinq = from user in db.Users
                           where user.Id == userid
                           select user;

                var usuario = userlinq.FirstOrDefault();

                var advisorlinq = from user in db.Users
                               where user.Id == usuario.Advisorid
                               select user;

                var advisor = advisorlinq.FirstOrDefault();

                var userViewModel = new UserViewModel(usuario.Id,
                                    usuario.Firstname + ' ' + usuario.Surname,
                                    advisor.Firstname + ' ' + advisor.Surname,
                                    usuario.Created);

                return  userViewModel;
            }
        }

        public ResumenViewModel getResumenById(int userid)
        {
            using(var db = _context)
            {
                var listaAportes = from goal in db.Goals
                                   join goaltransaction in db.Goaltransactions on goal.Id equals goaltransaction.Id
                                   where goal.Userid == userid
                                   select new 
                                   { monto = goaltransaction.Amount,
                                   cambio = (from currencyindicator in db.Currencyindicators
                                    where currencyindicator.Sourcecurrencyid == goal.Currencyid && currencyindicator.Destinationcurrencyid == goal.Displaycurrencyid
                                    select currencyindicator.Value).FirstOrDefault() };

                IEnumerable<MontoViewModel> aportes = (IEnumerable<MontoViewModel>)listaAportes.ToList();

                foreach(var item in aportes.Where(w => w.Cambio != null))
                {
                    item.MontoFinal = item.MontoOriginal * item.Cambio;
                }



                var balance = from goal in db.Goals
                              join goaltransactionfunding in db.Goaltransactionfundings on goal.Id equals goaltransactionfunding.Id
                              where goal.Userid == userid
                              select new
                              {
                                  monto = (from fundingsharevalue in db.Fundingsharevalues
                                           where fundingsharevalue.Id == goaltransactionfunding.Fundingid
                                           select fundingsharevalue.Value).FirstOrDefault(),
                                  cambio = (from currencyindicator in db.Currencyindicators
                                            where currencyindicator.Sourcecurrencyid == goal.Currencyid && currencyindicator.Destinationcurrencyid == goal.Displaycurrencyid
                                            select currencyindicator.Value).FirstOrDefault()
                              };


                IEnumerable<MontoViewModel> balancefinal = (IEnumerable<MontoViewModel>)balance.ToList();

                foreach (var item in balancefinal.Where(w => w.Cambio != null))
                {
                    item.MontoFinal = item.MontoOriginal * item.Cambio;
                }

                return new ResumenViewModel { aportestotales = aportes.Sum(a => a.MontoFinal) , balance = aportes.Sum(b => b.MontoFinal)};
            }

            

        }
        public ResumenViewModel getResumenByIdAndDate(int userid, DateTime date)
        {
            using (var db = _context)
            {
                var listaAportes = from goal in db.Goals
                                   join goaltransaction in db.Goaltransactions on goal.Id equals goaltransaction.Id
                                   where goal.Userid == userid && goaltransaction.Date.Equals(date)
                                   select new
                                   {
                                       monto = goaltransaction.Amount,
                                       cambio = (from currencyindicator in db.Currencyindicators
                                                 where currencyindicator.Sourcecurrencyid == goal.Currencyid && currencyindicator.Destinationcurrencyid == goal.Displaycurrencyid
                                                 select currencyindicator.Value).FirstOrDefault()
                                   };

                IEnumerable<MontoViewModel> aportes = (IEnumerable<MontoViewModel>)listaAportes.ToList();

                foreach (var item in aportes.Where(w => w.Cambio != null))
                {
                    item.MontoFinal = item.MontoOriginal * item.Cambio;
                }



                var balance = from goal in db.Goals
                              join goaltransactionfunding in db.Goaltransactionfundings on goal.Id equals goaltransactionfunding.Id
                              where goal.Userid == userid && goaltransactionfunding.Date.Equals(date)
                              select new
                              {
                                  monto = (from fundingsharevalue in db.Fundingsharevalues
                                           where fundingsharevalue.Id == goaltransactionfunding.Fundingid
                                           select fundingsharevalue.Value).FirstOrDefault(),
                                  cambio = (from currencyindicator in db.Currencyindicators
                                            where currencyindicator.Sourcecurrencyid == goal.Currencyid && currencyindicator.Destinationcurrencyid == goal.Displaycurrencyid
                                            select currencyindicator.Value).FirstOrDefault()
                              };


                IEnumerable<MontoViewModel> balancefinal = (IEnumerable<MontoViewModel>)balance.ToList();

                foreach (var item in balancefinal.Where(w => w.Cambio != null))
                {
                    item.MontoFinal = item.MontoOriginal * item.Cambio;
                }

                return new ResumenViewModel { aportestotales = aportes.Sum(a => a.MontoFinal), balance = aportes.Sum(b => b.MontoFinal) };
            }



        }

    }
}