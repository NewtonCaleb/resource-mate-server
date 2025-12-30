using System.Data;

namespace SocialWorkApi.Services.Database;

public interface IDbService
{
    /// <summary>
    /// Select query that auto maps to the specified type.
    /// </summary>
    /// <typeparam name="T">Type to map the data to</typeparam>
    /// <param name="sql">Sql query</param>
    /// <param name="parameters">Parameters without the @ symbol. Key is column name and value is the value to go off of it.</param>
    /// <returns>Enumerable that is mapped to the specified type</returns>
    public Task<IEnumerable<T>> Select<T>(string sql, IDictionary<string, object>? parameters = null);

    /// <summary>
    /// This function allows for anything and whatever is returned is put into a DataTable
    /// </summary>
    /// <param name="sql">Sql Query</param>
    /// <param name="parameters">Parameters without the @ symbol. Key is column name and value is the value to go off of it.</param>
    /// <returns>DataTable of whatever the Db sends back</returns>
    public Task<DataTable> Query(string sql, IDictionary<string, object>? parameters);

    /// <summary>
    /// Insert query that takes in any params.
    /// </summary>
    /// <param name="parameters">Parameters without the @ symbol. Key is column name and value is the value to go off of it.</param>
    /// <param name="table">Table to Insert into</param>
    /// <returns>Inserted Id</returns>
    public Task<int> Insert(Dictionary<string, object> parameters, string table);

    /// <summary>
    /// Insert query that takes whatever properties are given via "classObj" and puts it into the query
    /// </summary>
    /// <typeparam name="T">Class Obj type</typeparam>
    /// <param name="classObj">Any POCO that aligns with the table's structure</param>
    /// <param name="table">Table to Insert into</param>
    /// <returns>Inserted Id</returns>
    public Task<int> Insert<T>(T classObj, string table);

    /// <summary>
    /// Update query that takes in any params
    /// </summary>
    /// <param name="id">Id to update</param>
    /// <param name="parameters">Parameters without the @ symbol. Key is column name and value is the value to go off of it.</param>
    /// <param name="table">Table to update</param>
    /// <returns></returns>
    public Task Update(IDictionary<string, object> parameters, string table);

    /// <summary>
    /// Update query that takes whatever properties are given via "classObj" and puts it into the query.
    /// </summary>
    /// <typeparam name="T">Class Obj Type</typeparam>
    /// <param name="id">Id to update</param>
    /// <param name="classObj">Any POCO that aligns with the table's structure</param>
    /// <param name="table">Table to Update</param>
    /// <returns></returns>
    public Task Update<T>(T classObj, string table);

    public Task<Dictionary<string, List<object>>> BulkUpdate<T>(ICollection<T> classObjs, string table);

    /// <summary>
    /// Deletes resource
    /// </summary>
    /// <param name="id">Id of resource to delete</param>
    /// <param name="table">Table to delete from</param>
    /// <returns></returns>
    public Task Delete(int id, string table);

    /// <summary>
    /// Deletes multiple resources
    /// </summary>
    /// <param name="ids">Ids of resouces to delte</param>
    /// <param name="table">Table to delete from</param>
    /// <returns></returns>
    public Task BulkDelete(int[] ids, string table);
}