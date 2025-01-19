// ReSharper disable EmptyGeneralCatchClause
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using MySql.Data.MySqlClient;

namespace Calculator
{
    public class TrixOrm
    {
        private static readonly TrixOrm Me = new TrixOrm();
        public enum DbmsType { SqlServer, MySql }
        public DbmsType Type { get; set; }
        public string ConnectionString { get; set; }

        private TrixOrm() { }

        private DbConnection GetConnection()
        {
            switch (Type)
            {
                case DbmsType.SqlServer:
                    return new SqlConnection(ConnectionString);
                case DbmsType.MySql:
                    return new MySqlConnection(ConnectionString);
                default: throw new TypeUnloadedException();
            }
        }

        private DbCommand GetCommand(String s, DbConnection connection)
        {
            switch (Type)
            {
                case DbmsType.SqlServer:
                    return new SqlCommand(s, (SqlConnection)connection);
                case DbmsType.MySql:
                    return new MySqlCommand(s, (MySqlConnection)connection);
                default: throw new TypeUnloadedException();
            }
        }

        private DbDataAdapter GetDataAdapter(String s, DbConnection connection)
        {
            switch (Type)
            {
                case DbmsType.SqlServer:
                    return new SqlDataAdapter(s, (SqlConnection)connection);
                case DbmsType.MySql:
                    return new MySqlDataAdapter(s, (MySqlConnection)connection);
                default: throw new TypeUnloadedException();
            }
        }

        public static TrixOrm GetInstance()
        {
            return Me;
        }

        public Object GetScalar(String sqlQuery)
        {
            try { return GetArray(sqlQuery)[0]; }
            catch { return null; }
        }

        public Object[] GetCortage(String sqlQuery)
        {
            try { return GetListOfCortages(sqlQuery)[0]; }
            catch { return null; }
        }

        public List<Object> GetArray(String sqlQuery)
        {
            try { return GetListOfCortages(sqlQuery).Select(t => t[0]).ToList(); }
            catch { return null; }
        }

        public List<Object[]> GetListOfCortages(String sqlQuery)
        {
            try
            {
                var res = new List<Object[]>();
                using (var sql = GetConnection())
                {
                    var sqlcmd = GetCommand(sqlQuery, sql);
                    sql.Open();
                    using (var reader = sqlcmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var a = new Object[reader.FieldCount];
                            reader.GetValues(a);
                            res.Add(a);
                        }
                    }
                }
                return res;
            }
            catch { return null; }
        }

        public int Execute(String sqlQuery)
        {
            try
            {
                int rows;
                using (var sql = GetConnection())
                {
                    var sqlcmd = GetCommand(sqlQuery, sql);
                    sql.Open();
                    rows = sqlcmd.ExecuteNonQuery();
                }
                return rows;
            }
            catch { return -1; }
        }

        public DataTable GetDataTable(String sqlQuery)
        {
            DataTable result;
            using (var sql = GetConnection())
            {
                var ds = new DataSet();
                var adapter = GetDataAdapter(sqlQuery, sql);
                adapter.Fill(ds, "0");
                result = ds.Tables["0"];
            }
            return result;
        }
    }
}
