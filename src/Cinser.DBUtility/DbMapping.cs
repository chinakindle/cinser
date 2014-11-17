using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Cinser.DBUtility
{
    /// <summary>
    /// 数据库对应关系类
    /// 作者：wuxs
    /// 创建时间:2014-11-15
    /// </summary>
    public class DbMapping
    {
        internal DbInstanceType dbInstanceType;
        string _ParamHeaderStr;
        string _DbFieldHeaderStr;
        Type _modelType;
        string _tableName;
        Dictionary<string, DbMappingField> _fields;
        FieldInfo[] _fieldInfos;

        /// <summary>
        /// 实例化DbMapping，dbFieldHeaderStr表示数据库字段在model中的命名规则，如model有_ID字段，则表示数据库中的ID。
        /// </summary>
        /// <param name="dbFieldHeaderStr"> 
        /// 代表数据库字段的model字段前缀规则，如model中有_id,isVisible两个字段。
        /// 如果dbFieldHeaderStr='_'则表示_id为数据库字段，isVisible为用户自定义字段
        /// </param>
        /// <param name="paramHeaderStr">表示参数前缀，如@ID,:ID</param>
        public DbMapping(Type modelType,string dbFieldHeaderStr = "_", string paramHeaderStr = ":")
        {
            this.ModelType = modelType;
            this.DbFieldHeaderStr = dbFieldHeaderStr;
            this.ParamHeaderStr = paramHeaderStr;
        }

        public string ParamHeaderStr
        {
            get { return _ParamHeaderStr; }
            set { _ParamHeaderStr = value; }
        }

        public string DbFieldHeaderStr
        {
            get { return _DbFieldHeaderStr; }
            set { _DbFieldHeaderStr = value; }
        }

        public Type ModelType
        {
            get { return _modelType; }
            set { _modelType = value; }
        }                

        public string TableName
        {
            get { return _tableName; }
            set { _tableName = value; }
        }

        internal Dictionary<string, DbMappingField> Fields
        {
            get
            {
                if (_fields == null)
                    _fields = new Dictionary<string, DbMappingField>();
                return _fields;
            }
        }

        private FieldInfo[] FieldInfos
        {
            get
            {
                if (_fieldInfos == null)
                {
                    BindingFlags bindingAttr = BindingFlags.NonPublic | BindingFlags.Instance;
                    _fieldInfos = ModelType.GetFields(bindingAttr);
                }
                return _fieldInfos;
            }
        }

        /// <summary>
        /// 通过model反射自动创建对应字段集合
        /// </summary>
        public void AutoLoadFieldsFromModelType()
        {
            this.Fields.Clear();
            foreach (var item in this.FieldInfos)
            {
                if (item.Name.StartsWith(this.DbFieldHeaderStr))
                    this.AddField(new DbMappingField(item.Name));
            }
        }

        public void AddField(DbMappingField field)
        {
            field.OwnerType = ModelType;
            field.DbFieldHeaderStr = this.DbFieldHeaderStr;
            if (this.Fields.ContainsKey(field.MappingFieldName) == false)
                this.Fields.Add(field.MappingFieldName, field);
        }
        
        /// <summary>
        /// 通过model字段名或者数据库字段名检索字段
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public DbMappingField FindField(string value)
        {
            foreach (var item in Fields)
            {
                if (item.Value.Name == value || item.Value.MappingFieldName == value)
                    return item.Value;
            }
            return null;
        }

        public void SetPrimaryKeys(string keyFieldName)
        {
            DbMappingField field = FindField(keyFieldName);
            if (field != null)
                field.IsPrimaryKey = true;
        }

        public void SetPrimaryKeys(string keyFieldName1, string keyFieldName2)
        {
            SetPrimaryKeys(keyFieldName1);
            SetPrimaryKeys(keyFieldName2);
        }

        public void SetPrimaryKeys(string keyFieldName1, string keyFieldName2, string keyFieldName3)
        {
            SetPrimaryKeys(keyFieldName1);
            SetPrimaryKeys(keyFieldName2);
            SetPrimaryKeys(keyFieldName3);
        }

        public void SetPrimaryKeys(params string[] keyFieldNames)
        {
            foreach (var item in keyFieldNames)
            {
                SetPrimaryKeys(item);
            }
        }

        private FieldInfo FindFieldInfo(string fieldName)
        {
            foreach (var item in FieldInfos)
            {
                if (item.Name == fieldName )
                    return item;
            }
            return null;
        }
        
        /// <summary>
        /// 初始化所有字段的类型和FieldInfo信息
        /// </summary>
        /// <param name="dbInstanceType"></param>
        internal void Init(DbInstanceType dbInstanceType)
        {
            this.dbInstanceType = dbInstanceType;

            foreach (FieldInfo fieldInfo in FieldInfos)
            {
                DbMappingField item = FindField(fieldInfo.Name);
                if (item != null)
                {
                    item.SetFieldType(fieldInfo.FieldType, dbInstanceType);
                    item.FieldInfo = fieldInfo;
                }
            }
        }

        /// <summary>
        /// 创建字段连接字符串
        /// </summary>
        /// <param name="isParamStr">是否取带参数的字段字符串</param>
        /// <returns>返回如：id,name,age或者@id,@name,@age</returns>
        public string CreateFieldsConnectionString(bool isParamStr = false)
        {
            string returnValue = string.Empty;
            if (Fields != null && Fields.Count > 0)
            {
                foreach (var item in Fields)
                {
                    if (returnValue == string.Empty)
                        returnValue = isParamStr ? ParamHeaderStr + item.Value.Name : item.Value.Name;
                    else
                        returnValue += "," + (isParamStr ? ParamHeaderStr + item.Value.Name : item.Value.Name);
                }
            }
            return returnValue;
        }

        public void GetSqlWhere(object model, ref string sqlWhere, ref List<DbParameter> parameters)
        {
            List<DbMappingField> primaryKeys = GetPrimaryKeys();
            if (primaryKeys.Count > 0)
            {
                foreach (var item in primaryKeys)
                {
                    sqlWhere += (string.IsNullOrEmpty(sqlWhere) ? "" : " and ") + string.Format("{0}={1}{0}", item.Name, this.ParamHeaderStr);
                    DbParameter param = new DbParameter();
                    param.DbType = item.DbType;
                    param.ParameterName = this.ParamHeaderStr + item.Name;
                    param.Value = item.GetValue(model);
                    parameters.Add(param);
                }             
            }
            else
            {
                foreach (var item in Fields)
                {                    
                    sqlWhere += (string.IsNullOrEmpty(sqlWhere) ? "" : " and ") + string.Format("{0}={1}{0}", item.Value.Name, this.ParamHeaderStr);
                    DbParameter param = new DbParameter();
                    param.DbType = item.Value.DbType;
                    param.ParameterName = this.ParamHeaderStr + item.Value.Name;
                    param.Value = item.Value.GetValue(model);
                    parameters.Add(param);
                }
            }
        }

        public string GetSqlWhere(object model, string sqlWhere)
        {
            List<DbMappingField> primaryKeys = GetPrimaryKeys();
            if (primaryKeys.Count > 0)
            {
                foreach (var item in primaryKeys)
                {
                    sqlWhere += (string.IsNullOrEmpty(sqlWhere) ? "" : " and ") + string.Format("{0}='{1}'", item.Name, item.GetValue(model));
                }
            }
            else
            {
                foreach (var item in Fields)
                {
                    sqlWhere += (string.IsNullOrEmpty(sqlWhere) ? "" : " and ") + string.Format("{0}='{1}'", item.Value.Name, item.Value.GetValue(model));
                }
            }
            return sqlWhere;
        }

        public List<DbMappingField> GetPrimaryKeys()
        {
            List<DbMappingField> primaryKeys = new List<DbMappingField>();
            foreach (var item in this.Fields)
            {
                if (item.Value.IsPrimaryKey)
                {
                    primaryKeys.Add(item.Value);
                }
            }
            return primaryKeys;
        }
    }
}
