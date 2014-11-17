using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cinser.Demo.DAL
{
    public class AttachmentDal:DBUtility.MappingDbHelper<Domains.Attachment>
    {
        protected override DBUtility.DbMapping CreateDbMapping()
        {
            DBUtility.DbMapping map = new DBUtility.DbMapping(typeof(Domains.Attachment));
            map.TableName = "attachment";
            map.AutoLoadFieldsFromModelType();
            map.SetPrimaryKeys("_ID");
            return map;
        }

        public override DBUtility.DbInstanceType DbInstanceType
        {
            get { return  DBUtility.DbInstanceType.Oracle; }
        }

        public override System.Data.IDbConnection Conn
        {
            get { return ConnectionSettings.OracleIDbConnection; }
        }
    }
}
