using System.Data;
using Dapper;
using JobHubAPI.DbSetup;

namespace JobHubAPI.Crud
{
    public static class CrudHelper
    {
        public static IDatabaseFactory GetFactory(this IDbConnection connection)
        {
            return DbFactoryProvider.GetFactory();
        }
        public static async Task<TKey> InsertAsync<TKey>(this IDbConnection connection, object entityToInsert,
          IDbTransaction transaction = null, int? commandTimeout = null)
        {
            IDatabaseFactory factory = connection.GetFactory();
            var sql = factory.QueryBuilder.Insert<TKey>(entityToInsert);
            var result = await connection.QueryAsync(sql.QuerySql, sql.Parameters, transaction, commandTimeout);
            if (sql.IsKeyGuidType || sql.KeyHasPredefinedValue)
            {
                return (TKey)sql.Id;
            }
            return (TKey)result.First().id;
        }
    }
}
