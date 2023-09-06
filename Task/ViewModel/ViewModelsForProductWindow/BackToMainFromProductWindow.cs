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

namespace Task.ViewModel.ViewModelsForProductWindow
{
    public class BackToMainFromProductWindow : INotifyPropertyChanged
    {

        // Событие, используемое для уведомления об изменениях в свойствах ViewModel
        public event PropertyChangedEventHandler PropertyChanged;

        // Команды для управления клиентами
        public ICommand BackToMainFromProductWindowCommand { get; private set; }

        public BackToMainFromProductWindow()
        {
            BackToMainFromProductWindowCommand = new RelayCommand(BackToMainFromProduct);
        }
        private void BackToMainFromProduct()
        {
            ManagerWindow managerWindow = new ManagerWindow();
            managerWindow.Show();
            Application.Current.Windows.OfType<ProductDataProcessing>().FirstOrDefault()?.Close();
        }

    }
}
