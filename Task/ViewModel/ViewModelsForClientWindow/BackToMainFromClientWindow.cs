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

namespace Task.ViewModel.ViewModelsForClientWindow
{
    public class BackToMainFromClientWindow : INotifyPropertyChanged
    {
        

        // Событие, используемое для уведомления об изменениях в свойствах ViewModel
        public event PropertyChangedEventHandler PropertyChanged;

        // Команды для управления клиентами
        public ICommand BackToMainFromClientWindowCommand { get; private set; }

        public BackToMainFromClientWindow()
        {

          BackToMainFromClientWindowCommand = new RelayCommand(BackToMainFromClient);


        }

        private void BackToMainFromClient()
        {

            ManagerWindow managerWindow = new ManagerWindow();
            managerWindow.Show();
            Application.Current.Windows.OfType<ClientDataProcessing>().FirstOrDefault()?.Close();

        }

    }
   
}
