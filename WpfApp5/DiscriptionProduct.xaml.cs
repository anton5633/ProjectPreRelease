using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Логика взаимодействия для DiscriptionProduct.xaml
    /// </summary>
    /// 


    public partial class DiscriptionProduct : Page
    {
        private int productId;
        private ProductCards _currentUser;
        private SportEntities sportEntities;

        public DiscriptionProduct(int productId)
        {
            InitializeComponent();
            this.productId = productId;
            LoadProductData();

            if (GlobalVar.EditorReg)
            {
                Update.Visibility = Visibility.Visible;
            }

            else { Update.Visibility = Visibility.Collapsed; }
        }

        private void LoadProductData()
        {
            using (var context = new SportEntities())
            {
                // Загрузка данных о товаре по его Id
                _currentUser = context.ProductCards.FirstOrDefault(p => p.ProductID == productId);

                // Присвоение загруженных данных элементам управления на странице
                if (_currentUser != null)
                {
                    ProductNameBox.Text = _currentUser.ProductName;
                    DiscriptionBox.Text = _currentUser.Discription;
                    PriceBox.Text = _currentUser.Price.ToString();
                    if (!string.IsNullOrEmpty(_currentUser.ProductImagePath))
                    {
                        // Получаем путь к директории проекта
                        string projectDirectory = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\");
                        // Обработка относительного пути к изображению
                        string absoluteImagePath = System.IO.Path.Combine(projectDirectory, _currentUser.ProductImagePath);

                        try
                        {
                            // Создать BitmapImage и установить источник изображения
                            BitmapImage bitmap = new BitmapImage(new Uri(absoluteImagePath));
                            ProductImageImage.Source = bitmap;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Ошибка при загрузке изображения: " + ex.Message);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Товар не найден.");
                }
            }
        }

        private void ChangePhoto_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image files (*.jpg, *.jpeg, *.png) | *.jpg; *.jpeg; *.png"
            };

            using (var context = new SportEntities())
            {
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
                        _currentUser.ProductImagePath = relativeImagePath;

                        // Сохраняем изменения в базе данных
                        context.SaveChanges();
                    }
                    else
                    {
                        // Выводим сообщение об ошибке, если файл не был скопирован
                        MessageBox.Show("Ошибка при копировании файла.");
                    }
                }

                // Загружаем изображение независимо от результата копирования
                ProductImageImage.Source = new BitmapImage(new Uri(_currentUser.ProductImagePath, UriKind.RelativeOrAbsolute));

                MessageBox.Show("Изменения успешно применены.");
                ClassChangePage.frame1.Navigate(new Main());
            }
        }

        private void AlterBack_Click(object sender, RoutedEventArgs e)
        {
            ClassChangePage.frame1.Navigate(new Main());
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            DiscriptionBox1.Visibility = Visibility.Visible;
            PriceBox1.Visibility = Visibility.Visible;
            ProductNameBox1.Visibility = Visibility.Visible;
            label1.Visibility = Visibility.Visible;
            label2.Visibility = Visibility.Visible;
            label3.Visibility = Visibility.Visible;
            ChangePhoto.Visibility = Visibility.Visible;
        }
    }
}
