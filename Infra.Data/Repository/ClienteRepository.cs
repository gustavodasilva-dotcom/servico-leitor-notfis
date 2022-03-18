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
            query.Append(@" INNER JOIN Clientes_Layouts CL (NOLOCK) ON C.ID = CL.ClienteID");
            query.Append(@" INNER JOIN Layouts LY (NOLOCK) ON LY.ID = CL.LayoutID");
            query.Append(@" INNER JOIN PastasClientes PC (NOLOCK) ON CL.ID = PC.Cliente_LayoutID");
            query.Append(@" WHERE C.Ativo = 1");
            query.Append(@" AND C.Excluido = 0");
            query.Append(@" AND CL.Ativo = 1");
            query.Append(@" AND CL.Excluido = 0");
            query.Append(@" AND LY.Ativo = 1");
            query.Append(@" AND LY.Excluido = 0");
            query.Append(@" AND PC.Ativo = 1");
            query.Append(@" AND PC.Excluido = 0;");

            #endregion

            try
            {
                return await _connection.ExecutarSelectListaAsync<PastasClientes>(query.ToString());
            }
            catch (Exception) { throw; }
        }

        public async Task<Extensoes> ObterClientesExtensoesAsync(int cliente_layoutID)
        {
            var query = new StringBuilder();

            #region SQL

            query.Append(@" SELECT E.*");
            query.Append(@" FROM Clientes C (NOLOCK)");
            query.Append(@" INNER JOIN Clientes_Layouts CL (NOLOCK) ON C.ID = CL.ClienteID");
            query.Append(@" INNER JOIN Layouts LY (NOLOCK) ON LY.ID = CL.LayoutID");
            query.Append(@" INNER JOIN Extensoes E (NOLOCK) ON E.ID = LY.ExtensaoID");
            query.Append(@" WHERE C.Ativo = 1");
            query.Append(@" AND C.Excluido = 0");
            query.Append(@" AND CL.Ativo = 1");
            query.Append(@" AND CL.Excluido = 0");
            query.Append(@" AND LY.Ativo = 1");
            query.Append(@" AND LY.Excluido = 0");
            query.Append(@" AND E.Ativo = 1");
            query.Append(@" AND E.Excluido = 0");
            query.Append($@" AND CL.ID = {cliente_layoutID};");

            #endregion

            try
            {
                return await _connection.ExecutarSelectAsync<Extensoes>(query.ToString());
            }
            catch (Exception) { throw; }
        }

        public async Task<IEnumerable<Clientes_Layouts_Tag>> ObterTagsClienteAsync(int cliente_layoutID)
        {
            var query = new StringBuilder();

            #region SQL

            query.Append(@" SELECT *");
            query.Append(@" FROM Clientes_Layouts_Tag (NOLOCK)");
            query.Append(@" WHERE Ativo = 1");
            query.Append(@" AND Excluido = 0");
            query.Append($@" AND Cliente_LayoutID = {cliente_layoutID};");

            #endregion

            try
            {
                return await _connection.ExecutarSelectListaAsync<Clientes_Layouts_Tag>(query.ToString());
            }
            catch (Exception) { throw; }
        }
    }
}
