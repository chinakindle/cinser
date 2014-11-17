using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Reflection;

namespace Cinser.DBUtility
{
    /// <summary>
    /// 数据库对应关系字段类
    /// 作者：wuxs
    /// 创建时间:2014-11-15
    /// </summary>
    public class DbMappingField
    {
        string dbFieldHeaderStr;
        Type ownerType;
        string _name;
        string _caption;
        bool isPrimaryKey = false;
        string _mappingFieldName;
        DbType _fieldType;
        FieldInfo _fieldInfo;


        public DbMappingField(string _mappingFieldName)
        {
            this.MappingFieldName = _mappingFieldName;
        }

        /// <summary>
        /// 代表数据库字段的model字段前缀规则，如model中有_id,isVisible两个字段。
        /// 如果DbFieldHeaderStr='_'则表示_id为数据库字段，isVisible为用户自定义字段
        /// </summary>
        internal string DbFieldHeaderStr
        {
            get { return dbFieldHeaderStr; }
            set { dbFieldHeaderStr = value; }
        }

        /// <summary>
        /// 所属类的Type值
        /// </summary>
        public Type OwnerType
        {
            get { return ownerType; }
            set { ownerType = value; }
        }
        
        /// <summary>
        /// 数据库中的字段名
        /// </summary>
        public string Name
        {
            get
            {
                if (string.IsNullOrEmpty(_name))
                {
                    _name = MappingFieldName.StartsWith(this.DbFieldHeaderStr) ? MappingFieldName.Substring(1) : MappingFieldName;
                }
                return _name;
            }
            set { _name = value; }
        }

        /// <summary>
        /// 字段标题
        /// </summary>
        public string Caption
        {
            get
            {
                if (string.IsNullOrEmpty(_caption))
                    _caption = MappingFieldName;
                return _caption;
            }
            set { _caption = value; }
        }

        /// <summary>
        /// 是否为主键
        /// </summary>
        public bool IsPrimaryKey
        {
            get { return isPrimaryKey; }
            set { isPrimaryKey = value; }
        }

        /// <summary>
        /// 对应model中的字段名
        /// </summary>
        public string MappingFieldName
        {
            get
            {
                return _mappingFieldName;
            }
            set { _mappingFieldName = value; }
        }

        /// <summary>
        /// 字段信息
        /// </summary>
        public FieldInfo FieldInfo
        {
            get { return _fieldInfo; }
            internal set { _fieldInfo = value; }
        }

        /// <summary>
        /// 字段类型
        /// </summary>
        public DbType DbType
        {
            get { return _fieldType; }
        }

        /// <summary>
        /// 根据model字段类型设置数据库字段类型
        /// </summary>
        /// <param name="mappingFieldType"></param>
        /// <param name="dbInstanceType"></param>
        internal void SetFieldType(Type mappingFieldType, DbInstanceType dbInstanceType)
        {
            if (mappingFieldType == typeof(Int32))
                _fieldType = DbType.Int32;
            else if (mappingFieldType == typeof(String))
                _fieldType = DbType.String;
            else if (mappingFieldType == typeof(DateTime))
            {
                if (dbInstanceType == DbInstanceType.Access)
                    _fieldType = DbType.AnsiString;
                else
                    _fieldType = DbType.DateTime;
            }
            else if (mappingFieldType == typeof(Double))
                _fieldType = DbType.Double;
            else if (mappingFieldType == typeof(Decimal))
                _fieldType = DbType.Decimal;
            else if (mappingFieldType == typeof(float))
                _fieldType = DbType.Single;
            else if (mappingFieldType == typeof(long))
                _fieldType = DbType.Int64;
            else if (mappingFieldType == typeof(Boolean))
                _fieldType = DbType.Boolean;
        }

        /// <summary>
        /// 取model中该字段的值
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public object GetValue(object model)
        {
            return FieldInfo.GetValue(model); 
        }

        /// <summary>
        /// 设置pObject当前字段的值为pValue
        /// </summary>
        public void SetFieldValue(object pObject, object pValue)
        {
            if (!(pValue is DBNull))
            {
                if (pValue is decimal)
                {
                    if (FieldInfo.FieldType == typeof(double))
                    {
                        FieldInfo.SetValue(pObject, decimal.ToDouble((decimal)pValue));
                    }
                    else if (FieldInfo.FieldType == typeof(int))
                    {
                        FieldInfo.SetValue(pObject, decimal.ToInt32((decimal)pValue));
                    }
                    else if (FieldInfo.FieldType == typeof(float))
                    {
                        FieldInfo.SetValue(pObject, decimal.ToSingle((decimal)pValue));
                    }
                }
                else if (pValue is long)
                {
                    if (FieldInfo.FieldType == typeof(int))
                    {
                        FieldInfo.SetValue(pObject, Convert.ToInt32((long)pValue));
                    }
                }
                else
                {
                    FieldInfo.SetValue(pObject, pValue);
                }
            }
        }

    }
}
