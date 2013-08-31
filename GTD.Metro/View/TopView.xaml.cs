using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Mono.App.GTD.Common;
using Mono.App.GTD.Model;
using Mono.App.GTD.Service;
using Mono.App.GTD.ViewModel;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Mono.App.GTD.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TopView : LayoutAwarePage
    {
        public TopView()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            this.DataContext = e.Parameter;
            await ViewModel.Initialize();
        }

        private void ListView_ItemClick_1(object sender, ItemClickEventArgs e)
        {
            var item = e.ClickedItem as TodoItem;
            var vm = new TodoItemViewModel(DataService.Inbox, item);
            this.Frame.Navigate(typeof(TodoItemView), vm);
        }

        private void GridView_ItemClick_2(object sender, ItemClickEventArgs e)
        {
            var item = e.ClickedItem as TodoItem;
            var vm = new TodoItemViewModel(null, item, ViewModel.Stars, "Stars");
            this.Frame.Navigate(typeof(TodoItemView), vm);
        }

        private void Project_ItemClick(object sender, ItemClickEventArgs e)
        {
            ProcessItemClick<Project>(e);
        }

        private void Context_ItemClick(object sender, ItemClickEventArgs e)
        {
            ProcessItemClick<Context>(e);
        }

        private void ProcessItemClick<TModel>(ItemClickEventArgs e) where TModel : ItemCollectionBase
        {
            var item = e.ClickedItem as TModel;
            var vm = new TodoItemViewModel(item, item.Items.FirstOrDefault());
            this.Frame.Navigate(typeof(TodoItemView), vm);
        }

        public TopViewModel ViewModel { get { return DataContext as TopViewModel; } }

        private void GridView_ItemClick_1(object sender, ItemClickEventArgs e)
        {
            ProcessItemClick<Due>(e);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var item = TodoItem.CreateNew();
            DataService.Inbox.AddItem(item);
            var vm = new TodoItemViewModel(DataService.Inbox, item);
            this.Frame.Navigate(typeof(TodoItemView), vm);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var vm = new TodoItemViewModel(DataService.Inbox, null);
            this.Frame.Navigate(typeof(TodoItemView), vm);
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            var vm = new ProjectsViewModel(ViewModel.Projects);
            this.Frame.Navigate(typeof(ProjectsView), vm);
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            var vm = new ContextsViewModel(ViewModel.Contexts);
            this.Frame.Navigate(typeof(ContextsView), vm);
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            var vm = new DueViewModel(ViewModel.Dues);
            this.Frame.Navigate(typeof(DueView), vm);
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            var vm = new TodoItemViewModel(null, null, ViewModel.Stars, "Stars");
            this.Frame.Navigate(typeof(TodoItemView), vm);
        }
    }
}