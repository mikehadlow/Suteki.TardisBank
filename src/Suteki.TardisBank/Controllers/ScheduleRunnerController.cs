using System;
using System.Web.Mvc;
using Suteki.TardisBank.IoC;
using Suteki.TardisBank.Mvc;
using Suteki.TardisBank.Services;

namespace Suteki.TardisBank.Controllers
{
    public class ScheduleRunnerController : Controller
    {
        readonly ISchedulerService schedulerService;
        readonly TardisConfiguration configuration;

        public ScheduleRunnerController(ISchedulerService schedulerService, TardisConfiguration configuration)
        {
            this.schedulerService = schedulerService;
            this.configuration = configuration;
        }

        [HttpGet, UnitOfWork]
        public ActionResult Execute(string id)
        {
            if (id == null || configuration.ScheduleKey != id) return StatusCode.NotFound;

            schedulerService.ExecuteUpdates(DateTime.Now);
            return StatusCode.Ok;
        }
    }
}