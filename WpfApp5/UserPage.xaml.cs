using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Логика взаимодействия для UserPage.xaml
    /// </summary>
    public partial class UserPage : Page
    {
        private Users _currentUser;
        private SportEntities sportEntities;

        public UserPage()
        {
            InitializeComponent();

            sportEntities = new SportEntities();
            _currentUser = sportEntities.Users.FirstOrDefault(u => u.alogin == GlobalVar.PanelLogin);

            if (_currentUser != null)
            {
                firstNameTextBlock.Text = _currentUser.FirstName;
                secondNameTextBlock.Text = _currentUser.SecondName;
                if (!string.IsNullOrEmpty(_currentUser.UserImagePath))
                {
                    // Получаем путь к директории проекта
                    string projectDirectory = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\");
                    // Обработка относительного пути к изображению
                    string absoluteImagePath = System.IO.Path.Combine(projectDirectory, _currentUser.UserImagePath);

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
        }

        private void ChangePhoto_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.png) | *.jpg; *.jpeg; *.png";

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
                    _currentUser.UserImagePath = relativeImagePath;
                }
                else
                {
                    // Выводим сообщение об ошибке, если файл не был скопирован
                    MessageBox.Show("Ошибка при копировании файла.");
                }
            }

            // Загружаем изображение независимо от результата копирования
            profileImage.Source = new BitmapImage(new Uri(_currentUser.UserImagePath, UriKind.RelativeOrAbsolute));
            sportEntities.SaveChanges();
            MessageBox.Show("Изменения успешно применены.");
            ClassChangePage.frame1.Navigate(new UserPage());
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Вы уверены?", "Выход из учётной записи", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                GlobalVar.StatusAuth = false;
                GlobalVar.UserReg = false;
                ClassChangePage.frame1.Navigate(new Login());
            }
        }

        private void LeaveButton_Click(object sender, RoutedEventArgs e)
        {
            ClassChangePage.frame1.Navigate(new Main());
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ClassChangePage.frame1.Navigate(new Main());
        }

        private void ChangeName(object sender, RoutedEventArgs e)
        {
            _currentUser.FirstName = firstNameTextBox.Text;
            _currentUser.SecondName = secondNameTextBox.Text;
            sportEntities.SaveChanges();
            MessageBox.Show("Изменения успешно применены.");
            ClassChangePage.frame1.Navigate(new UserPage());
        }
    }
}

