using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CrimeInvestigationSystem.Models;
using CrimeInvestigationSystem.Models.Utility;

namespace CrimeInvestigationSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private ApplicationDbContext con;
        public AdminController()
        {
            con = new ApplicationDbContext();
        }


        #region      PROFILE CRUD



        [HttpGet]
        public ActionResult UsersProfile()
        {

            string Msg = null;

            if (TempData["Msg"] != null)
            {
                Msg += "\n" + TempData["Msg"].ToString();
            }

            try
            {
                var profiles = new List<Profile>();

                if (TempData["suspect"] != null)
                {
                    profiles.Add((Profile)TempData["suspect"]);
                    ViewBag.Profiles = profiles.ToList();

                    ViewBag.ProfileCount = profiles.Count;
                }
                else
                {
                    ViewBag.Profiles = con.Profiles.ToList();
                    ViewBag.ProfileCount = con.Profiles.Count();
                }

            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    Msg += "\n" + "TRY AGAIN LATER, IF PERSISTED, CONTACT ADMIN! \n" + ex.InnerException.Message.ToString();
                else
                    Msg += "\n" + "TRY AGAIN LATER, IF PERSISTED, CONTACT ADMIN! \n" + ex.Message.ToString();
            }

            ViewBag.Msg = Msg;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult UsersProfile(Profile model)
        {
            string msg = "";
            if (model.ProfileId == null)
            {
                var util = new Utilities();
                var genbvn = util.RandomDigits(10);
                model.ProfileId = genbvn;
            }



            var dbObj = con.Profiles.SingleOrDefault(o => o.ProfileId == model.ProfileId);
            try
            {
                if (model.ProfileId != null && dbObj != null)
                {
                    // JUST UPDATE - BVN Table

                    dbObj.FirstName = model.FirstName;
                    dbObj.MiddleName = model.MiddleName;
                    dbObj.SurName = model.SurName;
                    dbObj.Email = model.Email;
                    dbObj.Gender = model.Gender;
                    dbObj.IdentificationIssuedDate = model.IdentificationIssuedDate;
                    dbObj.IdentificationNumber = model.IdentificationNumber;
                    dbObj.IdentificationType = model.IdentificationType;
                    dbObj.IdentificationValidTill = model.IdentificationValidTill;
                    dbObj.LGA = model.LGA;
                    dbObj.State = model.State;
                    dbObj.MaritalStatus = model.MaritalStatus;
                    dbObj.IsActive = dbObj.IsActive;
                    dbObj.LeftThumb = model.LeftThumb;
                    dbObj.RightThumb = model.RightThumb;
                    dbObj.ResidentialAddress = model.ResidentialAddress;
                    dbObj.ResidentialCountry = model.ResidentialCountry;
                    dbObj.Phone = model.Phone;
                    dbObj.PermanentResident = model.PermanentResident;
                    dbObj.Occupation = model.Occupation;
                    dbObj.NextofKinName = model.NextofKinName;
                    dbObj.NextofKinPhone = model.NextofKinPhone;
                    dbObj.Nationality = model.Nationality;
                    dbObj.Passport = model.Passport ?? dbObj.Passport;
                    dbObj.LeftThumb = model.LeftThumb ?? dbObj.LeftThumb;
                    dbObj.RightThumb = model.RightThumb ?? dbObj.RightThumb;

                    //  CHECK IF THERE'S ACCOUNT WITH SAME EMAIL AND UPDATE IT


                    con.SaveChanges();

                    msg = "User profile record has been updated successfully!";

                }
                else
                {
                    // CREATE NEW RECORD

                    Profile objProfile = new Profile()
                    {
                        FirstName = model.FirstName,
                        MiddleName = model.MiddleName,
                        SurName = model.SurName,
                        DOB = model.DOB,
                        Email = model.Email,
                        Phone = model.Phone,
                        Gender = model.Gender,
                        IdentificationIssuedDate = model.IdentificationIssuedDate,
                        IdentificationNumber = model.IdentificationNumber,
                        IdentificationType = model.IdentificationType,
                        IdentificationValidTill = model.IdentificationValidTill,
                        LGA = model.LGA,
                        State = model.State,
                        MaritalStatus = model.MaritalStatus,
                        IsActive = true,
                        ResidentialAddress = model.ResidentialAddress,
                        ResidentialCountry = model.ResidentialCountry,
                        PermanentResident = model.PermanentResident,
                        Occupation = model.Occupation,
                        NextofKinName = model.NextofKinName,
                        NextofKinPhone = model.NextofKinPhone,
                        Nationality = model.Nationality,
                        CreatedOn = DateTime.UtcNow
                    };

                    con.Profiles.Add(objProfile);

                    con.SaveChanges();
                    msg = "User Profile record has been created successfully!";

                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    msg = "TRY AGAIN, IF PERSISTED, CONTACT ADMIN! \n" + ex.InnerException.Message.ToString();
                else
                    msg = "TRY AGAIN, IF PERSISTED, CONTACT ADMIN! \n" + ex.Message.ToString();
            }


            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public ActionResult AddEditUserProfile(string profileId)
        {
            string msg = "";

            //Profile model = new Profile();

            UtilityHelpers utilityHelpers = new UtilityHelpers();

            ViewBag.GenderList = utilityHelpers.GetGenders();
            ViewBag.CountryList = utilityHelpers.CountryList();
            ViewBag.StateList = utilityHelpers.StateList();
            ViewBag.IDType = utilityHelpers.IdentificationTypeList();
            ViewBag.MaritalStatus = utilityHelpers.MaritalStatusList();
            ViewBag.GenderList = utilityHelpers.GetGenders();

            try
            {
                if (profileId != "")
                {
                    //string uId = userID();
                    var obj = con.Profiles.SingleOrDefault(o => o.ProfileId == profileId);
                    if (obj != null)
                    {

                        Profile model = new Profile()
                        {
                            FirstName = obj.FirstName,
                            MiddleName = obj.MiddleName,
                            SurName = obj.SurName,
                            DOB = obj.DOB,
                            Email = obj.Email,
                            Phone = obj.Phone,
                            Gender = obj.Gender,
                            IdentificationIssuedDate = obj.IdentificationIssuedDate,
                            IdentificationNumber = obj.IdentificationNumber,
                            IdentificationType = obj.IdentificationType,
                            IdentificationValidTill = obj.IdentificationValidTill,
                            LGA = obj.LGA,
                            State = obj.State,
                            MaritalStatus = obj.MaritalStatus,
                            IsActive = obj.IsActive,
                            LeftThumb = obj.LeftThumb,
                            RightThumb = obj.RightThumb,
                            ResidentialAddress = obj.ResidentialAddress,
                            ResidentialCountry = obj.ResidentialCountry,
                            PermanentResident = obj.PermanentResident,
                            Occupation = obj.Occupation,
                            NextofKinName = obj.NextofKinName,
                            NextofKinPhone = obj.NextofKinPhone,
                            Nationality = obj.Nationality,
                            Passport = obj.Passport
                        };

                        return PartialView("AddEditProfile", model);
                    }
                }
                var util = new Utilities();

                var profile = new Profile()
                {
                    CreatedOn = DateTime.UtcNow,
                    ProfileId = util.RandomDigits(10)
                };

                return PartialView("AddEditProfile", profile);

            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    msg = "TRY AGAIN LATER, IF PERSISTED, CONTACT ADMIN! \n" + ex.InnerException.Message.ToString();
                else
                    msg = "TRY AGAIN LATER, IF PERSISTED, CONTACT ADMIN! \n" + ex.Message.ToString();

                TempData["Msg"] = msg;
                return RedirectToAction("Index", "Home");
            }
        }







        [Authorize(Roles = "Admin")]
        public ActionResult ViewProfile(string profileId = null)
        {
            if (profileId != null)
            {
                var profile = con.Profiles.FirstOrDefault(x => x.ProfileId == profileId);

                if (profile != null)
                {
                    if (profile.Passport != null)
                    {
                        var base64 = Convert.ToBase64String(profile.Passport);
                        var imgSrc = String.Format("data:image/png;base64,{0}", base64);
                        ViewBag.Cert = imgSrc;
                    }
                    return PartialView("ViewProfile", profile);
                }
                else
                {
                    HttpNotFound();
                }
            }
            TempData["Msg"] = "Sorry, the profile may have been dropped!";
            return RedirectToAction("Index", "Home");

        }



        [Authorize(Roles = "Admin")]
        public JsonResult DeleteUserProfile(string id)
        {

            bool result = false;
            if (id != null)
            {
                var objDel = con.Profiles.SingleOrDefault(o => o.ProfileId == id);
                if (objDel != null)
                {
                    con.Profiles.Remove(objDel);
                    con.SaveChanges();
                    result = true;
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }



        [HttpGet]
        public ActionResult UploadPassport()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadPassport(UploadPassportViewModel model)
        {
            try
            {
                string msg = "";

                // CHECK IF IMAGE1 UPLOAD FILE IS EMPTY
                if (model.PassportUpload == null || model.PassportUpload.ContentLength < 0)
                {
                    msg = "Please select a file, Try again!";
                    ViewBag.Msg = msg;

                    return View();
                }
                else if (model.PassportUpload != null &&
                         model.PassportUpload.ContentLength > 0 &&
                         model.Email != " " &&
                         model.PassportUpload.FileName.ToLower().EndsWith("jpg") ||
                         model.PassportUpload.FileName.ToLower().EndsWith("png"))
                {
                    //UPLOAD NOT EMPTY


                    var userProfile = con.Profiles.FirstOrDefault(x => x.Email == model.Email);
                    // CHECK IF IMAGE1 UPLOAD FILE IS EMPTY
                    if (userProfile == null)
                    {
                        msg = "Please the user does not exist!";
                        ViewBag.Msg = msg;

                        return View();
                    }






                    string passName = userProfile.Email.Replace(".", "").Replace("@", "") + Path.GetExtension(model.PassportUpload.FileName);

                    string pathx = Server.MapPath("~/Uploads/Passports/" + passName);
                    model.PassportUpload.SaveAs(pathx);

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

                                userProfile.Passport = bytes;
                                con.SaveChanges();
                                ViewBag.Msg = "Passport Upload Completed";

                            }
                            else
                            {
                                ViewBag.Msg = "Image conversion failed!";
                            }
                        }
                    }
                    else
                        ViewBag.Msg = "User Passport Upload counld not be completed successfully" +
                            " because, the image renaming failed!";

                    if (System.IO.File.Exists(pathx))
                        System.IO.File.Delete(pathx);
                    //model.PassportUpload.SaveAs(pathx);
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException.Message == null)
                {
                    TempData["Msg"] = "Please copy response and send to admin: \n" + ex.Message.ToString();
                    ViewBag.Msg = TempData["Msg"].ToString();
                    return View();
                }
                else
                {
                    TempData["Msg"] = "Please copy response and send to admin: \n" +
                                            ex.Message.ToString() + "\n" +
                                            ex.InnerException.Message.ToString();
                    ViewBag.Msg = TempData["Msg"].ToString();
                    return View();
                }
            }
            return View();
        }





        [HttpGet]
        public ActionResult UploadFingerPrint()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadFingerPrint(UploadFingerPrintViewModel model)
        {
            string msg = "";
            try
            {

                // CHECK IF IMAGE1 UPLOAD FILE IS EMPTY
                if (model.LeftThumUpload == null ||
                    model.LeftThumUpload.ContentLength < 0 ||
                    model.RightThumUpload == null ||
                    model.RightThumUpload.ContentLength < 0)
                {
                    msg = "Please select a file, Try again!";
                    ViewBag.Msg = msg;

                    return View();
                }
                else if (model.LeftThumUpload != null &&
                         model.LeftThumUpload.ContentLength > 0 &&
                         model.RightThumUpload != null &&
                         model.RightThumUpload.ContentLength > 0 &&
                         model.Email == " " &&
                         model.LeftThumUpload.FileName.ToLower().EndsWith("tif") ||
                         model.LeftThumUpload.FileName.ToLower().EndsWith("png") ||
                         model.RightThumUpload.FileName.ToLower().EndsWith("tif") ||
                         model.RightThumUpload.FileName.ToLower().EndsWith("png"))
                {
                    //UPLOAD NOT EMPTY


                    var userProfile = con.Profiles.FirstOrDefault(x => x.Email == model.Email);
                    // CHECK IF IMAGE1 UPLOAD FILE IS EMPTY
                    if (userProfile == null)
                    {
                        msg = "Please the user does not exist!";
                        ViewBag.Msg = msg;

                        return View();
                    }






                    string lThumbName = userProfile.Email.Replace(".", "").Replace("@", "") + "LT" + Path.GetExtension(model.LeftThumUpload.FileName);
                    string rThumbName = userProfile.Email.Replace(".", "").Replace("@", "") + "RT" + Path.GetExtension(model.RightThumUpload.FileName);

                    string pathx = Server.MapPath("~/Uploads/FingerPrints/" + lThumbName);
                    model.LeftThumUpload.SaveAs(pathx);

                    string pathy = Server.MapPath("~/Uploads/FingerPrints/" + rThumbName);
                    model.RightThumUpload.SaveAs(pathy);


                    string lThumb = null;
                    string rThumb = null;

                    //  FOR LEFT THUMB

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
                                lThumb = Convert.ToBase64String(bytes);
                            }
                        }
                    }

                    //  FOR RIGHT THUMB

                    if (pathy != null)
                    {
                        using (Image img = Image.FromFile(pathy))
                        {
                            var ms = new MemoryStream();

                            img.Save(ms, ImageFormat.Jpeg);

                            var bytes = ms.ToArray();

                            // NOW SAVE BYTE WITH STUDENT REGNO NUMBER
                            if (bytes.Length > 0)
                            {
                                rThumb = Convert.ToBase64String(bytes);
                            }
                        }
                    }



          
                    if (lThumb != null && rThumb != null)
                    {
                       
                        userProfile.RightThumb = rThumb;
                        userProfile.LeftThumb = lThumb;

                        con.Entry(userProfile).State = EntityState.Modified;

                        con.SaveChanges();
                        msg = "Finger print uploaded successfully!";
                    }
                    else
                        msg = "Finger print uploaded FAILED!";

                    if (System.IO.File.Exists(pathx))
                        System.IO.File.Delete(pathx);

                    if (System.IO.File.Exists(pathy))
                        System.IO.File.Delete(pathy);


                    ViewBag.Msg = msg;
                    return View();
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException.Message == null)
                {
                    TempData["Msg"] = "Please copy response and send to admin: \n" + ex.Message.ToString();
                    ViewBag.Msg = TempData["Msg"].ToString();
                    return View();
                }
                else
                {
                    TempData["Msg"] = "Please copy response and send to admin: \n" +
                                            ex.Message.ToString() + "\n" +
                                            ex.InnerException.Message.ToString();
                    ViewBag.Msg = TempData["Msg"].ToString();
                    return View();
                }
            }
            return RedirectToAction("Index","Home");
        }











        #endregion




    }
}