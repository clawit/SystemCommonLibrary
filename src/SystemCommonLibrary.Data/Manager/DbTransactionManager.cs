using MySql.Data.MySqlClient;
using Npgsql;
using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Threading.Tasks;
using SystemCommonLibrary.Enums;

namespace SystemCommonLibrary.Data.Manager
{
    public static class DbTransactionManager
    {
        public static async Task<System.Data.IDbTransaction> TryBeginTransaction(DbType type, string db)
        {
            DbConnection conn = null;
            try
            {
                switch (type)
                {
                    case DbType.MySql:
                        conn = new MySqlConnection(db);
                        break;
                    case DbType.SqlServer:
                        conn = new SqlConnection(db);
                        break;
                    case DbType.PostgreSql:
                        conn = new NpgsqlConnection(db);
                        break;
                    case DbType.Sqlite:
                        conn = new SQLiteConnection(db);
                        break;
                    default:
                        throw new NotImplementedException();
                }

                await conn.OpenAsync();

                return await conn.BeginTransactionAsync();
            }
            catch (Exception ex)
            {
#if DEBUG
                Console.WriteLine(ex.Message);
#endif

                if (conn != null && conn.State == System.Data.ConnectionState.Open)
                {
                    await conn.CloseAsync();
                }
                conn.Dispose();

                return null;
            }
        }

        public static bool CommitTransaction(System.Data.IDbTransaction transaction)
        {
            if (transaction != null)
            {
                if (transaction.Connection.State == System.Data.ConnectionState.Open)
                {
                    transaction.Commit();
                }
                transaction.Connection.Dispose();
                transaction.Dispose();
                return true;
            }

            return false;
        }

        public static void RollbackTransaction(System.Data.IDbTransaction transaction)
        {
            if (transaction != null)
            {
                if (transaction.Connection.State == System.Data.ConnectionState.Open)
                {
                    transaction.Rollback();
                }
                transaction.Connection.Dispose();
                transaction.Dispose();
            }
        }
    }
}
