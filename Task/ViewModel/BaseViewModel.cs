using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task.Models;

namespace Task.ViewModel
{
    public  class BaseViewModel<T> where T : class
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private ObservableCollection<T> items;

        public ObservableCollection<T> Items
        {
            get { return items; }
            set
            {
                if (items != value)
                {
                    items = value;
                    OnPropertyChanged(nameof(Items));
                }
            }
        }

        // Конструктор базового класса
        protected BaseViewModel()
        {
            Items = new ObservableCollection<T>();
        }

        // Метод для получения коллекции из базы данных (здесь вы можете реализовать вашу логику получения данных)
        public async void LoadDataAsync()
        {
            try
            {
                using (var context = new TaskEntities())
                {
                    IQueryable<T> query = context.Set<T>();

                    if (typeof(T) == typeof(Client))
                    {
                        query = query.Include("Manager").Include("ClientStatus");
                    }

                    var data = await query.ToListAsync();

                    Items.Clear();
                    foreach (var item in data)
                    {
                        Items.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                // Обработка ошибок при получении данных
                Console.WriteLine($"Ошибка при получении данных: {ex.Message}");
            }
        }

        // Метод для оповещения об изменении свойств
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
