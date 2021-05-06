using ChargeBackCalculation.Models;
using ChargeBackCalculation.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChargeBackCalculation.Controllers
{
    public class UserLoginsController : Controller
    {
        // GET: UserLogins
        ChargeBackCalculation.Methods.Methods methods = new ChargeBackCalculation.Methods.Methods();
        private UserLoginDbContext db = new UserLoginDbContext();
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(string UserRole)
        {

            if (UserRole == "Admin")
            {
                return RedirectToAction("AdminLogin");
            }
            else if (UserRole == "Customer")
            {
                return RedirectToAction("CustomerLogin");
            }
            else if (UserRole == "Employee")
            {
                return RedirectToAction("EmployeeLogin");
            }

            return View();
        }
        // registration for admin
        public ActionResult AdminRegister()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AdminRegister([Bind(Include = "Id,UserId,FirstName,LastName,DateOfBirth,Gender,PhoneNumber,Address,City,State,ZipCode,Email,UserName,Password,ConfirmPassword,SecretQuestions,Answer,RegisterDate")] UserLogin userLogin, string UserRole1)
        {
            if (ModelState.IsValid)
            {
                if (UserRole1 == "Admin")
                {
                    db.UserLogins.Add(userLogin);
                    db.SaveChanges();
                    return RedirectToAction("AdminLogin");
                }
            }
            else
            {
                ViewBag.ErrorMessage="pass the correct user role";
            }
            return View();
        }
        // login for admin
        public ActionResult AdminLogin()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AdminLogin(string username, string password)
        {
            var user = db.UserLogins.Where(x => x.UserName == username && x.Password == password).FirstOrDefault();
            if (user != null)
            {

                return RedirectToAction("Index", "Admins");
            }
            else
            {
                ViewBag.ErrorMessage="Invalid credentials";
            }

            return View();

        }
        // registration for customer
        public ActionResult CustomerRegister()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CustomerRegister([Bind(Include = "Id,UserId,FirstName,LastName,DateOfBirth,Gender,PhoneNumber,Address,City,State,ZipCode,Email,UserName,Password,ConfirmPassword,SecretQuestions,Answer,RegisterDate")] UserLogin userLogin1, string UserRole2)
        {
            if (ModelState.IsValid)
            {
                if (UserRole2 == "Customer")
                {
                    db.UserLogins.Add(userLogin1);
                    db.SaveChanges();
                    return RedirectToAction("CustomerLogin");
                }
            }
            else
            {
                ViewBag.ErrorMessage = "pass the correct user role";
            }
            return View();
        }
        // login for customer
        public ActionResult CustomerLogin()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CustomerLogin(string username, string password)
        {
            var user = db.UserLogins.Where(x => x.UserName == username && x.Password == password).FirstOrDefault();
            if (user != null)
            {

                return RedirectToAction("Index1", "ComplaintRegisters");
            }
            else
            {
                ViewBag.ErrorMessage = "Invalid credentials";
            }

            return View();

        }
        // registration for employee
        public ActionResult EmployeeRegister()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EmployeeRegister([Bind(Include = "Id,UserId,FirstName,LastName,DateOfBirth,Gender,PhoneNumber,Address,City,State,ZipCode,Email,UserName,Password,ConfirmPassword,SecretQuestions,Answer,RegisterDate")] UserLogin userLogin2, string UserRole3)
        {
            if (ModelState.IsValid)
            {
                if (UserRole3 == "Employee")
                {
                    db.UserLogins.Add(userLogin2);
                    db.SaveChanges();
                    return RedirectToAction("EmployeeLogin");
                }
            }
            else
            {
                ViewBag.ErrorMessage = "pass the correct user role";
            }
            return View();
        }
        // login for employee
        public ActionResult EmployeeLogin()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EmployeeLogin(string username, string password)
        {
            var user = db.UserLogins.Where(x => x.UserName == username && x.Password == password).FirstOrDefault();
            if (user != null)
            {

                return RedirectToAction("Index", "Employees");
            }
            else
            {
                ViewBag.ErrorMessage = "Invalid credentials";
            }

            return View();

        }
        // recovering forgot userid
        public ActionResult RecoveryUserId()
        {
            return View();
        }
        [HttpPost]
        public ActionResult RecoveryUserId(UserIdRecovery userIdRecovery)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            else
            {
                using (UserLoginDbContext db = new UserLoginDbContext())
                {
                    if (userIdRecovery != null)
                    {
                        var user = db.UserLogins.FirstOrDefault(x => x.Answer == userIdRecovery.Answer && x.SecretQuestions == userIdRecovery.SecretQuestions && x.Email == userIdRecovery.Email);
                        ViewBag.msg = "Your User Id is:" + user.UserId;
                        return View();
                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid Credentials.");
                        return View();
                    }
                }
            }
        }
        // Setting up new password for customer
        public ActionResult PasswordReset()
        {
            return View();
        }
        [HttpPost]
        public ActionResult PasswordReset(PasswordReset passwordReset)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            else
            {
                if (passwordReset != null)
                {
                    UserLoginDbContext db = new UserLoginDbContext();
                    var user = db.UserLogins.FirstOrDefault(x => x.UserId == passwordReset.UserId && x.SecretQuestions == passwordReset.SecretQuestions && x.Answer == passwordReset.Answer);
                    return RedirectToAction("PasswordUpdate", new { id = user.UserId });
                }
                else
                {
                    ModelState.AddModelError("", "Invalid Credentials.");
                    return View();
                }
            }
        }
        // Updating the new customer password in the database
        public ActionResult PasswordUpdate(int id)
        {
            return View();
        }
        [HttpPost]
        public ActionResult PasswordUpdate(int id, NewPassword newPassword)
        {
            UserLogin user1 = new UserLogin();
            if (!ModelState.IsValid)
            {
                return View();
            }
            else
            {
                if (newPassword != null)
                {
                    user1 = db.UserLogins.FirstOrDefault(x => x.UserId == id);
                    user1.Password = newPassword.Password;
                    user1.ConfirmPassword = newPassword.ConfirmPassword;
                    methods.Save(db);
                    ViewBag.msg = "Password Reset Successfully";
                    return View();
                }
                ModelState.AddModelError("", "Invalid Credentials.");
                return View();
            }
        }

    }
}