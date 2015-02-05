using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
//using System.Web.Security;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using MicroAssistant.Web.Models;


using WebMatrix.WebData;
using MicroAssistant.Lib;
using MicroAssistant.Data;
using System.Text;
using System.Web.Security;
using System.Data;
using MicroAssistant.Web.Utility;

namespace MicroAssistant.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        RegdUser regcls = new RegdUser();
        MicroAssistantEntities objcontext = new MicroAssistantEntities();
        AlertService arlt = new AlertService();
      
        public AccountController()
            : this(new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext())))
        {
        }

        public AccountController(UserManager<ApplicationUser> userManager)
        {
            UserManager = userManager;
        }

        public UserManager<ApplicationUser> UserManager { get; private set; }

        public ActionResult LogOff()
        {
            WebSecurity.Logout();

            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //

        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel logmodel, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                MembershipUser user = Membership.GetUser(logmodel.UserName);
                if (user != null)
                {
                    if (user.IsApproved && !user.IsLockedOut)
                    {

                        if (Membership.ValidateUser(logmodel.UserName.Trim(), logmodel.Password.Trim()))
                        {
                            int validusr = 0;
                            DataSet dschkvaliduser = new DataSet();
                            dschkvaliduser = regcls.validUser(logmodel.UserName.Trim());
                            if (dschkvaliduser.Tables.Count > 0)
                            {
                                if (dschkvaliduser.Tables[0].Rows.Count > 0)
                                {
                                    if (Convert.ToInt32(dschkvaliduser.Tables[0].Rows[0]["conter"].ToString()) > 0)
                                    {
                                        validusr = 1;
                                    }
                                }
                                if (dschkvaliduser.Tables[1].Rows.Count > 0)
                                {
                                    if (Convert.ToInt32(dschkvaliduser.Tables[1].Rows[0]["conter"].ToString()) > 0)
                                    {
                                        validusr = 2;
                                    }
                                }
                                if (dschkvaliduser.Tables[2].Rows.Count > 0)
                                {
                                    if (Convert.ToInt32(dschkvaliduser.Tables[2].Rows[0]["conter"].ToString()) > 0)
                                    {
                                        validusr = 3;
                                    }
                                }
                            }
                            if (validusr == 0)
                            {
                                FormsAuthentication.SetAuthCookie(logmodel.UserName, logmodel.RememberMe);
                                regcls.LoginHistory(logmodel.UserName, Request.UserHostAddress);
                                return RedirectToAction( "Index","MyAccount");
                            }
                            else if (validusr == 1)
                            {
                                TempData["LoginMessage"] = "Un Verified User!";
                                return View(logmodel);
                            }
                            else if (validusr == 2)
                            {
                                TempData["LoginMessage"] = "User InActive By Micro Assistance Team!";
                                return View(logmodel);
                            }
                            else if (validusr == 3)
                            {
                                TempData["LoginMessage"] = "User Deleted By Micro Assistance Team!";
                                return View(logmodel);
                            }
                        }

                        if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                                            && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                        {
                            return Redirect(returnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                            // return RedirectToUrl(logmodel.UserName);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "The email Address or password provided is incorrect.");
                    }
                }
                else if (!user.IsApproved)
                {
                    ModelState.AddModelError("", "Invalid username or password.");
                }
                else if (user.IsLockedOut)
                {
                    ModelState.AddModelError("", "Your account has been locked. Please contact administrator for further details..");
                }
            }
            else
            {
                ModelState.AddModelError("", "Invalid Username or Password.");
            }
            return View(logmodel);
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser() { UserName = model.UserName };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    AddErrors(result);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // POST: /Account/Disassociate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Disassociate(string loginProvider, string providerKey)
        {
            ManageMessageId? message = null;
            IdentityResult result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
            }
            return RedirectToAction("Manage", new { Message = message });
        }

        //
        // GET: /Account/Manage
        public ActionResult Manage(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : message == ManageMessageId.Error ? "An error has occurred."
                : "";
            ViewBag.HasLocalPassword = HasPassword();
            ViewBag.ReturnUrl = Url.Action("Manage");
            return View();
        }

        //
        // POST: /Account/Manage
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Manage(ManageUserViewModel model)
        {
            bool hasPassword = HasPassword();
            ViewBag.HasLocalPassword = hasPassword;
            ViewBag.ReturnUrl = Url.Action("Manage");
            if (hasPassword)
            {
                if (ModelState.IsValid)
                {
                    IdentityResult result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
            }
            else
            {
                // User does not have a password so remove any validation errors caused by a missing OldPassword field
                ModelState state = ModelState["OldPassword"];
                if (state != null)
                {
                    state.Errors.Clear();
                }

                if (ModelState.IsValid)
                {
                    IdentityResult result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var user = await UserManager.FindAsync(loginInfo.Login);
            if (user != null)
            {
                await SignInAsync(user, isPersistent: false);
                return RedirectToLocal(returnUrl);
            }
            else
            {
                // If the user does not have an account, then prompt the user to create an account
                ViewBag.ReturnUrl = returnUrl;
                ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { UserName = loginInfo.DefaultUserName });
            }
        }

        //
        // POST: /Account/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            // Request a redirect to the external login provider to link a login for the current user
            return new ChallengeResult(provider, Url.Action("LinkLoginCallback", "Account"), User.Identity.GetUserId());
        }

        //
        // GET: /Account/LinkLoginCallback
        public async Task<ActionResult> LinkLoginCallback()
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
            if (loginInfo == null)
            {
                return RedirectToAction("Manage", new { Message = ManageMessageId.Error });
            }
            var result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
            if (result.Succeeded)
            {
                return RedirectToAction("Manage");
            }
            return RedirectToAction("Manage", new { Message = ManageMessageId.Error });
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser() { UserName = model.UserName };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInAsync(user, isPersistent: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
      
        

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult RemoveAccountList()
        {
            var linkedAccounts = UserManager.GetLogins(User.Identity.GetUserId());
            ViewBag.ShowRemoveButton = HasPassword() || linkedAccounts.Count > 1;
            return (ActionResult)PartialView("_RemoveAccountPartial", linkedAccounts);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && UserManager != null)
            {
                UserManager.Dispose();
                UserManager = null;
            }
            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private async Task SignInAsync(ApplicationUser user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            var identity = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            Error
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

        private class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties() { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion

        [HttpGet]
        [AllowAnonymous]
        public ActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult SignUp(RegisterModel regmodel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    WebSecurity.CreateUserAndAccount(regmodel.Email, regmodel.Password);
                    WebSecurity.Login(regmodel.Email, regmodel.Password);
                    // Roles.AddUserToRoles(regmodel.Email, new string[] { "SiteUser" });
                    Roles.AddUserToRoles(regmodel.Email, new string[] { UserRoles.SiteUser.ToString() });

                    UserProfile profile = DataHelper.GetList<UserProfile>().Where(p => p.UserName == regmodel.Email).Single();
                    if (profile != null)
                    {
                        regcls.RegUser(0, regmodel.UserName, regmodel.Password, regmodel.Email, "None", "None");
                        StringBuilder emailcontent = new StringBuilder();
                        string verifyURL = UrlManager.GetAccountActivatoinUrl(Crypto.EncryptText(regmodel.Email.ToString()), Crypto.EncryptText(regmodel.DateCreated.ToString()));
                        emailcontent.Append("<html><body>");

                        emailcontent.Append("<p>Dear " + regmodel.UserName.ToString() + ", </p>");
                        emailcontent.Append("<p>Thanks for creating account at Micro Assistance!</p>");
                        emailcontent.Append("<p>Click on the following link to verify your email :- </p>");
                        emailcontent.Append("<p><a href='\"" + verifyURL + " target='_Blank' />Confirm Joblisting Account</a></p>");
                        emailcontent.Append("<p>Or Copy and paste following link in your browser</p>");
                        emailcontent.Append("<p>" + verifyURL.ToString() + "</p>");
                        emailcontent.Append("<p>Thanks</p>");
                        emailcontent.Append("<p>Micro Assistance Team</p>");
                        emailcontent.Append("<p></p>");
                        emailcontent.Append("</body></html>");

                        string[] receipents = new string[1];
                        receipents[0] = regmodel.Email;

                        //arlt.SendMail("Micro Assistance Register Confirmation Email", receipents, emailcontent.ToString());
                        TempData["SaveData"] = "Your account has been created successfull. Confirmation email has been sent to your registered email address.";
                    }
                    return View();
                }

                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
                }
            }
            return View();
        }
        public ActionResult Thanks()
        {

            return View();
        }
        [AllowAnonymous]
        public ActionResult VerifyAccount(string code, string name)
        {
            if (!string.IsNullOrWhiteSpace(code) && !string.IsNullOrWhiteSpace(name))
            {
                if (ProceedVerification(code, name))
                {
                    DateTime creationDate;
                    string userEmailAddress;
                    DeCryptParameters(code, name, out creationDate, out userEmailAddress);
                    regcls.UserActivation(userEmailAddress, "Verify");
                    Session.Remove("inverifyval");
                    var reguserVal = (from emp in objcontext.RegisteredUsers where emp.usrEmail == userEmailAddress select emp).ToList();
                    if (reguserVal.Count > 0)
                    {
                        StringBuilder emailcontent = new StringBuilder();
                        emailcontent.Append("<html><body>");

                        emailcontent.Append("<p>Dear " + reguserVal[0].UserName.ToString() + ", </p>");
                        emailcontent.Append("<p>Thanks for verified your account at Micro Assistance!</p>");
                        emailcontent.Append("<p>Now, Please login with given below credential :- </p>");
                        emailcontent.Append("<p>Username :- " + reguserVal[0].UserName.ToString() + "<br /> Password:- " + reguserVal[0].UserName.ToString() + "</p>");
                        emailcontent.Append("<p>&nbsp;</p>");
                        emailcontent.Append("<p>Thanks</p>");
                        emailcontent.Append("<p>Micro Assistance Team</p>");
                        emailcontent.Append("<p></p>");
                        emailcontent.Append("</body></html>");

                        string[] receipents = new string[1];
                        receipents[0] = userEmailAddress;

                        //arlt.SendMail("Micro Assistance Register Confirmation Email", receipents, emailcontent.ToString());
                    }
                    return View("thanks");
                }
                else
                {
                    return View();
                }
            }
            else
            {
                return View();
            }
        }
        /**/
        private bool ProceedVerification(string code, string userName)
        {
            DateTime creationDate;
            string userEmailAddress;
            Session["inverifyval"] = null;
            try
            {
                DeCryptParameters(code, userName, out creationDate, out userEmailAddress);

                if (IsUserAlreadyVerified(userEmailAddress))
                {
                    DataSet dschkvaliduser = new DataSet();
                    dschkvaliduser = regcls.validUser(userEmailAddress.Trim());
                    if (dschkvaliduser.Tables.Count > 0)
                    {
                        if (dschkvaliduser.Tables[0].Rows.Count > 0)
                        {
                            if (Convert.ToInt32(dschkvaliduser.Tables[0].Rows[0]["conter"].ToString()) > 0)
                            {
                                Session["inverifyval"] = 1;
                                return (VerifiyUser(creationDate, userEmailAddress));
                            }
                            else
                            {
                                ViewBag.Message = "Account already verified.";

                                return false;
                            }
                        }
                    }

                }
                else
                {
                    return (VerifiyUser(creationDate, userEmailAddress));
                }
                return false;
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Invalid request.";
                return false;

            }
        }
        private bool VerifiyUser(DateTime creationDate, string userEmailAddress)
        {
            MembershipUser memUser = Membership.GetUser(userEmailAddress);

            if (memUser != null)
            {
                if (Session["inverifyval"] == null)
                {
                    Roles.RemoveUserFromRole(userEmailAddress, UserRoles.UnverifiedUser.ToString());
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        private void DeCryptParameters(string code, string userName, out DateTime creationDate, out string userEmailAddress)
        {
            creationDate = Convert.ToDateTime(Crypto.DecryptText(code));
            userEmailAddress = Crypto.DecryptText(userName);
        }
        private bool IsUserAlreadyVerified(string userName)
        {
            return !Roles.IsUserInRole(userName, UserRoles.UnverifiedUser.ToString());
        }
        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        
    }
}