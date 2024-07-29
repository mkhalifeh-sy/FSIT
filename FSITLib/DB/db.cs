using FSITLib.AUTH;
using FSITLib.OPERATION;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Numerics;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FSITLib.DB
{
    public class DBManager
    {
        #region Vars
        private bool dbConnectionStatus = false;
        private string db_conn_string =
            "Data Source=MKHALIFEH-LAPTO\\MSSQLSERVER2022;" +
            "Initial Catalog=FSITTask;" +
            "Integrated Security=True";

        private Dictionary<string, List<string>> db_table_fields;
        private List<string> db_Tables;
        #endregion

        #region Properties
        public bool IsDBConnectable
        {
            get
            {
                return dbConnectionStatus;
            }
        }
        #endregion

        #region Constructor
        public DBManager()
        {
            // Inite Connection for testing puposes
            try
            {
                using (var db_conn = new SqlConnection(db_conn_string))
                {

                    db_conn.Open();
                    dbConnectionStatus = true;
                    db_conn.Close();
                }
            }
            catch (Exception)
            {
                // To-Do : Log error of database connection is not valid
            }

            InitiateClassVars();
        }

        private void InitiateClassVars()
        {
            db_table_fields = new Dictionary<string, List<string>>()
            { 
                // SECURITY.AUTH.USERS
                {
                    "[USER_ID]",
                    new List<string>() {
                        "[USER_EMAIL]" ,
                        "[USER_PASSSWORD]" ,
                        "[USER_FIRST_NAME]" ,
                        "[USER_LAST_NAME]" ,
                        "[USER_MOBILE]"
                    }
                },
                // OPERATION.CATEGORY
                {
                    "[CATEGORY_ID]",
                    new List<string>() {
                    "[CATEGORY_NAME]",
                    "[CATEGORY_ISDELETED]"
                    }
                },
                // OPERATION.PRODUCT
                {
                    "[PRODUCT_ID]",
                    new List<string>() {
                    "[PRODUCT_NAME]",
                    "[PRODUCT_ISDELETED]"
                    }
                },
                // OPERATION.CATEGORY_PRODUCT
                {
                    "[CATEGORY_PRODUCT_ID]",
                    new List<string>() {
                    "[CATEGORY_ID]",
                    "[PRODUCT_ID]"
                    }
                },
            };
            db_Tables = new List<string>() {
                "SECURITY.AUTH.USERS",
                "OPERATION.CATEGORY",
                "OPERATION.PRODUCT",
                "OPERATION.CATEGORY_PRODUCT",
            };
        }
        #endregion

        #region DB Operations

        private List<object> GenerateIDField_ListOfValues(string TableName, object DataObj)
        {
            string ID_Field = string.Empty;
            long TablePK = 0;
            List<object> lst = new List<object>();
            switch (TableName)
            {
                case var value when value == db_Tables[0].ToString(): // "SECURITY.AUTH.USERS":
                    // Register New User
                    ID_Field = "[USER_ID]";
                    if (DataObj != null)
                    {
                        lst = ((USER)DataObj).GetFieldsDataList(DataListOptions.NoID);
                        TablePK = ((USER)DataObj).USER_ID;
                    }
                    break;
                case var value when value == db_Tables[1].ToString(): //"OPERATION.CATEGORY":
                    // Add New Category
                    ID_Field = "[CATEGORY_ID]";
                    if (DataObj != null)
                    {
                        lst = ((CATEGORY)DataObj).GetFieldsDataList(DataListOptions.NoID);
                        TablePK = ((CATEGORY)DataObj).CATEGORY_ID;
                    }
                    
                    break;
                case var value when value == db_Tables[2].ToString(): //"OPERATION.PRODUCT":
                    // Add New Product 
                    ID_Field = "[PRODUCT_ID]";
                    if (DataObj != null)
                    {
                        lst = ((PRODUCT)DataObj).GetFieldsDataList(DataListOptions.NoID);
                        TablePK = ((PRODUCT)DataObj).PRODUCT_ID;
                    }
                    
                    break;
                case var value when value == db_Tables[3].ToString(): //"OPERATION.CATEGORY_PRODUCT":
                    // Add Product - Category Relation M:M
                    ID_Field = "[CATEGORY_PRODUCT_ID]";
                    if (DataObj != null)
                    {
                        lst = ((CATEGORY_PRODUCT)DataObj).GetFieldsDataList(DataListOptions.NoID);
                        TablePK = ((CATEGORY_PRODUCT)DataObj).CATEGORY_PRODUCT_ID;
                    }
                   
                    break;
            }
            return new List<object>() { ID_Field, lst, TablePK };
        }
        private int ExcuteNonQuery(string sql)
        {
            int rowsAffected = -1;
            // Run on SQL Server Database
            using (var db_conn = new SqlConnection(db_conn_string))
            using (var cmd = new SqlCommand(sql, db_conn)
            {
                CommandType = CommandType.Text
            })
            {
                try
                {
                    db_conn.Open();
                    rowsAffected = cmd.ExecuteNonQuery();

                }
                catch (Exception)
                {
                    // To do: Exception Handling
                }
                finally
                {
                    db_conn.Close();
                }

            }
            return rowsAffected;
        }
        // Inserting a new Row
        public List<object> InsertNewRecord(string TableName, object DataObj)
        {
            List<object> InitData = GenerateIDField_ListOfValues(TableName, DataObj);
            string ID_Field = InitData[0].ToString();
            List<object> lst = (List<object>)InitData[1];

            // Setting up the sql string
            string sql = "INSERT INTO [" + TableName + "]";
            sql += "(";
            foreach (var item in db_table_fields[ID_Field])
            {
                sql += item + ",";
            }

            // Clean up sql Statement
            sql = sql.Substring(0, sql.Length - 1);

            // Adding Values to sql
            sql += ") VALUES(";
            switch (TableName)
            {
                case var value when value == db_Tables[0].ToString(): // "SECURITY.AUTH.USERS":
                    // Register New User
                    sql += "'"+lst[0] +"','"+ lst[1] + "','" + lst[2] + "','" + lst[3] + "','" + lst[4] + "'";
                    break;
                case var value when value == db_Tables[1].ToString(): //"OPERATION.CATEGORY":
                    // Add New Category
                    sql += "'" + lst[0] + "'," + lst[1] + " ";
                    break;
                case var value when value == db_Tables[2].ToString(): //"OPERATION.PRODUCT":
                    // Add New Product 
                    sql += "'" + lst[0] + "'," + lst[1] + " ";
                    break;
                case var value when value == db_Tables[3].ToString(): //"OPERATION.CATEGORY_PRODUCT":
                    // Add Product - Category Relation M:M
                    sql += " " + lst[0] + " , " + lst[1] + " ";
                    break;
            }

            // Finish up sql Statement
            sql += ");";

            // Run on SQL Server Database
            int rowsAffected = ExcuteNonQuery(sql);

            return new List<object>() { sql, rowsAffected };
        }
        // Updating a Record
        public List<object> UpdateRecordByID(string TableName, object DataObj)
        {
            List<object> InitData = GenerateIDField_ListOfValues(TableName, DataObj);
            string ID_Field = InitData[0].ToString();
            List<object> lst = (List<object>)InitData[1];
            long TablePK = long.Parse(InitData[2].ToString());

            // Setting up the sql string
            string sql = "UPDATE [" + TableName + "]";
            sql += " SET ";
            int iCounter = 0;
            switch (TableName)
            {
                case var value when value == db_Tables[0].ToString(): // "SECURITY.AUTH.USERS":
                    // Register New User
                    sql += db_table_fields[ID_Field][0] + " = '" + lst[0] + "'," +
                        db_table_fields[ID_Field][1] + " = '" + lst[1] + "'," +
                        db_table_fields[ID_Field][2] + " = '" + lst[2] + "'," +
                        db_table_fields[ID_Field][3] + " = '" + lst[3] + "'," +
                        db_table_fields[ID_Field][4] + " = '" + lst[4] + "' ";
                    break;
                case var value when value == db_Tables[1].ToString(): //"OPERATION.CATEGORY":
                    // Add New Category
                    sql += db_table_fields[ID_Field][0] + " = '" + lst[0] + "'," +
                        db_table_fields[ID_Field][1] + " = " + lst[1] + " ";
                    break;
                case var value when value == db_Tables[2].ToString(): //"OPERATION.PRODUCT":
                    // Add New Product 
                    sql += db_table_fields[ID_Field][0] + " = '" + lst[0] + "'," +
                        db_table_fields[ID_Field][1] + " = " + lst[1] + " ";
                    break;
                case var value when value == db_Tables[3].ToString(): //"OPERATION.CATEGORY_PRODUCT":
                    // Add Product - Category Relation M:M
                    sql += db_table_fields[ID_Field][0] + " = " + lst[0] + " ," +
                        db_table_fields[ID_Field][1] + " = " + lst[1] + " ";
                    break;
            }

            // Clean up sql Statement
            sql = sql.Substring(0, sql.Length - 1);

            // Adding Where Statement - The record PK (ID)
            sql += " WHERE " + ID_Field + " = " + TablePK + " ;";

            // Clean up sql Statement
            sql = sql.Substring(0, sql.Length - 1) + " ;";

            // Run on SQL Server Database
            //int rowsAffected = ExcuteNonQuery(sql);

            return new List<object>() { sql, 0 };
        }
        public List<object> DeleteRecordByID(string TableName, object DataObj)
        {
            List<object> InitData = GenerateIDField_ListOfValues(TableName, DataObj);
            string ID_Field = InitData[0].ToString();
            long TablePK = long.Parse(InitData[2].ToString());

            // Setting up the sql string
            string sql = "Delete [" + TableName + "]";

            // Adding Where Statement - The record PK (ID)
            sql += " WHERE " + ID_Field + " = " + TablePK + " ;";

            // Clean up sql Statement
            sql = sql.Substring(0, sql.Length - 1) + " ;";

            // Run on SQL Server Database
            //int rowsAffected = ExcuteNonQuery(sql);

            return new List<object>() { sql, 0 };
        }
        // Query Data from Tables
        public List<object> ExcuteReaderOrderByID(string TableName, int PageNo = 0, QueryPageSize PageSize = QueryPageSize.All)
        {
            List<object> InitData = GenerateIDField_ListOfValues(TableName, null);
            string ID_Field = InitData[0].ToString();

            string sql = "SELECT * FROM [" + TableName + "] ORDER BY " + ID_Field + " ";

            if (PageSize != QueryPageSize.All)
            {
                sql += "OFFSET " + PageNo * (int)PageSize + " ROWS FETCH NEXT " + (int)PageSize + " ROWS ONLY;";
            }

            List<object> data = null;
            // Run on SQL Server Database
            using (var db_conn = new SqlConnection(db_conn_string))
            using (var cmd = new SqlCommand(sql, db_conn)
            {
                CommandType = CommandType.Text
            })
            {
                try
                {
                    db_conn.Open();
                     data = LoadQuery(cmd.ExecuteReader(), TableName);

                }
                catch (Exception)
                {
                    // To do: Exception Handling
                }
                finally
                {
                    db_conn.Close();
                }

            }
            return data;
        }
        public List<object> ExcuteReaderWithWhere(string TableName, string whereClause)
        {
            List<object> InitData = GenerateIDField_ListOfValues(TableName, null);
            string ID_Field = InitData[0].ToString();

            string sql = "SELECT * FROM [" + TableName + "] Where " + whereClause + " ORDER BY " + ID_Field + " ;";


            List<object> data = null;
            // Run on SQL Server Database
            using (var db_conn = new SqlConnection(db_conn_string))
            using (var cmd = new SqlCommand(sql, db_conn)
            {
                CommandType = CommandType.Text
            })
            {
                try
                {
                    db_conn.Open();
                    data = LoadQuery(cmd.ExecuteReader(), TableName);

                }
                catch (Exception)
                {
                    // To do: Exception Handling
                }
                finally
                {
                    db_conn.Close();
                }

            }
            return data;
        }
        private List<object> LoadQuery(SqlDataReader SqlDataReader, string TableName)
        {
            switch (TableName)
            {
                case var value when value == db_Tables[0].ToString():
                    return new List<object>(LoadQueryUser(SqlDataReader).ToArray());
                case var value when value == db_Tables[1].ToString():
                    return new List<object>(LoadQueryCategory(SqlDataReader).ToArray());
                case var value when value == db_Tables[2].ToString():
                    return new List<object>(LoadQueryProduct(SqlDataReader).ToArray());
                case var value when value == db_Tables[3].ToString():
                    return new List<object>(LoadQueryCategoryProduct(SqlDataReader).ToArray());
            }
            return null;
        }
        private List<USER> LoadQueryUser(SqlDataReader reader)
        {
            if (reader == null)
                return null;
            List<USER> xTemp = new List<USER>();

            while (reader.Read())
            {
                USER xxTemp = new USER
                {
                    USER_ID = (long)((IDataRecord)reader)["USER_ID"],
                    USER_FIRST_NAME = (string)((IDataRecord)reader)["USER_FIRST_NAME"],
                    USER_LAST_NAME = (string)((IDataRecord)reader)["USER_LAST_NAME"],
                    USER_PASSSWORD = (string)((IDataRecord)reader)["USER_PASSSWORD"],
                    USER_EMAIL = (string)((IDataRecord)reader)["USER_EMAIL"],
                    USER_MOBILE = (string)((IDataRecord)reader)["USER_MOBILE"],
                };
                xTemp.Add(xxTemp);
            }

            return xTemp;
        }
        private List<CATEGORY> LoadQueryCategory(SqlDataReader reader)
        {
            if (reader == null)
                return null;
            List<CATEGORY> xTemp = new List<CATEGORY>();

            while (reader.Read())
            {
                CATEGORY xxTemp = new CATEGORY
                {
                    CATEGORY_ID = (long)((IDataRecord)reader)["CATEGORY_ID"],
                    CATEGORY_NAME = (string)((IDataRecord)reader)["CATEGORY_NAME"],
                    CATEGORY_ISDELETED = (long)((IDataRecord)reader)["CATEGORY_ISDELETED"],
                };
                xTemp.Add(xxTemp);
            }

            return xTemp;
        }
        private List<PRODUCT> LoadQueryProduct(SqlDataReader reader)
        {
            if (reader == null)
                return null;
            List<PRODUCT> xTemp = new List<PRODUCT>();

            while (reader.Read())
            {
                PRODUCT xxTemp = new PRODUCT
                {
                    PRODUCT_ID = (long)((IDataRecord)reader)["PRODUCT_ID"],
                    PRODUCT_NAME = (string)((IDataRecord)reader)["PRODUCT_NAME"],
                    PRODUCT_ISDELETED = (long)((IDataRecord)reader)["PRODUCT_ISDELETED"],
                };
                xTemp.Add(xxTemp);
            }

            return xTemp;
        }
        private List<CATEGORY_PRODUCT> LoadQueryCategoryProduct(SqlDataReader reader)
        {
            if (reader == null)
                return null;
            List<CATEGORY_PRODUCT> xTemp = new List<CATEGORY_PRODUCT>();

            while (reader.Read())
            {
                CATEGORY_PRODUCT xxTemp = new CATEGORY_PRODUCT
                {
                    PRODUCT_ID = (long)((IDataRecord)reader)["PRODUCT_ID"],
                    CATEGORY_ID = (long)((IDataRecord)reader)["CATEGORY_ID"],
                    CATEGORY_PRODUCT_ID = (long)((IDataRecord)reader)["CATEGORY_PRODUCT_ID"],
                };
                xTemp.Add(xxTemp);
            }

            return xTemp;
        }
        #endregion

        #region
        #endregion
    }
}
