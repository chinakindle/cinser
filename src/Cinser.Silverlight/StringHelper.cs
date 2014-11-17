using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;

namespace Cinser.Silverlight
{
    public class StringHelper
    {
        public static string CombineSqlWhere(List<string> fieldValueList, string fieldName)
        {
            string sqlWhere = string.Empty;
            if (fieldValueList != null && fieldValueList.Count > 0)
            {
                sqlWhere = "'" + fieldValueList[0] + "'";
                for (int i = 1; i < fieldValueList.Count; i++)
                {
                    sqlWhere += ",'" + fieldValueList[i] + "'";
                }
            }
            if (sqlWhere != string.Empty)
            {
                sqlWhere = " " + fieldName + " in (" + sqlWhere + ") ";
            }
            else
            {
                sqlWhere = " 1=2 ";
            }
            return sqlWhere;
        }
    }
}
