
using JobHubAPI.Context;
using System.Data;

namespace JobHubAPI.DbSetup
{
    public enum Dialect
    {
        SQLServer,
        PostgreSQL,
        SQLite,
        MySQL,
    }
    public interface IDatabaseFactory : IDisposable
    {
        IDbConnection Db { get; }
        Dialect Dialect { get; }
        QueryBuilder QueryBuilder { get; }
        IDbConnection GetConnection();
    }
   

    public class DatabaseFactories
    {
        /// <summary>
        ///  gets a provider specific (i.e. database specific) factory 
        /// </summary>
        /// <param name="dialect"></param>
        /// <param name="serviceProvider"></param>
        /// <returns>an instance of service factory of given provider.</returns>
        public static IDatabaseFactory GetFactory()
        {
            return DbFactoryProvider.GetFactory();
        }




        public static IDatabaseFactory SetFactory(IServiceProvider serviceProvider)
        {
            // return the requested DaoFactory
            var configuration = serviceProvider.GetService<IConfiguration>();


            var dbfactory = new MsSQLFactory(configuration, serviceProvider);
            DbFactoryProvider.SetCurrentDbFactory(dbfactory);
            return DbFactoryProvider.GetFactory();
        }
    }
}

