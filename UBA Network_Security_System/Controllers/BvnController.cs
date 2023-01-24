using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UBA_Network_Security_System.Models;

namespace UBA_Network_Security_System.Controllers
{
    public class BvnController : Controller
    {
        private ApplicationDbContext con;
        public BvnController()
        {
            con = new ApplicationDbContext();
        }


        #region      BVN CRUD

        [HttpGet]
        public ActionResult BVN(string Msg)
        {

            if (Msg != null)
            {
                ViewBag.Msg = Msg.ToString();
            }

            try
            {
                var allBVN = con.BVNBanks.ToList();
                if (allBVN != null && allBVN.Count > 0)
                {
                    ViewBag.AllBVNs = allBVN;
                    ViewBag.BVNs = allBVN.Count();
                }
            }
            catch (Exception ex)
            {
                ViewBag.msg = Session["csmsg"].ToString() + " " + ex.Message.ToString();
                return View();
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult BVN(BVNBank model)
        {
            string msg = "";

            if (ModelState.IsValid && model.BVN != null)
            {

                var dbObj = con.BVNBanks.SingleOrDefault(o => o.BVN == model.BVN);
                try
                {
                    if (model.BVN != null && dbObj != null)
                    {
                        // JUST UPDATE

                        dbObj.FirstName = model.FirstName;
                        dbObj.MiddleName = model.MiddleName;
                        dbObj.SurName = model.SurName;
                        dbObj.DOB = model.DOB;
                        dbObj.Email = model.Email;
                        dbObj.Gender = model.Gender;
                        dbObj.Phone = model.Phone;
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
            }
            catch (Exception ex)
            {
                msg = "TRY AGAIN, IF PERSISTED, CONTACT ADMIN!";
                Session["grmsg"] = msg;
                return RedirectToAction("Dashboard", msg);
            }
            return PartialView("AddEditBVN", model);
        }

        [Authorize(Roles = "SuperAdmin")]
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