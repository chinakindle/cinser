using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Cinser.DBUtility
{
    /// <summary>
    /// Cinser.DBUtility专用数据库参数类
    /// 作者：wuxs
    /// 创建时间:2014-11-15
    /// </summary>
    public class DbParameter:System.Data.IDbDataParameter
    {
        byte _Precision;
        byte _Scale;
        int _Size;
        System.Data.DbType _DbType;
        System.Data.ParameterDirection _Direction;
        bool _IsNullable;
        string _ParameterName;
        string _SourceColumn;
        System.Data.DataRowVersion _SourceVersion;
        object _Value;

        public DbParameter() { }

        public DbParameter(string _ParameterName, object _Value, System.Data.DbType _DbType = System.Data.DbType.String)
        {
            this.ParameterName = _ParameterName;
            this.Value = _Value;
            this.DbType = this._DbType;
        }

        public byte Precision
        {
            get { return _Precision; }
            set { _Precision = value; }
        }

        public byte Scale
        {
            get { return _Scale; }
            set { _Scale = value; }
        }

        public int Size
        {
            get { return _Size; }
            set { _Size = value; }
        }

        public System.Data.DbType DbType
        {
            get { return _DbType; }
            set { _DbType = value; }
        }

        public System.Data.ParameterDirection Direction
        {
            get { return _Direction; }
            set { _Direction = value; }
        }

        public bool IsNullable
        {
            get { return _IsNullable; }
            set { _IsNullable = value; }
        }

        public string ParameterName
        {
            get { return _ParameterName; }
            set { _ParameterName = value; }
        }

        public string SourceColumn
        {
            get { return _SourceColumn; }
            set { _SourceColumn = value; }
        }

        public System.Data.DataRowVersion SourceVersion
        {
            get { return _SourceVersion; }
            set { _SourceVersion = value; }
        }

        public object Value
        {
            get { return _Value; }
            set { _Value = value; }
        }

        /// <summary>
        /// 将DbParameter转换成传入的IDbDataParameter数据库参数
        /// </summary>
        /// <param name="param">接收参数</param>
        public void Converter(IDbDataParameter param)
        {
            param.DbType = this.DbType;
            param.Direction = this.Direction;
            param.ParameterName = this.ParameterName;
            param.Precision = this.Precision;
            param.Scale = this.Scale;
            param.Size = this.Size;
            param.SourceColumn = this.SourceColumn;
            //param.SourceVersion = this.SourceVersion;
            param.Value = this.Value;            
        }

    }
}
