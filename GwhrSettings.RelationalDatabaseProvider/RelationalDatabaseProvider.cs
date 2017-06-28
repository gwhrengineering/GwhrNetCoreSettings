using System;
using System.Collections.Generic;
using System.Text;
using GwhrSettings.Core;

namespace GwhrSettings.Providers
{
    public class RelationalDatabaseProvider<T> : GwhrSettingsBase<T> where T : RelationalDatabaseProvider<T>
    {
        //Internal fields
        private string _strCnxnStr = string.Empty;
        private DatabaseProvider _enmDbProvider;

        #region Private properties

        private string ConnectionString
        {
            get
            {
                if (string.IsNullOrEmpty(_strCnxnStr))
                {
                    throw new Exception("Connection string missing");
                }
                return _strCnxnStr;
            }
        }

        #endregion

        #region Connectrion string methods

        public T UseMicrosoftSqlServer(string strCnxnStr)
        {
            this._strCnxnStr = strCnxnStr;
            this._enmDbProvider = DatabaseProvider.MicrosoftSqlServer;
            return (T)this;
        }

        public T UseMySqlServer(string strCnxnStr)
        {
			this._strCnxnStr = strCnxnStr;
            this._enmDbProvider = DatabaseProvider.MySql;
            return (T)this;
        }

        public T UseSQLite(string strCnxnStr)
        {
			this._strCnxnStr = strCnxnStr;
			this._enmDbProvider = DatabaseProvider.SQLite;
			return (T)this;
        }

        #endregion

        //
        public override T Build(string strFileName)
        {
            throw new NotSupportedException();
			//return (T)this;
        }
    }


}
