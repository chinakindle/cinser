using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;

namespace Cinser.Demo.Wpf
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            dataGrid.SelectionMode = DataGridSelectionMode.Extended;
        }
        
        private void btnAccessPublishData_Click(object sender, RoutedEventArgs e)
        {
            Demo.DAL.PublishDataDal dal = new DAL.PublishDataDal();
            Domains.PublishData item = new Domains.PublishData();
            item.DataType = "DataType";
            item.Description = "Description";
            item.ID = Guid.NewGuid().ToString();
            item.Name = "Name";
            item.OwnerMenuId = "OwnerMenuId";
            item.PublishDateTime = DateTime.Now;
            item.PublishMan = "PublishMan";
            item.SourceInfo = "SourceInfo";
            dal.Insert(item);
            btnAccessGetPublishData_Click(null, null);

        }

        private void btnAccessGetPublishData_Click(object sender, RoutedEventArgs e)
        {
            Demo.DAL.PublishDataDal dal = new DAL.PublishDataDal();
            List<Domains.PublishData> items = dal.GetList();
            this.dataGrid.ItemsSource = items;
        }

        private void btnAccessDelPublishData_Click(object sender, RoutedEventArgs e)
        {
            Demo.DAL.PublishDataDal dal = new DAL.PublishDataDal();
            dal.BeginTransaction();
            foreach (var item in dataGrid.SelectedItems)
            {
                dal.Delete(item as Domains.PublishData);
            }
            dal.CommitTransaction();
            btnAccessGetPublishData_Click(null, null);
        }

        private void btnAccessUpdatePublishData_Click(object sender, RoutedEventArgs e)
        {
            Demo.DAL.PublishDataDal dal = new DAL.PublishDataDal();
            Domains.PublishData item = dataGrid.SelectedItem as Domains.PublishData;
            item.PublishDateTime = DateTime.Now;
            dal.Update(item);
            btnAccessGetPublishData_Click(null, null);

        }
        
        private void btnOracleGetPublishData_Click(object sender, RoutedEventArgs e)
        {
            DAL.AttachmentDal dal = new DAL.AttachmentDal();
            dataGridOracle.ItemsSource = dal.GetList();
        }

        private void btnOracleDelPublishData_Click(object sender, RoutedEventArgs e)
        {
            DAL.AttachmentDal dal = new DAL.AttachmentDal();
            Domains.Attachment item = dataGridOracle.SelectedItem as Domains.Attachment;
            dal.Delete(item);
            btnOracleGetPublishData_Click(null, null);
        }

        private void btnOracleUpdatePublishData_Click(object sender, RoutedEventArgs e)
        {
            DAL.AttachmentDal dal = new DAL.AttachmentDal();
            Domains.Attachment item = dataGridOracle.SelectedItem as Domains.Attachment;
            item.UPLOAD_TIME = DateTime.Now;
            dal.Update(item);
            btnOracleGetPublishData_Click(null, null);
        }

        private void btnOracleAddPublishData_Click(object sender, RoutedEventArgs e)
        {
            DAL.AttachmentDal dal = new DAL.AttachmentDal();
            Domains.Attachment item = new Domains.Attachment();
            item.AT_EXTENSION = "AT_EXTENSION";
            item.AT_NAME = "AT_NAME";
            item.AT_PATH = "AT_PATH";
            item.AT_SIZE = 1204;
            item.GROUP_ID = "GROUP_ID";
            item.ID = Guid.NewGuid().ToString();
            item.RELATED_ID = "RELATED_ID";
            item.REMARK = "REMARK";
            item.TABEL_NAME = "TABEL_NAME";
            item.UPLOAD_TIME = DateTime.Now;
            item.UPLOADER = "UPLOADER";
            dal.Insert(item);
            btnOracleGetPublishData_Click(null, null);
        }
    }
}
