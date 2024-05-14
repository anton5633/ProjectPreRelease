using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для ProductCard.xaml
    /// </summary>
    public partial class ProductCard : Page
    {
        public ProductCard(ProductCards product, Reviews reviews)
        {
            InitializeComponent();

            DataContext = product;
            DataContext = reviews;
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

        private void CabinetButton(object sender, RoutedEventArgs e)
        {
            ClassChangePage.frame1.Navigate(new Main());
        }

    }
}
