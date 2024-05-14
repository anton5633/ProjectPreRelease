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
    /// Логика взаимодействия для Login.xaml
    /// </summary>
    public partial class Login : Page
    {
        public Login()
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
            }
            , Dispatcher);

        }
        private void Button_Login(object sender, RoutedEventArgs e)
        {
            NavigationCommands.BrowseBack.InputGestures.Clear();
            NavigationCommands.BrowseForward.InputGestures.Clear();

            if (box_login.Text.Length > 0)
            {
                if (box_password.Password.Length > 0)
                {
                    using (SportEntities DataBase = new SportEntities())
                    {
                        string login = box_login.Text;
                        string password = box_password.Password;

                        GlobalVar.PanelLogin = login;

                        bool isAdminExistsLogin = DataBase.Admins.Any(u => u.alogin == login);
                        bool isAdminExistsPass = DataBase.Admins.Any(u => u.aPassword == password);
                        bool isUserExistsLogin = DataBase.Users.Any(u => u.alogin == login);
                        bool isUserExistsPass = DataBase.Users.Any(u => u.aPassword == password);
                        bool isEditorExistsLogin = DataBase.Editors.Any(u => u.alogin == login);
                        bool isEditorExistsPass = DataBase.Editors.Any(u => u.aPassword == password);

                        if (isAdminExistsLogin && isAdminExistsPass)
                        {
                            GlobalVar.StatusAuth = true;
                            GlobalVar.AdminReg = true;
                            MessageBox.Show("Админ авторизовался");
                            ClassChangePage.frame1.Navigate(new Main());
                        }
                        else
                        {
                            if (isUserExistsLogin && isUserExistsPass)
                            {
                                GlobalVar.StatusAuth = true;
                                GlobalVar.UserReg = true;
                                MessageBox.Show("Пользователь авторизовался");
                                ClassChangePage.frame1.Navigate(new Main());
                            }
                            else
                            {
                                if (isEditorExistsLogin && isEditorExistsPass)
                                {
                                    GlobalVar.StatusAuth = true;
                                    GlobalVar.EditorReg = true;
                                    MessageBox.Show("Редактор авторизовался");
                                    ClassChangePage.frame1.Navigate(new Main());
                                }
                                else
                                {
                                    MessageBox.Show("Неверный логин или пароль");
                                }
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Введите пароль");
                }
            }
            else
            {
                MessageBox.Show("Введите логин");
            }
        }

        private void Button_Registration(object sender, RoutedEventArgs e)
        {
            NavigationCommands.BrowseBack.InputGestures.Clear();
            NavigationCommands.BrowseForward.InputGestures.Clear();

            GlobalVar.AdminReg = false;
            ClassChangePage.frame1.Navigate(new Registration());
        }

        private void OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            Watermark.Visibility = box_password.Password.Length > 0 ? Visibility.Collapsed : Visibility.Visible;
        }
    }
}
