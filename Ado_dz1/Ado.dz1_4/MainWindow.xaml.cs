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
using System.Configuration;
using System.Globalization;
using System.Collections;

namespace Ado.dz1_4
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
            nameTable.SelectionChanged += NameDB_SelectionChanged;

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder
            {
                DataSource = @"DESKTOP-O33O61B",
                InitialCatalog = "Sample",
                IntegratedSecurity = true, 
            };
            conn = new SqlConnection(builder.ConnectionString);
            //conn.ConnectionString = ConfigurationManager.ConnectionStrings["MyConnString"].ConnectionString;
            try
            {
                conn.Open();
                //CreateAndInsertTable(); //создать и заполнить таблицы
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
        private void NameDB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                conn.Open();
                if (nameTable.SelectedIndex == 1)
                {
                    
                    SqlCommand cmd = new SqlCommand("select * from Buyer", conn);
                    SqlDataReader rdr = null;
                    rdr = cmd.ExecuteReader();
                    DateDB.Items.Clear();
                    while (rdr.Read())
                    {
                        DateDB.Items.Add($"{rdr[0]} {rdr[1]} {rdr[2]}");
                    }
                }
                else if(nameTable.SelectedIndex == 2)
                {
                    SqlCommand cmd = new SqlCommand("select * from Sellers", conn);
                    SqlDataReader rdr = null;
                    rdr = cmd.ExecuteReader();
                    DateDB.Items.Clear();
                    while (rdr.Read())
                    {
                        DateDB.Items.Add($"{rdr[0]} {rdr[1]} {rdr[2]}");
                    }
                }
                else if (nameTable.SelectedIndex == 3)
                {
                    SqlCommand cmd = new SqlCommand("select Sales.ID, Buyer.Имя, Buyer.Фамилия, " +
                        "Sales.Date, Sales.Sum, Sellers.Имя, Sellers.Фамилия " +
                        "from Sales join Buyer on Buyer.ID = Sales.IDBuyer " +
                        "join Sellers on Sellers.ID = Sales.IDSeller", conn);
                    SqlDataReader rdr = null;
                    rdr = cmd.ExecuteReader();
                    DateDB.Items.Clear();
                    while (rdr.Read())
                    {
                        DateTime dt = (DateTime)rdr[3]; 
                        DateDB.Items.Add($"{rdr[0]}  {rdr[1]} {rdr[2]}  {dt.ToString("d")}  {rdr[4]}  {rdr[5]} {rdr[6]}");
                    }
                }
                else
                {
                    DateDB.Items.Clear();
                }
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

        public void CreateAndInsertTable()
        {
            string queryString = "IF OBJECTPROPERTY(OBJECT_ID(N'dbo.Buyer'),'ISTABLE') IS NULL" + " CREATE TABLE " +
                         "Buyer(ID int IDENTITY(1,1) not null , Имя nvarchar(50) not null, Фамилия nvarchar(50) not null, PRIMARY KEY (ID))";
            SqlCommand cmdCreateTableBuyer = new SqlCommand(queryString, conn);

            queryString = "IF OBJECTPROPERTY(OBJECT_ID(N'dbo.Sellers'),'ISTABLE') IS NULL" + " CREATE TABLE " +
                         "Sellers(ID int IDENTITY(1,1) not null , Имя nvarchar(50) not null, Фамилия nvarchar(50) not null, PRIMARY KEY (ID))";
            SqlCommand cmdCreateTableSellers = new SqlCommand(queryString, conn);

            queryString = "IF OBJECTPROPERTY(OBJECT_ID(N'dbo.Sales'),'ISTABLE') IS NULL" + " CREATE TABLE " +
                         "Sales(ID int IDENTITY(1,1) not null , IDBuyer int not null FOREIGN KEY REFERENCES Buyer(ID) ON DELETE CASCADE," +
                         " IDSeller int not null FOREIGN KEY REFERENCES Sellers(ID) ON DELETE CASCADE," +
                         " Sum decimal not null, Date date not null, PRIMARY KEY (ID))";
            SqlCommand cmdCreateTableSales = new SqlCommand(queryString, conn);

            queryString = "insert Buyer values ('Иван', 'Иванов'), ('Петр', 'Петров'), ('Сидор', 'Сидоров'), ('Вася', 'Васичкин'), ('Фёдор', 'Фёдоров')";
            SqlCommand cmdInsertBuyer = new SqlCommand(queryString, conn);

            queryString = "insert Sellers values ('Абзал', 'Абзалович'), ('Бакир', 'Бакирович'), ('Дамир', 'Дамирвич'), ('Кадыр', 'Кадырвич'), ('Касымхан', 'Касымханвич')";
            SqlCommand cmdInsertSellers = new SqlCommand(queryString, conn);

            queryString = "insert Sales values (1, 5, 300, '2018-06-20'), (1, 2, 100, '2018-06-20'), (2, 5, 330, '2018-06-21'), (2, 2, 130, '2018-06-21'), (3, 4, 230, '2018-06-19'), (4, 1, 330, '2018-06-21'), (5, 3, 30, '2018-06-21')";
            SqlCommand cmdInsertSales = new SqlCommand(queryString, conn);

            try
            {
                cmdCreateTableBuyer.ExecuteNonQuery();
                cmdCreateTableSellers.ExecuteNonQuery();
                cmdCreateTableSales.ExecuteNonQuery();
                cmdInsertBuyer.ExecuteNonQuery();
                cmdInsertSellers.ExecuteNonQuery();
                cmdInsertSales.ExecuteNonQuery();

                Label1.Content = $"Соедение успешно произведено! Таблицы добвелнны!!!";
            }
            catch (SqlException)
            {
                
            }
        }
    }
}
