using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mono.App.GTD.Model
{
    public abstract class ProjectBase : ItemCollectionBase
    {
        public override void AddRangeItem(IEnumerable<TodoItem> items)
        {
            foreach (var i in items)
            {
                AddItem(i);
            }
        }

        public sealed override void AddItem(TodoItem item)
        {
            base.AddItem(item);
            item.Project = this;
        }

        public override void RemoveItem(TodoItem item)
        {
            base.RemoveItem(item);
        }
    }
}