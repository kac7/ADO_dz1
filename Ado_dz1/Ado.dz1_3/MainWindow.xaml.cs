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

namespace Ado.dz1_3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SqlConnection conn = null;
        public MainWindow()
        {
            InitializeComponent();
            //Создать базу данных MyDB. Создать простейшее приложение WinForms, позволяющее пользователю 
            //подключаться к базе данных MyDB, используя аутентификацию SQL Server.Для построения строки 
            //подключения использовать SqlConnectionStringBuilder.

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder
            {
                DataSource = @"DESKTOP-O33O61B",
                InitialCatalog = "MyDB",
                IntegratedSecurity = true, //аутентификация SQL Server
            };
            conn = new SqlConnection(builder.ConnectionString);
            try
            {
                conn.Open();
                Label1.Content = $"Соедение к {builder.InitialCatalog} успешно произведено!";
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
