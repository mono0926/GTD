using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mono.App.GTD.Common;

namespace Mono.App.GTD.Model
{
    public class MenuItem : BindableBase
    {
        public ItemType Type { get; set; }

        public string Name { get { return Type.ConvertToString(); } }

        private int count;

        public int Count
        {
            get { return count; }
            set
            {
                count = value;
                OnPropertyChanged();
            }
        }
    }

    public enum ItemType
    {
        Inbox,
        Project,
    }

    public static class ItemTypeExtension
    {
        public static string ConvertToString(this ItemType item)
        {
            switch (item)
            {
                case ItemType.Inbox:
                case ItemType.Project:
                    return item.ToString();
                default:
                    return string.Empty;
            }
        }
    }
}