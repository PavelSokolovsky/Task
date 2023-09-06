using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using Task.Helpers;
using Task.Models;

namespace Task.ViewModel.ViewModelsForManagerWindow
{
    public class ForManagerViewModel : BaseViewModel<Manager>, INotifyPropertyChanged
    {
        // Событие, используемое для уведомления об изменениях в свойствах ViewModel
        public event PropertyChangedEventHandler PropertyChanged;
        
        // Команды для добавления, удаления и обновления менеджеров
        public ICommand AddManagerCommand { get; private set; }
        public ICommand DeleteManagerCommand { get; private set; }
        public ICommand UpdateNameCommand { get; private set; }

        // Имя нового менеджера, для добавления
        private string newManagerName;
        public string NewManagerName
        {
            get { return newManagerName; }
            set
            {
                if (newManagerName != value)
                {
                    newManagerName = value;
                    OnPropertyChanged(nameof(NewManagerName));
                }
            }
        }

        // Выбранный менеджер
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

        // ID менеджера, введенное пользователем для поиска
        private string inputId;
        public string InputId
        {
            get { return inputId; }
            set
            {
                inputId = value;
                OnPropertyChanged(nameof(InputId));
                UpdateName();

            }
        }

        // Имя менеджера, отображаемое в пользовательском интерфейсе
        private string managerName;
        public string ManagerName
        {
            get { return managerName; }
            set
            {
                managerName = value;
                OnPropertyChanged(nameof(ManagerName));

            }
        }

        // Конструктор ViewModel
        public ForManagerViewModel()
        {
            // Загрузка списка менеджеров из базы данных
            LoadDataAsync();

            AddManagerCommand = new RelayCommand(AddManager);
            DeleteManagerCommand = new RelayCommand(DeleteManager, CanDeleteManager);
            UpdateNameCommand = new RelayCommand(UpdateNewName);
        }
       

        // Метод для добавления нового менеджера
        private void AddManager()
        {
            // Проверка наличия имени нового менеджера
            if (string.IsNullOrWhiteSpace(NewManagerName))
            {
                MessageBox.Show("Пожалуйста, введите имя менеджера.");
                return;
            }

            else
            {
                MessageBoxResult result = MessageBox.Show("Сохранить нового менеджера?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    // Добавление нового менеджера в базу данных
                    using (var context = new TaskEntities())
                    {
                        Manager newManager = new Manager
                        {
                            name = NewManagerName
                        };

                        context.Manager.Add(newManager);
                        context.SaveChanges();
                        MessageBox.Show("Новый менеджер успешно сохранён");
                        //LoadManagers();
                        

                    }
                    LoadDataAsync();
                }
                else
                {
                    NewManagerName = string.Empty;
                }
            }

            NewManagerName = string.Empty;
        }

        // Метод для удаления выбранного менеджера
        private void DeleteManager()
        {
            if (SelectedManager == null)
            {
                MessageBox.Show("Выберите менеджера для удаления.");
                return;
            }

            using (var context = new TaskEntities())
            {
                // Проверяем, есть ли у менеджера связанные клиенты
                var managerHasClients = context.Client.Any(c => c.idManager == SelectedManager.id);

                if (managerHasClients)
                {
                    MessageBox.Show("Нельзя удалить менеджера, у которого есть активные клиенты клиенты.");
                    return; // Предотвращаем удаление
                }

                MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите удалить выбранного менеджера?", "Подтверждение удаления", MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                {
                    var managerToDelete = context.Manager.FirstOrDefault(m => m.id == SelectedManager.id);
                    if (managerToDelete != null)
                    {
                        context.Manager.Remove(managerToDelete);
                        context.SaveChanges();
                        Items.Remove(SelectedManager);
                        LoadDataAsync();
                    }
                }
            }
        }

        // Метод для обновления имени менеджера
        private void UpdateNewName()
        {
            int targetId = Convert.ToInt32(InputId);

            if (!string.IsNullOrEmpty(ManagerName))
            {
                using (var context = new TaskEntities())
                {
                    var managerToUpdate = context.Manager.FirstOrDefault(m => m.id == targetId);

                    if (managerToUpdate != null)
                    {
                        managerToUpdate.name = ManagerName;
                        context.SaveChanges();
                        MessageBox.Show("Вы успешно внесли изменения");
                        LoadDataAsync();
                        InputId = string.Empty;
                        ManagerName = string.Empty;
                    }
                }
            }

        }

        // Метод для предоставления данных менеджера по введёному ID
        private void UpdateName()
            {
                if (!string.IsNullOrEmpty(InputId))
                {
                    int id = Convert.ToInt32(InputId);
                    var manager = Items.FirstOrDefault(m => m.id == id);
                    if (manager != null)
                    {
                        ManagerName = manager.name;
                    }
                    else
                    {
                        ManagerName = string.Empty;
                    }
                }
                else
                {
                    ManagerName = string.Empty;
                }

        }

        // Метод для проверки, можно ли удалять менеджера
        private bool CanDeleteManager()
        {
            return SelectedManager != null;
        }

        // Метод для уведомления об изменении свойства
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
