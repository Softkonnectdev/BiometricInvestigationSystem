using CrimeInvestigationSystem.Models;
using CrimeInvestigationSystem.Models.Utility;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CrimeInvestigationSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class InvestigationController : Controller
    {
        private ApplicationDbContext con;
        public InvestigationController()
        {
            con = new ApplicationDbContext();
        }


        #region      CRIME CRUD

        [HttpGet]
        public ActionResult Crime(string profileId = null)
        {
            string msg = "";

            if (TempData["Msg"] != null)
            {
                ViewBag.Msg = TempData["Msg"].ToString();
            }


            try
            {

                if (TempData["profileID"] != null && TempData["profileID"].ToString() != "")
                {
                    profileId = TempData["profileID"].ToString();
                }


                var crimes = con.Crimes.Where(x => x.ProfileID == profileId).ToList();
                if (crimes.Count > 0)
                {
                    ViewBag.Crimes = crimes;
                    ViewBag.CrimeCount = crimes.Count;
                    TempData["profileID"] = profileId;
                }
                else
                    msg = "Add new Record";

            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    msg = "TRY AGAIN LATER, IF PERSISTED, CONTACT ADMIN! \n" + ex.InnerException.Message.ToString();
                else
                    msg = "TRY AGAIN LATER, IF PERSISTED, CONTACT ADMIN! \n" + ex.Message.ToString();
            }

            ViewBag.Msg = msg;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Crime(Crime model)
        {
            string msg = "";
            if (model.Id == null)
            {
                var util = new Utilities();
                var genId = util.RandomDigits(11);
                model.Id = genId;
            }

            //jsonReturningMessage jsonRMsg = null;

            try
            {


                //  PROCESS IMAGE
                // CHECK IF IMAGE1 UPLOAD FILE IS EMPTY
                if (model.PhotoUpload == null || model.PhotoUpload.ContentLength < 0)
                {
                    msg = "Please select crime photo, and Try again!";
                    TempData["profileID"] = model.ProfileID;
                    //jsonRMsg = new jsonReturningMessage()
                    //{
                    //    Profileid = model.ProfileID,
                    //    Message = msg
                    //};
                    //return Json(jsonRMsg, JsonRequestBehavior.AllowGet);
                    TempData["Msg"] = msg;
                    return RedirectToAction("Crime",new { profileId = model.ProfileID });
                }
                else if (model.PhotoUpload != null &&
                         model.PhotoUpload.ContentLength > 0 &&
                         model.PhotoUpload.FileName.ToLower().EndsWith("jpeg") ||
                         model.PhotoUpload.FileName.ToLower().EndsWith("jpg") ||
                         model.PhotoUpload.FileName.ToLower().EndsWith("png"))
                {
                    //UPLOAD NOT EMPTY

                    string crimeTitle = DateTime.Now.ToString("dd/MM/yyyy:HH:mm:ss").Replace("/", "_").Replace(":", "_") + Path.GetExtension(model.PhotoUpload.FileName);

                    string pathx = Server.MapPath("~/Uploads/Passports/" + crimeTitle);
                    model.PhotoUpload.SaveAs(pathx);

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

                                model.Photo = bytes;

                            }
                            else
                            {
                                msg = "Image conversion failed!";
                                TempData["Msg"] = msg;
                                return RedirectToAction("Crime", new { profileId = model.ProfileID });
                                //return Json(msg, JsonRequestBehavior.AllowGet);
                            }
                        }
                    }
                }
                else
                {
                    msg = "Please select crime photo in jpg, jpeg, png format, and Try again!";
                    //jsonRMsg = new jsonReturningMessage()
                    //{
                    //    Profileid = model.ProfileID,
                    //    Message = msg
                    //};
                    //return Json(jsonRMsg, JsonRequestBehavior.AllowGet);
                    TempData["Msg"] = msg;
                    return RedirectToAction("Crime", new { profileId = model.ProfileID });

                }
                if (model.ProfileID == null)
                {
                    model.ProfileID = TempData["profileID"].ToString();
                }

                TempData["profileID"] = model.ProfileID;


                var dbObj = con.Crimes.SingleOrDefault(o => o.Id == model.Id);
                if (dbObj != null)
                {
                    // JUST UPDATE - CRIME Table

                    dbObj.DefaultedLaw = model.DefaultedLaw;
                    dbObj.CommittedDate = model.CommittedDate;
                    dbObj.AddressofCrime = model.AddressofCrime;
                    dbObj.Tried = model.Tried;
                    dbObj.Type = model.Type;
                    dbObj.Photo = model.Photo;



                    con.SaveChanges();
                    msg = "Crime record has been updated successfully!";
                }
                else
                {
                    // CREATE NEW RECORD

                     model = new Crime()
                    {
                        DefaultedLaw = model.DefaultedLaw,
                        CommittedDate = model.CommittedDate,
                        AddressofCrime = model.AddressofCrime,
                        Tried = model.Tried,
                        Type = model.Type,
                        Photo = model.Photo,
                        CreatedAt = DateTime.UtcNow,
                        ProfileID = TempData["profileID"].ToString()
                    };

                    con.Crimes.Add(model);
                    TempData["profileID"] = model.ProfileID;

                    con.SaveChanges();
                    msg = "Crime record has been created successfully!";

                }

            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    msg = "TRY AGAIN, IF PERSISTED, CONTACT ADMIN! \n" + ex.InnerException.Message.ToString();
                else
                    msg = "TRY AGAIN, IF PERSISTED, CONTACT ADMIN! \n" + ex.Message.ToString();
            }

            //jsonRMsg = new jsonReturningMessage()
            //{
            //    Profileid = model.ProfileID,
            //    Message = msg
            //};
            //return Json(jsonRMsg, JsonRequestBehavior.AllowGet);
            TempData["Msg"] = msg;
            return RedirectToAction("Crime", new { profileId = model.ProfileID });

        }

        [AllowAnonymous]
        public ActionResult AddEditCrime(string crimeId)
        {
            string msg = "";
            string profileID = TempData["profileID"].ToString();


            try
            {
                ViewBag.profileID = profileID;

                //if (TempData["crimeProfileID"] != null)
                //{
                //    profileID = TempData["crimeProfileID"].ToString();
                //}

                UtilityHelpers utilityHelpers = new UtilityHelpers();

                ViewBag.CrimeTypeList = utilityHelpers.CrimeTypeList();

                if (crimeId != null)
                {
                    var obj = con.Crimes.SingleOrDefault(o => o.Id == crimeId);
                    if (obj != null)
                    {

                        Crime objCrime = new Crime()
                        {
                            DefaultedLaw = obj.DefaultedLaw,
                            CommittedDate = obj.CommittedDate,
                            AddressofCrime = obj.AddressofCrime,
                            Tried = obj.Tried,
                            Id = obj.Id,
                            Type = obj.Type,
                            CreatedAt = DateTime.UtcNow,
                            ProfileID = obj.ProfileID
                        };
                        return PartialView("AddEditCrime", objCrime);
                    }
                }
                var model = new Crime()
                {
                    ProfileID = profileID
                };

                return PartialView("AddEditCrime", model);

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


        public ActionResult ViewCrime(string crimeId = null)
        {
            if (crimeId != null)
            {
                var crime = con.Crimes.FirstOrDefault(x => x.Id == crimeId);

                if (crime != null)
                {
                    if (crime.Photo != null)
                    {
                        var base64 = Convert.ToBase64String(crime.Photo);
                        var imgSrc = String.Format("data:image/png;base64,{0}", base64);
                        ViewBag.Cert = imgSrc;
                    }
                    crime.Profile = con.Profiles.FirstOrDefault(x => x.ProfileId == crime.ProfileID);
                    return PartialView("ViewCrime", crime);
                }
                else
                {
                    HttpNotFound();
                }
            }
            TempData["Msg"] = "Sorry, the CRIME may have been dropped!";
            return RedirectToAction("Index", "Home");

        }

        #endregion

        #region     GET USER PROFILE BY THUMB PRINT
        [HttpGet]
        public ActionResult ComparePrints()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ComparePrints(ComparePrintViewModel model)
        {
            string msg = "";
            try
            {

                // CHECK IF IMAGE1 UPLOAD FILE IS EMPTY
                if (model.SuspectPrint == null ||
                    model.SuspectPrint.ContentLength < 0)
                {
                    msg = "Please select a file, Try again!";
                    ViewBag.Msg = msg;

                    return View();
                }
                else if (model.SuspectPrint != null &&
                         model.SuspectPrint.ContentLength > 0 &&
                         model.SuspectPrint.FileName.ToLower().EndsWith("tif") ||
                         model.SuspectPrint.FileName.ToLower().EndsWith("png"))
                {
                    //UPLOAD NOT EMPTY


                    // CHECK IF IMAGE1 UPLOAD FILE IS EMPTY


                    string suspectPrint = "suspectPrint_" + DateTime.Now.ToString("dd/MM/yyyy:HH:mm:ss").Replace("/", "_").Replace(":", "_") + Path.GetExtension(model.SuspectPrint.FileName);

                    string path = Server.MapPath("~/Uploads/FingerPrints/SuspectsPrint/" + suspectPrint);
                    model.SuspectPrint.SaveAs(path);


                    if (path != null)
                    {
                        string susPectmatch = null;

                        using (Image img = Image.FromFile(path))
                        {
                            var ms = new MemoryStream();

                            img.Save(ms, ImageFormat.Jpeg);

                            var bytes = ms.ToArray();

                            susPectmatch = Convert.ToBase64String(bytes);

                            var dbFingers = con.Profiles.Where(x => x.LeftThumb.Equals(susPectmatch) ||
                                               x.RightThumb.Equals(susPectmatch))
                                               .FirstOrDefault();

                            if (dbFingers != null)
                            {
                                TempData["suspect"] = dbFingers;
                                return RedirectToAction("UsersProfile", "Admin");
                            }
                            msg = "NO RECORD FOUND!";
                        }






                    }
                }
                else
                    msg = "Invalid File type, please choose jpg, png, tif only!";

                ViewBag.Msg = msg;
                return View();
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    msg = "TRY AGAIN, IF PERSISTED, CONTACT ADMIN! \n" + ex.InnerException.Message.ToString();
                else
                    msg = "TRY AGAIN, IF PERSISTED, CONTACT ADMIN! \n" + ex.Message.ToString();
            }

            ViewBag.Msg = msg;
            return View();
        }


        #endregion

    }
}