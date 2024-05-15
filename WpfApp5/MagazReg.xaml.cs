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
using System.Windows.Threading;

namespace WpfApp5
{
    /// <summary>
    /// Логика взаимодействия для MagazReg.xaml
    /// </summary>
    public partial class MagazReg : Page
    {
        public MagazReg()
        {
            InitializeComponent();

            BrushConverter converter = new BrushConverter();
            DispatcherTimer timer = new DispatcherTimer(new TimeSpan(0, 0, 0, 0, 1), DispatcherPriority.Normal, delegate
            {

                if (GlobalVar.ThemeNegr == true)
                {
                    label1.Foreground = (Brush)converter.ConvertFromString("White");
                    label2.Foreground = (Brush)converter.ConvertFromString("White");
                }

                if (GlobalVar.ThemeNegr == false)
                {
                    label1.Foreground = (Brush)converter.ConvertFromString("Black");
                    label2.Foreground = (Brush)converter.ConvertFromString("Black");
                }
            }, Dispatcher);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (box_name.Text.Length > 0)
            {
                if (box_discription.Text.Length > 0) 
                {
                    string name = box_name.Text;
                    string discription = box_discription.Text;

                    using (SportEntities DataBase = new SportEntities())
                    {

                        bool isUserExists = DataBase.Stores.Any(u => u.NameStore == name);

                        if (isUserExists)
                        {
                            MessageBox.Show("Такой магазин уже существует");
                        }
                        else
                        {
                            var store = new Stores {NameStore = name, Discription = discription};
                            DataBase.Stores.Add(store);
                            DataBase.SaveChanges();
                            MessageBox.Show("Магазин зарегистрирован");
                            ClassChangePage.frame1.Navigate(new AdminPage());
                        }
                    }
                }
            }
        }

        private void AlterBack_Click(object sender, RoutedEventArgs e)
        {
            ClassChangePage.frame1.Navigate(new AdminPage());
        }
    }
}
