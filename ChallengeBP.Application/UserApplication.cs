using AutoMapper;
using ChallengeBP.Entities.ViewModels;
using ChallengeBP.Repository;

namespace ChallengeBP.Application
{
    public class UserApplication : IUserApplication
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserApplication(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<UserViewModel> GetUserById(int userid)
        {
            var user = await _userRepository.GetUserById(userid);

            int advisorid = user.Advisorid ?? default(int);
            var advisor = advisorid != default(int) ? await _userRepository.GetUserById(advisorid) : null;

            UserViewModel userViewModel = new UserViewModel(userid, user.Firstname + ' ' + user.Surname, user.Created);

            //Validando si el advisor existe
            if (advisor != null)
                userViewModel.nombrecompletoadvisor = advisor.Firstname + ' ' + user.Surname;

            return userViewModel;

        }       

        public async Task<ResumenViewModel> GetResumenById(int userid)
        {
            
            var aportes = await _userRepository.GetAportesById(userid);

            foreach(var item in aportes)
            {
                if (item.Cambio != 0)
                    item.MontoFinal = item.MontoOriginal * item.Cambio;
                else
                    item.MontoFinal = item.MontoOriginal;
            }
            
            var balance = await _userRepository.GetBalanceById(userid);

            foreach(var item in balance)
            {
                if (item.Cambio != 0)
                    item.MontoFinal = item.Quotas * item.Cambio * item.ShareValue;
                else
                    item.MontoFinal = item.Quotas * item.Cambio;
            }

            //Calculando la sumatoria de los Monto Finales para calcular el aporte total y balance
            double aportetotal = aportes.Sum(a => a.MontoFinal);
            double balancetotal = balance.Sum(b => b.MontoFinal);

            return new ResumenViewModel(aportetotal, balancetotal);
        }

        public async Task<ResumenViewModel> GetResumenByIdAndDate(int userid, DateOnly date)
        {

            var aportes = await _userRepository.GetAportesById(userid);

            //Filtrando la fecha
            aportes = aportes.Where(a => a.Fecha <= date).ToList();

            foreach (var item in aportes)
            {
                if (item.Cambio != 0)
                    item.MontoFinal = item.MontoOriginal * item.Cambio;
                else
                    item.MontoFinal = item.MontoOriginal;
            }

            var balance = await _userRepository.GetBalanceById(userid);

            //Filtrando la fecha
            balance = balance.Where(b => b.Fecha <= date).ToList();

            foreach (var item in balance)
            {
                if (item.Cambio != 0)
                    item.MontoFinal = item.Quotas * item.Cambio * item.ShareValue;
                else
                    item.MontoFinal = item.Quotas * item.Cambio;
            }

            //Calculando la sumatoria de los Monto Finales para calcular el aporte total y balance
            double aportetotal = aportes.Sum(a => a.MontoFinal);
            double balancetotal = balance.Sum(b => b.MontoFinal);

            return new ResumenViewModel(aportetotal, balancetotal);
        }

        public async Task<List<MetaViewModel>> GetMetasById(int userid)
        {
            return (await _userRepository.getMetas(userid));
        }

        public async Task<MetaDetalleViewModel> GetMetaDetail(int userid, int goalid)
        {
            //Obteniendo información de metas

            var meta = await _userRepository.getMetas(userid, goalid);

            MetaViewModel metaViewModel = meta.FirstOrDefault();
            MetaDetalleViewModel metaDetalleViewModel = _mapper.Map<MetaDetalleViewModel>(metaViewModel);

            //Calculando Aportes totales y retiros totales

            var aportes = await _userRepository.GetAportesById(userid, goalid);

            var retiros = aportes.Where(r => r.Tipo == EnumHelper.GetDescription(AporteViewModel.KDTipos.KDSale));
            aportes = aportes.Where(a => a.Tipo == EnumHelper.GetDescription(AporteViewModel.KDTipos.KDBuy)).ToList();

            foreach (var item in aportes)
            {
                if (item.Cambio != 0)
                    item.MontoFinal = item.MontoOriginal * item.Cambio;
                else
                    item.MontoFinal = item.MontoOriginal;
            }

            double totalaportes = aportes.Sum(a => a.MontoFinal);
            double totalretiros = aportes.Sum(r => r.MontoFinal);

            //Calculando Porcentaje Cumplimiento

            var balance = await _userRepository.GetBalanceById(userid, goalid);

            foreach (var item in balance)
            {
                if (item.Cambio != 0)
                    item.MontoFinal = item.Quotas * item.Cambio * item.ShareValue;
                else
                    item.MontoFinal = item.Quotas * item.Cambio;
            }

            double balancetotal = balance.Sum(b => b.MontoFinal);

            //Asignacion de Valores faltantes
            metaDetalleViewModel.TotalAportes = totalaportes;
            metaDetalleViewModel.TotalRetiros = totalretiros;
            metaDetalleViewModel.PorcenajeCumplimiento = totalaportes / metaViewModel.MontoObjetivo;

            return metaDetalleViewModel;
        }
    }
}