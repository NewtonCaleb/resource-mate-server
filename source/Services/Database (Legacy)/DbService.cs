using System.Data;
using System.Reflection;
using Microsoft.Extensions.Options;
using MySqlConnector;
using Namotion.Reflection;

namespace SocialWorkApi.Services.Database;

public class DbService(IOptions<DbOptions> options) : IDbService
{
    private readonly DbOptions _dbConfig = options.Value;

    public async Task<IEnumerable<T>> Select<T>(string sql, IDictionary<string, object>? parameters)
    {
        IEnumerable<T> results = [];
        try
        {
            await using MySqlConnection connection = new(_dbConfig.ConnectionString);
            await connection.OpenAsync();
            await using MySqlCommand command = new(sql, connection);

            command.Parameters.AddRange(GetSqlParamters(parameters).ToArray());

            await using MySqlDataReader? reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                results = results.Append(await TranslateToEntity<T>(reader));
            }

        }
        catch (Exception)
        {
            throw;
        }
        return results;
    }

    public async Task<DataTable> Query(string sql, IDictionary<string, object>? parameters)
    {
        DataTable results = new();
        try
        {
            await using MySqlConnection connection = new(_dbConfig.ConnectionString);
            await connection.OpenAsync();
            await using MySqlCommand command = new(sql, connection);

            command.Parameters.AddRange(GetSqlParamters(parameters).ToArray());

            using MySqlDataAdapter adapter = new(command);
            adapter.Fill(results);
        }
        catch (Exception)
        {
            throw;
        }
        return results;
    }

    public async Task<int> Insert(Dictionary<string, object> parameters, string table)
    {
        int insertedId = -1;

        IEnumerable<string> valueKeys = parameters.Keys.Select((key) => !key.StartsWith('@') ? $"@{key}" : key);
        IEnumerable<string> columnNames = parameters.Keys.Select((key) => key.StartsWith('@') ? key[1..] : key);

        string sql = $"INSERT INTO {table} ({string.Join(",", columnNames)}) VALUES ({string.Join(",", valueKeys)});";
        Console.WriteLine(sql);
        try
        {
            await using MySqlConnection connection = new(_dbConfig.ConnectionString);
            await connection.OpenAsync();
            await using MySqlCommand command = new(sql, connection);

            List<MySqlParameter> args = GetSqlParamters(parameters);

            command.Parameters.AddRange(args.ToArray());
            await command.ExecuteNonQueryAsync();

            await using MySqlCommand lastInsertCommand = new($"SELECT Id FROM {table} WHERE Id = LAST_INSERT_ID()", connection);

            await using MySqlDataReader? reader = await lastInsertCommand.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                insertedId = reader.GetInt32(reader.GetOrdinal("Id"));
            }
        }
        catch (Exception)
        {
            throw;
        }
        return insertedId;
    }

    public async Task<int> Insert<T>(T classObj, string table)
    {
        Dictionary<string, object>? parameters = GetKeyValuePairs(classObj) ?? throw new Exception("No parameters received");
        IEnumerable<string> valueKeys = parameters.Keys.Select((key) => $"@{key}");

        int insertedId = -1;

        string sql = $"INSERT INTO {table} ({string.Join(",", parameters.Keys)}) VALUES ({string.Join(",", valueKeys)});";

        try
        {
            await using MySqlConnection connection = new(_dbConfig.ConnectionString);
            await connection.OpenAsync();
            await using MySqlCommand command = new(sql, connection);

            List<MySqlParameter> args = [..GetSqlParamters(parameters)];
            command.Parameters.AddRange(args.ToArray());
            await command.ExecuteNonQueryAsync();

            await using MySqlCommand lastInsertCommand = new($"SELECT Id FROM {table} WHERE Id = LAST_INSERT_ID()", connection);

            await using MySqlDataReader? reader = await lastInsertCommand.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                insertedId = reader.GetInt32(reader.GetOrdinal("Id"));
            }

        }
        catch (Exception)
        {
            throw;
        }
        return insertedId;
    }

    public async Task Update(IDictionary<string, object> parameters, string table)
    {
        object id = parameters?.TryGetPropertyValue<IDictionary<string,object>>("Id", null) ?? throw new Exception("Id is missing");
        parameters.Remove("Id");

        IEnumerable<string> valueKeys = parameters.Keys.Select((key) => !key.StartsWith('@') ? $"@{key}" : key);
        IEnumerable<string> args = parameters.Select((pair) => pair.Key.StartsWith('@') ? $"{pair.Key[1..]}={pair.Key}" : $"{pair.Key}=@{pair.Key}");

        string sql = $"UPDATE {table} SET {string.Join(",", args)} WHERE Id = @Id";

        try
        {
            await using MySqlConnection connection = new(_dbConfig.ConnectionString);
            await connection.OpenAsync();
            await using MySqlCommand command = new(sql, connection);

            List<MySqlParameter> sqlArgs = [new("@Id", id), ..GetSqlParamters(parameters)];

            command.Parameters.AddRange(sqlArgs.ToArray());

            await using MySqlDataReader? reader = await command.ExecuteReaderAsync();
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task Update<T>(T classObj, string table)
    {
        Dictionary<string, object> parameters = GetKeyValuePairs(classObj) ?? throw new Exception("Parameters not received");
        parameters.TryGetValue("Id", out object? id);
        if (id == null)
        {
            throw new Exception("No Id was passed");            
        }
        parameters.Remove("Id");

        IEnumerable<string> valueKeys = parameters.Keys.Select((key) => !key.StartsWith('@') ? $"@{key}" : key);
        IEnumerable<string> args = parameters.Select((pair) => pair.Key.StartsWith('@') ? $"{pair.Key[1..]}={pair.Key}" : $"{pair.Key}=@{pair.Key}");

        string sql = $"UPDATE {table} SET {string.Join(",", args)} WHERE Id = @Id";

        try
        {
            await using MySqlConnection connection = new(_dbConfig.ConnectionString);
            await connection.OpenAsync();
            await using MySqlCommand command = new(sql, connection);

            List<MySqlParameter> sqlArgs = [new MySqlParameter("@Id", id), ..GetSqlParamters(parameters)];

            command.Parameters.AddRange(sqlArgs.ToArray());

            await using MySqlDataReader? reader = await command.ExecuteReaderAsync();
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<Dictionary<string, List<object>>> BulkUpdate<T>(ICollection<T> objectsToUpdate, string table)
    {
        if (objectsToUpdate.Count == 0) return [];

        List<object> errors = [];
        List<object> successes = [];

        foreach (T obj in objectsToUpdate)
        {
            Dictionary<string, object>? parameters = GetKeyValuePairs(obj);

            if (parameters == null)
            {
                errors.Add("-1");
                continue;
            }

            parameters.TryGetValue("Id", out object? id);
            if (id == null)
            {
                errors.Add("-1");
                continue;
            }

            parameters.Remove("Id");

            IEnumerable<string> valueKeys = parameters.Keys.Select((key) => !key.StartsWith('@') ? $"@{key}" : key);
            IEnumerable<string> args = parameters.Select((pair) => pair.Key.StartsWith('@') ? $"{pair.Key[1..]}={pair.Key}" : $"{pair.Key}=@{pair.Key}");

            string sql = $"UPDATE {table} SET {string.Join(",", args)} WHERE Id = @Id";

            try
            {
                await using MySqlConnection connection = new(_dbConfig.ConnectionString);
                await connection.OpenAsync();
                await using MySqlCommand command = new(sql, connection);

                List<MySqlParameter> sqlArgs = [new("@Id", id), .. GetSqlParamters(parameters)];

                if (sqlArgs.FirstOrDefault((item) => item?.ParameterName == "@Id", null) == null)
                {
                    errors.Add("-1");
                    continue;
                }

                command.Parameters.AddRange(sqlArgs.ToArray());

                await command.ExecuteNonQueryAsync();

                successes.Add(parameters!.GetValueOrDefault("Id", null) ?? "0");
            }
            catch (Exception)
            {
                throw;
            }
        }

        return new()
        {
            {"errors", errors},
            {"successes", successes}
        };
    }

    public async Task Delete(int id, string table)
    {
        string sql = $"UPDATE {table} SET Deleted = 1 WHERE Id = @Id";
        try
        {
            await using MySqlConnection connection = new(_dbConfig.ConnectionString);
            await connection.OpenAsync();
            await using MySqlCommand command = new(sql, connection);

            MySqlParameter[] args = {
                new("@Id", id)
            };

            command.Parameters.AddRange(args);

            await command.ExecuteNonQueryAsync();
        }
        catch (Exception)
        {
            throw;
        }

    }

    public async Task BulkDelete(int[] ids, string table)
    {
        string sql = $"DELETE FROM {table} WHERE Id IN (@Ids)";
        try
        {
            await using MySqlConnection connection = new(_dbConfig.ConnectionString);
            await connection.OpenAsync();
            await using MySqlCommand command = new(sql, connection);

            MySqlParameter args = new("@Ids", string.Join(",", ids));
            command.Parameters.Add(args);

            await command.ExecuteNonQueryAsync();
        }
        catch (Exception)
        {
            throw;
        }

    }

    static private async Task<T> TranslateToEntity<T>(MySqlDataReader reader)
    {
        Type t = typeof(T);
        T translatedObj = Activator.CreateInstance<T>();

        foreach (PropertyInfo propInfo in t.GetProperties().Where((i) => i.MemberType == MemberTypes.Property))
        {
            int index;
            try
            {
                index = reader.GetOrdinal(propInfo.Name);

                if (!await reader.IsDBNullAsync(index))
                {
                    var value = reader.GetValue(index);
                    propInfo.SetValue(translatedObj, value);
                }
            }
            catch (IndexOutOfRangeException)
            {
                // Column no longer exists
                continue;
            }
            catch (Exception)
            {
                // Unknown error
                continue;
            }
        }

        return translatedObj;
    }

    static private Dictionary<string, object>? GetKeyValuePairs<T>(T obj)
    {
        Type? objType = obj?.GetType();

        if (objType == null)
        {
            return default;
        }

        Dictionary<string, object> results = [];

        if (!objType.IsPrimitive)
        {
            foreach (PropertyInfo p in objType.GetProperties().Where((i) => i.MemberType == MemberTypes.Property))
            {
                object? value = p.GetValue(obj);
                if (value != null)
                {
                    results.Add(p.Name, value);
                }
            }
        }
        else
        {
            return default;
        }

        return results;
    }

    static private List<MySqlParameter> GetSqlParamters(IDictionary<string, object>? parameters)
    {
        List<MySqlParameter> sqlArgs = [];
        if (parameters != null && parameters.Count != 0)
        {
            foreach (KeyValuePair<string, object> p in parameters)
            {
                if (!p.Key.StartsWith('@'))
                {
                    sqlArgs.Add(new MySqlParameter($"@{p.Key}", p.Value));
                }
                else
                {
                    sqlArgs.Add(new MySqlParameter(p.Key, p.Value));
                }
            }
        }

        return sqlArgs;
    }
}