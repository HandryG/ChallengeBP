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

                return await list.Take(20).ToListAsync();
            }
        }

        public async Task<User> GetUserById(int userid)
        {
            
                var userlinqasync = from user in _context.Users
                                    where user.Id == userid
                                    select user;

                return await userlinqasync.FirstOrDefaultAsync();
            
        }

        public async Task<List<AporteViewModel>> GetAportesById(int userid, int? goalid = null)
        {
            var listaAportes = from goal in _context.Goals
                               join goaltransaction in _context.Goaltransactions on goal.Id equals goaltransaction.Goalid
                               join user in _context.Users on goal.Userid equals user.Id
                               where goal.Userid == userid
                               select new AporteViewModel
                               {
                                   MontoOriginal = (double)goaltransaction.Amount,
                                   Cambio = (from currencyindicator in _context.Currencyindicators
                                             where currencyindicator.Sourcecurrencyid == user.Currencyid && currencyindicator.Destinationcurrencyid == goaltransaction.Currencyid && currencyindicator.Date.Equals(goaltransaction.Date)
                                             select currencyindicator.Value).FirstOrDefault(),
                                   Fecha = goaltransaction.Date,
                                   GoalId = goal.Id
                               };

            if (goalid != null)
                listaAportes = listaAportes.Where(m => m.GoalId.Equals(goalid));

            return await listaAportes.ToListAsync();
        }

        public async Task<List<BalanceViewModel>> GetBalanceById(int userid)
        {
            var balance = from goal in _context.Goals
                          join goaltransactionfunding in _context.Goaltransactionfundings on goal.Id equals goaltransactionfunding.Goalid
                          join user in _context.Users on goal.Userid equals user.Id
                          where goal.Userid == userid
                          select new BalanceViewModel
                          {
                              ShareValue = (from fundingsharevalue in _context.Fundingsharevalues
                                            where fundingsharevalue.Id == goaltransactionfunding.Fundingid && fundingsharevalue.Date.Equals(goaltransactionfunding.Date)
                                            select fundingsharevalue.Value).FirstOrDefault(),
                              Cambio = (from currencyindicator in _context.Currencyindicators
                                        where currencyindicator.Sourcecurrencyid == user.Currencyid && currencyindicator.Destinationcurrencyid == (from goaltransaction in _context.Goaltransactions where goaltransaction.Id == goaltransactionfunding.Transactionid select goaltransaction.Currencyid).FirstOrDefault()
                                        && currencyindicator.Date == goaltransactionfunding.Date
                                        select currencyindicator.Value).FirstOrDefault(),
                              Quotas = (double)goaltransactionfunding.Quotas,
                              Fecha = goaltransactionfunding.Date
                          };
            return await balance.ToListAsync();
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
                              let sharevalue = (from fundingsharevalue in db.Fundingsharevalues
                                                where fundingsharevalue.Id == goaltransactionfunding.Fundingid && fundingsharevalue.Date == goaltransactionfunding.Date
                                                select fundingsharevalue.Value).Max()
                              join user in db.Users on goal.Userid equals user.Id
                              where goal.Userid == userid
                              select new BalanceViewModel
                              {
                                  ShareValue = sharevalue,
                                  Cambio = (from currencyindicator in db.Currencyindicators
                                            where currencyindicator.Sourcecurrencyid == user.Currencyid && currencyindicator.Destinationcurrencyid == (from goaltransaction in db.Goaltransactions where goaltransaction.Id == goaltransactionfunding.Transactionid select goaltransaction.Currencyid).FirstOrDefault()
                                            && currencyindicator.Date == goaltransactionfunding.Date
                                            select currencyindicator.Value).FirstOrDefault(),
                                  Quotas = (double)goaltransactionfunding.Quotas
                              };

                var b = balance.ToList();

                foreach (var item in balance.Where(w => w.Cambio != 0))
                {
                    item.MontoFinal = item.Quotas * item.Cambio * item.ShareValue;
                }

                foreach (var item in balance.Where(w => w.Cambio == 0))
                {
                    item.MontoFinal = item.Quotas * item.Cambio;
                }

                return new ResumenViewModel(listaAportes.Sum(a => a.MontoFinal) , balance.Sum(b => b.MontoFinal));
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

                return new ResumenViewModel(listaAportes.Sum(a => a.MontoFinal), balance.Sum(b => b.MontoFinal));
            }

        }

        public async Task<List<MetaViewModel>> getMetas(int userid, int? goalid = null)
        {
            
                var listaMetas = from metas in _context.Goals
                                 join portafolio in _context.Portfolios on metas.Portfolioid equals portafolio.Id
                                 join riesgo in _context.Risklevels on portafolio.Risklevelid equals riesgo.Id
                                 join estrategiainversion in _context.Investmentstrategies on portafolio.Investmentstrategyid equals estrategiainversion.Id
                                 join entidadfinanciera in _context.Financialentities on metas.Financialentityid equals entidadfinanciera.Id
                                 where metas.Userid == userid
                                 select new MetaViewModel
                                 {
                                     Id = metas.Id,
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

                if (goalid != null)
                    listaMetas = listaMetas.Where(m => m.Id.Equals(goalid));
                
                return await listaMetas.ToListAsync();
            

        }

        public MetaViewModel getMetaDetail(int userid, int goalid)
        {
            throw new NotImplementedException();
        }
    }
}