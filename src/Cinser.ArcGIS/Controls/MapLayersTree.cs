

//wanghl
//2013.11.8
//图层树

using System;
using System.Windows.Controls;
using ESRI.ArcGIS.Client;
using System.Collections.Generic;
using System.Windows.Media.Imaging;
using System.Windows;


namespace Cinser.ArcGIS.Controls
{

    /// <summary>
    /// 图层树
    /// </summary>
    public class MapLayersTree:TreeView
    {
        private ArcGISDynamicMapServiceLayer _mapLayer = null;
        private List<CheckBox> _checkBoxList = new List<CheckBox>();

        private List<TreeViewItem> _treeViewItemList = new List<TreeViewItem>();
        private List<LayerInfo> _LayerInfoList = new List<LayerInfo>();


        public void InitUI(ArcGISDynamicMapServiceLayer pMapLayer)
        {
            this.BorderThickness = new System.Windows.Thickness(0);
            this._mapLayer = pMapLayer;
            this.LoadTree(pMapLayer);

            pMapLayer.QueryLegendInfos(this.ActionLayerLegendInfoCallback, this.ActionExceptionErrorCallback);
            //UIsSL.ProgressUI.Show(this.LayoutRoot, "正在加载图例，请稍后...");
        }


        private List<LayerInfo> GetChilrenLayerInfoListByLayerInfo(ArcGISDynamicMapServiceLayer pMapLayer, LayerInfo pLayerInfo)
        {
            List<LayerInfo> myLayerInfoList = new List<LayerInfo>();
            int[] myLyaerInfoIDArray = pLayerInfo.SubLayerIds;
            if (myLyaerInfoIDArray == null)
            {
                return myLayerInfoList;
            }
            foreach (LayerInfo myLayerInfo in pMapLayer.Layers)
            {
                foreach (int myID in myLyaerInfoIDArray)
                {
                    if (myLayerInfo.ID == myID)
                    {
                        myLayerInfoList.Add(myLayerInfo);
                    }
                }
            }
            return myLayerInfoList;
        }


        private void LoadTree(ArcGISDynamicMapServiceLayer pMapLayer)
        {
            List<int> mySubIDList = new List<int>();
            foreach (LayerInfo myLayerInfo in pMapLayer.Layers)
            {
                if (myLayerInfo.SubLayerIds != null)
                {
                    mySubIDList.AddRange(myLayerInfo.SubLayerIds);
                }
            }

            foreach (LayerInfo myLayerInfo in pMapLayer.Layers)
            {
                if (mySubIDList.Contains(myLayerInfo.ID))
                {
                    continue;
                }
                this.Items.Add(this.GetTreeViewItemByLayerInfo(pMapLayer, myLayerInfo));
            }

            if (this.Items.Count > 0)
            {
                (this.Items[0] as TreeViewItem).IsExpanded = true;
            }
        }

        private TreeViewItem GetTreeViewItemByLayerInfo(ArcGISDynamicMapServiceLayer pMapLayer, LayerInfo pLayerInfo)
        {
            TreeViewItem myTreeViewItem = new TreeViewItem();
            StackPanel myStackPanel = new StackPanel();
            myStackPanel.Orientation = Orientation.Horizontal;

            Image myImage = new Image();
            myImage.Margin = new System.Windows.Thickness(2, 0, 2, 0);

            if (pLayerInfo.SubLayerIds == null)
            {
                CheckBox myCheckBox = new CheckBox();
                this._checkBoxList.Add(myCheckBox);
                myCheckBox.Tag = myTreeViewItem;
                myCheckBox.IsChecked = pMapLayer.GetLayerVisibility(pLayerInfo.ID);
                myStackPanel.Children.Add(myCheckBox);
                myCheckBox.Click += new System.Windows.RoutedEventHandler(myCheckBox_Click);
            }
            else
            {
                myImage.Source = new BitmapImage(new Uri("/Cinser.ArcGIS;component/Images/文件夹闭合16.png", UriKind.RelativeOrAbsolute));
            }
            myStackPanel.Children.Add(myImage);

            Label myLabel = new Label();
            myLabel.Content = pLayerInfo.Name;
            myStackPanel.Children.Add(myLabel);
            myTreeViewItem.Header = myStackPanel;
            myTreeViewItem.Tag = pLayerInfo;

            this._treeViewItemList.Add(myTreeViewItem);
            this._LayerInfoList.Add(pLayerInfo);

            List<LayerInfo> myLayerInfoList = this.GetChilrenLayerInfoListByLayerInfo(pMapLayer, pLayerInfo);
            foreach (LayerInfo myLayerInfo in myLayerInfoList)
            {
                myTreeViewItem.Items.Add(this.GetTreeViewItemByLayerInfo(pMapLayer, myLayerInfo));
            }

            myTreeViewItem.Expanded += new System.Windows.RoutedEventHandler(myTreeViewItem_Expanded);
            myTreeViewItem.Collapsed += new System.Windows.RoutedEventHandler(myTreeViewItem_Collapsed);
            return myTreeViewItem;
        }

        void myTreeViewItem_Collapsed(object sender, System.Windows.RoutedEventArgs e)
        {
            TreeViewItem myTreeViewItem = sender as TreeViewItem;
            if ((myTreeViewItem.Tag as LayerInfo).SubLayerIds != null)
            {
                StackPanel myStackPanel = myTreeViewItem.Header as StackPanel;
                Image myImage = myStackPanel.Children[0] as Image;
                myImage.Source = new BitmapImage(new Uri("/Cinser.ArcGIS;component/Images/文件夹闭合16.png", UriKind.RelativeOrAbsolute));
            }
        }

        void myTreeViewItem_Expanded(object sender, System.Windows.RoutedEventArgs e)
        {
            TreeViewItem myTreeViewItem = sender as TreeViewItem;
            if ((myTreeViewItem.Tag as LayerInfo).SubLayerIds != null)
            {
                StackPanel myStackPanel = myTreeViewItem.Header as StackPanel;
                Image myImage = myStackPanel.Children[0] as Image;
                myImage.Source = new BitmapImage(new Uri("/Cinser.ArcGIS;component/Images/文件夹打开16.png", UriKind.RelativeOrAbsolute));
            }
        }


        void myCheckBox_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            CheckBox myCheckBox = sender as CheckBox;
            TreeViewItem myTreeViewItem = myCheckBox.Tag as TreeViewItem;
            LayerInfo myLayerInfo = myTreeViewItem.Tag as LayerInfo;
            this._mapLayer.SetLayerVisibility(myLayerInfo.ID, (bool)myCheckBox.IsChecked);
        }



        



        private void ActionLayerLegendInfoCallback(LayerLegendInfo pLayerLegendInfo)
        {
            if (pLayerLegendInfo.LayerLegendInfos == null)
            {
                return;
            }
            foreach (LayerLegendInfo myLayerLegendInfo in pLayerLegendInfo.LayerLegendInfos)
            {
                for (int i = 0; i < this._LayerInfoList.Count; i++)
                {
                    if (myLayerLegendInfo.SubLayerID == this._LayerInfoList[i].ID)
                    {
                        TreeViewItem myLayerTreeViewItem = this._treeViewItemList[i];
                        if (myLayerLegendInfo.LegendItemInfos == null)
                        {
                            continue;
                        }
                        foreach (LegendItemInfo myLegendItemInfo in myLayerLegendInfo.LegendItemInfos)
                        {
                            myLayerTreeViewItem.Items.Add(this.CreateTreeViewItem(myLegendItemInfo));
                        }
                        myLayerTreeViewItem.IsExpanded = true;
                    }
                }
                //递归
                this.ActionLayerLegendInfoCallback(myLayerLegendInfo);
            }
        }


        private TreeViewItem CreateTreeViewItem(LegendItemInfo pLegendItemInfo)
        {
            TreeViewItem myTreeViewItem = new TreeViewItem();
            myTreeViewItem.Margin = new Thickness(0, -2, 0, -2);
            StackPanel myStackPanel = new StackPanel();
            myTreeViewItem.Header = myStackPanel;
            myStackPanel.Orientation = Orientation.Horizontal;

            Image myImage = new Image();
            myImage.Stretch = System.Windows.Media.Stretch.Uniform;
            myImage.Width =40;
            myImage.Height = 18;
            myStackPanel.Children.Add(myImage);
            myImage.Source = pLegendItemInfo.ImageSource;

            Label myLabel = new Label();
            myStackPanel.Children.Add(myLabel);
            myLabel.Margin = new Thickness(5, 0, 0, 0);
            myLabel.Content = pLegendItemInfo.Label;

            return myTreeViewItem;
        }


        private void ActionExceptionErrorCallback(Exception error)
        {

        }


    }
}
