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

namespace Task.ViewModel.ViewModelsForClientWindow
{
    public class ForClientViewModel : INotifyPropertyChanged
    {
        // Событие, используемое для уведомления об изменениях в свойствах ViewModel
        public event PropertyChangedEventHandler PropertyChanged;

        // Команды для управления клиентами
        public ICommand AddClientCommand { get; private set; }
        public ICommand DeleteClientCommand { get; private set; }
        public ICommand UpdateClientDataCommand { get; private set; }

        // Список менеджеров
        private ObservableCollection<Manager> newManagers;
        public ObservableCollection<Manager> NewManagers
        {
            get { return newManagers; }
            set
            {
                if (newManagers != value)
                {
                    newManagers = value;
                    OnPropertyChanged(nameof(NewManagers));
                }
            }
        }


        private int newManagerId;
        public int NewManagerId
        {
            get { return newManagerId; }
            set
            {
                if (newManagerId != value)
                {
                    newManagerId = value;
                    OnPropertyChanged(nameof(NewManagerId));
                }
            }
        }

        private ObservableCollection<ClientStatus> newClientStatus;
        public ObservableCollection<ClientStatus> NewClientStatus
        {
            get { return newClientStatus; }
            set
            {
                if (newClientStatus != value)
                {
                    newClientStatus = value;
                    OnPropertyChanged(nameof(NewClientStatus));
                }
            }
        }
        private int newClientStatusId;
        public int NewClientStatusId
        {
            get { return newClientStatusId; }
            set
            {
                if (newClientStatusId != value)
                {
                    newClientStatusId = value;
                    OnPropertyChanged(nameof(NewClientStatusId));
                }
            }
        }

        private ObservableCollection<Client> clientsForDataGrid;
        public ObservableCollection<Client> ClientsForDataGrid
        {
            get { return clientsForDataGrid; }
            set
            {
                if (clientsForDataGrid != value)
                {
                    clientsForDataGrid = value;
                    OnPropertyChanged(nameof(ClientsForDataGrid));
                }
            }
        }
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
        private ObservableCollection<Manager> managers;
        public ObservableCollection<Manager> Managers
        {
            get { return managers; }
            set
            {
                if (managers != value)
                {
                    managers = value;
                    OnPropertyChanged(nameof(Managers));
                }
            }
        }
        private ObservableCollection<ClientStatus> clientStatusForDataGrid;
        public ObservableCollection<ClientStatus> ClientStatusForDataGrid
        {
            get { return clientStatusForDataGrid; }
            set
            {
                if (clientStatusForDataGrid != value)
                {
                    clientStatusForDataGrid = value;
                    OnPropertyChanged(nameof(ClientStatusForDataGrid));
                }
            }
        }


        // Для ввода имени нового клиента
        private string newClientName;
        public string NewClientName
        {
            get { return newClientName; }
            set
            {
                if (newClientName != value)
                {
                    newClientName = value;
                    OnPropertyChanged(nameof(NewClientName));
                }
            }
        }

        // Для ввода менеджера нового клиента
        private string newClientManager;
        public string NewClientManager
        {
            get { return newClientManager; }
            set
            {
                if (newClientManager != value)
                {
                    newClientManager = value;
                    OnPropertyChanged(nameof(NewClientManager));
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
                }
            }
        }

        // Идентификатор клиента для поиска и обновления данных
        private string inputClientId;
        public string InputClientId
        {
            get { return inputClientId; }
            set
            {
                inputClientId = value;
                OnPropertyChanged(nameof(InputClientId));
                UpdateClientName();

            }
        }

        // Информация для поля с именем клиента для возможности редактирования
        private string clientName;
        public string ClientName
        {
            get { return clientName; }
            set
            {
                clientName = value;
                OnPropertyChanged(nameof(ClientName));

            }
        }

        // Информация для поля со статусом клиента для возможности редактирования
        private ClientStatus selectedClientStatus;
        public ClientStatus SelectedClientStatus
        {
            get { return selectedClientStatus; }
            set
            {
                if (selectedClientStatus != value)
                {
                    selectedClientStatus = value;
                    OnPropertyChanged(nameof(SelectedClientStatus));
                }
            }
        }

        // Информация для поля с менеджером клиента для возможности редактирования
        private Manager selectedManager;
        public Manager SelectedManager
        {
            get { return selectedManager; }
            set
            {
                if (selectedManager != value)
                {
                    selectedManager = value;
                    OnPropertyChanged(nameof(SelectedManager));
                }
            }
        }

        // Конструктор класса
        public ForClientViewModel()
        {
            using (var context = new TaskEntities())
            {
                NewClientStatus = new ObservableCollection<ClientStatus>(context.ClientStatus.ToList());
                NewManagers = new ObservableCollection<Manager>(context.Manager.ToList());
            }

            // Загрузка клиентов
            LoadClients();

            // Инициализация команд
            AddClientCommand = new RelayCommand(AddNewClient);
            DeleteClientCommand = new RelayCommand(DeleteClient, CanDeleteClient);
            UpdateClientDataCommand = new RelayCommand(UpdateClientData);
        }

        // Метод для загрузки списка клиентов из базы данных
        private void LoadClients()
        {
            using (var context = new TaskEntities())
            {
                ClientsForDataGrid = new ObservableCollection<Client>(context.Client.ToList());
                ClientStatusForDataGrid = new ObservableCollection<ClientStatus>(context.ClientStatus.ToList());
                Managers = new ObservableCollection<Manager>(context.Manager.ToList());
                Clients = new ObservableCollection<Client>(context.Client.ToList());

            }

        }

        // Добавление нового клиента
        private void AddNewClient()
        {
            if (string.IsNullOrWhiteSpace(NewClientName))
            {
                MessageBox.Show("Для добавления клиента в систему необходимо указать имя и статус клиента.");
                return;
            }

            else
            {
                MessageBoxResult result = MessageBox.Show("Сохранить нового клиента?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    using (var context = new TaskEntities())
                    {
                        Client newClient = new Client
                        {
                            name = NewClientName,
                            idClientStatus = NewClientStatusId,
                            idManager = NewManagerId

                        };

                        context.Client.Add(newClient);
                        context.SaveChanges();
                        MessageBox.Show("Новый клиент успешно добавлен");
                        LoadClients();

                    }

                }

                else
                {
                    NewClientName = string.Empty;
                }
            }

            NewClientName = string.Empty;
        }

        // Удаление клиента
        private void DeleteClient()
        {
            if (SelectedClient == null)
            {
                MessageBox.Show("Выберите клиента для удаления.");
                return;
            }

            MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите удалить выбранного клиента?", "Подтверждение удаления", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                using (var context = new TaskEntities())
                {
                    var clientToDelete = context.Client.FirstOrDefault(c => c.id == SelectedClient.id);
                    if (clientToDelete != null)
                    {
                        context.Client.Remove(clientToDelete);
                        context.SaveChanges();
                        Clients.Remove(SelectedClient);
                        LoadClients();
                    }
                }
            }
        }

        // Обновление данных о клиенте
        private void UpdateClientData()
        {
            int targetClientId = Convert.ToInt32(InputClientId);

            if (!string.IsNullOrEmpty(ClientName) && SelectedClientStatus != null && SelectedManager != null && !string.IsNullOrEmpty(InputClientId))
            {
                using (var context = new TaskEntities())
                {
                    var clientToUpdate = context.Client.FirstOrDefault(c => c.id == targetClientId);

                    if (clientToUpdate != null)
                    {
                        // Процедура изменения данных
                        clientToUpdate.name = ClientName;
                        clientToUpdate.idClientStatus = SelectedClientStatus.id; // Используйте реальные свойства для выбранных объектов
                        clientToUpdate.idManager = SelectedManager.id; // Используйте реальные свойства для выбранных объектов
                        context.SaveChanges();
                        MessageBox.Show("Вы успешно внесли изменения");

                        // Пустые строки после выполнения операции
                        InputClientId = string.Empty;
                        ClientName = string.Empty;
                        SelectedClientStatus = null;
                        SelectedManager = null;
                        LoadClients();
                    }
                }
            }
            else
            {
                MessageBox.Show("Проверьте правильно ли заполнены поля ввода номера клиента, его имени и статуса");
            }

        }

        // Предоставление данных клиента по введёному ID
        private void UpdateClientName()
        {
            if (!string.IsNullOrEmpty(InputClientId))
            {
                using (var context = new TaskEntities())
                {
                    Clients = new ObservableCollection<Client>(context.Client.ToList());
                }
                foreach (var client in Clients)
                {
                    if (client.id == Convert.ToInt32(InputClientId))
                    {
                        // Заполнение полей данными для их изменений
                        ClientName = client.name;
                        SelectedClientStatus = NewClientStatus.FirstOrDefault(status => status.id == client.idClientStatus);
                        SelectedManager = NewManagers.FirstOrDefault(manager => manager.id == client.idManager);

                        break;
                    }
                }
            }

            if (string.IsNullOrEmpty(InputClientId))
            {
                ClientName = string.Empty;
                SelectedClientStatus = null;
                SelectedManager = null;
            }

        }

        // Метод проверки, можно ли удалить клиента
        private bool CanDeleteClient()
        {
            return SelectedClient != null;
        }

        // Метод для уведомления об изменении свойства
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
