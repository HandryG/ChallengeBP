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
                                   join goaltransaction in db.Goaltransactions on goal.Id equals goaltransaction.Goalid
                                   join user in db.Users on goal.Userid equals user.Id
                                   where goal.Userid == userid
                                   select new AporteViewModel
                                   { MontoOriginal = (double)goaltransaction.Amount,
                                   Cambio = (from currencyindicator in db.Currencyindicators
                                    where currencyindicator.Sourcecurrencyid == user.Currencyid && currencyindicator.Destinationcurrencyid == goaltransaction.Currencyid && currencyindicator.Date == goaltransaction.Date
                                    select currencyindicator.Value).FirstOrDefault() };

                foreach(var item in listaAportes.Where(a => a.Cambio != 0))
                {
                    item.MontoFinal = item.MontoOriginal * item.Cambio;
                }

                foreach(var item in listaAportes.Where(a => a.Cambio == 0))
                {
                    item.MontoFinal = item.MontoOriginal;
                }

                var balance = from goal in db.Goals
                              join goaltransactionfunding in db.Goaltransactionfundings on goal.Id equals goaltransactionfunding.Goalid
                              join user in db.Users on goal.Userid equals user.Id
                              where goal.Userid == userid
                              select new BalanceViewModel
                              {
                                  ShareValue = (from fundingsharevalue in db.Fundingsharevalues
                                                where fundingsharevalue.Id == goaltransactionfunding.Fundingid && fundingsharevalue.Date == goaltransactionfunding.Date
                                                select fundingsharevalue.Value).FirstOrDefault(),
                                  Cambio = (from currencyindicator in db.Currencyindicators
                                            where currencyindicator.Sourcecurrencyid == user.Currencyid && currencyindicator.Destinationcurrencyid == (from goaltransaction in db.Goaltransactions where goaltransaction.Id == goaltransactionfunding.Transactionid select goaltransaction.Currencyid).FirstOrDefault()
                                            && currencyindicator.Date == goaltransactionfunding.Date
                                            select currencyindicator.Value).FirstOrDefault(),
                                  Quotas = (double)goaltransactionfunding.Quotas
                              };


                foreach (var item in balance.Where(w => w.Cambio != 0))
                {
                    item.MontoFinal = item.Quotas * item.Cambio * item.ShareValue;
                }

                foreach (var item in balance.Where(w => w.Cambio == 0))
                {
                    item.MontoFinal = item.Quotas * item.Cambio;
                }

                return new ResumenViewModel { aportestotales = listaAportes.Sum(a => a.MontoFinal) , balance = balance.Sum(b => b.MontoFinal)};
            }

            

        }
        public ResumenViewModel getResumenByIdAndDate(int userid, DateTime date)
        {
            using (var db = _context)
            {
                var listaAportes = from goal in db.Goals
                                   join goaltransaction in db.Goaltransactions on goal.Id equals goaltransaction.Goalid
                                   join user in db.Users on goal.Userid equals user.Id
                                   where goal.Userid == userid && goaltransaction.Date.Equals(date)
                                   select new AporteViewModel
                                   {
                                       MontoOriginal = (double)goaltransaction.Amount,
                                       Cambio = (from currencyindicator in db.Currencyindicators
                                                 where currencyindicator.Sourcecurrencyid == user.Currencyid && currencyindicator.Destinationcurrencyid == goaltransaction.Currencyid && currencyindicator.Date == goaltransaction.Date
                                                 select currencyindicator.Value).FirstOrDefault()
                                   };

                foreach (var item in listaAportes.Where(a => a.Cambio != 0))
                {
                    item.MontoFinal = item.MontoOriginal * item.Cambio;
                }

                foreach (var item in listaAportes.Where(a => a.Cambio == 0))
                {
                    item.MontoFinal = item.MontoOriginal;
                }

                var balance = from goal in db.Goals
                              join goaltransactionfunding in db.Goaltransactionfundings on goal.Id equals goaltransactionfunding.Goalid
                              join user in db.Users on goal.Userid equals user.Id
                              where goal.Userid == userid && goaltransactionfunding.Date.Equals(date)
                              select new BalanceViewModel
                              {
                                  ShareValue = (from fundingsharevalue in db.Fundingsharevalues
                                                where fundingsharevalue.Id == goaltransactionfunding.Fundingid && fundingsharevalue.Date == goaltransactionfunding.Date
                                                select fundingsharevalue.Value).FirstOrDefault(),
                                  Cambio = (from currencyindicator in db.Currencyindicators
                                            where currencyindicator.Sourcecurrencyid == user.Currencyid && currencyindicator.Destinationcurrencyid == (from goaltransaction in db.Goaltransactions where goaltransaction.Id == goaltransactionfunding.Transactionid select goaltransaction.Currencyid).FirstOrDefault()
                                            && currencyindicator.Date == goaltransactionfunding.Date
                                            select currencyindicator.Value).FirstOrDefault(),
                                  Quotas = (double)goaltransactionfunding.Quotas
                              };


                foreach (var item in balance.Where(w => w.Cambio != 0))
                {
                    item.MontoFinal = item.Quotas * item.Cambio * item.ShareValue;
                }

                foreach (var item in balance.Where(w => w.Cambio == 0))
                {
                    item.MontoFinal = item.Quotas * item.Cambio;
                }

                return new ResumenViewModel { aportestotales = listaAportes.Sum(a => a.MontoFinal), balance = balance.Sum(b => b.MontoFinal) };
            }

        }

        public List<MetaViewModel> getMetas(int userid)
        {
            using(var db = _context)
            {
                var listaMetas = from metas in db.Goals
                                 join portafolio in db.Portfolios on metas.Portfolioid equals portafolio.Id
                                 join riesgo in db.Risklevels on portafolio.Risklevelid equals riesgo.Id
                                 join estrategiainversion in db.Investmentstrategies on portafolio.Investmentstrategyid equals estrategiainversion.Id
                                 join entidadfinanciera in db.Financialentities on metas.Financialentityid equals entidadfinanciera.Id
                                 where metas.Userid == userid
                                 select new MetaViewModel
                                 {
                                     Titulo = metas.Title,
                                     Years = metas.Years,
                                     InversionInicial = metas.Initialinvestment,
                                     AporteMensual = metas.Monthlycontribution,
                                     MontoObjetivo = metas.Targetamount,
                                     FechaCreacion = metas.Created,
                                     EntidadFinanciera = entidadfinanciera.Title,
                                     Portafolio = new PortafolioViewModel
                                     {
                                         RangoFechaMaximo = portafolio.Maxrangeyear,
                                         RangoFechaMinimo = portafolio.Minrangeyear,
                                         Descripcion = portafolio.Description,
                                         NivelRiesgo = riesgo.Title,
                                         Rentabilidad = portafolio.Profitability,
                                         EstrategiaInversion = estrategiainversion.Title,
                                         Version = portafolio.Version,
                                         RentabilidadEstimada = portafolio.Estimatedprofitability,
                                         BpComision = portafolio.Bpcomission
                                     }
                                 };

                return listaMetas.ToList();
            }

        }

        public MetaViewModel getMetaDetail(int userid, int goalid)
        {
            throw new NotImplementedException();
        }
    }
}