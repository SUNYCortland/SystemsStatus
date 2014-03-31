using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SystemsStatus.Core.Services;
using SystemsStatus.Web.Areas.Admin.ViewModels;
using SystemsStatus.Common.Authentication;
using SystemsStatus.Core.Data.Entities;
using System.Web.Security;
using System.IO;
using System.Net;
using System.Xml;
using SystemsStatus.Web.Areas.Admin.Authentication;
using System.Web.Script.Serialization;
using Castle.Core.Logging;

namespace SystemsStatus.Web.Areas.Admin.Controllers
{
    public class AuthController : Controller
    {
        private string CASHOST = System.Configuration.ConfigurationManager.AppSettings["CasHost"];

        private readonly IAuthenticationService _authenticationService;
        private readonly IUserService _userService;

        public ILogger Logger { get; set; }

        public AuthController(IAuthenticationService authenticationService, IUserService userService)
        {
            _authenticationService = authenticationService;
            _userService = userService;
        }

        //
        // GET: /Admin/Auth/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Admin/Auth/LoginCAS

        public ActionResult LoginCAS(string ticket, string ReturnUrl)
        {

            string service = Request.Url.GetLeftPart(UriPartial.Path);

            // First time through there is no ticket=, so redirect to CAS login
            if (String.IsNullOrWhiteSpace(ticket))
            {
                if (!String.IsNullOrWhiteSpace(ReturnUrl))
                {
                    Session["ReturnUrl"] = ReturnUrl;
                }

                return Redirect(CASHOST + "login?" + "service=" + service);
            }

            // Second time (back from CAS) there is a ticket= to validate
            string validateurl = CASHOST + "serviceValidate?" + "ticket=" + ticket + "&" + "service=" + service;
            StreamReader Reader = new StreamReader(new WebClient().OpenRead(validateurl));
            string resp = Reader.ReadToEnd();

            // Some boilerplate to set up the parse.
            NameTable nt = new NameTable();
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(nt);
            XmlParserContext context = new XmlParserContext(null, nsmgr, null, XmlSpace.None);
            XmlTextReader reader = new XmlTextReader(resp, XmlNodeType.Element, context);

            string netid = null;

            // A very dumb use of XML. Just scan for the "user". If it isn't there, its an error.
            while (reader.Read())
            {
                if (reader.IsStartElement())
                {
                    string tag = reader.LocalName;
                    if (tag == "user")
                        netid = reader.ReadString();
                }
            }

            reader.Close();

            if (netid != null)
            {
                if (!_userService.UserExists(netid, true))
                {
                    return HttpNotFound();
                }
                else
                {
                    Response.Cookies.Add(GetFormsAuthenticationCookie(netid));
                }
            }

            if (Session["ReturnUrl"] != null)
            {
                ReturnUrl = Session["ReturnUrl"].ToString();
                Session["ReturnUrl"] = null;
            }

            if (String.IsNullOrEmpty(ReturnUrl) && Request.UrlReferrer != null)
                ReturnUrl = Server.UrlEncode(Request.UrlReferrer.PathAndQuery);

            if (Url.IsLocalUrl(ReturnUrl) && !String.IsNullOrEmpty(ReturnUrl))
            {
                return Redirect(ReturnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }
        }

        //
        // GET: /Admin/Auth/

        public ActionResult Login()
        {
            var vm = new LoginViewModel();

            return View(vm);
        }

        //
        // POST: /Admin/Auth/

        [HttpPost]
        public ActionResult Login(LoginViewModel vm, string ReturnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = _userService.GetUserByUsername(vm.Username);

                if (user != null)
                {
                    var hashedPassword = AuthPasswordHelper.HashEncode(vm.Password, user.PasswordSalt, 100);

                    if (_authenticationService.Authenticate(vm.Username, hashedPassword))
                    {
                        Response.Cookies.Add(GetFormsAuthenticationCookie(user.Username));

                        if (String.IsNullOrEmpty(ReturnUrl) && Request.UrlReferrer != null)
                            ReturnUrl = Server.UrlEncode(Request.UrlReferrer.PathAndQuery);

                        if (Url.IsLocalUrl(ReturnUrl) && !String.IsNullOrEmpty(ReturnUrl))
                        {
                            return Redirect(ReturnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home", new { area = "" });
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("Username", "Invalid username and password combination. Please try again.");
                    }
                }
                else
                {
                    ModelState.AddModelError("Username", "The specified username is invalid. User not found.");
                }

                ViewBag.LoggedIn = false;
            }

            return View(vm);
        }

        //
        // GET: Admin/Auth/Logout

        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", new { controller = "Home", area = "" });
        }

        private HttpCookie GetFormsAuthenticationCookie(string Username)
        {
            var user = _userService.GetUserByUsername(Username);

            UserPrincipalPoco pocoModel = new UserPrincipalPoco();
            pocoModel.Id = user.Id.Value;
            pocoModel.FirstName = user.FirstName;
            pocoModel.LastName = user.LastName;
            pocoModel.Role = user.Role;
            pocoModel.Username = Username;

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            string userData = serializer.Serialize(pocoModel);

            FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
                1,
                Username,
                DateTime.Now,
                DateTime.Now.AddMinutes(15),
                false,
                userData);

            string encryptedTicket = FormsAuthentication.Encrypt(authTicket);

            return new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
        }
    }
}
