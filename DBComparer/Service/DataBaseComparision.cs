using DBComparer.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace DBComparer.Service
{
    public class DataBaseComparision
    {

        public async void CompareDatabase(Database database, Database databaseb)
        {
            database.storedProcedures = await GetStoredProcedures(database.connection.connection);
            databaseb.storedProcedures = await GetStoredProcedures(databaseb.connection.connection);
        }


        public static async Task<List<KeyValue>> GetStoredProcedures(string connectionString)
        {
            SqlCommand sql = new SqlCommand();
            SqlConnection con = new SqlConnection();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            List<KeyValue> result = new List<KeyValue>();
            try
            {
                con.ConnectionString = connectionString;
                con.Open();
                sql.Connection = con;
                sql.CommandType = CommandType.Text;
                sql.CommandText = "SELECT ROUTINE_DEFINITION, SPECIFIC_NAME FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_TYPE = 'PROCEDURE'";
                da.SelectCommand = sql;

                da.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    result.Add(new KeyValue()
                    {
                        definition = dr["ROUTINE_DEFINITION"].ToString(),
                        name = dr["SPECIFIC_NAME"].ToString()
                    });
                }
                return await Task.FromResult(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                sql.Dispose();
                con.Dispose();
                dt.Dispose();
                da.Dispose();
            }
        }



        public static async Task<List<KeyValue>> GetModel(string connectionString)
        {
            SqlCommand sql = new SqlCommand();
            SqlConnection con = new SqlConnection();
            DataTable dt = new DataTable();
            DataTable dt2 = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            List<KeyValue> result = new List<KeyValue>();
            try
            {
                con.ConnectionString = connectionString;
                con.Open();
                sql.Connection = con;
                sql.CommandType = CommandType.Text;
                sql.CommandText = "SELECT ORDINAL_POSITION, COLUMN_NAME, DATA_TYPE, CHARACTER_MAXIMUM_LENGTH, TABLE_NAME, IS_NULLABLE FROM INFORMATION_SCHEMA.COLUMNS";
                da.SelectCommand = sql;

                da.Fill(dt);


                foreach (DataRow dr in dt.Rows)
                {
                    if (result.Exists(x => x.name == dr["TABLE_NAME"].ToString()))
                    {
                        result.Find(x => x.name == dr["TABLE_NAME"].ToString()).definition += "\n" + dr["COLUMN_NAME"].ToString() + " " + dr["DATA_TYPE"].ToString() + (dr["CHARACTER_MAXIMUM_LENGTH"] != null ? "(" + dr["CHARACTER_MAXIMUM_LENGTH"].ToString() + ")" : string.Empty) + " " + (dr["IS_NULLABLE"].ToString() == "YES" ? "NULL" : "NOT NULL");
                    }
                    else
                    {
                        result.Add(new KeyValue()
                        {
                            definition = dr["COLUMN_NAME"].ToString() + " " + dr["DATA_TYPE"].ToString() + (dr["CHARACTER_MAXIMUM_LENGTH"] != null ? "(" + dr["CHARACTER_MAXIMUM_LENGTH"].ToString() + ")" : string.Empty) + " " + (dr["IS_NULLABLE"].ToString() == "YES" ? "NULL" : "NOT NULL"),
                            name = dr["TABLE_NAME"].ToString()
                        });
                    }

                }


                sql.CommandType = CommandType.Text;
                sql.CommandText = "SELECT CONSTRAINT_NAME, TABLE_NAME FROM INFORMATION_SCHEMA.CONSTRAINT_TABLE_USAGE";
                da.SelectCommand = sql;

                da.Fill(dt2);


                foreach (DataRow dr in dt2.Rows)
                {
                    if (result.Exists(x => x.name == dr["TABLE_NAME"].ToString()))
                    {
                        result.Find(x => x.name == dr["TABLE_NAME"].ToString()).definition += "\n" + "CONSTRAINT " + dr["CONSTRAINT_NAME"].ToString();
                    }


                }
                return await Task.FromResult(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                sql.Dispose();
                con.Dispose();
                dt.Dispose();
                da.Dispose();
            }
        }
    }
}
