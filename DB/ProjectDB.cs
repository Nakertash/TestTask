using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LinqToDB.Mapping;

namespace TestTask.DB
{
    [Table(Name = "Projects")]
    public class ProjectDB
    {
        [PrimaryKey]
        public Guid id { get; set; }
        [Column(Name = "Name"), NotNull]
        public string Name { get; set; }
        [Column(Name = "Description"), Nullable]
        public string Description { get; set; }
        [Column(Name = "StartDate"), NotNull]
        public DateTime StartDate { get; set; }
        [Column(Name = "ComplitionDate"), NotNull]
        public DateTime ComplitionDate { get; set; }
        [Column(Name = "State"), NotNull]
        public int State { get; set; }
        [Column(Name = "Priority"), NotNull]
        public int Priority { get; set; }

        public Models.Project ToProject()
        {
            var result = new Models.Project() {
            Description=this.Description,
            Name=this.Name,
            State=(Models.ProjectState)this.State,
            id=this.id,
            Priority=this.Priority,
            };
            return result;
        }
    }
}
