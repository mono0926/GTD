using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mono.App.GTD.Model;

namespace Mono.App.GTD.ViewModel
{
    public abstract class CollectionViewModelBase<T> where T : ItemCollectionBase
    {
        public CollectionViewModelBase(ObservableCollection<T> collections, T current)
        {
            this.Current = current;
            this.Collections = collections;
        }

        public T Current { get; set; }

        public ObservableCollection<T> Collections { get; set; }
    }
}