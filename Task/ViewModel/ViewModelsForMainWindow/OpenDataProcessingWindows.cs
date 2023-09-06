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

namespace Task.ViewModel.ViewModelsForMainWindow
{
    public class OpenDataProcessingWindows : INotifyPropertyChanged
    {


        // Событие, используемое для уведомления об изменениях в свойствах ViewModel
        public event PropertyChangedEventHandler PropertyChanged;

        // Команды для управления клиентами
        public ICommand OpenClientWindowCommand { get; private set; }
        public ICommand OpenManagerWindowCommand { get; private set; }
        public ICommand OpenProductWindowCommand { get; private set; }

        public OpenDataProcessingWindows()
        {

            OpenClientWindowCommand = new RelayCommand(OpenClient);
            OpenManagerWindowCommand = new RelayCommand(OpenManager);
            OpenProductWindowCommand = new RelayCommand(OpenProduct);


        }

        private void OpenManager()
        {
            ManagerDataProcessing managerDataProcessing = new ManagerDataProcessing();
            managerDataProcessing.Show();
            Application.Current.Windows.OfType<ManagerWindow>().FirstOrDefault()?.Close();
        }

        private void OpenClient()
        {

            ClientDataProcessing clientDataProcessing = new ClientDataProcessing();
            clientDataProcessing.Show();
            Application.Current.Windows.OfType<ManagerWindow>().FirstOrDefault()?.Close();

        }
        private void OpenProduct()
        {


            ProductDataProcessing productDataProcessing = new ProductDataProcessing();
            productDataProcessing.Show();
            Application.Current.Windows.OfType<ManagerWindow>().FirstOrDefault()?.Close();
        }
    }
}
