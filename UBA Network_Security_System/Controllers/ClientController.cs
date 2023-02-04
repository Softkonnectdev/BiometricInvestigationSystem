using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using UBA_Network_Security_System.Models;
using UBA_Network_Security_System.Models.Utility;

namespace UBA_Network_Security_System.Controllers
{
    [Authorize(Roles = "Admin, Accountant")]
    public class ClientController : Controller
    {

        private ApplicationDbContext con;
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;


        public ClientController()
        {
            con = new ApplicationDbContext();
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

     
        public ActionResult Account(string AccountNumber = null)
        {
            string msg = "";
            string Msg = null;
            ViewBag.CurrencyFmt = CultureInfo.CreateSpecificCulture("NG-NG");

            if (TempData["Msg"] != null)
            {
                Msg += "\n" + TempData["Msg"].ToString();
            }

            List<Account> accounts = new List<Account>();
            try
            {
                if (AccountNumber == null && !User.IsInRole("Admin"))
                {
                    Msg += "\n" + "Please provide customer account number on the box!";
                    return View();
                }
                else if (User.IsInRole("Admin"))
                {
                    var customerAccounts = con.Accounts.ToList();
                    if (customerAccounts.Count > 0)
                    {
                        foreach (var bvn in customerAccounts)
                        {
                            accounts.Add(bvn);
                        }
                    }
                    ViewBag.Accounts = accounts;
                }
                else
                {
                    var customerAccount = con.Accounts.FirstOrDefault(x => x.AccountNumber == AccountNumber);
                    if (customerAccount != null)
                    {
                        accounts.Add(customerAccount);
                        ViewBag.Accounts = accounts;
                    }
                    else
                    {
                        Msg += "\n" + "Sorry, No Candidate found with provided account number!";
                    }
                }


            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    Msg += "\n" + "TRY AGAIN LATER, IF PERSISTED, CONTACT ADMIN! \n" + ex.InnerException.Message.ToString();
                else
                    Msg += "\n" + "TRY AGAIN LATER, IF PERSISTED, CONTACT ADMIN! \n" + ex.Message.ToString();
            }

            ViewBag.AccountCount = accounts.Count;
            ViewBag.Msg = Msg;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Account(Account model)
        {
            string msg = "";

            var userEmail = User.Identity.Name;
            
                var user = con.Users.FirstOrDefault(x => x.Email == userEmail);

                model.AccountManagerName = user.Email;
            

            if (ModelState.IsValid)
            {

                try
                {
                    var util = new Utilities();

                    var bvn = con.BVNBanks.FirstOrDefault(x => x.BVN == model.BVN);
                    if (bvn != null)
                    {

                        var chkAccountExist = con.Accounts.FirstOrDefault(x => x.Email == model.Email &&
                                                           x.AccountNumber == model.AccountNumber);

                        if (chkAccountExist == null)
                        {

                            model.DOB = bvn.DOB;
                            model.Gender = bvn.Gender;
                            model.AccountNumber = util.RandomDigits(10);
                            model.IsActive = true;
                            model.DOO = DateTime.UtcNow;


                            con.Accounts.Add(model);
                            con.SaveChanges();
                            msg = "Account saved successfully";
                           }
                        else
                        {
                            //EDIT
                            var dbObj = con.Accounts.SingleOrDefault(o => o.AccountNumber == model.AccountNumber &&
                                                                     o.Email == model.Email);

                            if (dbObj != null)
                            {
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

                                msg = "Account updated successfully!";
                                con.SaveChanges();
                            }

                            else
                            {
                                msg = "No record found to modify!";
                            }
                        }
                    }
                    else
                    {
                        msg = "INVALID BANK VERIFICATION NUMBER!";
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

            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public ActionResult AddEditAccount(string ID = null)
        {
            Account model = new Account();
            string msg = "";

            try
            {

                if (ID != "")
                {
                    var obj = con.Accounts.SingleOrDefault(o => o.AccountNumber == ID);
                    if (obj != null)
                    {

                        model.AccountNumber = obj.AccountNumber;
                        model.Email = obj.Email;
                        model.BVN = obj.BVN;
                        model.Phone = obj.Phone;
                        model.LGA = obj.LGA;
                        model.State = obj.State;
                        model.FirstName = obj.FirstName;
                        model.MiddleName = obj.MiddleName;
                        model.SurName = obj.SurName;
                        model.AccountManagerName = obj.AccountManagerName;
                        model.DOO = obj.DOO;
                        model.DOB = obj.DOB;
                        model.AccountTypeID = obj.AccountTypeID;
                        model.ResidentialAddress = obj.ResidentialAddress;
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
                        model.Gender = obj.Gender;
                        model.Occupation = obj.Occupation;

                    }
                    else
                    {
                        msg = "Record doesn't exist anymore!";
                    }
                }

                UtilityHelpers utilityHelpers = new UtilityHelpers();

                ViewBag.AccountTypes = utilityHelpers.AccountTypeList();
                ViewBag.IDType = utilityHelpers.IdentificationTypeList();
                ViewBag.MaritalStatus = utilityHelpers.MaritalStatusList();
                ViewBag.GenderList = utilityHelpers.GetGenders();

                return PartialView("AddEditAccount", model);

            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    msg = "TRY AGAIN, IF PERSISTED, CONTACT ADMIN! \n" + ex.InnerException.Message.ToString();
                else
                    msg = "TRY AGAIN, IF PERSISTED, CONTACT ADMIN! \n" + ex.Message.ToString();
            }
            TempData["Msg"] = msg;
            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "Admin")]
        public JsonResult DeleteAccount(string ID)
        {

            bool result = false;
            if (ID != null)
            {
                var objDel = con.Accounts.SingleOrDefault(o => o.AccountNumber == ID);
                if (objDel != null)
                {
                    con.Accounts.Remove(objDel);
                    con.SaveChanges();
                    result = true;
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Admin")]
        public JsonResult SuspendAccount(string ID)
        {

            bool result = false;
            if (ID != null)
            {
                var objDel = con.Accounts.SingleOrDefault(o => o.AccountNumber == ID);
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