using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mono.App.GTD.Model
{
    public class Context : ItemCollectionBase
    {
        private static int idSeed;

        public static Context CreateNew(string title)
        {
            var id = ++idSeed;
            return new Context(id, title);
        }

        public static Context RestoreFromDB(int id, string title)
        {
            if (id > idSeed)
            {
                idSeed = id;
            }
            return new Context(id, title);
        }

        private Context(int id, string title)
        {
            this.Id = id;
            this.Title = title;
        }

        public override string Title
        {
            get;
            set;
        }

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
            if (item.Contexts.Contains(this))
            {
                return;
            }
            item.Contexts.Add(this);
        }

        public override void RemoveItem(TodoItem item)
        {
            base.RemoveItem(item);
            if (item.Contexts.Contains(this))
            {
                item.Contexts.Remove(this);
            }
        }
    }
}