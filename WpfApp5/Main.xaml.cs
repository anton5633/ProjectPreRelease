using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace WpfApp5
{
    /// <summary>
    /// Логика взаимодействия для Main.xaml
    /// </summary>
    public partial class Main : Page
    {
        public Main()
        {   
            InitializeComponent();
            DataContext = this;
            LoadProducts();

            if (GlobalVar.AdminReg)
            {
                CabinetButton.Click += Button_ClickAdmin;
            }
            else
            {
                if (GlobalVar.EditorReg) 
                {
                    CabinetButton.Click += Button_ClickEditor;
                }
                else
                {
                    if (GlobalVar.UserReg)
                    {
                        CabinetButton.Click += Button_ClickUser;
                    }
                    else MessageBox.Show("Ошибка.");
                }
            }
        }

        public ObservableCollection<ProductCards> Products { get; set; }

        private void LoadProducts()
        {
            // Подключение к базе данных через Entity Framework
            using (var context = new SportEntities())
            {
                // Загрузка списка товаров из базы данных
                var products = context.ProductCards.ToList();
                Products = new ObservableCollection<ProductCards>(products);
            }
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Вы уверены?", "Выход из учётной записи", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                GlobalVar.StatusAuth = false;
                GlobalVar.AdminReg = false;
                GlobalVar.UserReg = false;
                GlobalVar.EditorReg = false;
                ClassChangePage.frame1.Navigate(new Login());
            }
        }

        private void Button_ClickAdmin(object sender, RoutedEventArgs e)
        {
            ClassChangePage.frame1.Navigate(new AdminPage());   
        }

        private void Button_ClickEditor(object sender, RoutedEventArgs e)
        {
            ClassChangePage.frame1.Navigate(new EditorPage());
        }

        private void Button_ClickUser(object sender, RoutedEventArgs e)
        {
            ClassChangePage.frame1.Navigate(new UserPage());
        }

        private void ToProduct(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                if (button.DataContext is ProductCards product)
                {
                    var productDetailsPage = new DiscriptionProduct(product);
                    NavigationService.Navigate(productDetailsPage);
                }
            }
        }
    }    
}
