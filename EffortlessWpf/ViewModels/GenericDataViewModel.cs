using Caliburn.Micro;
using EffortlessStdLibrary.DataAccess;
using EffortlessStdLibrary.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EffortlessWpf.ViewModels
{
    public class GenericDataViewModel<T> : Screen where T : IEffortlessModel
    {
        //private readonly GenericDataAccess<T> _dataAccess;
        //public GenericDataAccess<T> DataAccess { get { return _dataAccess} }
        private GenericDataAccess<T> _dataAccess;

        public GenericDataAccess<T> DataAccess
        {
            get { return _dataAccess; }
            private set { _dataAccess = value; }
        }

        public BindableCollection<T> DataItems { get; set; }
        public T SelectedItem { get; set; }

        public GenericDataViewModel(string apiUrl) => DataAccess = new GenericDataAccess<T>(apiUrl);

        protected override async void OnInitialize()
        {
            await LoadItemsAsync();
        }

        public async Task LoadItemsAsync()
        {
            DataItems = new BindableCollection<T>(await _dataAccess.GetAllAsync());
            DataItems.CollectionChanged += HandleItemChange;

            NotifyOfPropertyChange(() => DataItems);
        }

        private async void HandleItemChange(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (var item in e.OldItems)
                {
                    await _dataAccess.DeleteAsync((T)item);

                    await LoadItemsAsync();
                }
            }
        }
    }
}
