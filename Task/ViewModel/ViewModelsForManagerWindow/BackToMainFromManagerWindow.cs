using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Task.Helpers;
using Task.Views.Windows;

namespace Task.ViewModel.ViewModelsForManagerWindow
{
    public class BackToMainFromManagerWindow : INotifyPropertyChanged
    {

        // Событие, используемое для уведомления об изменениях в свойствах ViewModel
        public event PropertyChangedEventHandler PropertyChanged;

        // Команды для управления клиентами
        public ICommand BackToMainFromManagerWindowCommand { get; private set; }

        public BackToMainFromManagerWindow() // Передача текущего окна в конструктор
        {
            BackToMainFromManagerWindowCommand = new RelayCommand(BackToMainFromManager);
        }

        private void BackToMainFromManager()
        {

            ManagerWindow managerWindow = new ManagerWindow();
            managerWindow.Show();
            Application.Current.Windows.OfType<ManagerDataProcessing>().FirstOrDefault()?.Close();

        }
    }
}
