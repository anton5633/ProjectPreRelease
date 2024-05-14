using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace WpfApp5
{
    /// <summary>
    /// Логика взаимодействия для AdminPage.xaml
    /// </summary>
    public partial class AdminPage : Page
    {
        private readonly Admins _currentUser;
        private readonly SportEntities sportEntities;

        public AdminPage()
        {
            InitializeComponent();

            Hello.Text = "Здравствуйте, " + GlobalMethods.GetNameAdmin(GlobalVar.PanelLogin);

            sportEntities = new SportEntities();
            _currentUser = sportEntities.Admins.FirstOrDefault(u => u.alogin == GlobalVar.PanelLogin);

            if (_currentUser != null)
            {
                firstNameTextBlock.Text = _currentUser.FirstName;
                secondNameTextBlock.Text = _currentUser.SecondName;
                if (!string.IsNullOrEmpty(_currentUser.AdminImagePath))
                {
                    // Получаем путь к директории проекта
                    string projectDirectory = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\");
                    // Обработка относительного пути к изображению
                    string absoluteImagePath = System.IO.Path.Combine(projectDirectory, _currentUser.AdminImagePath);

                    try
                    {
                        // Создать BitmapImage и установить источник изображения
                        BitmapImage bitmap = new BitmapImage(new Uri(absoluteImagePath));
                        profileImage.Source = bitmap;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка при загрузке изображения: " + ex.Message);
                    }
                }
            }

            BrushConverter converter = new BrushConverter();

            DispatcherTimer timer = new DispatcherTimer(new TimeSpan(0, 0, 0, 0, 1), DispatcherPriority.Normal, delegate
            {
                if (GlobalVar.ThemeNegr == true)
                {
                    Hello.Foreground = (Brush)converter.ConvertFromString("White");
                    Hello.Foreground = (Brush)converter.ConvertFromString("White");
                }
                if (GlobalVar.ThemeNegr == false)
                {
                    Hello.Foreground = (Brush)converter.ConvertFromString("Black");
                    Hello.Foreground = (Brush)converter.ConvertFromString("Black");
                }
            }
           , Dispatcher);
        }

        private void ChangePhoto_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image files (*.jpg, *.jpeg, *.png) | *.jpg; *.jpeg; *.png"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;
                string destinationDirectory = "ProfilePhoto";
                string destinationPath = System.IO.Path.Combine(destinationDirectory, System.IO.Path.GetFileName(filePath));
                string absoluteDirectoryPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\", destinationDirectory);

                // Проверяем существует ли директория, если нет, то создаем ее
                if (!Directory.Exists(absoluteDirectoryPath))
                {
                    Directory.CreateDirectory(absoluteDirectoryPath);
                }

                File.Copy(filePath, System.IO.Path.Combine(absoluteDirectoryPath, System.IO.Path.GetFileName(filePath)), true);

                // Путь к изображению относительно корня проекта
                string relativeImagePath = System.IO.Path.Combine(destinationPath);

                // Проверяем, существует ли файл в целевой директории после копирования
                if (File.Exists(System.IO.Path.Combine(absoluteDirectoryPath, System.IO.Path.GetFileName(filePath))))
                {
                    // Выводим сообщение об успешном копировании
                    MessageBox.Show("Файл успешно скопирован.");

                    // Устанавливаем путь к изображению
                    _currentUser.AdminImagePath = relativeImagePath;
                }
                else
                {
                    // Выводим сообщение об ошибке, если файл не был скопирован
                    MessageBox.Show("Ошибка при копировании файла.");
                }
            }

            // Загружаем изображение независимо от результата копирования
            profileImage.Source = new BitmapImage(new Uri(_currentUser.AdminImagePath, UriKind.RelativeOrAbsolute));
            sportEntities.SaveChanges();
            MessageBox.Show("Изменения успешно применены.");
            ClassChangePage.frame1.Navigate(new AdminPage());
        }

        private void ChangeName(object sender, RoutedEventArgs e)
        {
            _currentUser.FirstName = firstNameTextBox.Text;
            _currentUser.SecondName = secondNameTextBox.Text;
            sportEntities.SaveChanges();
            MessageBox.Show("Изменения успешно применены.");
            ClassChangePage.frame1.Navigate(new AdminPage());
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Вы уверены?", "Выход из учётной записи", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                GlobalVar.StatusAuth = false;
                GlobalVar.AdminReg = false;
                ClassChangePage.frame1.Navigate(new Login());
            }
        }
        private void AdminPage_Loaded(object sender2, RoutedEventArgs e)
        {
            // Находим элемент Border по его имени
            Border myBorder = (Border)FindName("myBorder");

            // Создаем анимацию для изменения прозрачности
            DoubleAnimation opacityAnimation = new DoubleAnimation
            {
                From = 0, // Начальное значение прозрачности
                To = 1, // Конечное значение прозрачности
                Duration = new Duration(TimeSpan.FromSeconds(1)) // Длительность анимации
            };

            // Создаем анимацию для изменения положения
            ThicknessAnimation positionAnimation = new ThicknessAnimation
            {
                From = new Thickness(50, 100, 0, -100), // Начальное значение положения (скрытое)
                To = new Thickness(50, 0, 0, 0), // Конечное значение положения (отображение на экране)
                Duration = new Duration(TimeSpan.FromSeconds(1)) // Длительность анимации
            };

            // Запускаем анимации
            myBorder.BeginAnimation(OpacityProperty, opacityAnimation);
            myBorder.BeginAnimation(MarginProperty, positionAnimation);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            GlobalVar.AdminReg = true;
            ClassChangePage.frame1.Navigate(new Registration());
        }

        private void ButtonEditor(object sender, RoutedEventArgs e)
        {
        
        }

        private void ButtonMagaz(object sender, RoutedEventArgs e)
        {
        
        }

        private void LeaveButton_Click(object sender, RoutedEventArgs e)
        {
            ClassChangePage.frame1.Navigate(new Main());
        }
    }
}

