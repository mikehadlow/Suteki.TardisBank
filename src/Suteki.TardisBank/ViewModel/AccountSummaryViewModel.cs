using Suteki.TardisBank.Model;

namespace Suteki.TardisBank.ViewModel
{
    public class AccountSummaryViewModel
    {
        public Child Child { get; set; }
        public Parent Parent { get; set; }

        public bool IsParentView
        {
            get { return Parent != null;  }
        }
    }
}