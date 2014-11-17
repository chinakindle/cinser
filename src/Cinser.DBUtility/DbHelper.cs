using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Cinser.DBUtility
{
    /// <summary>
    /// 数据库底层操作类
    /// 作者：wuxs
    /// 创建时间:2014-11-15
    /// </summary>
    public abstract class DbHelper
    {
        IDbTransaction _transaction;
        IDbCommand _cmd;
        IDbDataAdapter _dataAdapter;
        
        public abstract DbInstanceType DbInstanceType{get;}

        public abstract IDbConnection Conn{get;}

        protected IDbTransaction Transaction
        {
            get { return _transaction; }
            set { _transaction = value; }
        }

        protected bool IsInTransaction
        {
            get { return this.Transaction != null; }
        }

        protected IDbCommand Cmd
        {
            get { return _cmd; }
            set { _cmd = value; }
        }

        protected IDbDataAdapter DataAdapter
        {
            get { return _dataAdapter; }
            set { _dataAdapter = value; }
        }

        public void BeginTransaction()
        {
            if (Conn.State != ConnectionState.Open)
                Conn.Open();            
            Transaction = Conn.BeginTransaction();
        }

        public void CommitTransaction()
        {
            Transaction.Commit();
            Transaction = null;
            Conn.Close();
        }

        public void RollbackTransaction()
        {
            Transaction.Rollback();
            Transaction = null;
            Conn.Close();
        }

        protected void OpenConnection()
        {
            if (IsInTransaction == false)
            {
                if (Conn.State != ConnectionState.Open)
                    Conn.Open();
            }
        }

        protected void CloseConnection()
        {
            if (IsInTransaction == false)
            {
                if (Conn.State != ConnectionState.Closed)
                {
                    Conn.Close();
                }
            }
        }

        private void SetCmdParameters(DbParameter[] parameters)
        {
            if (parameters != null)
            {
                foreach (var item in parameters)
                {
                    IDbDataParameter param = Cmd.CreateParameter();
                    item.Converter(param);
                    Cmd.Parameters.Add(param);
                }
            }
        }

        public int ExecuteNonQuery(string sql, DbParameter[] parameters = null)
        {
            int returnValue = -1;
            OpenConnection();
            Cmd = Conn.CreateCommand();
            Cmd.CommandText = sql;
            SetCmdParameters(parameters);
            if (IsInTransaction)
                Cmd.Transaction = this.Transaction;
            returnValue = Cmd.ExecuteNonQuery();
            CloseConnection();
            return returnValue;
        }

        public IDataReader ExecuteReader(string sql, DbParameter[] parameters = null)
        {
            OpenConnection();
            Cmd = Conn.CreateCommand();
            if (IsInTransaction)
                Cmd.Transaction = Transaction;
            Cmd.CommandText = sql;
            SetCmdParameters(parameters);
            IDataReader returnValue = Cmd.ExecuteReader();
            //CloseConnection();
            return returnValue;
        }

        public object ExecuteScalar(string sql, DbParameter[] parameters = null)
        {
            OpenConnection();
            Cmd = Conn.CreateCommand();
            if (IsInTransaction)
                Cmd.Transaction = Transaction;
            Cmd.CommandText = sql;
            SetCmdParameters(parameters);
            if (IsInTransaction)
                Cmd.Transaction = this.Transaction;
            object returnValue = Cmd.ExecuteScalar();
            CloseConnection();
            return returnValue;
        }
    }
}
