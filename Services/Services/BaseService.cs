using System;
using Services.Interfaces;

namespace Services.Services
{
    public class BaseService : IBaseService
    {
        private readonly IArquivoService _arquivoService;

        public BaseService(IArquivoService arquivoService)
        {
            _arquivoService = arquivoService;
        }

        public void ExecutarServico()
        {
            try
            {
                _arquivoService.VerificarArquivos();
            }
            catch (Exception) { throw; }
        }
    }
}
