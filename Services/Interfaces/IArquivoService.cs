using Domain.Entities;

namespace Services.Interfaces
{
    public interface IArquivoService
    {
        void VerificarArquivos(PastasClientes pastaCliente, Extensoes extensao);
    }
}
