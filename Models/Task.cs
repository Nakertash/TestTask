using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestTask.Models
{
    public class Task
    {
        public Guid id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int State { get; set; }
        public int Priority { get; set; }
        public Guid ProjectId { get; set; }
    }
}
