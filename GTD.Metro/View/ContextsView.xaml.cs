using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Mono.App.GTD.Model;
using Mono.App.GTD.ViewModel;
using Mono.Framework.Common.Extensions;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace Mono.App.GTD.View
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class ContextsView : Mono.App.GTD.Common.LayoutAwarePage
    {
        public ContextsView()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            this.DataContext = e.Parameter;
        }

        public ContextsViewModel ViewModel { get { return this.DataContext as ContextsViewModel; } }

        private void GridView_ItemClick_1(object sender, ItemClickEventArgs e)
        {
            var item = e.ClickedItem as TodoItem;
            var context = ViewModel.Current;
            var vm = new TodoItemViewModel(context, item);
            this.Frame.Navigate(typeof(TodoItemView), vm);
        }
    }
}