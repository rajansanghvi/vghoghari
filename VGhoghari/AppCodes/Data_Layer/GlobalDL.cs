using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using VGhoghari.AppCodes.Utilities;

namespace VGhoghari.AppCodes.Data_Layer {
  public class GlobalDL {
    private List<KeyValuePair<string, object>> paramCollection;
    private MySqlConnection sqlConnection;

    public GlobalDL() {
      paramCollection = new List<KeyValuePair<string, object>>();
    }

    public void AddDateParam(string paramName, DateTime? paramValue) {
      DateTime value = (paramValue == null) ? DateTime.MinValue : (DateTime)paramValue;
      paramCollection.Add(new KeyValuePair<string, object>(paramName, Utility.GetDateForSqlParam(value)));
    }

    public void AddDateParamWithNull(string paramName, DateTime? paramValue) {
      if (paramValue == null) {
        paramCollection.Add(new KeyValuePair<string, object>(paramName, DBNull.Value));
      }
      else {
        paramCollection.Add(new KeyValuePair<string, object>(paramName, Utility.GetDateForSqlParam((DateTime)paramValue)));
      }
    }

    public void AddIntParam(string paramName, object paramValue) {
      if (paramValue == null || String.IsNullOrWhiteSpace(paramValue.ToString())) {
        paramValue = DBNull.Value;
      }
      AddParam(paramName, paramValue);
    }

    public void AddLikeParam(string paramName, string paramValue) {
      paramCollection.Add(new KeyValuePair<string, object>(paramName, Utility.AddWildCard(paramValue)));
    }

    public void AddParam(string paramName, object paramValue) {
      paramCollection.Add(new KeyValuePair<string, object>(paramName, paramValue));
    }

    public void ClearParams() {
      paramCollection.Clear();
    }

    public void ExecuteProcedureNonQuery(string connectionString, string procedureName) {
      try {
        using (MySqlConnection connection = new MySqlConnection(connectionString)) {
          using (MySqlCommand command = connection.CreateCommand()) {
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = procedureName;

            foreach (KeyValuePair<string, object> item in paramCollection) {
              command.Parameters.AddWithValue(item.Key, item.Value);
            }

            connection.Open();
            command.ExecuteNonQuery();
          }
        }
      }
      catch (Exception) {
        throw;
      }
      finally {
        ClearParams();
      }
    }

    public void ExecuteProcedureNonQueryAsync(string connectionString, string procedureName) {
      try {
        sqlConnection = new MySqlConnection(connectionString);

        MySqlCommand command = sqlConnection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = procedureName;

        foreach (KeyValuePair<string, object> item in paramCollection) {
          command.Parameters.AddWithValue(item.Key, item.Value);
        }

        sqlConnection.Open();
        command.BeginExecuteNonQuery(new AsyncCallback(HandleCallback), command);
      }
      catch (Exception) {
        throw;
      }
      finally {
        ClearParams();
      }
    }

    public MySqlDataReader ExecuteProcedureReturnReader(string connectionString, string procedureName) {
      MySqlConnection connection = new MySqlConnection(connectionString);

      try {
        using (MySqlCommand command = connection.CreateCommand()) {
          command.CommandType = CommandType.StoredProcedure;
          command.CommandText = procedureName;

          foreach (KeyValuePair<string, object> item in this.paramCollection) {
            command.Parameters.AddWithValue(item.Key, item.Value);
          }

          connection.Open();
          return command.ExecuteReader(CommandBehavior.CloseConnection);
        }
      }
      catch (Exception) {
        connection.Close();
        throw;
      }
      finally {
        ClearParams();
      }
    }

    public object ExecuteProcedureReturnScalar(string connectionString, string procedureName) {
      try {
        using (MySqlConnection connection = new MySqlConnection(connectionString)) {
          using (MySqlCommand command = connection.CreateCommand()) {
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = procedureName;

            foreach (KeyValuePair<string, object> item in paramCollection) {
              command.Parameters.AddWithValue(item.Key, item.Value);
            }

            connection.Open();
            return command.ExecuteScalar();
          }
        }
      }
      catch (Exception) {
        throw;
      }
      finally {
        ClearParams();
      }
    }

    public T ExecuteProcedureReturnScalar<T>(string connectionString, string procedureName) {
      object val = this.ExecuteProcedureReturnScalar(connectionString, procedureName);

      if (val == null || val == DBNull.Value) {
        return default(T);
      }
      else {
        return (T)Convert.ChangeType(val, typeof(T));
      }
    }

    public int ExecuteSqlNonQuery(string sql) {
      return this.ExecuteSqlNonQuery(Utility.ConnectionString, sql);
    }

    public int ExecuteSqlNonQuery(string connectionString, string sql) {
      int recordsAffected = 0;

      try {
        using (MySqlConnection connection = new MySqlConnection(connectionString)) {
          using (MySqlCommand command = connection.CreateCommand()) {
            command.CommandType = CommandType.Text;
            command.CommandText = sql;

            connection.Open();

            foreach (KeyValuePair<string, object> item in paramCollection) {
              command.Parameters.AddWithValue(item.Key, item.Value);
            }

            recordsAffected = command.ExecuteNonQuery();
          }
        }
      }
      catch (Exception) {
        throw;
      }
      finally {
        ClearParams();
      }
      return recordsAffected;
    }

    public MySqlDataReader ExecuteSqlReturnReader(string connectionString, string sql) {
      MySqlConnection connection = new MySqlConnection(connectionString);

      try {
        using (MySqlCommand command = connection.CreateCommand()) {
          command.CommandType = CommandType.Text;
          command.CommandText = sql;

          foreach (KeyValuePair<string, object> item in paramCollection) {
            command.Parameters.AddWithValue(item.Key, item.Value);
          }

          connection.Open();
          return command.ExecuteReader(CommandBehavior.CloseConnection);
        }
      }
      catch (Exception) {
        connection.Close();
        throw;
      }
      finally {
        ClearParams();
      }
    }

    public object ExecuteSqlReturnScalar(string connectionString, string sql) {
      try {
        using (MySqlConnection connection = new MySqlConnection(connectionString)) {
          using (MySqlCommand command = connection.CreateCommand()) {
            command.CommandType = CommandType.Text;
            command.CommandText = sql;
            connection.Open();

            foreach (KeyValuePair<string, object> item in paramCollection) {
              command.Parameters.AddWithValue(item.Key, item.Value);
            }

            return command.ExecuteScalar();
          }
        }
      }
      catch (Exception) {
        throw;
      }
      finally {
        ClearParams();
      }
    }

    public T ExecuteSqlReturnScalar<T>(string connectionString, string sql) {
      object val = this.ExecuteSqlReturnScalar(connectionString, sql);

      if (val == null || val == DBNull.Value) {
        return default(T);
      }
      else {
        return (T)Convert.ChangeType(val, typeof(T));
      }
    }

    public DateTime GetDbTime(string connStr) {
      const string sql = @"select current_timestamp();";

      DateTime returnValue = DateTime.Parse(this.ExecuteSqlReturnScalar(connStr, sql).ToString());
      return returnValue;
    }

    public int GetIntWithDefault(object value) {
      int returnValue;

      if (Int32.TryParse(value.ToString(), out returnValue)) {
        return returnValue;
      }
      else {
        return 0;
      }
    }

    public void GetResults(string sql, List<FilterTO> filters, List<KeyValuePair<string, string>> orderByCols, int pageSize, int pageNumber) {
      StringBuilder sb = new StringBuilder();
      int counter = 0;

      sb.AppendLine(sql);

      if (filters.Count > 0) {
        foreach (FilterTO f in filters) {
          if (counter == 0) {
            sb.AppendLine(" where ");
          }
          else {
            sb.AppendLine(" and ");
          }
          sb.AppendLine(f.ColumnName);
          sb.AppendLine(" like ");
          sb.AppendLine(f.ColumnValue);
          counter++;
        }
      }

      if (orderByCols.Count > 0) {
        sb.AppendLine("order by ");
        foreach (KeyValuePair<string, string> k in orderByCols) {
          sb.AppendLine(k.Key);
          sb.AppendLine(" ");
          sb.AppendLine(k.Value);
        }
      }

      if (pageSize != -1) {
        const string LIMIT = "limit {0}, {1}";
        int startCount = pageSize * pageNumber;
        sb.AppendLine(string.Format(LIMIT, startCount, pageSize));
      }

      sb.AppendLine(";");
    }

    private string GetSqlWithParams(string sql) {
      StringBuilder sb = new StringBuilder();

      sb.AppendLine(sql);

      foreach (KeyValuePair<string, object> item in paramCollection) {
        sb.Append(item.Key).Append(':').AppendLine((item.Value == null) ? "null" : item.Value.ToString());
      }
      return sb.ToString();
    }

    private void HandleCallback(IAsyncResult result) {
      try {
        // Retrieve the original command object, passed
        // to this procedure in the AsyncState property
        // of the IAsyncResult parameter.
        MySqlCommand command = (MySqlCommand)result.AsyncState;
        try {
          command.EndExecuteNonQuery(result);
        }
        catch { }
      }
      catch (Exception) {
      }
      finally {
        if (sqlConnection != null) {
          sqlConnection.Close();
        }
      }
    }
  }
}