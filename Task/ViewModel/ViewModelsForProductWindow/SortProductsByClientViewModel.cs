using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task.Models;

namespace Task.ViewModel.ViewModelsForProductWindow
{
    internal class SortProductsByClientViewModel : INotifyPropertyChanged
    {
        // Событие, используемое для уведомления об изменениях в свойствах ViewModel
        public event PropertyChangedEventHandler PropertyChanged;

        // Список клиентов для отображения в пользовательском интерфейсе
        private ObservableCollection<Client> clients;
        public ObservableCollection<Client> Clients
        {
            get { return clients; }
            set
            {
                if (clients != value)
                {
                    clients = value;
                    OnPropertyChanged(nameof(Clients));
                }
            }
        }

        // Список продуктов для отображения в пользовательском интерфейсе
        private ObservableCollection<Product> productsByClient;
        public ObservableCollection<Product> ProductsByClient
        {
            get { return productsByClient; }
            set
            {
                if (productsByClient != value)
                {
                    productsByClient = value;
                    OnPropertyChanged(nameof(ProductsByClient));
                }
            }
        }

        // Выбранный клиент
        private Client selectedClient;
        public Client SelectedClient
        {
            get { return selectedClient; }
            set
            {
                if (selectedClient != value)
                {
                    selectedClient = value;
                    OnPropertyChanged(nameof(SelectedClient));

                    // Загрузка продуктов, приобретенных выбранным клиентом
                    LoadProductsByClient(selectedClient?.id);
                }
            }
        }

        // Конструктор класса
        public SortProductsByClientViewModel()
        {
            // Загрузка списка клиентов из базы данных
            using (var context = new TaskEntities())
            {
                Clients = new ObservableCollection<Client>(context.Client.ToList());
            }
        }

        // Метод для загрузки товаров, приобретенных выбранным клиентом
        private void LoadProductsByClient(int? clientId)
        {
            if (clientId.HasValue)
            {
                using (var context = new TaskEntities())
                {
                    ProductsByClient = new ObservableCollection<Product>(

                             // Получение списка продуктов, приобретенных клиентом
                             context.ProductsPurchasedByClients
                            .Where(p => p.idClient == clientId)
                            .Select(p => p.Product)
                            .ToList());
                }
            }

            else
            {
                ProductsByClient = new ObservableCollection<Product>();
            }
        }

        // Метод для уведомления об изменении свойства
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

