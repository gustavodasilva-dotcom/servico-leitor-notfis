using System;
using System.Text;
using Domain.Entities;
using Domain.Interfaces;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Infra.Data.Repository
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly IConnection _connection;

        public ClienteRepository(IConnection connection)
        {
            _connection = connection;
        }

        public async Task<IEnumerable<PastasClientes>> ObterCaminhosAsync()
        {
            var query = new StringBuilder();

            #region SQL

            query.Append(@" SELECT PC.*");
            query.Append(@" FROM Clientes C (NOLOCK)");
            query.Append(@" INNER JOIN PastasClientes PC (NOLOCK) ON C.ID = PC.ClienteID");
            query.Append(@" WHERE C.Ativo = 1");
            query.Append(@" AND C.Excluido = 0");
            query.Append(@" AND PC.Ativo = 1");
            query.Append(@" AND PC.Excluido = 0;");

            #endregion

            try
            {
                return await _connection.ExecutarSelectListaAsync<PastasClientes>(query.ToString());
            }
            catch (Exception) { throw; }
        }

        public async Task<IEnumerable<Clientes_Tags>> ObterTagsClienteAsync(int clienteID)
        {
            var query = new StringBuilder();

            #region SQL

            query.Append(@" SELECT *");
            query.Append(@" FROM Clientes_Tags (NOLOCK)");
            query.Append(@" WHERE Ativo = 1");
            query.Append(@" AND Excluido = 0");
            query.Append($@" AND ClienteID = {clienteID};");

            #endregion

            try
            {
                return await _connection.ExecutarSelectListaAsync<Clientes_Tags>(query.ToString());
            }
            catch (Exception) { throw; }
        }

        public async Task<IEnumerable<InformacoesLinhas>> ObterInformacoesLinhasAsync(int clienteID)
        {
            var query = new StringBuilder();

            #region SQL

            query.Append(@"SELECT IL.Descricao,");
            query.Append(@" LN.*");
            query.Append(@" FROM Clientes C (NOLOCK)");
            query.Append(@" INNER JOIN Layouts L (NOLOCK)");
            query.Append(@" ON C.LayoutID = L.ID");
            query.Append(@" INNER JOIN Linhas LN (NOLOCK)");
            query.Append(@" ON LN.LayoutID = L.ID");
            query.Append(@" INNER JOIN Linhas_InformacoesLinhas LI (NOLOCK)");
            query.Append(@" ON LI.LinhaID = LN.ID");
            query.Append(@" INNER JOIN InformacoesLinhas IL (NOLOCK)");
            query.Append(@" ON LI.InformacaoLinhaID = IL.ID");
            query.Append($@" WHERE C.ID = {clienteID};");

            #endregion

            try
            {
                return await _connection.ExecutarSelectListaAsync<InformacoesLinhas>(query.ToString());
            }
            catch (Exception) { throw; }
        }
    }
}
