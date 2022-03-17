using System;
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
            #region SQL

            var query =
            @"  SELECT		PC.*
                FROM		Clientes			C  (NOLOCK)
                INNER JOIN	Clientes_Layouts	CL (NOLOCK) ON C.ID  = CL.ClienteID
                INNER JOIN	Layouts				LY (NOLOCK) ON LY.ID = CL.LayoutID
                INNER JOIN	PastasClientes		PC (NOLOCK) ON CL.ID = PC.Cliente_LayoutID
                WHERE		C.Ativo		= 1
                  AND		C.Excluido  = 0
                  AND		CL.Ativo	= 1
                  AND		CL.Excluido = 0
                  AND		LY.Ativo	= 1
                  AND		LY.Excluido	= 0
                  AND		PC.Ativo	= 1
                  AND		PC.Excluido	= 0;";

            #endregion

            try
            {
                return await _connection.ExecutarSelectListaAsync<PastasClientes>(query);
            }
            catch (Exception) { throw; }
        }

        public async Task<Extensoes> ObterClientesExtensoesAsync(int cliente_layoutID)
        {
            #region SQL

            var query =
            $@" SELECT		E.*
                FROM		Clientes			C  (NOLOCK)
                INNER JOIN	Clientes_Layouts	CL (NOLOCK) ON C.ID  = CL.ClienteID
                INNER JOIN	Layouts				LY (NOLOCK) ON LY.ID = CL.LayoutID
                INNER JOIN	Extensoes			E  (NOLOCK) ON E.ID	 = LY.ExtensaoID
                WHERE		C.Ativo		= 1
                  AND		C.Excluido  = 0
                  AND		CL.Ativo	= 1
                  AND		CL.Excluido = 0
                  AND		LY.Ativo	= 1
                  AND		LY.Excluido	= 0
                  AND		E.Ativo		= 1
                  AND		E.Excluido	= 0
                  AND		CL.ID = {cliente_layoutID};";

            #endregion

            try
            {
                return await _connection.ExecutarSelectAsync<Extensoes>(query);
            }
            catch (Exception) { throw; }
        }
    }
}
