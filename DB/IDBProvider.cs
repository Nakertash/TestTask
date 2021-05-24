using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestTask.Models;

namespace TestTask.DB
{
    public interface IDBProvider
    {
        public Project[] GetAll();
        public void SetProject(ProjectDB project);
        public void UpdateProject(Guid projectId, string name, string description, DateTime? ComplitionDate, int? priority);
        public void SetTask(Models.Task setter);
        public Project GetProjectByID(Guid id);
    }
}
