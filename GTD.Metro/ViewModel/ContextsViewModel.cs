using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mono.App.GTD.Model;

namespace Mono.App.GTD.ViewModel
{
    public class ContextsViewModel : CollectionViewModelBase<Context>
    {
        public ContextsViewModel(ObservableCollection<Context> collections)
            : base(collections, null)
        {
        }
    }
}