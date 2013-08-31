using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Mono.App.GTD.Model;
using Windows.Storage;
using Windows.Storage.Streams;

namespace Mono.App.GTD.Service
{
    public static class XmlSerializer
    {
        public static async Task<IEnumerable<TodoItem>> LoadTodoItems(IStorageFile file)
        {
            var doc = await LoadXDocument(file);
            var items = doc.Descendants("TodoItem")
                .Select(x =>
                    TodoItem.RestoreFromDB(
                    Convert.ToInt32(x.Element("Id").Value),
                    x.Element("Title").Value,
                    x.Element("Description").Value,
                     DateTime.Parse(x.Element("Due").Value),
                     Convert.ToBoolean(x.Element("Done").Value),
                     Convert.ToBoolean(x.Element("IsStar").Value),
                     Convert.ToInt32(x.Element("ProjectId").Value),
                     x.Element("Contexts").Descendants("ContextId").
                                Select(y => Convert.ToInt32(y.Value)).ToList(),
                    x.Element("Images").Descendants("ImagePath")
                                .Select(y => y.Value).ToList(),
                    x.Element("Videos").Descendants("VideoPath")
                                .Select(y => y.Value).ToList()));

            return items;
        }

        public static async Task<IEnumerable<Project>> LoadProjects(IStorageFile file)
        {
            var doc = await LoadXDocument(file);
            var items = doc.Descendants("Project")
                .Select(x => Project.RestoreFromDB(Convert.ToInt32(x.Element("Id").Value), x.Element("Title").Value));
            //new Project(x.Element("Title").Value)
            //{
            //    Id = Convert.ToInt32(x.Element("Id").Value),
            //});
            return items;
        }

        public static async Task<IEnumerable<Context>> LoadContexts(IStorageFile file)
        {
            var doc = await LoadXDocument(file);
            var items = doc.Descendants("Context")
                .Select(x => Context.RestoreFromDB(Convert.ToInt32(x.Element("Id").Value), x.Element("Title").Value));
            return items;
        }

        private static async Task<XDocument> LoadXDocument(IStorageFile file)
        {
            using (var stream = await file.OpenReadAsync())
            {
                return XDocument.Load(stream.AsStreamForRead());
            }
        }

        public static async Task SaveTodoItems(IEnumerable<TodoItem> items, IStorageFile file)
        {
            await SaveXElement(CreateTodoItemsElement(items), file);
        }

        public static async Task SaveProjects(IEnumerable<Project> items, IStorageFile file)
        {
            await SaveXElement(CreateProjectsElement(items), file);
        }

        public static async Task SaveContexts(IEnumerable<Context> items, IStorageFile file)
        {
            await SaveXElement(CreateContextsElement(items), file);
        }

        //public static async Task SaveXmlFile(IStorageFile file)
        //{
        //    await SaveXElement(null, file);
        //}

        private static async Task SaveXElement(XElement elem, IStorageFile file)
        {
            var doc = new XDocument(new XDeclaration("1.0", "utf-8", "yes"));
            doc.Add(elem);
            using (var stream = await file.OpenAsync(FileAccessMode.ReadWrite))
            {
                doc.Save(stream.AsStreamForWrite());
            }
        }

        private static XElement CreateTodoItemsElement(IEnumerable<TodoItem> items)
        {
            var elem = new XElement("TodoItems");
            foreach (var item in items)
            {
                elem.Add(CreateTodoElement(item));
            }
            return elem;
        }

        private static XElement CreateTodoElement(TodoItem model)
        {
            var item = new XElement("TodoItem");
            item.Add(new XElement("Id", model.Id));
            item.Add(new XElement("Title", model.Title));
            item.Add(new XElement("Description", model.Description));
            item.Add(new XElement("Due", model.Due));
            item.Add(new XElement("Done", model.Done));
            item.Add(new XElement("IsStar", model.IsStar));
            item.Add(new XElement("ProjectId", model.Project.Id));
            var contexts = new XElement("Contexts");
            foreach (var c in model.Contexts)
            {
                contexts.Add(new XElement("ContextId", c.Id));
            }
            item.Add(contexts);
            var images = new XElement("Images");
            foreach (var img in model.ImagePathList)
            {
                images.Add(new XElement("ImagePath", img));
            }
            item.Add(images);
            var videos = new XElement("Videos");
            foreach (var v in model.VideoPathList)
            {
                videos.Add(new XElement("VideoPath", v));
            }
            item.Add(videos);
            return item;
        }

        private static XElement CreateProjectsElement(IEnumerable<Project> items)
        {
            var elem = new XElement("Projects");
            foreach (var item in items)
            {
                elem.Add(CreateProjectElement(item));
            }
            return elem;
        }

        private static XElement CreateProjectElement(Project model)
        {
            var item = new XElement("Project");
            item.Add(new XElement("Id", model.Id));
            item.Add(new XElement("Title", model.Title));
            return item;
        }

        private static XElement CreateContextsElement(IEnumerable<Context> items)
        {
            var elem = new XElement("Contexts");
            foreach (var item in items)
            {
                elem.Add(CreateContextElement(item));
            }
            return elem;
        }

        private static XElement CreateContextElement(Context model)
        {
            var item = new XElement("Context");
            item.Add(new XElement("Id", model.Id));
            item.Add(new XElement("Title", model.Title));
            return item;
        }
    }
}