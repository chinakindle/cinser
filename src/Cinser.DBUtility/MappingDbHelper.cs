using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Reflection;

namespace Cinser.DBUtility
{
    /// <summary>
    /// 基于对象直接操作的数据库模块
    /// 作者：wuxs
    /// 创建时间:2014-11-15
    /// </summary>
    public abstract class MappingDbHelper<T> : DbHelper
    {
        DbMapping _DbMapping;
        Type _objectType;

        protected Type ObjectType
        {
            get {
                if (_objectType == null)
                    _objectType = typeof(T);
                return _objectType;
            }
        }

        public DbMapping DbMapping
        {
            get
            {
                if (_DbMapping == null)
                {
                    this._DbMapping = CreateDbMapping();
                    this._DbMapping.Init(this.DbInstanceType);
                }
                return _DbMapping;
            }
        }

        protected abstract DbMapping CreateDbMapping();

        public virtual bool Insert(T model)
        {
            string sql = string.Format("Insert into {0} ({1}) values ({2})", this.DbMapping.TableName,
                this.DbMapping.CreateFieldsConnectionString(), this.DbMapping.CreateFieldsConnectionString(true));
            List<DbParameter> parameters = new List<DbParameter>();
            foreach (var item in this.DbMapping.Fields)
            {
                DbParameter param = new DbParameter();
                param.ParameterName = this.DbMapping.ParamHeaderStr + item.Value.Name;
                param.Value = item.Value.GetValue(model);
                param.DbType = item.Value.DbType;
                parameters.Add(param);
            }
            return base.ExecuteNonQuery(sql, parameters.ToArray()) > 0;
        }

        public virtual int Delete(string sqlWhere, DbParameter[] parameters = null)
        {
            if (string.IsNullOrEmpty(sqlWhere) == false)
            {
                string sql = string.Format("delete from {0} where {1}", this.DbMapping.TableName, sqlWhere);
                return base.ExecuteNonQuery(sql, parameters);
            }
            else
                return -1;
        }

        public virtual bool Update(T model)
        {
            List<DbMappingField> primaryKeys = this.DbMapping.GetPrimaryKeys();
            if (primaryKeys != null && primaryKeys.Count > 0)
            {
                string sqlWhere = string.Empty;
                foreach (var item in primaryKeys)
                {
                    sqlWhere += (string.IsNullOrEmpty(sqlWhere) ? "" : " and ") + string.Format("{0}={1}{0}", item.Name, this.DbMapping.ParamHeaderStr);
                }

                string setStr = string.Empty;
                List<DbParameter> parameters = new List<DbParameter>();
                foreach (var item in this.DbMapping.Fields)
                {
                    setStr += (string.IsNullOrEmpty(setStr) ? "" : ",") + string.Format("{0}={1}{0}", item.Value.Name, this.DbMapping.ParamHeaderStr);

                    DbParameter param = new DbParameter();
                    param.ParameterName = this.DbMapping.ParamHeaderStr + item.Value.Name;
                    param.Value = item.Value.GetValue(model);
                    param.DbType = item.Value.DbType;
                    parameters.Add(param);
                }

                string sql = string.Format("update {0} set {1} where {2}", this.DbMapping.TableName, setStr, sqlWhere);
                return base.ExecuteNonQuery(sql, parameters.ToArray()) > 0;
            }
            else
                throw new Exception("无法更新未定义主键的表数据。");
        }

        public virtual int Delete(T model)
        {
            string sqlWhere = string.Empty;
            List<DbParameter> parameters = new List<DbParameter> ();
            this.DbMapping.GetSqlWhere(model, ref sqlWhere, ref parameters);
            return Delete(sqlWhere, parameters.ToArray());
        }

        public virtual List<T> GetList(string sqlWhere = "1=1", DbParameter[] parameters = null)
        {
            List<T> returnvalue = new List<T>();
            string sql = string.Format("select * from {0} where {1}", this.DbMapping.TableName, sqlWhere);
            IDataReader reader = base.ExecuteReader(sql, parameters);
            while (reader.Read())
            {
                T pObject = (T)Activator.CreateInstance(ObjectType);
                foreach (var item in this.DbMapping.Fields)
                {
                    object pValue = reader[item.Value.Name];
                    item.Value.SetFieldValue(pObject, pValue);
                }
                returnvalue.Add(pObject);
            }
            reader.Close();
            CloseConnection();
            return returnvalue;
        }                
    }
}
