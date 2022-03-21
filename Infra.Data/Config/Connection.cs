using Dapper;
using Domain.Interfaces;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace Infra.Data.Config
{
    public class Connection : IConnection
    {
        private int Timeout { get; set; }

        private readonly SqlConnection _sqlConnection;

        private readonly IConfiguration _configuration;

        public Connection(IConfiguration configuration)
        {
            Timeout = 30;

            _configuration = configuration;

            _sqlConnection = new SqlConnection(_configuration.GetConnectionString("Default"));
        }

        private async Task<SqlConnection> ConectarAsync()
        {
            if (_sqlConnection.State == System.Data.ConnectionState.Closed)
            {
                if (string.IsNullOrEmpty(_sqlConnection.ConnectionString))
                    _sqlConnection.ConnectionString = _configuration.GetConnectionString("Default");

                await _sqlConnection.OpenAsync();
            }

            return _sqlConnection;
        }

        public async Task<IEnumerable<T>> ExecutarSelectListaAsync<T>(string query)
        {
            var connection = await ConectarAsync();

            return await connection.QueryAsync<T>(query, commandTimeout: Timeout);
        }

        public async Task<T> ExecutarSelectAsync<T>(string query)
        {
            var connection = await ConectarAsync();

            return await connection.QueryFirstOrDefaultAsync<T>(query, commandTimeout: Timeout);
        }

        public async Task<bool> ExecutarComandoAsync(string query)
        {
            var connection = await ConectarAsync();

            return await connection.ExecuteAsync(query, commandTimeout: Timeout) > 0;
        }

        public async Task<T> ExecutarProcedureAsync<T>(string query)
        {
            var connection = await ConectarAsync();

            return await connection.QueryFirstOrDefaultAsync<T>(query, commandTimeout: Timeout);
        }
    }
}
