using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Cinser.ArcGIS.DAL
{

    public class SdeDalBase
    {
        public enum ShapeType
        {
            Point,      //点
            Line,       //线
            Area        //面
        }

        private IFeatureClass _featureClass = null;
        ShapeType _shapeType;

        public SdeDalBase(SdeSettings p, string featureClassName, ShapeType shapeType = ShapeType.Point)
        {
            if (SDEWorkspace.Workspace == null)
                SDEWorkspace.Workspace = new SDEWorkspace(p);
            this._featureClass = SDEWorkspace.Workspace.GetFeatureClassByName(featureClassName);
            this._shapeType = shapeType;
        }

        /// <summary>
        /// 将一个键值对modle添加至表中。
        /// </summary>
        public virtual void Add(Dictionary<string, object> modle,double x,double y)
        {
            IDataset myDataset = this._featureClass as IDataset;
            IWorkspaceEdit myWorkspaceEdit = myDataset.Workspace as IWorkspaceEdit;
            myWorkspaceEdit.StartEditing(false);
            myWorkspaceEdit.StartEditOperation();

            IFeatureCursor myFeatureC = this._featureClass.Insert(true);
            IFeatureBuffer myFeatureBuffer = this._featureClass.CreateFeatureBuffer();

            foreach (var item in modle)
            {
                if (item.Value != null && item.Value != "")
                    myFeatureBuffer.set_Value(this._featureClass.FindField(item.Key), item.Value);
            }

            IGeometry myShape = null;
            if (_shapeType == ShapeType.Point)
            {
                PointClass myPoint = new PointClass();
                myPoint.X = x;
                myPoint.Y = y;
                myShape = myPoint;
            }
            myFeatureBuffer.Shape = myShape;
            myFeatureC.InsertFeature(myFeatureBuffer);
            myFeatureC.Flush();
            myWorkspaceEdit.StopEditOperation();
            myWorkspaceEdit.StopEditing(false);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(myFeatureC);
        }

        public virtual void Add(Dictionary<string, object> modle)
        {
            Add(modle, double.Parse(modle["X"].ToString()), double.Parse(modle["Y"].ToString()));
        }

        public void Delete(string sqlWhere)
        {
            QueryFilterClass myQueryFilterClass = new QueryFilterClass();
            myQueryFilterClass.WhereClause = sqlWhere;

            IFeatureCursor myFeatureCursor = this._featureClass.Search(myQueryFilterClass, false);
            IFeature myFeature = myFeatureCursor.NextFeature();
            while (myFeature != null)
            {
                myFeature.Delete();
                myFeature = myFeatureCursor.NextFeature();
            }
            System.Runtime.InteropServices.Marshal.ReleaseComObject(myFeatureCursor);
        }

        public void Update(Dictionary<string, object> modle)
        {
            if (IsContainsId(modle["ID"].ToString()) == true)
            {
                Update(modle, " ID='" + modle["ID"].ToString() + "'");
            }
        }

        public void Update(Dictionary<string, object> modle, string sqlWhere)
        {
            Update(modle, sqlWhere, double.Parse(modle["X"].ToString()), double.Parse(modle["Y"].ToString()));
        }

        public void Update(Dictionary<string, object> modle, string sqlWhere,double x,double y)
        {
            IDataset myDataset = this._featureClass as IDataset;
            IWorkspaceEdit myWorkspaceEdit = myDataset.Workspace as IWorkspaceEdit;
            myWorkspaceEdit.StartEditing(false);
            myWorkspaceEdit.StartEditOperation();
            QueryFilterClass myQueryFilterClass = new QueryFilterClass();
            myQueryFilterClass.WhereClause = sqlWhere;
            IFeatureCursor myFeatureCursor = this._featureClass.Update(myQueryFilterClass, false);
            IFeature myFeature = myFeatureCursor.NextFeature();
            while (myFeature != null)
            {
                foreach (var item in modle)
                {
                    if (item.Value != null && item.Value != "")
                        myFeature.set_Value(this._featureClass.FindField(item.Key), item.Value);
                }

                if (CheckShapeIfChanged(myFeature, x,y))
                {
                    IGeometry myShape = null;
                    if (this._shapeType == ShapeType.Point)
                    {
                        PointClass myPoint = new PointClass();
                        myPoint.X = x;
                        myPoint.Y = y;
                        myShape = myPoint;
                    }
                    myFeature.Shape = myShape;
                }
                myFeatureCursor.UpdateFeature(myFeature);
                myFeature = myFeatureCursor.NextFeature();
            }

            myWorkspaceEdit.StopEditOperation();
            myWorkspaceEdit.StopEditing(false);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(myFeatureCursor);
        }

        private bool CheckShapeIfChanged(IFeature myFeature, double x,double y)
        {
            bool bReturn = true;
            //if (this._shapeType == ShapeType.Point)
            //{
            //    if ((myFeature.Shape as PointClass).X != x ||
            //     (myFeature.Shape as PointClass).Y != y)
            //    {
            //        bReturn = true;
            //    }
            //    else
            //    {
            //        bReturn = false;
            //    }
            //}
            return bReturn;
        }

        public bool IsContainsId(string id)
        {
            return GetCount(" ID='" + id + "'") > 0;
        }

        public int GetCount(string sqlWhere)
        {
            QueryFilterClass myQueryFilterClass = new QueryFilterClass();
            myQueryFilterClass.WhereClause = sqlWhere;
            int myCount = this._featureClass.FeatureCount(myQueryFilterClass);
            return myCount;
        }

        public DataTable GetData(string sqlWhere = " 1=1")
        {
            if (sqlWhere == string.Empty)
            {
                sqlWhere = " 1=1";
            }
            DataTable dt = new DataTable();
            QueryFilterClass myQueryFilterClass = new QueryFilterClass();
            myQueryFilterClass.WhereClause = sqlWhere;
            IFeatureCursor myFeatureCursor = this._featureClass.Search(myQueryFilterClass, false);
            List<string> Fields = GetAllFields();
            for (int i = 0; i < Fields.Count; i++)
            {
                dt.Columns.Add(Fields[i]);
            }

            IFeature myFeature = myFeatureCursor.NextFeature();
            while (myFeature != null)
            {
                DataRow row = dt.NewRow();
                for (int i = 0; i < Fields.Count; i++)
                {
                    row[Fields[i]] = myFeature.get_Value(this._featureClass.FindField(Fields[i]));
                }
                dt.Rows.Add(row);
                myFeature = myFeatureCursor.NextFeature();
            }
            return dt;
        }
        
        public List<string> GetAllFields()
        {
            List<string> Fields = new List<string>();
            for (int i = 0; i < this._featureClass.Fields.FieldCount; i++)
            {
                IField myField = _featureClass.Fields.get_Field(i);

                Fields.Add(myField.Name);
            }
            return Fields;
        }        
    }
}
