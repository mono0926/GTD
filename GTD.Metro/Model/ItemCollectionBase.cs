using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mono.App.GTD.Model
{
    public abstract class ItemCollectionBase
    {
        public ItemCollectionBase()
        {
            this.Items = new ObservableCollection<TodoItem>();
        }

        public int Id { get; set; }

        public abstract string Title { get; set; }

        public string Description { get; set; }

        public ObservableCollection<TodoItem> Items { get; private set; }

        public abstract void AddRangeItem(IEnumerable<TodoItem> items);

        public virtual void AddItem(TodoItem item)
        {
            if (this.Items.Contains(item))
            {
                return;
            }
            this.Items.Add(item);
        }

        public virtual void RemoveItem(TodoItem item)
        {
            if (this.Items.Contains(item))
            {
                this.Items.Remove(item);
            }
        }

        public int Count { get { return this.Items.Count; } }
    }
}