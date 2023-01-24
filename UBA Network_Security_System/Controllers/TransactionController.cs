using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UBA_Network_Security_System.Models;

namespace UBA_Network_Security_System.Controllers
{
    public class TransactionController : Controller
    {
        private ApplicationDbContext con;
        public TransactionController()
        {
            con = new ApplicationDbContext();
        }

        #region      WITHDRAWAL CRUD

        [HttpGet]
        public ActionResult Withdrawal(string Msg)
        {

            if (Msg != null)
            {
                ViewBag.Msg = Msg.ToString();
            }

            try
            {
                var allWithdrawal = con.Withdrawals.ToList();
                if (allWithdrawal != null && allWithdrawal.Count > 0)
                {
                    ViewBag.AllWithdrawals = allWithdrawal;
                    ViewBag.Withdrawals = allWithdrawal.Count();
                }
            }
            catch (Exception ex)
            {
                ViewBag.Msg = ex.Message.ToString();
                return View();
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Withdrawal(Withdrawal model)
        {
            string msg = "";

            if (ModelState.IsValid)
            {

                var dbObj = con.Withdrawals.SingleOrDefault(o => o.Id == model.Id);

                var account = con.Accounts.FirstOrDefault(x => x.Id == model.AccountID
                                                             && x.AccountNumber == model.AccountNumber
                                                             && x.IsActive == true
                                                             && x.SecurePass == model.SecurePass);


                try
                {
                    var empOperatorEmail = User.Identity.Name;
                    var empOperator = con.Users.FirstOrDefault(x => x.Email == empOperatorEmail);
                    // JUST UPDATE

                    if (account != null)
                    {
                        if (dbObj != null)
                        {
                            decimal oldAmount = dbObj.Amount;
                            account.Balance += oldAmount;
                            //con.SaveChanges();

                            dbObj.Amount = model.Amount;
                            account.Balance -= model.Amount;

                            con.SaveChanges();

                            msg = "Withdrawal record has been updated successfully!";
                            dbObj.Remark = msg;
                            con.SaveChanges();
                        }
                        else
                        {
                            // CREATE NEW RECORD

                            Withdrawal objWithdrawal = new Withdrawal()
                            {
                                AccountName = account.SurName + " " +
                                              account.MiddleName +
                                              account.FirstName,

                                AccountNumber = model.Account.AccountNumber,
                                SecurePass = account.SecurePass,
                                CashaierID = empOperator.Id,
                                Amount = model.Amount
                            };

                            string id = objWithdrawal.Id;

                            con.Withdrawals.Add(objWithdrawal);
                            account.Balance -= model.Amount;

                            int isSaved = con.SaveChanges();
                            var dbObjx = con.Withdrawals.SingleOrDefault(o => o.Id == id);
                            var dbObjAcc = con.Accounts.SingleOrDefault(o => o.Id == model.AccountNumber && o.IsActive == true);

                            if (isSaved > 0)
                            {
                                msg = "Withdrawal transaction has been completed successfully!";
                                dbObjx.Remark = msg;

                                dbObjx.Status = true;
                                con.SaveChanges();
                            }
                            else
                            {
                                msg = "Withdrawal transaction was not completed successfully!";
                                dbObjx.Remark = msg;
                                dbObjx.Status = false;
                                dbObjAcc.Balance += model.Amount;
                                con.SaveChanges();
                            }
                        }
                    }
                    else
                    {
                        msg = "Account does not exist anymore!";
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

        [Authorize(Roles = "MD, BranchOfficer")]
        public ActionResult AddEditWithdrawal(string transactionID)
        {
            string msg = "";

            Withdrawal model = new Withdrawal();
            try
            {
                if (transactionID != null)
                {
                    //string uId = userID();
                    var obj = con.Withdrawals.SingleOrDefault(o => o.Id == transactionID);
                    if (obj != null)
                    {
                        model.Id = obj.Id;
                        model.AccountName = obj.AccountName;
                        model.AccountID = obj.AccountID;
                        model.AccountNumber = obj.AccountNumber;
                        model.Amount = obj.Amount;
                        model.Remark = obj.Remark;
                        model.Status = obj.Status;
                        model.SecurePass = obj.SecurePass;
                    }
                }
            }
            catch (Exception ex)
            {
                msg = "TRY AGAIN, IF PERSISTED, CONTACT ADMIN!";
                Session["grmsg"] = msg;
                return RedirectToAction("Dashboard", msg);
            }
            return PartialView("AddEditWithdrawal", model);
        }

        #endregion  -    END OF WITHDRAWAL

        #region      DEPOSIT CRUD

        [HttpGet]
        public ActionResult Deposit(string Msg)
        {

            if (Msg != null)
            {
                ViewBag.Msg = Msg.ToString();
            }

            try
            {
                var allDeposit = con.Deposits.ToList();
                if (allDeposit != null && allDeposit.Count > 0)
                {
                    ViewBag.AllDeposits = allDeposit;
                    ViewBag.Deposits = allDeposit.Count();
                }
            }
            catch (Exception ex)
            {
                ViewBag.Msg = ex.Message.ToString();
                return View();
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Deposit(Deposit model)
        {
            string msg = "";

            if (ModelState.IsValid)
            {

                var dbObj = con.Deposits.SingleOrDefault(o => o.Id == model.Id);

                var account = con.Accounts.FirstOrDefault(x => x.Id == model.AccountID
                                                             && x.AccountNumber == model.AccountNumber
                                                             && x.IsActive == true);


                try
                {
                    // JUST UPDATE

                    if (account != null)
                    {
                        if (dbObj != null)
                        {
                            decimal oldAmount = dbObj.Amount;
                            account.Balance -= oldAmount;
                            //con.SaveChanges();

                            dbObj.Amount = model.Amount;
                            account.Balance += model.Amount;

                            int isSaved = con.SaveChanges();
                            if (isSaved > 0)
                            {
                                dbObj.Status = true;
                                msg = "Deposit record has been updated successfully!";
                                dbObj.Remark = msg;
                                con.SaveChanges();
                            }
                            else
                            {
                                dbObj.Status = false;
                                account.Balance -= model.Amount;
                                msg = "Deposit record was not updated successfully!";
                                dbObj.Remark = msg;
                                con.SaveChanges();
                            }
                        }
                        else
                        {
                            // CREATE NEW RECORD
                            var accOperator = User.Identity.Name;

                            Deposit objDeposit = new Deposit()
                            {
                                AccountName = account.SurName + " " +
                                              account.MiddleName +
                                              account.FirstName,

                                AccountNumber = model.AccountNumber,
                                DepositorName = model.DepositorName,
                                DepositorPhone = model.DepositorPhone,
                                CashaierID = con.Users.FirstOrDefault(x => x.Email == accOperator).Id,
                                Amount = model.Amount
                            };

                            string id = objDeposit.Id;

                            con.Deposits.Add(objDeposit);
                            account.Balance += model.Amount;

                            con.SaveChanges();
                            msg = "Deposit transaction has been completed successfully!";

                            var dbObjx = con.Deposits.SingleOrDefault(o => o.Id == id);
                            var dbObjAcc = con.Accounts.SingleOrDefault(o => o.Id == model.AccountNumber && o.IsActive == true);

                            dbObjx.Remark = msg;
                            if (msg.Contains("successfully"))
                            {
                                dbObjx.Status = true;
                                con.SaveChanges();
                            }
                            else
                            {
                                dbObjx.Status = false;
                                dbObjAcc.Balance -= model.Amount;
                                con.SaveChanges();
                            }
                        }
                    }
                    else
                    {
                        msg = "Account does not exist anymore!";
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

        [Authorize(Roles = "MD, BranchOfficer")]
        public ActionResult AddEditDeposit(string transactionID)
        {
            string msg = "";

            Withdrawal model = new Withdrawal();
            try
            {
                if (transactionID != null)
                {
                    //string uId = userID();
                    var obj = con.Withdrawals.SingleOrDefault(o => o.Id == transactionID);
                    if (obj != null)
                    {
                        model.AccountName = obj.AccountName;
                        model.AccountID = obj.AccountID;
                        model.AccountNumber = obj.AccountNumber;
                        model.Amount = obj.Amount;
                        model.Remark = obj.Remark;
                        model.Status = obj.Status;
                        model.Id = obj.Id;
                        model.SecurePass = obj.SecurePass;
                    }
                }
            }
            catch (Exception ex)
            {
                msg = "TRY AGAIN, IF PERSISTED, CONTACT ADMIN!";
                Session["grmsg"] = msg;
                return RedirectToAction("Dashboard", msg);
            }
            return PartialView("AddEditDeposit", model);
        }

        #endregion  -    END OF DEPOSIT

        #region      TRANSFER CRUD

        [HttpGet]
        public ActionResult Transfer(string Msg)
        {

            if (Msg != null)
            {
                ViewBag.Msg = Msg.ToString();
            }

            try
            {
                var allTransfer = con.Transfers.ToList();
                if (allTransfer != null && allTransfer.Count > 0)
                {
                    ViewBag.AllTransfers = allTransfer;
                    ViewBag.Transfers = allTransfer.Count();
                }
            }
            catch (Exception ex)
            {
                ViewBag.Msg = ex.Message.ToString();
                return View();
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Transfer(Transfer model)
        {
            string msg = "";

            if (ModelState.IsValid)
            {


                try
                {
                    // CONTINUE FROM HERE BY VERIFYING THE ACCOUNT NUMBER
                    // YOU WANT TO TRANSFER TO...
                    var dbObj = con.Transfers.SingleOrDefault(o => o.Id == model.Id);

                    var account = con.Accounts.FirstOrDefault(x => x.Id == model.AccountID
                                                                 && x.AccountNumber == model.SenderAccountNumber
                                                                 && x.IsActive == true
                                                                 && x.SecurePass == model.SecurePass);


                    var accOperator = User.Identity.Name;
                    var empOperator = con.Users.FirstOrDefault(x => x.Email == accOperator);

                    if (account != null)
                    {

                        var recieverAccount = con.Accounts.FirstOrDefault(rec => rec.AccountNumber == model.RecieverAccountNumber);

                        if (recieverAccount != null)
                        {
                            if (dbObj != null)
                            {

                                decimal oldAmount = dbObj.Amount;
                                decimal recieverOldBalance = recieverAccount.Balance;
                                decimal senderOldBalance = account.Balance;


                                recieverAccount.Balance -= oldAmount;
                                account.Balance += oldAmount;
                                //con.SaveChanges();

                                dbObj.Amount = model.Amount;
                                //DEBIT SENDER
                                account.Balance -= model.Amount;

                                //CREDIT RECIEVER
                                recieverAccount.Balance += model.Amount;


                                int isSaved = con.SaveChanges();
                                if (isSaved > 0)
                                {
                                    dbObj.Status = true;
                                    msg = "Transfer record has been updated successfully!";
                                    dbObj.Remark = msg;
                                    con.SaveChanges();
                                }
                                else
                                {
                                    dbObj.Status = false;
                                    account.Balance += model.Amount;
                                    recieverAccount.Balance -= model.Amount;
                                    msg = "Transfer record was not updated successfully!";
                                    dbObj.Remark = msg;
                                    con.SaveChanges();
                                }

                            }
                            else
                            {
                                // CREATE NEW RECORD

                                Transfer objTranser = new Transfer()
                                {
                                    RecieverAccountName = recieverAccount.SurName + " " +
                                                  recieverAccount.MiddleName +
                                                  recieverAccount.FirstName,

                                    RecieverAccountNumber = model.RecieverAccountNumber,
                                    SenderAccountNumber = model.SenderAccountNumber,
                                    SenderPhone = account.Phone,
                                    SecurePass = account.SecurePass,
                                    AccountID = account.Id,
                                    CashaierID = con.Users.FirstOrDefault(x => x.Email == accOperator).Id,
                                    Amount = model.Amount
                                };

                                string id = objTranser.Id;

                                con.Transfers.Add(objTranser);
                                account.Balance -= model.Amount;
                                recieverAccount.Balance += model.Amount;

                                int isSaved = con.SaveChanges();

                                var dbObjx = con.Transfers.SingleOrDefault(o => o.Id == id);
                                var dbObjAcc = con.Accounts.SingleOrDefault(o => o.Id == model.SenderAccountNumber && o.IsActive == true);
                                var dbObjRecAcc = con.Accounts.SingleOrDefault(o => o.Id == model.RecieverAccountNumber && o.IsActive == true);


                                if (isSaved > 0)
                                {
                                    dbObjx.Status = true;
                                    msg = "Transfer record has been updated successfully!";
                                    dbObj.Remark = msg;
                                    con.SaveChanges();
                                }
                                else
                                {
                                    dbObj.Status = false;
                                    account.Balance += model.Amount;
                                    recieverAccount.Balance -= model.Amount;
                                    msg = "Transfer record was not updated successfully!";
                                    dbObj.Remark = msg;
                                    con.SaveChanges();
                                }

                            }
                        }
                        else
                        {
                            msg = "Reciever Account does not exist anymore!";
                        }

                    }
                    else
                    {
                        msg = "Account does not exist anymore!";
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

        [Authorize(Roles = "MD, BranchOfficer")]
        public ActionResult AddEditTransfer(string transactionID)
        {
            string msg = "";

            Transfer model = new Transfer();
            try
            {
                if (transactionID != null)
                {
                    //string uId = userID();
                    var obj = con.Transfers.SingleOrDefault(o => o.Id == transactionID);
                    if (obj != null)
                    {
                        model.SenderAccountNumber = obj.SenderAccountNumber;
                        model.SenderPhone = obj.SenderPhone;
                        model.RecieverAccountName = obj.RecieverAccountName;
                        model.RecieverAccountNumber = obj.RecieverAccountNumber;
                        model.CashaierID = obj.CashaierID;
                        model.Amount = obj.Amount;
                        model.SecurePass = obj.SecurePass;
                        model.Id = obj.Id;
                        model.Remark = obj.Remark;
                        model.Status = obj.Status;
                        model.AccountID = obj.AccountID;
                    }
                }
            }
            catch (Exception ex)
            {
                msg = "TRY AGAIN, IF PERSISTED, CONTACT ADMIN!";
                Session["grmsg"] = msg;
                return RedirectToAction("Dashboard", msg);
            }
            return PartialView("AddEditTransfer", model);
        }

        #endregion  -    END OF TRANSFER
    }
}