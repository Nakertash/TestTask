using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LinqToDB.Mapping;

namespace TestTask.DB
{
    [Table(Name = "Tasks")]
    public class TaskDB
    {
        [PrimaryKey]
        public Guid id { get; set; }
        [Column(Name = "Name"), NotNull]
        public string Name { get; set; }
        [Column(Name = "Description"), Nullable]
        public string Description { get; set; }
        [Column(Name = "ProjectId"), NotNull]
        public Guid ProjectId { get; set; }
        [Column(Name = "State"), NotNull]
        public int State { get; set; }
        [Column(Name = "Priority"), NotNull]
        public int Priority { get; set; }
        public Models.Task ToTask()
        {
            var result = new Models.Task() {
                id = this.id,
                Name = this.Name,
                ProjectId = this.ProjectId,
                Description=this.Description,
                Priority=this.Priority,
                State=this.State 
            };
            return result;
        }
    }
}
