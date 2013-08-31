using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Mono.App.GTD.Command;
using Mono.App.GTD.Model;
using Mono.App.GTD.Service;
using Mono.App.GTD.View;
using Mono.Framework.Common.Extensions;
using Mono.Framework.Common.IO;
using Windows.ApplicationModel;

namespace Mono.App.GTD.ViewModel
{
    public class TopViewModel : ViewModelBase
    {
        public TopViewModel()
        {
            //Initialize();

            if (DesignMode.DesignModeEnabled)
            {
                InboxItems = new ObservableCollection<TodoItem>
                    {
                        TodoItem.RestoreFromDB(1, "a", "b", DateTime.Now, true, true, 0, new List<int>(), new List<string>{}, new List<string>{}),
                    };
                Projects = new ObservableCollection<Project>
                    {
                        Project.RestoreFromDB(1, "SampleProject"),
                    };
                Contexts = new ObservableCollection<Context>
                    {
                        Context.RestoreFromDB(1, "ContextSample"),
                    };
            }
        }

        public async Task Initialize()
        {
            if (initialized)
            {
                return;
            }
            initialized = true;
            await DataService.Initialize();
            InboxItems = DataService.Inbox.Items;
            Projects = DataService.Projects;
            Contexts = DataService.Contexts;
            Dues = DataService.Dues;
            OnPropertyChanged("Stars");
        }

        public string Title { get { return "GTD for Metro"; } }

        private ObservableCollection<TodoItem> inboxItems;

        public ObservableCollection<TodoItem> InboxItems
        {
            get { return inboxItems; }
            set { SetProperty(ref inboxItems, value); }
        }

        private ObservableCollection<Project> projects;

        public ObservableCollection<Project> Projects
        {
            get { return projects; }
            set { SetProperty(ref projects, value); }
        }

        private ObservableCollection<Context> contexts;

        public ObservableCollection<Context> Contexts
        {
            get { return contexts; }
            set { SetProperty(ref contexts, value); }
        }

        private ObservableCollection<Due> dues;

        public ObservableCollection<Due> Dues
        {
            get { return dues; }
            set { SetProperty(ref dues, value); }
        }

        public ObservableCollection<TodoItem> Stars
        {
            get { return DataService.ProjectWithInbox.SelectMany(x => x.Items).Where(x => x.IsStar).ToObservableCollection(); }
        }

        private bool isProjectPopupOpen;

        public bool IsProjectPopupOpen
        {
            get { return isProjectPopupOpen; }
            set { SetProperty(ref isProjectPopupOpen, value); }
        }

        private bool isContextPopupOpen;

        public bool IsContextPopupOpen
        {
            get { return isContextPopupOpen; }
            set { SetProperty(ref isContextPopupOpen, value); }
        }

        private ICommand addProjectCommand;

        public ICommand AddProjectCommand
        {
            get
            {
                return addProjectCommand ??
                    (addProjectCommand = new RelayCommand(obj =>
                  {
                      var name = obj as string;
                      Projects.Add(Project.CreateNew(name));
                      IsProjectPopupOpen = false;
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
                        var name = obj as string;
                        Contexts.Add(Context.CreateNew(name));
                        IsContextPopupOpen = false;
                    }));
            }
        }

        private ICommand toggleAddProjectPopup;

        public ICommand ToggleAddProjectPopup
        {
            get
            {
                return toggleAddProjectPopup ??
                    (toggleAddProjectPopup = new RelayCommand(obj =>
                  {
                      IsProjectPopupOpen = !IsProjectPopupOpen;
                  }));
            }
        }

        private ICommand toggleAddContextPopup;
        private bool initialized;

        public ICommand ToggleAddContextPopup
        {
            get
            {
                return toggleAddContextPopup ??
                    (toggleAddContextPopup = new RelayCommand(obj =>
                    {
                        IsContextPopupOpen = !IsContextPopupOpen;
                    }));
            }
        }

        //private ICommand inboxCommand;

        //public ICommand InboxCommand
        //{
        //    get
        //    {
        //        return inboxCommand ??
        //            (inboxCommand = new RelayCommand(obj =>
        //          {
        //              this.Frame.Navigate(typeof(TodoItemView));
        //          }));
        //    }
        //}
    }
}