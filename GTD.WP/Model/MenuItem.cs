using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using GalaSoft.MvvmLight;

namespace Mono.App.GTD.Model
{
    public class MenuItem : ViewModelBase
    {
        public MenuItemType Type { get; set; }

        public string Name { get { return Type.ConvertToString(); } }

        private int count;

        public int Count
        {
            get { return count; }
            set
            {
                count = value;
                RaisePropertyChanged("Count");
            }
        }
    }

    public enum MenuItemType
    {
        Inbox,
        Projects,
        Contexts,
    }

    public static class MenuItemTypeExtension
    {
        public static string ConvertToString(this MenuItemType item)
        {
            switch (item)
            {
                case MenuItemType.Inbox:
                case MenuItemType.Contexts:
                case MenuItemType.Projects:
                    return item.ToString();
                default:
                    return string.Empty;
            }
        }
    }
}