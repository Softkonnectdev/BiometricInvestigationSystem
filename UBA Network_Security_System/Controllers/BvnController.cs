using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UBA_Network_Security_System.Models;
using UBA_Network_Security_System.Models.Utility;

namespace UBA_Network_Security_System.Controllers
{
    [Authorize(Roles = "Admin,Accountant")]
    public class BvnController : Controller
    {
        private ApplicationDbContext con;
        public BvnController()
        {
            con = new ApplicationDbContext();
        }


        #region      BVN CRUD

        [HttpGet]
        public ActionResult BVN(string phone = null)
        {
            string msg = "";

            if (TempData["Msg"] != null)
            {
                ViewBag.Msg = TempData["Msg"].ToString();
            }

            List<BVNBank> bvns = new List<BVNBank>();
            try
            {


                if (phone == null && !User.IsInRole("Admin"))
                {
                    ViewBag.Msg = "Please provide customer phone number on the box!";
                    return View();
                }
                else if (User.IsInRole("Admin"))
                {
                    var customerBvns = con.BVNBanks.ToList();
                    if (customerBvns.Count > 0)
                    {
                        foreach (var bvn in customerBvns)
                        {
                            bvns.Add(bvn);
                        }
                    }
                    ViewBag.BVNs = bvns;
                }
                else
                {
                    var customerBvn = con.BVNBanks.FirstOrDefault(x => x.Phone == phone);
                    if (customerBvn != null)
                    {
                        bvns.Add(customerBvn);
                        ViewBag.BVNs = bvns;
                    }
                    else
                    {
                        ViewBag.Msg = "Sorry, No Candidate found with provided phone number!";
                        return View();
                    }
                }


            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    msg = "TRY AGAIN LATER, IF PERSISTED, CONTACT ADMIN! \n" + ex.InnerException.Message.ToString();
                else
                    msg = "TRY AGAIN LATER, IF PERSISTED, CONTACT ADMIN! \n" + ex.Message.ToString();
            }

            ViewBag.BVNCount = bvns.Count;
            ViewBag.Msg = msg;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult BVN(BVNBank model)
        {
            string msg = "";
            if (model.BVN == null)
            {
                var util = new Utilities();
                var genbvn = util.RandomDigits(11);
                model.BVN = genbvn;
            }

            if (ModelState.IsValid && model.BVN != null)
            {

                var dbObj = con.BVNBanks.SingleOrDefault(o => o.BVN == model.BVN);
                try
                {
                    if (model.BVN != null && dbObj != null)
                    {
                        // JUST UPDATE - BVN Table

                        dbObj.FirstName = model.FirstName;
                        dbObj.MiddleName = model.MiddleName;
                        dbObj.SurName = model.SurName;
                        dbObj.DOB = model.DOB;
                        dbObj.Email = model.Email;
                        dbObj.Gender = model.Gender;
                        dbObj.Phone = model.Phone;

                        //  CHECK IF THERE'S ACCOUNT WITH SAME BVN AND UPDATE IT

                        var customerAccts = con.Accounts.Where(x => x.BVN == dbObj.BVN).ToList();
                        if (customerAccts.Count > 0)
                        {
                            foreach (var acc in customerAccts)
                            {                               
                                acc.DOB= model.DOB;
                                acc.Gender = model.Gender;
                            }
                        }


                        con.SaveChanges();
                        msg = "BVN record has been updated successfully!";
                    }
                    else
                    {
                        // CREATE NEW RECORD

                        BVNBank objBvnBank = new BVNBank()
                        {
                            FirstName = model.FirstName,
                            MiddleName = model.MiddleName,
                            SurName = model.SurName,
                            DOB = model.DOB,
                            Email = model.Email,
                            Phone = model.Phone,
                            Gender = model.Gender
                        };

                        con.BVNBanks.Add(objBvnBank);
                        con.SaveChanges();
                        msg = "BVN record has been created successfully!";

                    }
                }
                catch (Exception ex)
                {
                    if (ex.InnerException != null)
                        msg = "TRY AGAIN, IF PERSISTED, CONTACT ADMIN! \n" + ex.InnerException.Message.ToString();
                    else
                        msg = "TRY AGAIN, IF PERSISTED, CONTACT ADMIN! \n" + ex.Message.ToString();
                }
            }
            else
            {
                msg = "OOOPS, PLEASE PROVIDE ALL VALUES!";
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public ActionResult AddEditBVN(string bvn)
        {
            string msg = "";

            BVNBank model = new BVNBank();

            UtilityHelpers utilityHelpers = new UtilityHelpers();

            ViewBag.GenderList = utilityHelpers.GetGenders();

            try
            {
                if (bvn != null)
                {
                    //string uId = userID();
                    var obj = con.BVNBanks.SingleOrDefault(o => o.BVN == bvn);
                    if (obj != null)
                    {
                        model.BVN = obj.BVN;
                        model.FirstName = obj.FirstName;
                        model.MiddleName = obj.MiddleName;
                        model.SurName = obj.SurName;
                        model.DOB = obj.DOB;
                        model.Email = obj.Email;
                        model.Phone = obj.Phone;
                        model.Gender = obj.Gender;
                    }
                }
                return PartialView("AddEditBVN", model);

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
        public JsonResult DeleteBVN(string bvn)
        {

            bool result = false;
            if (bvn != null)
            {
                var objDel = con.BVNBanks.SingleOrDefault(o => o.BVN == bvn);
                if (objDel != null)
                {
                    con.BVNBanks.Remove(objDel);
                    con.SaveChanges();
                    result = true;
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion


    }
}