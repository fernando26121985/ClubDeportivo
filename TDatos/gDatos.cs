
using Npgsql;
namespace ClubDeportivo.TDatos
{
    using Npgsql;
    using System.Data;

  
        public class TDatosPostgreSQL : IDisposable
        {
            private readonly string _connectionString;
            private readonly NpgsqlConnection _connection;
            private NpgsqlTransaction _transaction;

            public TDatosPostgreSQL(string connectionString)
            {
                _connectionString = connectionString;
                _connection = new NpgsqlConnection(_connectionString);
            }
        public NpgsqlConnection Connection => _connection;

        public NpgsqlTransaction? Transaction => _transaction;
        public async Task OpenConnectionAsync()
            {
                if (_connection.State != ConnectionState.Open)
                    await _connection.OpenAsync();
            }

            public NpgsqlConnection GetConnection()
            {
                return _connection;
            }

            public async Task BeginTransactionAsync()
            {
                _transaction = await _connection.BeginTransactionAsync();
            }

            public async Task CommitTransactionAsync()
            {
                if (_transaction != null)
                {
                    await _transaction.CommitAsync();
                    await _transaction.DisposeAsync();
                    _transaction = null;
                }
            }

            public async Task RollbackTransactionAsync()
            {
                if (_transaction != null)
                {
                    await _transaction.RollbackAsync();
                    await _transaction.DisposeAsync();
                    _transaction = null;
                }
            }
        public async Task<List<T>> QueryFunctionAsync<T>(
          string sql,
          Func<NpgsqlDataReader, T> map,
          NpgsqlParameter[] parameters = null)
        {
            var lista = new List<T>();

            try
            {
                using var command = new NpgsqlCommand(
                    sql,
                    _connection,
                    _transaction);

                // Agregar parámetros
                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters);
                }

                // Ejecutar lectura
                using var reader = await command.ExecuteReaderAsync();

                // Recorrer filas
                while (await reader.ReadAsync())
                {
                    lista.Add(map(reader));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    $"Error en QueryFunctionAsync: {ex.Message}");

                throw;
            }

            return lista;
        }
    
            public async Task<int> ExecuteNonQueryAsync(
                string sql,
                NpgsqlParameter[] parameters = null)
            {
                using var command = CreateCommand(sql, parameters);

                return await command.ExecuteNonQueryAsync();
            }

            public async Task<object> ExecuteScalarAsync(
                string sql,
                NpgsqlParameter[] parameters = null)
            {
                using var command = CreateCommand(sql, parameters);

                return await command.ExecuteScalarAsync();
            }

            public async Task<List<T>> QueryAsync<T>(
                string sql,
                Func<NpgsqlDataReader, T> map,
                NpgsqlParameter[] parameters = null)
            {
                var lista = new List<T>();

                using var command = CreateCommand(sql, parameters);

                using var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    lista.Add(map(reader));
                }

                return lista;
            }

            private NpgsqlCommand CreateCommand(
                string sql,
                NpgsqlParameter[] parameters = null)
            {
                var command = new NpgsqlCommand(sql, _connection, _transaction);

                if (parameters != null)
                    command.Parameters.AddRange(parameters);

                return command;
            }


            public async Task CloseConnectionAsync()
            {
                if (_connection.State != ConnectionState.Closed)
                    await _connection.CloseAsync();
            }

            public void Dispose()
            {
                _transaction?.Dispose();
                _connection?.Dispose();
            }
        }
    
}
