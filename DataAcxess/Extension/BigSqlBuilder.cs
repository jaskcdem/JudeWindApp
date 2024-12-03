using GreenUtility.Exception;
using GreenUtility.Extension;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Data.Common;
using System.Text;

namespace DataAcxess.Extension
{
    /// <summary> Sql自動產生工具 </summary>
    public class BigSqlBuilder(string tbName)
    {
        string Table { get; set; } = tbName;

        List<(string columnName, DbParameter parameter)> ColumnList = [];
        List<(string columnName, DbParameter parameter)> WhrColumnList = [];

        enum Sqltype { select, insert, update, delete }

        #region AddParameter
        /// <summary>  </summary>
        public BigSqlBuilder AddParameter(string colName, DbParameter parameter)
        {
            if (ParaNameExist(parameter.ParameterName)) throw new SqlBuildingException($"ParameterName [{parameter.ParameterName}] is already Exist, please check !");
            ColumnList.Add((colName, parameter));
            return this;
        }
        /// <summary>  </summary>
        public BigSqlBuilder AddWhrParameter(string colName, DbParameter parameter)
        {
            if (ParaNameExist(parameter.ParameterName)) throw new SqlBuildingException($"ParameterName [{parameter.ParameterName}] is already Exist, please check !");
            WhrColumnList.Add((colName, parameter));
            return this;
        }
        /// <summary>  </summary>
        /// <remarks> columnName is same to parameterName </remarks>
        public BigSqlBuilder AddSqlParameter(string colName, SqlDbType type, object data)
        {
            string paraName = colName.ColumnNameToSqlParameter();
            if (ParaNameExist(paraName)) throw new SqlBuildingException($"ParameterName [{paraName}] is already Exist, please check !");
            ColumnList.Add((colName, new SqlParameter(paraName, data) { SqlDbType = type }));
            return this;
        }
        /// <summary>  </summary>
        /// <remarks> columnName is same to parameterName </remarks>
        public BigSqlBuilder AddSqlParameter(string colName, SqlDbType type, int size, object data)
        {
            string paraName = colName.ColumnNameToSqlParameter();
            if (ParaNameExist(paraName)) throw new SqlBuildingException($"ParameterName [{paraName}] is already Exist, please check !");
            ColumnList.Add((colName, new SqlParameter(paraName, type, size) { Value = data }));
            return this;
        }
        /// <summary>  </summary>
        /// <remarks> columnName isn't same to parameterName </remarks>
        public BigSqlBuilder AddSqlParameter(string colName, string paraName, SqlDbType type, object data)
        {
            if (ParaNameExist(paraName)) throw new SqlBuildingException($"ParameterName [{paraName}] is already Exist, please check !");
            ColumnList.Add((colName, new SqlParameter(paraName, data) { SqlDbType = type }));
            return this;
        }
        /// <summary>  </summary>
        /// <remarks> columnName isn't same to parameterName </remarks>
        public BigSqlBuilder AddSqlParameter(string colName, string paraName, SqlDbType type, int size, object data)
        {
            if (ParaNameExist(paraName)) throw new SqlBuildingException($"ParameterName [{paraName}] is already Exist, please check !");
            ColumnList.Add((colName, new SqlParameter(paraName, type, size) { Value = data }));
            return this;
        }
        /// <summary>  </summary>
        /// <remarks> columnName is same to parameterName </remarks>
        public BigSqlBuilder AddWhrSqlParameter(string colName, SqlDbType type, object data)
        {
            string paraName = colName.ColumnNameToSqlParameter();
            if (ParaNameExist(paraName)) throw new SqlBuildingException($"ParameterName [{paraName}] is already Exist, please check !");
            WhrColumnList.Add((colName, new SqlParameter(paraName, data) { SqlDbType = type }));
            return this;
        }
        /// <summary>  </summary>
        /// <remarks> columnName is same to parameterName </remarks>
        public BigSqlBuilder AddWhrSqlParameter(string colName, SqlDbType type, int size, object data)
        {
            string paraName = colName.ColumnNameToSqlParameter();
            if (ParaNameExist(paraName)) throw new SqlBuildingException($"ParameterName [{paraName}] is already Exist, please check !");
            WhrColumnList.Add((colName, new SqlParameter(paraName, type, size) { Value = data }));
            return this;
        }
        /// <summary>  </summary>
        /// <remarks> columnName isn't same to parameterName </remarks>
        public BigSqlBuilder AddWhrSqlParameter(string colName, string paraName, SqlDbType type, object data)
        {
            if (ParaNameExist(paraName)) throw new SqlBuildingException($"ParameterName [{paraName}] is already Exist, please check !");
            WhrColumnList.Add((colName, new SqlParameter(paraName, data) { SqlDbType = type }));
            return this;
        }
        /// <summary>  </summary>
        /// <remarks> columnName isn't same to parameterName </remarks>
        public BigSqlBuilder AddWhrSqlParameter(string colName, string paraName, SqlDbType type, int size, object data)
        {
            if (ParaNameExist(paraName)) throw new SqlBuildingException($"ParameterName [{paraName}] is already Exist, please check !");
            WhrColumnList.Add((colName, new SqlParameter(paraName, type, size) { Value = data }));
            return this;
        }
        bool ParaNameExist(string pn) => ColumnList.Any(c => c.parameter.ParameterName == pn) || WhrColumnList.Any(c => c.parameter.ParameterName == pn);
        #endregion

        #region << CreateSql >>
        /// <summary> 自動產生Sql </summary>
        string CreateSql(Sqltype st)
        {
            StringBuilder sql = new();
            switch (st)
            {
                case Sqltype.select:
                    if (WhrColumnList.Count <= 0) throw new SqlBuildingException("select SQL must have filter condition !");
                    sql.Append($"select * from {Table} where ");
                    for (int i = 0; i < WhrColumnList.Count; i++)
                        sql.Append($"{WhrColumnList[i].columnName} = {WhrColumnList[i].parameter.ParameterName}{(i != WhrColumnList.Count - 1 ? ", " : "")}");
                    break;
                case Sqltype.insert:
                    if (ColumnList.Count <= 0) throw new SqlBuildingException("insert SQL must give table column Info !");
                    StringBuilder _inest = new($"insert into {Table} ("), _value = new($"values (");
                    for (int i = 0; i < ColumnList.Count; i++)
                    {
                        _inest.Append($"{ColumnList[i].columnName}{(i != ColumnList.Count - 1 ? ", " : "")}");
                        _value.Append($"{ColumnList[i].parameter.ParameterName}{(i != ColumnList.Count - 1 ? ", " : "")}");
                    }
                    sql.Append($"{_inest})");
                    sql.AppendLine();
                    sql.Append($"{_value})");
                    break;
                case Sqltype.update:
                    if (WhrColumnList.Count <= 0) throw new SqlBuildingException("update SQL must have filter condition !");
                    if (ColumnList.Count <= 0) throw new SqlBuildingException("update SQL must give table column Info !");
                    StringBuilder _update = new($"update {Table} set "), _whr = new("where ");
                    for (int i = 0; i < ColumnList.Count; i++)
                        _update.Append($"{ColumnList[i].columnName} = {ColumnList[i].parameter.ParameterName}{(i != ColumnList.Count - 1 ? ", " : "")}");
                    for (int i = 0; i < WhrColumnList.Count; i++)
                        sql.Append($"{WhrColumnList[i].columnName} = {WhrColumnList[i].parameter.ParameterName}{(i != WhrColumnList.Count - 1 ? ", " : "")}");
                    sql.Append(_update);
                    sql.AppendLine();
                    sql.Append(_whr);
                    break;
                case Sqltype.delete:
                    if (WhrColumnList.Count <= 0) throw new SqlBuildingException("delete SQL must have filter condition !");
                    sql.Append($"delete from {Table} where ");
                    for (int i = 0; i < WhrColumnList.Count; i++)
                        sql.Append($"{WhrColumnList[i].columnName} = {WhrColumnList[i].parameter.ParameterName}{(i != WhrColumnList.Count - 1 ? ", " : "")}");
                    break;
            }
            return sql.ToString();
        }
        #endregion

        /// <summary>  </summary>
        public string GetSelect() => CreateSql(Sqltype.select);
        /// <summary>  </summary>
        public string GetInsert() => CreateSql(Sqltype.insert);
        /// <summary>  </summary>
        public string GetUpdate() => CreateSql(Sqltype.update);
        /// <summary>  </summary>
        public string GetDelete() => CreateSql(Sqltype.delete);

        /// <summary>  </summary>
        public DbParameter[] GetParas()
        {
            var ret = ColumnList.Select(x => x.parameter).ToList();
            ret.AddRange(WhrColumnList.Select(x => x.parameter));
            return [.. ret];
        }
    }
}
