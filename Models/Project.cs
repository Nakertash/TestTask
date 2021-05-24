using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestTask.Models
{
    public class Project
    {
        public Guid id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ProjectState State { get; set; }
        public int Priority { get; set; }
        public List<Task> tasks { get; set; }

        public DB.ProjectDB ToProjectDB()
        {
            var result = new DB.ProjectDB()
            {
                Description = this.Description,
                Name = this.Name,
                State = (int)this.State,
                id = this.id,
                Priority = this.Priority,
            };
            return result;
        }
    }
    public enum ProjectState
    {
        NotStarted, 
        Active, 
        Completed
    }
}
