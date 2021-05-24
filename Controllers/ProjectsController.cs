using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TestTask.DB;

namespace TestTask.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProjectsController : Controller
    {
        public IDBProvider Provider { get; set; }
        public ProjectsController(IDBProvider provider)
        {
            Trace.WriteLine("Project controller is started!");
            Provider = provider;
        }
        /// <summary>
        /// Return info about all projects in range between 0 and "count"
        /// </summary>
        /// <param name="count">Count of projects to display</param>
        /// <returns></returns>
        [HttpGet("getall/{count}", Name = "GetAll")]
        public ObjectResult GetById(int count)
        {
            return new ObjectResult((from p in Provider.GetAll() select p).Take(count));
        }
        [HttpPost("create/", Name = "Create")]
        public ObjectResult AddProject(string name,string description,int priority)
        {
            string error="";
            string result = "";
            try
            {
                Provider.SetProject(new DB.ProjectDB()
                {
                    Description = description,
                    Name = name,
                    State = (int)Models.ProjectState.NotStarted,
                    id = Guid.NewGuid(),
                    Priority = priority,
                });
                result = "Complete!";
            }
            catch(Exception ex)
            {
                error = ex.ToString();
                result = "Error!";
            }
            
            return new ObjectResult(new { result = result,error = error });
        }
        [HttpPost("{projectId}/update/", Name = "Update")]
        public ObjectResult UpdateProject(Guid projectId, string name, string description, DateTime? completeDate, int? priority)
        {
            string error = "";
            string result = "";
            try
            {
                /*Provider.SetProject(new DB.ProjectDB()
                {
                    Description = description,
                    Name = name,
                    State = (int)Models.ProjectState.NotStarted,
                    id = Guid.NewGuid(),
                    StartDate = completeDate,
                    Priority = priority,
                });*/
                Provider.UpdateProject(projectId,name,description,completeDate,priority);
                result = "Complete!";
            }
            catch (Exception ex)
            {
                error = ex.ToString();
                result = "Error!";
            }

            return new ObjectResult(new { result = result, error = error });
        }
        [HttpPost("{projectId}/")]
        public ObjectResult Get(Guid projectId)
        {
            var result = Provider.GetProjectByID(projectId);
            return new ObjectResult(result);
        }
        [HttpPost("{projectId}/addtask/")]
        public ObjectResult AddTask(Guid projectId,string name, string description,int priority,int state)
        {
            var result = new Models.Task()
            {
                id = Guid.NewGuid(),
                Name = name,
                ProjectId = projectId,
                Description = description,
                Priority = priority,
                State = state
            };
            Provider.SetTask(result);
            return new ObjectResult(new { result="Complete!"});
        }
    }
}
