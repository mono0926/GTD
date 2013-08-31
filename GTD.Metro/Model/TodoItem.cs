using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mono.App.GTD.Common;
using Windows.UI.Xaml.Media.Imaging;

namespace Mono.App.GTD.Model
{
    public class TodoItem : BindableBase
    {
        private static int idSeed;

        public static TodoItem CreateNew()
        {
            var id = ++idSeed;
            return new TodoItem
            {
                Id = id,
                Contexts = new ObservableCollection<Context>(),
                ImagePathList = new List<string>(),
                VideoPathList = new List<string>(),
            };
        }

        public static TodoItem RestoreFromDB(int id, string title, string description, DateTime due, bool done, bool isStar, int projectId, IList<int> contextIds, IList<string> imagePaths, IList<string> videoPaths)
        {
            if (id > idSeed)
            {
                idSeed = id;
            }
            return new TodoItem
            {
                Id = id,
                Title = title,
                Description = description,
                Due = due,
                Done = done,
                IsStar = isStar,
                ProjectId = projectId,
                ContextIds = contextIds,
                Contexts = new ObservableCollection<Context>(),
                ImagePathList = imagePaths,
                VideoPathList = videoPaths,
            };
        }

        private TodoItem()
        {
        }

        private int id;

        public int Id
        {
            get { return id; }
            set { SetProperty(ref id, value); }
        }

        private string title;

        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        private string description;

        public string Description
        {
            get { return description; }
            set { SetProperty(ref description, value); }
        }

        private DateTime due;

        public DateTime Due
        {
            get { return due; }
            set { SetProperty(ref due, value); }
        }

        private bool done;

        public bool Done
        {
            get { return done; }
            set { SetProperty(ref done, value); }
        }

        private bool isStar;

        public bool IsStar
        {
            get { return isStar; }
            set { SetProperty(ref isStar, value); }
        }

        private Location location;

        public Location Location
        {
            get { return location; }
            set { SetProperty(ref location, value); }
        }

        public int ProjectId { get; set; }

        public ProjectBase Project { get; set; }

        public IList<int> ContextIds { get; set; }

        public ObservableCollection<Context> Contexts { get; set; }

        public IList<string> ImagePathList { get; set; }

        public IList<string> VideoPathList { get; set; }
    }
}