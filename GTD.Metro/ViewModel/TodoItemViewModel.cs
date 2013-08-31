using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Mono.App.GTD.Command;
using Mono.App.GTD.Model;
using Mono.App.GTD.Service;
using Mono.Framework.Common.Extensions;
using Mono.Framework.Common.IO;
using Windows.ApplicationModel;
using Windows.Media.Capture;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace Mono.App.GTD.ViewModel
{
    public class TodoItemViewModel : ViewModelBase
    {
        public TodoItemViewModel()
        {
            if (DesignMode.DesignModeEnabled)
            {
                var item1 = TodoItem.CreateNew();
                // item1.ImagePathList = new List<string> { "1.jpg", "2.jpg" };
                var item2 = TodoItem.CreateNew();
                var item3 = TodoItem.CreateNew();
                this.Items = new ObservableCollection<TodoItem> { item1, item2, item3 };
                this.Current = item1;
                this.Images = (new List<BitmapImage> { new BitmapImage { UriSource = new Uri(@"C:\Users\Masayuki\AppData\Local\Packages\gtd.metro.mono_txhgjw6kq8vz4\LocalState\photo\1.jpg") } }).ToObservableCollection();
            }
        }

        public TodoItemViewModel(ItemCollectionBase itemCollection, TodoItem current, ObservableCollection<TodoItem> items = null, string title = null)
        {
            this.ItemCollection = itemCollection;
            this.Items = items ?? itemCollection.Items;
            this.Current = current;

            this.Title = title ?? itemCollection.Title;
            this.TitleCandidate = this.Title;
            this.DescriptionCandidate = ItemCollection != null ? ItemCollection.Description : string.Empty;

            this.ProjectWithInbox = DataService.ProjectWithInbox;

            this.Contexts = DataService.Contexts;

            LoadMedia();
            this.PropertyChanged += (sender, e) =>
                {
                    if (e.PropertyName == "Current")
                    {
                        LoadMedia();
                    }
                };
        }

        public async void LoadMedia()
        {
            if (Current == null)
            {
                return;
            }
            var photoRoot = await StorageUtil.OpenOrCreateFolder(Consts.photoFolderName);
            this.Images = Current.ImagePathList
                .Select(x => new BitmapImage { UriSource = new Uri(Path.Combine(photoRoot.Path, x)) })
                .ToObservableCollection();
            var videoRoot = await StorageUtil.OpenOrCreateFolder(Consts.videoFolderName);
            this.Videos = Current.VideoPathList
                .Select(x => new Uri(Path.Combine(videoRoot.Path, x)))
                .ToObservableCollection();
        }

        private string title;

        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        public string TitleCandidate { get; set; }

        public string DescriptionCandidate { get; set; }

        public IList<ProjectBase> ProjectWithInbox { get; set; }

        public IList<Context> Contexts { get; set; }

        public ObservableCollection<TodoItem> Items { get; set; }

        private ObservableCollection<BitmapImage> images = new ObservableCollection<BitmapImage>();

        public ObservableCollection<BitmapImage> Images
        {
            get { return images; }
            set { SetProperty(ref images, value); }
        }

        private ObservableCollection<Uri> videos = new ObservableCollection<Uri>();

        public ObservableCollection<Uri> Videos
        {
            get { return videos; }
            set { SetProperty(ref videos, value); }
        }

        private TodoItem current;

        public TodoItem Current
        {
            get { return current; }
            set { SetProperty(ref current, value); }
        }

        public ItemCollectionBase ItemCollection { get; set; }

        private ICommand capturePhotoCommand;

        public ICommand CapturePhotoCommand
        {
            get
            {
                return capturePhotoCommand ??
                    (capturePhotoCommand = new RelayCommand(async obj =>
                    {
                        var ui = new CameraCaptureUI();
                        var file = await ui.CaptureFileAsync(CameraCaptureUIMode.Photo);
                        if (file != null)
                        {
                            var bitmap = new BitmapImage { UriSource = new Uri(file.Path) };
                            Images.Add(bitmap);

                            var photoPath = await SaveFile(file, Consts.photoFolderName, DataService.NextPhotoId, "jpg");

                            Current.ImagePathList.Add(photoPath);
                        }
                    }));
            }
        }

        private static async Task<string> SaveFile(StorageFile file, string folderName, int id, string extension)
        {
            var folder = await StorageUtil.OpenOrCreateFolder(folderName);
            var path = string.Format("{0}.{1}", id, extension);
            var copiedFile = await file.CopyAsync(folder, path, NameCollisionOption.ReplaceExisting);
            return path;
        }

        private ICommand captureVideoCommand;

        public ICommand CaptureVideoCommand
        {
            get
            {
                return captureVideoCommand ??
                    (captureVideoCommand = new RelayCommand(async obj =>
                    {
                        var ui = new CameraCaptureUI();
                        var file = await ui.CaptureFileAsync(CameraCaptureUIMode.Video);
                        if (file != null)
                        {
                            Videos.Add(new Uri(file.Path));

                            var videoPath = await SaveFile(file, Consts.videoFolderName, DataService.NextVideoId, "mp4");
                            Current.VideoPathList.Add(videoPath);
                        }
                    }));
            }
        }

        private ICommand addContextCommand;

        public ICommand AddContextCommand
        {
            get
            {
                return addContextCommand ??
                    (addContextCommand = new RelayCommand(obj =>
                  {
                      var context = obj as Context;
                      this.current.Contexts.Add(context);
                  }));
            }
        }

        private ICommand addTodoItemCommand;

        public ICommand AddTodoItemCommand
        {
            get
            {
                return addTodoItemCommand ??
                    (addContextCommand = new RelayCommand(obj =>
                  {
                      var item = TodoItem.CreateNew();
                      DataService.Inbox.AddItem(item);
                      //                      this.Items = DataService.Inbox.Items;
                      //this.Items.Add(item);
                      Current = item;
                  }));
            }
        }

        private ICommand deleteTodoItemCommand;

        public ICommand DeleteTodoItemCommand
        {
            get
            {
                return deleteTodoItemCommand ??
                    (deleteTodoItemCommand = new RelayCommand(async obj =>
                  {
                      var ok = new UICommand("OK");
                      var dialog = new MessageDialog("Delete this item permanently?");
                      dialog.Commands.Add(ok);
                      dialog.Commands.Add(new UICommand("Cancel"));
                      var result = await dialog.ShowAsync();
                      if (result == ok)
                      {
                          DataService.Inbox.RemoveItem(Current);
                          Current = null;
                      }
                  }));
            }
        }

        private ICommand editCommand;

        public ICommand EditCommand
        {
            get
            {
                return editCommand ??
                    (editCommand = new RelayCommand(obj =>
                  {
                      IsOpenEdit = true;
                  }));
            }
        }

        private ICommand editSubmitCommand;

        public ICommand EditSubmitCommand
        {
            get
            {
                return editSubmitCommand =
                    (editSubmitCommand = new RelayCommand(obj =>
                  {
                      ItemCollection.Title = TitleCandidate;
                      ItemCollection.Description = DescriptionCandidate;
                      this.Title = TitleCandidate;
                      IsOpenEdit = false;
                  }));
            }
        }

        private ICommand editCancelCommand;

        public ICommand EditCancelCommand
        {
            get
            {
                return editCancelCommand =
                    (editCancelCommand = new RelayCommand(obj =>
                  {
                      IsOpenEdit = false;
                  }));
            }
        }

        private bool isOpenEdit;

        public bool IsOpenEdit
        {
            get { return isOpenEdit; }
            set { SetProperty(ref isOpenEdit, value); }
        }

        public void ReflectToDataService()
        {
            var target = Items.ToArray();
            foreach (var i in Enumerable.Range(0, target.Length))
            {
                var item = target[i];
                foreach (var p in ProjectWithInbox.Except(new ProjectBase[] { item.Project }))
                {
                    p.RemoveItem(item);
                }
                item.Project.AddItem(item);
                foreach (var c in Contexts.Except(item.Contexts))
                {
                    c.RemoveItem(item);
                }
                foreach (var c in item.Contexts)
                {
                    c.AddItem(item);
                }
            }
        }

        private bool isImageVisible;

        public bool IsImageVisible
        {
            get { return isImageVisible; }
            set { SetProperty(ref isImageVisible, value); }
        }

        private bool isVideoVisible;

        public bool IsVideoVisible
        {
            get { return isVideoVisible; }
            set { SetProperty(ref isVideoVisible, value); }
        }
    }
}