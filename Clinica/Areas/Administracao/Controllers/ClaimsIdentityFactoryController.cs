using Areas.Administracao.IdentityExtensions;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Clinica.Models;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Areas.Administracao.Controllers
{
    public class ClaimsIdentityFactoryController : Controller
    {
        // GET: ClaimsIdentityFactory
        public async Task<ActionResult> Index(Areas.Administracao.Models.LoginViewModel model, string returnUrl)
        {
            var context = new ApplicationDbContext();
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            userManager.ClaimsIdentityFactory = new MyClaimsIdentityFactory<ApplicationUser>();

            // Create a User to SignIn
            var user = await userManager.FindAsync(model.UserName, model.Password);

            //SignIn the User by generating a ClaimsIdentity            
            var claimsIdentity = await userManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);

            // This claimsIdentity should have a claim called LastLoginTime
            var authManager = HttpContext.GetOwinContext().Authentication;
            authManager.SignIn(claimsIdentity);
            
            return RedirectToLocal(returnUrl);
        }
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}