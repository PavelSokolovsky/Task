using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task.Models;

namespace Task.ViewModel.ViewModelsForManagerWindow
{
    internal class SortClientsByManagerViewModel : INotifyPropertyChanged
    {
        // Событие, используемое для уведомления об изменениях в свойствах ViewModel
        public event PropertyChangedEventHandler PropertyChanged;

        // Список менеджеров для отображения в пользовательском интерфейсе
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

        // Выбранный менеджер для фильтрации клиентов
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

                    // При выборе нового менеджера выполняем фильтрацию клиентов
                    FilterClientsByManager();
                }
            }
        }

        // Метод для фильтрации клиентов по выбранному менеджеру
        private void FilterClientsByManager()
        {
            if (SelectedManager != null)
            {
                using (var context = new TaskEntities())
                {
                    Clients = new ObservableCollection<Client>(context.Client.Where(client => client.idManager == SelectedManager.id));
                }
            }
        }

        // Конструктор класса
        public SortClientsByManagerViewModel()
        {
            // Загружаем список менеджеров из базы данных
            using (var context = new TaskEntities())
            {
                Managers = new ObservableCollection<Manager>(context.Manager.ToList());
            }

        }

        // Метод для уведомления об изменении свойства
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
