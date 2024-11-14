using JobHubAPI.Context;
using Microsoft.Data.SqlClient;
using System.Data;

namespace JobHubAPI.DbSetup
{
    public sealed class MsSqlQueryBuilder : QueryBuilder
    {
        public MsSqlQueryBuilder(ISQLTemplate template, ITableNameResolver tblresolver, IColumnNameResolver colresolver)
          : base(template, tblresolver, colresolver)
        {
        }
        public MsSqlQueryBuilder(ISQLTemplate template)
         : base(template)
        {
        }
    }
    public class MsSQLTemplate : ISQLTemplate
    {

        public string Select => $"Select {0}from {1}";
        public string IdentitySql => $"SELECT CAST(SCOPE_IDENTITY()  AS BIGINT) AS [id]";
        // public string PaginatedSql=>"SELECT * FROM (SELECT ROW_NUMBER() OVER(ORDER BY {OrderBy}) AS PagedNumber, {SelectColumns} FROM {TableName} {WhereClause}) AS u WHERE PagedNUMBER BETWEEN (({PageNumber}-1) * {RowsPerPage} + 1) AND ({PageNumber} * {RowsPerPage})";
        public string PaginatedSql
            =>
                "SELECT COUNT(1) OVER() AS RowTotal, {SelectColumns}FROM {TableName} {join} {WhereClause} Order By {OrderBy} OFFSET {RowsPerPage} * ({PageNumber}-1) ROWS FETCH NEXT {RowsPerPage} ROWS ONLY"
            ;
        public string Encapsulation => "[{0}]";



    }

    public class MsSQLFactory : IDatabaseFactory
    {
        public IDbConnection Db { get; set; }
        public Dialect Dialect => Dialect.SQLServer;

        public QueryBuilder QueryBuilder { get; }

        private readonly string _connectionString;


        public IDbConnection GetConnection()
        {
            var Db = new SqlConnection(_connectionString);
            return Db;
        }
        //in appsetting file
        //      "DBInfo": {
        //  "Name": "coresample",
        //  "ConnectionString": "User ID=postgres;Password=xxxxxx;Host=localhost;Port=5432;Database=coresample;Pooling=true;"
        //}
        public MsSQLFactory(IConfiguration configuration, IServiceProvider serviceProvider)
        {
            //IConfiguration configuration
            // connectionString = configuration.GetValue<string>("DBInfo:ConnectionString");
            _connectionString = configuration.GetConnectionString("DefaultConnection");// ConfigurationManager.ConnectionStrings[connectionStringName].ToString();
            var Db = new SqlConnection(_connectionString);
            QueryBuilder = new MsSqlQueryBuilder(new MsSQLTemplate());
            var hostingenv = serviceProvider.GetService<IWebHostEnvironment>();
        }

        public void Dispose()
        {
            if (Db.State == ConnectionState.Open)
                Db.Close();
            Db.Dispose();
        }

    }


}
