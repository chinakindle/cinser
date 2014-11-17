using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cinser.DBUtility;
using System.Data;

namespace Cinser.Demo.DAL
{
    public class PublishDataDal : DBUtility.MappingDbHelper<Domains.PublishData>
    {
        protected override DBUtility.DbMapping CreateDbMapping()
        {
            DbMapping map = new DbMapping(typeof(Domains.PublishData));
            map.TableName = "Publish_Data";
            map.AutoLoadFieldsFromModelType();
            map.SetPrimaryKeys("_ID");
            map.FindField("_PublishDateTime").Name = "Publish_DateTime";
            map.FindField("_PublishMan").Name = "Publish_Man";
            map.FindField("_DataType").Name = "Data_Type";
            map.FindField("_SourceInfo").Name = "Source_Info";
            return map;
        }

        public override DbInstanceType DbInstanceType
        {
            get { return ConnectionSettings.DbInstanceType; }
        }

        public override IDbConnection Conn
        {
            get { return ConnectionSettings.IDbConnection; }
        }
    }
}
