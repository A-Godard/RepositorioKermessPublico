using SolucionKermesseGrupo2.Models;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

namespace SolucionKermesseGrupo2.Controllers
{
    public class AuthController : Controller
    {
        private BDKermesseEntities db = new BDKermesseEntities();

        public ActionResult Login(string message = "")
        {
            ViewBag.Message = message;
            return View();
        }


        [HttpPost]
        public ActionResult Authentification(string email, string password)
        {
            if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password))
            {
                var user = db.Usuario.FirstOrDefault(e => e.email == email && e.pwd == password);

                if (user != null)
                {
                    FormsAuthentication.SetAuthCookie(user.email, true);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return RedirectToAction("Login", new { message = "No encontramos tus datos" });

                }
            }
            else
            {
                return RedirectToAction("Login", new { message = "Llena los campos para poder iniciar sesion" });
            }


        }

        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

    }
}