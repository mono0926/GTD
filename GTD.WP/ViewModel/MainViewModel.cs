using System;
using System.Collections.Generic;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Mono.App.GTD.Model;
using Mono.Framework.Mvvm.Message;
using Mono.Framework.Mvvm.ViewModel;

namespace Mono.App.GTD.ViewModel
{
    public class MainViewModel : MonoViewModelBase
    {
        #region field

        #endregion field

        #region Property

        private IEnumerable<MenuItem> menuItems;

        public IEnumerable<MenuItem> MenuItems
        {
            get { return menuItems; }
            set
            {
                menuItems = value;
                RaisePropertyChanged("MenuItems");
            }
        }

        #endregion Property

        #region Command

        private ICommand initializeCommand;

        public ICommand InitializeCommand
        {
            get
            {
                return initializeCommand ??
                    (initializeCommand = new RelayCommand(() =>
                    {
                        //var isDark = IsDarkStatic;
                        //RaisePropertyChanged("IsDark");
                        //var a = IsDark;
                        MenuItems = new List<MenuItem>
                        {
                            new MenuItem { Type = MenuItemType.Inbox, Count = 10 },
                            new MenuItem { Type = MenuItemType.Projects, Count = 10 },
                            new MenuItem { Type = MenuItemType.Contexts, Count = 10 },
                        };
                    }));
            }
        }

        private ICommand newItemCommand;

        public ICommand NewItemCommand
        {
            get
            {
                return newItemCommand ??
                    (newItemCommand = new RelayCommand<MenuItem>(item =>
                    {
                        var viewName = "NewItem";
                        var vm = new NewItemViewModel();
                        Messenger.Send(new NavigationMessage(
                            new Uri(string.Format("/View/{0}View.xaml", viewName), UriKind.Relative),
                            vm, "Transition"));
                    }));
            }
        }

        private ICommand itemCommand;

        public ICommand ItemCommand
        {
            get
            {
                return itemCommand ??
                    (itemCommand = new RelayCommand<MenuItem>(item =>
                    {
                        var viewName = item.Type.ToString();
                        var vm = new InboxViewModel();
                        Messenger.Send(new NavigationMessage(
                            new Uri(string.Format("/View/{0}View.xaml", viewName), UriKind.Relative),
                            vm, "Transition"));
                    }));
            }
        }

        #endregion Command
    }
}