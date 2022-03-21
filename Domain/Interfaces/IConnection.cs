using System.Threading.Tasks;
using System.Collections.Generic;

namespace Domain.Interfaces
{
    public interface IConnection
    {
        Task<IEnumerable<T>> ExecutarSelectListaAsync<T>(string query);

        Task<T> ExecutarSelectAsync<T>(string query);

        Task<bool> ExecutarComandoAsync(string query);

        Task<T> ExecutarProcedureAsync<T>(string query);
    }
}
