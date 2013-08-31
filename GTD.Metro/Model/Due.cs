using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mono.App.GTD.Common;

namespace Mono.App.GTD.Model
{
    public class Due : ItemCollectionBase
    {
        public Due(DateTime startDate, DateTime endDate, string title)
        {
            this.StartDate = startDate;
            this.EndDate = endDate;
            this.Title = title;
        }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

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
    }
}