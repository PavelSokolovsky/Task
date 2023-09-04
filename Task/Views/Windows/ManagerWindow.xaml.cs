using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Task.Views.Windows
{
    /// <summary>
    /// Логика взаимодействия для ManagerWindow.xaml
    /// </summary>
    public partial class ManagerWindow : Window
    {
        public ManagerWindow()
        {
            InitializeComponent();
        }

        private void ClientBtn_Click(object sender, RoutedEventArgs e)
        {
            ClientDataProcessing clientDataProcessing = new ClientDataProcessing();
            clientDataProcessing.Show();
            this.Close();
        }

        private void ManagerBtn_Click(object sender, RoutedEventArgs e)
        {
            ManagerDataProcessing managerDataProcessing = new ManagerDataProcessing();
            managerDataProcessing.Show();
            this.Close();
        }

        private void ProductBtn_Click(object sender, RoutedEventArgs e)
        {
            ProductDataProcessing productDataProcessing = new ProductDataProcessing();
            productDataProcessing.Show();
            this.Close();
        }
    }
}
