using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Ado_dz1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Напишите код с использованием технологии ADO.NET, который создает в базе данных таблицу gruppa.
                SqlConnection conn = null;
        public MainWindow()
        {
            InitializeComponent();
            
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder { 
                DataSource = @"DESKTOP-O33O61B",
                InitialCatalog = "Step",
                IntegratedSecurity = true,
            };
            conn = new SqlConnection(builder.ConnectionString);
            CreateNewTable();
        }
        public void CreateNewTable()
        {
            string queryString = "IF OBJECTPROPERTY(OBJECT_ID(N'dbo.gruppa'),'ISTABLE') IS NULL CREATE TABLE gruppa (ID INT NOT NULL PRIMARY KEY, Name NVARCHAR(60) NOT NULL)";
            SqlCommand cmdCreateTable = new SqlCommand(queryString, conn);
            try
            {
                conn.Open();
                cmdCreateTable.ExecuteNonQuery();
                Label1.Content = "Таблица создана успешно!";
            }
            catch (SqlException ex)
            {
                Label1.Content = ex.Message;
            }
            finally
            {
                conn.Close(); 
            }
        }

    }
}
