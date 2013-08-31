using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Mono.App.GTD.Model;
using Mono.App.GTD.ViewModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Capture;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace Mono.App.GTD.View
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class TodoItemView : Mono.App.GTD.Common.LayoutAwarePage
    {
        public TodoItemView()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            this.DataContext = e.Parameter;
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            ViewModel.ReflectToDataService();
        }

        public TodoItemViewModel ViewModel { get { return DataContext as TodoItemViewModel; } }

        private void Images_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            var gv = sender as GridView;
            ViewModel.IsImageVisible = (gv.SelectedItems.Count > 0);
            ViewModel.IsVideoVisible = false;
        }

        private void Videos_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            var gv = sender as GridView;
            ViewModel.IsVideoVisible = (gv.SelectedItems.Count > 0);
            ViewModel.IsImageVisible = false;
        }
    }
}