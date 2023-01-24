using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using UBA_Network_Security_System.Models;
using UBA_Network_Security_System.Models.Utility;

namespace UBA_Network_Security_System.Controllers
{
    public class ClientController : Controller
    {

        private ApplicationDbContext con;
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;


        public ClientController()
        {
        }

        public ClientController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            con = new ApplicationDbContext();
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ActionResult Dashboard(string Msg)
        {
            return View();
        }

        #region      Account CRUD

        [HttpGet]
        public ActionResult Account(string Msg)
        {

            if (Msg != null)
            {
                ViewBag.Msg = Msg.ToString();
            }

            try
            {
                var user_email = User.Identity.Name;
                var user = con.Users.FirstOrDefault(c => c.Email == user_email);
                var myAccount = con.Accounts.FirstOrDefault(x => x.UserId == user.Id);
                if (myAccount != null)
                {
                    ViewBag.MyAccount = myAccount;
                }
            }
            catch (Exception ex)
            {
                ViewBag.Msg = Session["csmsg"].ToString() + " " + ex.Message.ToString();
                return View();
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Account(Account model)
        {
            string msg = "";

            if (ModelState.IsValid)
            {

                try
                {
                    var util = new Utilities();

                    var bvn = con.BVNBanks.FirstOrDefault(x => x.BVN == model.BVN);
                    if (bvn != null)
                    {

                        var chkUserExist = con.Users.FirstOrDefault(x => x.Email == model.Email);
                        var chkAccountExist = con.Accounts.Where(x => x.Email == model.Email).ToList();

                        if (chkUserExist == null && chkAccountExist == null)
                        {
                            //  -   -   BOTH USER AND APPLICANTPROFILE DOES NOT EXIST
                            var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                            var result = await UserManager.CreateAsync(user, model.Password);
                            if (result.Succeeded)
                            {
                                model.FirstName = bvn.FirstName;
                                model.MiddleName = bvn.MiddleName;
                                model.SurName = bvn.SurName;
                                model.DOB = bvn.DOB;
                                model.Gender = bvn.Gender;
                                model.AccountNumber = util.RandomDigits(10);
                                model.UserId = user.Id;
                                model.IsActive = true;
                                con.Accounts.Add(model);
                                con.SaveChanges();
                                msg = "Account saved successfully";
                                return RedirectToAction("Account", msg);
                            }
                            else
                            {
                                msg = "Sorry, User account could not be saved successfully!";
                            }
                            ViewBag.Msg = msg;
                            return View(model);
                        }
                        else if (chkUserExist != null && chkAccountExist == null)
                        {
                            //  -   -   ONLY APPLICANTPROFILE IS CREATED

                            model.FirstName = bvn.FirstName;
                            model.MiddleName = bvn.MiddleName;
                            model.SurName = bvn.SurName;
                            model.DOB = bvn.DOB;
                            model.Gender = bvn.Gender;
                            model.AccountNumber = util.RandomDigits(10);
                            model.UserId = chkUserExist.Id;
                            model.IsActive = true;
                            con.Accounts.Add(model);
                            con.SaveChanges();
                            msg = "Account saved successfully";
                            return RedirectToAction("Account", msg);
                        }
                        else if (chkUserExist != null && chkAccountExist != null)
                        {
                            //EDIT
                            var dbObj = con.Accounts.SingleOrDefault(o => o.AccountNumber == model.AccountNumber && o.UserId == model.UserId);

                            if (dbObj != null)
                            {
                                dbObj.FirstName = bvn.FirstName;
                                dbObj.MiddleName = bvn.MiddleName;
                                dbObj.SurName = bvn.SurName;
                                dbObj.DOB = bvn.DOB;
                                dbObj.Gender = bvn.Gender;
                                dbObj.DOO = model.DOO;
                                dbObj.Email = model.Email;
                                dbObj.Phone = model.Phone;
                                dbObj.AccountTypeID = model.AccountTypeID;
                                dbObj.ResidentialCountry = model.ResidentialCountry;
                                dbObj.PermanentResident = model.PermanentResident;
                                dbObj.AccountManagerName = model.AccountManagerName;
                                dbObj.Nationality = model.Nationality;
                                dbObj.IdentificationType = model.IdentificationType;
                                dbObj.IdentificationIssuedDate = model.IdentificationIssuedDate;
                                dbObj.IdentificationNumber = model.IdentificationNumber;
                                dbObj.IdentificationValidTill = model.IdentificationValidTill;
                                dbObj.IntroducedBy = model.IntroducedBy;
                                dbObj.MaritalStatus = model.MaritalStatus;
                                dbObj.SecurePass = model.SecurePass;
                                dbObj.BranchOffice = model.BranchOffice;
                                dbObj.IsActive = true;

                                msg = "Account updated successfully!";
                                con.SaveChanges();
                                return RedirectToAction("Account", msg);
                            }

                            else
                            {
                                msg = "No record found!";
                                return RedirectToAction("Account", msg);
                            }
                        }
                    }
                    else
                    {
                        ViewBag.Msg = "INVALID BANK VERIFICATION NUMBER!";
                        return View(model);
                    }
                }
                catch (Exception ex)
                {
                    if (ex.InnerException != null)
                        msg = "TRY AGAIN LATER, IF PERSISTED, CONTACT ADMIN! \n" + ex.InnerException.Message.ToString();
                    else
                        msg = "TRY AGAIN LATER, IF PERSISTED, CONTACT ADMIN! \n" + ex.Message.ToString();
                }
            }
            else
            {
                msg = "OOOPS, PLEASE PROVIDE ALL VALUES AND TRY AGAIN LATER!";
            }

            //return Json(msg, JsonRequestBehavior.AllowGet);
            return RedirectToAction("MyProfile", msg);
        }

        [AllowAnonymous]
        public ActionResult AddEditAccount(string ID)
        {
            Account model = new Account();
            string msg = "";

            try
            {

                if (ID != null)
                {
                    var obj = con.Accounts.SingleOrDefault(o => o.UserId == ID);
                    if (obj != null)
                    {

                        model.Email = obj.Email;
                        model.Phone = obj.Phone;

                        model.AccountTypeID = obj.AccountTypeID;
                        model.ResidentialCountry = obj.ResidentialCountry;
                        model.PermanentResident = obj.PermanentResident;
                        model.AccountManagerName = obj.AccountManagerName;
                        model.Nationality = obj.Nationality;
                        model.IdentificationType = obj.IdentificationType;
                        model.IdentificationIssuedDate = obj.IdentificationIssuedDate;
                        model.IdentificationNumber = obj.IdentificationNumber;
                        model.IdentificationValidTill = obj.IdentificationValidTill;
                        model.IntroducedBy = obj.IntroducedBy;
                        model.MaritalStatus = obj.MaritalStatus;
                        model.SecurePass = obj.SecurePass;
                        model.BranchOffice = obj.BranchOffice;
                        model.UserId = obj.UserId;

                        //model.LGAOrigin = obj.LGAOrigin;
                        //model.StateOrigin = obj.StateOrigin;
                        //model.ResidentialCountry = obj.ResidentialCountry;
                        //model.PermanentResident = obj.PermanentResident;
                        //model.PreferredJobLocation = obj.PreferredJobLocation;
                        //model.EmailNotification = obj.EmailNotification;
                        //model.Nationality = obj.Nationality;
                        //model.Skills = obj.Skills;
                        //model.UserId = obj.UserId;

                        return PartialView("AddEditAddEditAccount", model);

                    }
                    else
                    {
                        msg = "Record doesn't exist anymore!";
                    }
                }
            }
            catch (Exception ex)
            {
                msg = "TRY AGAIN, IF PERSISTED, CONTACT ADMIN!";
            }
            return RedirectToAction("Dashboard", msg);
        }

        [Authorize(Roles = "SuperAdmin")]
        public JsonResult DeleteAccount(string ID)
        {

            bool result = false;
            if (ID != null)
            {
                var objDel = con.Accounts.SingleOrDefault(o => o.Id == ID);
                if (objDel != null)
                {
                    con.Accounts.Remove(objDel);
                    con.SaveChanges();
                    result = true;
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "SuperAdmin")]
        public JsonResult SuspendAccount(string ID)
        {

            bool result = false;
            if (ID != null)
            {
                var objDel = con.Accounts.SingleOrDefault(o => o.Id == ID);
                if (objDel != null)
                {
                    objDel.IsActive = false;
                    con.SaveChanges();
                    result = true;
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        #endregion  - END OF ACCOUNT CRUD

        #region     -   IMAGE UPLOAD
        [HttpGet]
        public ActionResult UploadImage(string Msg)
        {
            ViewBag.Msg = Msg;
            return View();
        }

        [HttpPost]
        public ActionResult UploadImage(UploadImageViewModel model)
        {
            string msg = "";
            string Msg = "";

            if (model.AcountNumber != null)
            {
                var account = con.Accounts.FirstOrDefault(x => x.AccountNumber == model.AcountNumber);
                if (account != null)
                {
                    // - IS PASSPORT NOT NULL

                    if (model.PassportUpload != null && model.PassportUpload.ContentLength > 0 &&
                        model.PassportUpload.FileName.ToLower().EndsWith("jpg") ||
                        model.PassportUpload.FileName.ToLower().EndsWith("png") ||
                        model.PassportUpload.FileName.ToLower().EndsWith("jpeg"))
                    {

                        //UPLOAD NOT EMPTY
                        //CHANGE THE PASSPORT NAME
                        string passName = model.AcountNumber + Path.GetExtension(model.PassportUpload.FileName);


                        string pathx = Server.MapPath("~/Uploads/Account/Passports/" + passName);
                        model.PassportUpload.SaveAs(pathx);

                        //CONVERT IMAGE OBJECT TO BYTE ARRAY
                        if (pathx != null)
                        {
                            using (Image img = Image.FromFile(pathx))
                            {
                                var ms = new MemoryStream();

                                img.Save(ms, ImageFormat.Jpeg);

                                var bytes = ms.ToArray();

                                // NOW SAVE BYTE WITH STUDENT REGNO NUMBER
                                if (bytes.Length > 0)
                                {
                                    account.Passport = bytes;
                                    msg = "Passport added successfully!";
                                    Msg = Msg + " :: " + msg;
                                    con.SaveChanges();
                                }
                                else
                                {
                                    msg = "Passport Image conversion failed!";
                                    Msg = Msg + " :: " + msg;
                                }
                            }
                        }
                        else
                            Msg = Msg + " :: " + "Passport Upload counld not be completed successfully" +
                                " because, the image renaming failed!";

                        if (System.IO.File.Exists(pathx))
                            System.IO.File.Delete(pathx);
                    }
                    else
                    {
                        //IMAGE NOT VALID
                        msg = "Invalid Image format only jpeg, png and jpg is allowed or Image was not selected!";
                        Msg = Msg + " :: " + msg;
                    }

                    //  -  IS SIGNATURE NOT NULL
                    if (model.SignatureUpload != null && model.SignatureUpload.ContentLength > 0 &&
                        model.SignatureUpload.FileName.ToLower().EndsWith("jpg") ||
                        model.SignatureUpload.FileName.ToLower().EndsWith("png") ||
                        model.SignatureUpload.FileName.ToLower().EndsWith("jpeg"))
                    {

                        //UPLOAD NOT EMPTY
                        //CHANGE THE PASSPORT NAME
                        string signName = model.AcountNumber + Path.GetExtension(model.SignatureUpload.FileName);


                        string pathx = Server.MapPath("~/Uploads/Account/Signatures/" + signName);
                        model.SignatureUpload.SaveAs(pathx);

                        //CONVERT IMAGE OBJECT TO BYTE ARRAY
                        if (pathx != null)
                        {
                            using (Image img = Image.FromFile(pathx))
                            {
                                var ms = new MemoryStream();

                                img.Save(ms, ImageFormat.Jpeg);

                                var bytes = ms.ToArray();

                                // NOW SAVE BYTE WITH STUDENT REGNO NUMBER
                                if (bytes.Length > 0)
                                {
                                    account.Signature = bytes;
                                    con.SaveChanges();
                                    msg = "Signature added successfully!";
                                    Msg = Msg + " :: " + msg;
                                    return RedirectToAction("Dashboard", Msg);
                                }
                                else
                                {
                                    msg = "Signature Image conversion failed!";
                                    Msg = Msg + " :: " + msg;
                                }
                            }
                        }
                        else
                            msg = "Signature Upload counld not be completed successfully" +
                                " because, the image renaming failed!";

                        Msg = Msg + " :: " + msg;

                        if (System.IO.File.Exists(pathx))
                            System.IO.File.Delete(pathx);
                    }

                    else
                    {
                        //IMAGE NOT VALID
                        msg = "Invalid Image format only jpeg, png and jpg is allowed or Image was not selected!";
                        Msg = Msg + " :: " + msg;
                    }


                }
                else
                {
                    //IMAGE NOT VALID
                    msg = "Sorry, Account does not exist anymore!";
                    Msg = Msg + " :: " + msg;
                }
            }
            else
            {
                msg = "Please you must select account number!";
                Msg = Msg + " :: " + msg;
            }

            Msg = Msg + " :: " + msg;

            return RedirectToAction("UploadImage", Msg);
        }

        #endregion  -   END OF IMAGE UPLOAD


    }
}