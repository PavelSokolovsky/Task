using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using Task.Helpers;
using Task.Models;

namespace Task.ViewModel.ViewModelsForProductWindow
{
    internal class ForProductViewModel : INotifyPropertyChanged
    {
        // Событие, используемое для уведомления об изменениях в свойствах ViewModel
        public event PropertyChangedEventHandler PropertyChanged;

        // Команды для добавления, удаления и обновления менеджеров
        public ICommand AddProductCommand { get; private set; }
        public ICommand DeleteProductCommand { get; private set; }
        public ICommand UpdateProductDataCommand { get; private set; }

        // Список продуктов для отображения в пользовательском интерфейсе
        private ObservableCollection<Product> products;
        public ObservableCollection<Product> Products
        {
            get { return products; }
            set
            {
                if (products != value)
                {
                    products = value;
                    OnPropertyChanged(nameof(Products));
                }
            }
        }

        // Новое имя продукта для добавления
        private string newProductName;
        public string NewProductName
        {
            get { return newProductName; }
            set
            {
                if (newProductName != value)
                {
                    newProductName = value;
                    OnPropertyChanged(nameof(NewProductName));
                }
            }
        }

        // Новая цена продукта для добавления
        private string newProductPrice;
        public string NewProductPrice
        {
            get { return newProductPrice; }
            set
            {
                if (newProductPrice != value)
                {
                    newProductPrice = value;
                    OnPropertyChanged(nameof(NewProductPrice));
                }
            }
        }

        // Новый тип продукта для добавления
        private string newProductTypeOfEula;
        public string NewProductTypeOfEula
        {
            get { return newProductTypeOfEula; }
            set
            {
                if (newProductTypeOfEula != value)
                {
                    newProductTypeOfEula = value;
                    OnPropertyChanged(nameof(NewProductTypeOfEula));
                }
            }
        }

        // Новый период подписки продукта для добавления
        private string newProductSubscriptionPeriod;
        public string NewProductSubscriptionPeriod
        {
            get { return newProductSubscriptionPeriod; }
            set
            {
                if (newProductSubscriptionPeriod != value)
                {
                    newProductSubscriptionPeriod = value;
                    OnPropertyChanged(nameof(NewProductSubscriptionPeriod));
                }
            }
        }

        // Выбранный продукт для редактирования
        private Product selectedProduct;
        public Product SelectedProduct
        {
            get { return selectedProduct; }
            set
            {
                if (selectedProduct != value)
                {
                    selectedProduct = value;
                    OnPropertyChanged(nameof(SelectedProduct));
                }
            }
        }

        // Ввод идентификатора продукта для поиска
        private string inputProductId;
        public string InputProductId
        {
            get { return inputProductId; }
            set
            {
                inputProductId = value;
                OnPropertyChanged(nameof(InputProductId));
                UpdateProductName();

            }
        }

        // Имя продукта для отображения при поиске
        private string productName;
        public string ProductName
        {
            get { return productName; }
            set
            {
                productName = value;
                OnPropertyChanged(nameof(ProductName));

            }
        }

        // Цена продукта для отображения при поиске
        private string productPrice;
        public string ProductPrice
        {
            get { return productPrice; }
            set
            {
                productPrice = value;
                OnPropertyChanged(nameof(ProductPrice));

            }
        }

        // Тип EULA для отображения при поиске
        private string productTypeOfEula;
        public string ProductTypeOfEula
        {
            get { return productTypeOfEula; }
            set
            {
                productTypeOfEula = value;
                OnPropertyChanged(nameof(ProductTypeOfEula));

            }
        }

        // Период подписки продукта для отображения при поиске
        private string productSubscriptionPeriod;
        public string ProductSubscriptionPeriod
        {
            get { return productSubscriptionPeriod; }
            set
            {
                productSubscriptionPeriod = value;
                OnPropertyChanged(nameof(ProductSubscriptionPeriod));

            }
        }

        // Конструктор класса
        public ForProductViewModel()
        {
            // Загрузка списка продуктов из базы данных
            LoadProducts();

            // Инициализация команд для управления продуктами
            AddProductCommand = new RelayCommand(AddNewProduct);
            DeleteProductCommand = new RelayCommand(DeleteProduct, CanDeleteProduct);
            UpdateProductDataCommand = new RelayCommand(UpdateProductData);
        }

        // Загрузка списка продуктов из базы данных
        private void LoadProducts()
        {
            using (var context = new TaskEntities())
            {
                Products = new ObservableCollection<Product>(context.Product.ToList());
            }
        }

        // Добавление нового продукта
        private void AddNewProduct()
        {
            // Проверка наличия имени продукта в строке ввода для добавления
            if (string.IsNullOrWhiteSpace(NewProductName))
            {
                MessageBox.Show("Для добавления продукта в систему необходимо указать хотя бы имя добавляемого продукта");
                return;
            }

            // Проверка на корректность ввода цены продукта
            if (!decimal.TryParse(NewProductPrice, out decimal productPrice))
            {
                MessageBox.Show("Неверный формат цены");
                return;
            }

            MessageBoxResult result = MessageBox.Show("Сохранить новый продукт?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    using (var context = new TaskEntities())
                    {
                        // Создание нового продукта и сохранение его в базе данных
                        Product newProduct = new Product
                        {
                            name = NewProductName,
                            price = Convert.ToDecimal(newProductPrice),
                            type = NewProductTypeOfEula,
                            period = NewProductSubscriptionPeriod
                            
                        };

                        context.Product.Add(newProduct);
                        context.SaveChanges();
                        MessageBox.Show("Новый продукт успешно добавлен");
                        LoadProducts();

                    }

                }
                else
                {
                    NewProductName = string.Empty;
                }

            NewProductName = string.Empty;
        }

        // Удаление выбранного продукта
        private void DeleteProduct()
        {
            if (SelectedProduct == null)
            {
                MessageBox.Show("Выберите продукт для удаления.");
                return;
            }

            MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите удалить выбранный продукт?", "Подтверждение удаления", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                using (var context = new TaskEntities())
                {
                    // Удаление продукта из базы данных
                    var productToDelete = context.Product.FirstOrDefault(p => p.id == SelectedProduct.id);
                    if (productToDelete != null)
                    {
                        context.Product.Remove(productToDelete);
                        context.SaveChanges();
                        Products.Remove(SelectedProduct);
                        LoadProducts();
                    }
                }
            }
        }

        // Обновление данных выбранного продукта
        private void UpdateProductData()
        {
            int targetProductId = Convert.ToInt32(InputProductId);

            if (!string.IsNullOrEmpty(ProductName) && !string.IsNullOrEmpty(ProductPrice))
            {
                using (var context = new TaskEntities())
                {
                    var productToUpdate = context.Product.FirstOrDefault(p => p.id == targetProductId);

                    if (productToUpdate != null)
                    {
                        // Обновление данных продукта и сохранение изменений
                        productToUpdate.name = ProductName;
                        productToUpdate.price = Convert.ToDecimal(ProductPrice);
                        productToUpdate.type = ProductTypeOfEula;
                        productToUpdate.period = ProductSubscriptionPeriod;
                        context.SaveChanges();
                        MessageBox.Show("Вы успешно внесли изменения");

                        // Очищаем полей после обновления и обновление списка
                        InputProductId = string.Empty;
                        ProductName = string.Empty;
                        ProductPrice = string.Empty;
                        ProductTypeOfEula = string.Empty;
                        ProductSubscriptionPeriod = string.Empty;

                        LoadProducts();
                    }
                }
            }

            else MessageBox.Show("Заполните поля с именем и ценой");

        }

        // Предоставление данных продукта при выборе его ID
        private void UpdateProductName()
        {
            if (!string.IsNullOrEmpty(InputProductId))
            {
                using (var context = new TaskEntities())
                {
                    Products = new ObservableCollection<Product>(context.Product.ToList());
                }
                foreach (var product in Products)
                {
                    if (product.id == Convert.ToInt32(InputProductId))
                    {
                        // Заполнение полей данными выбранного продукта
                        ProductName = product.name;
                        ProductPrice = Convert.ToString(product.price);
                        ProductTypeOfEula = product.type;
                        ProductSubscriptionPeriod = product.period;
                        break;
                    }
                }
            }
            if (string.IsNullOrEmpty(InputProductId))
            {
                ProductName = string.Empty;
                ProductPrice = string.Empty;
                ProductTypeOfEula = string.Empty;
                ProductSubscriptionPeriod = string.Empty;
            }

        }

        // Проверка возможности удаления продукта
        private bool CanDeleteProduct()
        {
            return SelectedProduct != null;
        }

        // Метод для уведомления об изменении свойства
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

