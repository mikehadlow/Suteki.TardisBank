using System;
using System.Web.Mvc;
using Suteki.TardisBank.Model;
using Suteki.TardisBank.Mvc;
using Suteki.TardisBank.Services;
using Suteki.TardisBank.ViewModel;

namespace Suteki.TardisBank.Controllers
{
    public class ScheduleController : Controller
    {
        readonly IUserService userService;

        public ScheduleController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet]
        public ActionResult AddSchedule(string id)
        {
            // id is the child's username
            var child = userService.GetUserByUserName(id) as Child;
            if (userService.IsNotChildOfCurrentUser(child)) return StatusCode.NotFound;

            // give the user some defaults
            var addScheduleViewModel = new AddScheduleViewModel
            {
                ChildId = child.Id,
                Amount = 1.0M,
                Description = "Pocket Money",
                Interval = Interval.Week,
                StartDate = DateTime.Now
            };

            return View("AddSchedule", addScheduleViewModel);
        }

        [HttpPost, UnitOfWork]
        public ActionResult AddSchedule(AddScheduleViewModel addScheduleViewModel)
        {
            if (!ModelState.IsValid) return View("AddSchedule", addScheduleViewModel);

            var child = userService.GetUser(addScheduleViewModel.ChildId) as Child;
            if (userService.IsNotChildOfCurrentUser(child)) return StatusCode.NotFound;

            child.Account.AddPaymentSchedule(
                addScheduleViewModel.StartDate,
                addScheduleViewModel.Interval,
                addScheduleViewModel.Amount,
                addScheduleViewModel.Description
                );

            return View("AddScheduleConfirm", addScheduleViewModel);
        }

        [HttpGet, UnitOfWork]
        public ActionResult RemoveSchedule(string id, int scheduleId)
        {
            // id is the child user name
            var child = userService.GetUserByUserName(id) as Child;
            if (userService.IsNotChildOfCurrentUser(child)) return StatusCode.NotFound;

            child.Account.RemovePaymentSchedule(scheduleId);

            return Redirect(Request.UrlReferrer.OriginalString);
        }
    }
}