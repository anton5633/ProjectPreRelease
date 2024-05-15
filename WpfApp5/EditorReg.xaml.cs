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
    /// Логика взаимодействия для EditorReg.xaml
    /// </summary>
    public partial class EditorReg : Page
    {
        public EditorReg()
        {
            InitializeComponent();

            BrushConverter converter = new BrushConverter();
            DispatcherTimer timer = new DispatcherTimer(new TimeSpan(0, 0, 0, 0, 1), DispatcherPriority.Normal, delegate
            {

                if (GlobalVar.ThemeNegr == true)
                {

                    label1.Foreground = (Brush)converter.ConvertFromString("White");
                    label2.Foreground = (Brush)converter.ConvertFromString("White");
                    label3.Foreground = (Brush)converter.ConvertFromString("White");
                    label4.Foreground = (Brush)converter.ConvertFromString("White");
                    label5.Foreground = (Brush)converter.ConvertFromString("White");
                }

                if (GlobalVar.ThemeNegr == false)
                {

                    label1.Foreground = (Brush)converter.ConvertFromString("Black");
                    label2.Foreground = (Brush)converter.ConvertFromString("Black");
                    label3.Foreground = (Brush)converter.ConvertFromString("Black");
                    label4.Foreground = (Brush)converter.ConvertFromString("Black");
                    label5.Foreground = (Brush)converter.ConvertFromString("Black");
                }
            }, Dispatcher);
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (box_login.Text.Length > 0)
            {
                if (box_password.Password.Length > 0)
                {
                    if (box_password.Password.Length >= 6)
                    {
                        if (box_storeid.Text.Length > 0 )
                        {
                            bool en = true; // английская раскладка
                            bool number = false;
                            bool number1 = false;

                            for (int i = 0; i < box_password.Password.Length; i++)
                            {
                                if (box_password.Password[i] >= 'А' && box_password.Password[i] <= 'Я')
                                {
                                    en = false; // если русская раскладка
                                }

                                if (box_password.Password[i] >= '0' && box_password.Password[i] <= '9')
                                {
                                    number = true; // если цифры
                                }
                            }

                            for (int i = 0; i < box_storeid.Text.Length; i++)
                            {
                                if (box_storeid.Text[i] >= 'А' && box_storeid.Text[i] <= 'Я')
                                {
                                    number1 = true; // если русская раскладка
                                }
                            }


                            if (!en)
                            {
                                MessageBox.Show("Доступна только английская раскладка");
                            }
                            else if (!number)
                            {
                                MessageBox.Show("Добавьте хотя бы одну цифру");
                            }

                            else if(number1)
                            {
                                MessageBox.Show("Введите число в поле ID магазина");
                            }

                            if (en && number && !number1)
                            {

                                string login = box_login.Text;
                                string password = box_password.Password;
                                string name = box_name.Text;
                                string surrname = box_surname.Text;
                                string idmagaz = box_storeid.Text;

                                using (SportEntities DataBase = new SportEntities())
                                {

                                    bool isUserExists = DataBase.Editors.Any(u => u.alogin == login);

                                    if (isUserExists)
                                    {
                                        MessageBox.Show("Такой редактор уже существует");
                                    }
                                    else
                                    {
                                        int.TryParse(box_storeid.Text, out int result);
                                        var editors = new Editors { alogin = login, aPassword = password, FirstName = name, SecondName = surrname, StoreID = result };
                                        DataBase.Editors.Add(editors);
                                        DataBase.SaveChanges();
                                        MessageBox.Show("Редактор зарегистрирован");
                                        ClassChangePage.frame1.Navigate(new AdminPage());
                                    }
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("Введите ID магазина, к которому пренадлежит редактор");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Пароль слишком короткий, минимум 6 символов");
                    }
                }
                else
                {
                    MessageBox.Show("Укажите пароль");
                }
            }
            else
            {
                MessageBox.Show("Укажите логин");
            }
        }

        private void AlterBack_Click(object sender, RoutedEventArgs e)
        {
            ClassChangePage.frame1.Navigate(new AdminPage());
        }
    }
}
