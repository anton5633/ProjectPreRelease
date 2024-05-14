using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Windows;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Path = System.IO.Path;
using System.Reflection;

namespace WpfApp5
{
    /// <summary>
    /// Логика взаимодействия для TEST.xaml
    /// </summary>
    public partial class TEST : Page
    {
        private Admins _currentAdmins;
        private SportEntities sportEntities;

        public TEST()
        {

            InitializeComponent();


            sportEntities = new SportEntities();
            _currentAdmins = sportEntities.Admins.FirstOrDefault(u => u.alogin == GlobalVar.PanelLogin);

            if (_currentAdmins != null)
            {
                firstNameTextBlock.Text = _currentAdmins.FirstName;
                secondNameTextBlock.Text = _currentAdmins.SecondName;
                if (!string.IsNullOrEmpty(_currentAdmins.AdminImagePath))
                {
                    // Получаем путь к директории проекта
                    string projectDirectory = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\");
                    // Обработка относительного пути к изображению
                    string absoluteImagePath = System.IO.Path.Combine(projectDirectory, _currentAdmins.AdminImagePath);

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
                    _currentAdmins.AdminImagePath = relativeImagePath;
                }
                else
                {
                    // Выводим сообщение об ошибке, если файл не был скопирован
                    MessageBox.Show("Ошибка при копировании файла.");
                }
            }

            // Загружаем изображение независимо от результата копирования
            profileImage.Source = new BitmapImage(new Uri(_currentAdmins.AdminImagePath, UriKind.RelativeOrAbsolute));
            sportEntities.SaveChanges();
            MessageBox.Show("Изменения успешно применены.");
            ClassChangePage.frame1.Navigate(new TEST());
        }
    }
}
