namespace Suteki.TardisBank.IoC
{
    public class TardisConfiguration
    {
        public TardisConfiguration()
        {
            // set some useful defaults here
            ScheduleKey = "run";
        }

        public string ScheduleKey { get; set; }
    }
}