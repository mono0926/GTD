using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mono.App.GTD.Service;

namespace Mono.App.GTD.Model
{
    public class Project : ProjectBase
    {
        private static int idSeed;

        public static Project CreateNew(string title)
        {
            var id = ++idSeed;
            return new Project(id, title);
        }

        public static Project RestoreFromDB(int id, string title)
        {
            if (id > idSeed)
            {
                idSeed = id;
            }
            return new Project(id, title);
        }

        private Project(int id, string title)
        {
            this.Id = id;
            this.Title = title;
        }

        public override string Title
        {
            get;
            set;
        }

        //public sealed override void RemoveItem(TodoItem item)
        //{
        //    base.RemoveItem(item);
        //    item.Project = DataService.Inbox;
        //}
    }
}