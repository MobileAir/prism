using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Prism.DAL;
using Prism.Models;

namespace Prism.Helper
{
    public class SystemVariables
    {
        private ApplicationDbContext db;
        protected UserManager<ApplicationUser> UserManager { get; set; }

        public SystemVariables(ApplicationDbContext context)
        {
            db = context;
            UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
        }
        public ApplicationUser GetCurrentUser()
        {
            return
                UserManager.FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
        }
    }
}