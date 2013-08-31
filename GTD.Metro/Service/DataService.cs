using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mono.App.GTD.Model;
using Mono.Framework.Common.Extensions;
using Mono.Framework.Common.IO;

namespace Mono.App.GTD.Service
{
    public static class DataService
    {
        public static ObservableCollection<Project> Projects { get; private set; }

        public static ObservableCollection<Context> Contexts { get; private set; }

        public static Inbox Inbox { get; private set; }

        public static ObservableCollection<Due> Dues { get; private set; }

        public static IList<ProjectBase> ProjectWithInbox
        {
            get
            {
                if (Inbox == null)
                {
                    return new List<ProjectBase>();
                }
                var projectWithInbox = new List<ProjectBase>() { Inbox };
                if (Projects != null)
                {
                    projectWithInbox.AddRange(Projects);
                }
                return projectWithInbox;
            }
        }

        private static int photoId;

        public static int NextPhotoId
        {
            get
            {
                return ++photoId;
            }
        }

        private static int videoId;

        public static int NextVideoId
        {
            get
            {
                return ++videoId;
            }
        }

        public async static Task Initialize()
        {
            var filenames = await StorageUtil.GetFilenames();
            foreach (var f in new string[] { "todo.xml", "project.xml", "context.xml" }.Except(filenames))
            {
                var file = await StorageUtil.CreateOrReplaceFile(f);
                switch (f)
                {
                    case "todo.xml":
                        await XmlSerializer.SaveTodoItems(new TodoItem[] { }, file);
                        break;
                    case "project.xml":
                        await XmlSerializer.SaveProjects(new Project[] { }, file);
                        break;
                    case "context.xml":
                        await XmlSerializer.SaveContexts(new Context[] { }, file);
                        break;
                    default:
                        break;
                }
            }

            var todoFile = await StorageUtil.OpenFile("todo.xml");
            var todoItems = (await XmlSerializer.LoadTodoItems(todoFile)).ToList();
            var projectFile = await StorageUtil.OpenFile("project.xml");
            Projects = (await XmlSerializer.LoadProjects(projectFile)).ToObservableCollection();
            var contextFile = await StorageUtil.OpenFile("context.xml");
            Contexts = (await XmlSerializer.LoadContexts(contextFile)).ToObservableCollection();

            BindRelation(todoItems);

            InitializeIdSeed(todoItems);
        }

        private static void BindRelation(IEnumerable<TodoItem> todoItems)
        {
            foreach (var todo in todoItems)
            {
                if (todo.ProjectId != 0)
                {
                    var p = Projects.First(x => x.Id == todo.ProjectId);
                    p.AddItem(todo);
                }
                var cs = Contexts.Where(x => todo.ContextIds.Contains(x.Id));
                foreach (var c in cs)
                {
                    c.AddItem(todo);
                }
            }
            Inbox = new Inbox();
            Inbox.AddRangeItem(todoItems.Where(x => x.ProjectId == 0));

            Dues = CreateDueContainers().ToObservableCollection();
            foreach (var d in Dues)
            {
                d.AddRangeItem(todoItems.Where(x => x.Due.CompareTo(d.StartDate) >= 0 && x.Due.CompareTo(d.EndDate) < 0));
            }
        }

        private static void InitializeIdSeed(IEnumerable<TodoItem> items)
        {
            photoId = items.SelectMany(x => x.ImagePathList)
                    .Select(x => Convert.ToInt32(Path.GetFileNameWithoutExtension(x)))
                    .OrderByDescending(x => x)
                    .FirstOrDefault();

            videoId = items.SelectMany(x => x.VideoPathList)
                .Select(x => Convert.ToInt32(Path.GetFileNameWithoutExtension(x)))
                .OrderByDescending(x => x)
                .FirstOrDefault();
        }

        private static IEnumerable<Due> CreateDueContainers()
        {
            return new Due[]
            {
            new Due(new DateTime(), DateTime.Now, "Overdue"),
            new Due(DateTime.Now, DateTime.Now.AddDays(1), "Within A Day"),
            new Due(DateTime.Now.AddDays(1), DateTime.Now.AddDays(7), "Within A Week"),
            new Due(DateTime.Now.AddDays(7), DateTime.Now.AddMonths(1), "Within A Month"),
            new Due(DateTime.Now.AddMonths(1), DateTime.Now.AddYears(1000), "Later"),
            };
        }
    }
}