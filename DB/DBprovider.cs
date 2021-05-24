using LinqToDB;
using System;
using System.Xml;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TestTask.Models;

namespace TestTask.DB
{
    public class DBprovider: IDBProvider
    {
        public ConcurrentDictionary<Guid, Project> concurrent;
        
        public string DataSource="";
        public string InitialCatalog = "";

        public DBprovider()
        {
            XmlDocument config = new XmlDocument();
            config.Load("App.config");
            XmlElement root = config.DocumentElement;

            foreach(XmlNode node in root)
            {
                Trace.WriteLine(node.Name);

                if (node.Name == "DataSource")
                {
                    DataSource = node.InnerText;
                }
                if (node.Name == "InitialCatalog")
                {
                    InitialCatalog = node.InnerText;
                }
            }
            Trace.WriteLine(DataSource+"; "+InitialCatalog);

        }
        
        public void CreateTable()
        {
            SqlConnection connection;
            SqlCommand command;
            connection = new SqlConnection(@"Data Source=DESKTOP-MN2NFJM\SQLEXPRESS;Initial Catalog=Task10;Integrated Security=true");

            string sql = @"IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Projects' AND xtype='U')
                            CREATE TABLE [dbo].[Projects](
                            [id] [uniqueidentifier]  NOT NULL PRIMARY KEY,
	                        [Name] [varchar](50) NOT NULL,
	                        [Description] [text] NULL,
	                        [StartDate] [datetime] NOT  NULL,
	                        [ComplitionDate] [datetime] NOT NULL,
	                        [State] [int] NOT NULL,
	                        [Priority] [int] NOT NULL);
                        IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Tasks' AND xtype='U')
                            CREATE TABLE [dbo].[Tasks](
                            [id] [uniqueidentifier]  NOT NULL PRIMARY KEY,
	                        [Name] [varchar](50) NOT NULL,
	                        [Description] [text] NOT NULL,
                            [ProjectId] [uniqueidentifier]  NOT NULL,
                            [State] [int] NOT NULL,
	                        [Priority] [int] NOT NULL
                            )";
            connection.Open();
            command = new SqlCommand(sql, connection);
            command.ExecuteNonQuery();
            connection.Close();
        }

        public Project GetProjectByID(Guid id)
        {
            Project result=null;
            using (var db = new DBclass(DataSource, InitialCatalog))
            {
                var projectFromDB = db.projects.Where(project=>project.id==id).ToArray()[0];
                result = projectFromDB.ToProject();

                var tasksFromDB = db.tasks.Where(task => task.ProjectId == result.id).ToArray();
                List<Models.Task> tasks = new List<Models.Task>();
                for (int l = 0; l < tasksFromDB.Length; l++)
                {
                    tasks.Add(tasksFromDB[l].ToTask());
                }
                result.tasks = tasks;
                
            }
            return result;
        }
        
        public Project[] GetAll()
        {
            Project[] projects=null;


            using (var db = new DBclass(DataSource, InitialCatalog))
            {
                var projectsFromDB=db.projects.OrderBy(project=>project.Priority).ToArray();
                projects = new Project[projectsFromDB.Length];
                for (int i=0;i< projectsFromDB.Length;i++)
                {
                    projects[i] = projectsFromDB[i].ToProject();
                    var tasksFromDB = db.tasks.Where(task => task.ProjectId == projects[i].id).OrderBy(task=>task.Priority).ToArray();
                    List<Models.Task> tasks = new List<Models.Task>();
                    for(int l=0;l< tasksFromDB.Length;l++)
                    {
                        tasks.Add (tasksFromDB[l].ToTask());
                    }
                    projects[i].tasks = tasks;
                }
                
            }

                return projects;
        }

        public void SetTask(Models.Task setter)
        {
            using (var db = new DBclass(DataSource, InitialCatalog))
            {
                db.tasks.Value(eva => eva.ProjectId, setter.ProjectId)
                    .Value(eva => eva.Name, setter.Name)
                    .Value(eva => eva.id, setter.id)
                    .Value(eva => eva.Description, setter.Description)
                    .Value(eva => eva.Priority, setter.Priority)
                    .Value(eva => eva.State, setter.State).Insert();
            }
        }

        public void SetProject(ProjectDB setter)
        {
            using (var db = new DBclass(DataSource, InitialCatalog))
            {
                db.projects.Value(eva => eva.Description, setter.Description)
                    .Value(eva => eva.Name, setter.Name)
                    .Value(eva => eva.id, setter.id)
                    .Value(eva => eva.State, (int)setter.State)
                    .Value(eva => eva.StartDate, DateTime.Now)
                    .Value(eva => eva.ComplitionDate, DateTime.MaxValue)
                    .Value(eva => eva.Priority, setter.Priority).Insert();
            }
        }

        public void UpdateProject(Guid projectId, string name, string description, DateTime? complitionDate, int? priority)
        {
            using (var db = new DBclass(DataSource, InitialCatalog))
            {
                var project=db.projects.Where(project=>project.id==projectId);
                if(!string.IsNullOrEmpty(name))
                {
                    project.Set(project=>project.Name,name).Update();
                }
                if (!string.IsNullOrEmpty(description))
                {
                    project.Set(project => project.Description, description).Update();
                }
                if (priority!=null)
                {
                    project.Set(project => project.Priority, priority.Value).Update();
                }
                if (complitionDate != null)
                {
                    project.Set(project => project.ComplitionDate, complitionDate).Update();
                }
                
            }
        }
    }
    public class DBclass : LinqToDB.Data.DataConnection
    {
        protected const string connectionString = @"Data Source=DESKTOP-MN2NFJM\SQLEXPRESS;Initial Catalog=Task10;Integrated Security=true";

        public DBclass(string DataSource,string InitialCatalog) :
            base(ProviderName.SqlServer2017, @"Data Source="+DataSource+";Initial Catalog="+ InitialCatalog + ";Integrated Security=true")
        {
            
        }
        public ITable<ProjectDB> projects => GetTable<ProjectDB>();
        public ITable<TaskDB> tasks => GetTable<TaskDB>();
    }
}
