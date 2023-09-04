using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task.Models;

namespace Task.ViewModel.ViewModelsForClientWindow
{
    public class SortClientsByStatusViewModel : INotifyPropertyChanged
    {
        // Событие, используемое для уведомления об изменениях в свойствах ViewModel
        public event PropertyChangedEventHandler PropertyChanged;

        // Список статусов клиентов
        private ObservableCollection<ClientStatus> clientStatus;
        public ObservableCollection<ClientStatus> ClientStatus
        {
            get { return clientStatus; }
            set
            {
                if (clientStatus != value)
                {
                    clientStatus = value;
                    OnPropertyChanged(nameof(ClientStatus));
                }
            }
        }

        // Список клиентов
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

        // Выбранный статус
        private ClientStatus selectedStatus;
        public ClientStatus SelectedStatus
        {
            get { return selectedStatus; }
            set
            {
                if (selectedStatus != value)
                {
                    selectedStatus = value;
                    OnPropertyChanged(nameof(SelectedStatus));

                    FilterClientsByStatus();
                }
            }
        }

        // Метод для фильтрации клиентов по статусу
        private void FilterClientsByStatus()
        {
            if (SelectedStatus != null)
            {
                using (var context = new TaskEntities())
                {
                    // Получение клиентов с выбранным статусом
                    Clients = new ObservableCollection<Client>(
                        context.Client.Where(client => client.idClientStatus == SelectedStatus.id)
                    );
                }
            }
        }

        // Конструктор класса
        public SortClientsByStatusViewModel()
        {
            // Загрузка списка статусов клиентов из базы данных
            using (var context = new TaskEntities())
            {
                ClientStatus = new ObservableCollection<ClientStatus>(context.ClientStatus.ToList());
            }

        }

        // Метод для уведомления об изменении свойства
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}

