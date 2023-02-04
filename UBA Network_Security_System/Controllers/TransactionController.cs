using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
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


        #region      WITHDRAW CRUD



        [HttpGet]
        public ActionResult Withdraw(string accountNumber = null)
        {
            if (TempData["accNo"] != null)
            {
                accountNumber = TempData["accNo"].ToString();
            }

            ViewBag.CurrencyFmt = CultureInfo.CreateSpecificCulture("NG-NG");

            string Msg = null;

            if (TempData["Msg"] != null)
            {
                Msg += "\n" + TempData["Msg"].ToString();
            }

            List<Withdrawal> withdrawals = new List<Withdrawal>();
            try
            {
                if (accountNumber == null)
                {
                    Msg += "\n" + "Please provide customer account number on the box!";
                    return View();
                }
                else
                {
                    var customerWithdrawals= con.Withdrawals.Where(x => x.AccountNumber == accountNumber).ToList();
                    if (customerWithdrawals.Count > 0)
                    {
                        foreach (var csDep in customerWithdrawals)
                        {
                            withdrawals.Add(csDep);
                        }
                        ViewBag.Withdraws = withdrawals;
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

            ViewBag.WithdrawCount = withdrawals.Count;
            ViewBag.Msg = Msg;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateWithdraw(Withdrawal model)
        {
            string msg = null;

            if (ModelState.IsValid)
            {
                //GET THE RECORD
                var rec = con.Withdrawals.FirstOrDefault(x => x.Id == model.Id);
                var account = con.Accounts.FirstOrDefault(x => x.AccountNumber == model.AccountNumber
                                                          && x.IsActive == true);

                if (rec != null && account != null)
                {
                    //UPDATE
                    decimal oldAmount = rec.Amount;
                    account.Balance += oldAmount;
                    //con.SaveChanges();

                    //  CONFIRM THAT THE WITHDRAWING AMOUNT IS NOT GREATER THAN THE BALANCE
                    if (model.Amount > account.Balance)
                    {
                        msg = "SORRY, TRANSACTION FAILED DUE TO INSUFFICIENT BALANCE!";
                        TempData["accNo"] = model.AccountNumber;
                        TempData["Msg"] = msg;
                        return RedirectToAction("Withdraw");
                    }

                    rec.Amount = model.Amount;
                    account.Balance -= model.Amount;

                    int isSaved = con.SaveChanges();
                    if (isSaved > 0)
                    {
                        //CREATE LEDGER HERE
                        TransactionLedger transactionLedger = new TransactionLedger()
                        {
                            TransactionType = "Withdrawal Update",
                            TransactionId = rec.Id
                        };

                        con.TransactionLedgers.Add(transactionLedger);

                        rec.Status = true;
                        msg = "Withdrawal record has been updated successfully!";
                        rec.Remark = msg + "\n - Updated on " + DateTime.UtcNow;
                        con.SaveChanges();
                    }
                    else
                    {
                        msg = "Withdrawal record was not updated successfully!";
                    }
                }
            }
            TempData["accNo"] = model.AccountNumber;
            TempData["Msg"] = msg;
            return RedirectToAction("Withdraw");
        }

        [HttpGet]
        public ActionResult NewWithdraw()
        {
            if (TempData["Msg"] != null)
            {
                ViewBag.Msg = TempData["Msg"].ToString();
            }

            if (TempData["Model"] != null)
            {
                return View((Withdrawal)TempData["Model"]);
            }

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Withdraw(Withdrawal model)
        {
            string msg = "";


            var account = con.Accounts.FirstOrDefault(x => x.AccountNumber == model.AccountNumber
                                                         && x.IsActive == true);


            try
            {
                // JUST UPDATE

                if (account != null)
                {

                    // CREATE NEW RECORD
                    var accOperator = User.Identity.Name;

                    Withdrawal objDeposit = new Withdrawal()
                    {
                        AccountName = account.SurName + " " +
                                      account.MiddleName +
                                      account.FirstName,

                        AccountNumber = model.AccountNumber,
                        SecurePass = model.SecurePass,
                        CashaierID = con.Users.FirstOrDefault(x => x.Email == accOperator).Id,
                        Amount = model.Amount,
                        Status = false
                    };

                    string id = objDeposit.Id;

                    //  CONFIRM THAT THE WITHDRAWING AMOUNT IS NOT GREATER THAN THE BALANCE
                    if (model.Amount > account.Balance)
                    {
                        msg = "SORRY, TRANSACTION FAILED DUE TO INSUFFICIENT BALANCE!";
                        TempData["accNo"] = model.AccountNumber;
                        TempData["Msg"] = msg;
                        TempData["Model"] = model;
                        return RedirectToAction("NewWithdraw");
                    }
                    else if(account.SecurePass != model.SecurePass)
                    {
                        msg = "SORRY, AUTHENTICATION FAILED!";
                        TempData["accNo"] = model.AccountNumber;
                        TempData["Msg"] = msg;
                        TempData["Model"] = model;
                        return RedirectToAction("NewWithdraw");
                    }

                    account.Balance -= model.Amount;

                    msg = "Withdrawal transaction has been completed successfully!";

                    //var dbObjx = con.Deposits.SingleOrDefault(o => o.Id == id);
                    var dbObjAcc = con.Accounts.SingleOrDefault(o => o.AccountNumber == model.AccountNumber && o.IsActive == true);

                    objDeposit.Remark = msg;
                    if (msg.Contains("successfully"))
                    {
                        objDeposit.Status = true;

                    }
                    else
                    {
                        objDeposit.Status = false;
                        dbObjAcc.Balance += model.Amount;


                    }
                    con.Entry(account).State = EntityState.Modified;
                    con.Entry(objDeposit).State = EntityState.Added;
                    //CREATE LEDGER HERE
                    TransactionLedger transactionLedger = new TransactionLedger()
                    {
                        TransactionType = "Created Withdrawal",
                        TransactionId = objDeposit.Id
                    };

                    con.TransactionLedgers.Add(transactionLedger);
                    con.SaveChanges();

                    TempData["Msg"] = msg;
                    return RedirectToAction("NewWithdraw");

                }
                else
                {
                    TempData["Msg"] = "Account does not exist anymore!";
                    return RedirectToAction("NewWithdraw");

                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    msg = "TRY AGAIN, IF PERSISTED, CONTACT ADMIN! \n" + ex.InnerException.Message.ToString();
                else
                    msg = "TRY AGAIN, IF PERSISTED, CONTACT ADMIN! \n" + ex.Message.ToString();
            }

            TempData["Msg"] = msg;
            return RedirectToAction("NewWithdraw");

        }

        [Authorize(Roles = "Admin")]
        public ActionResult AddEditWithdraw(string transactionID)
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
                        model.AccountNumber = obj.AccountNumber;
                        model.Amount = obj.Amount;
                        model.Remark = obj.Remark;
                        model.Status = obj.Status;
                        model.Id = obj.Id;
                        model.SecurePass = obj.SecurePass;
                        model.CashaierID = obj.CashaierID;
                        model.CreatedAt = obj.CreatedAt;
                    }
                    return PartialView("AddEditWithdraw", model);

                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    msg = "TRY AGAIN, IF PERSISTED, CONTACT ADMIN! \n" + ex.InnerException.Message.ToString();
                else
                    msg = "TRY AGAIN, IF PERSISTED, CONTACT ADMIN! \n" + ex.Message.ToString();
            }
            TempData["Msg"] = msg;
            return RedirectToAction("Withdraw");
        }

        #endregion  -    END OF WITHDRAWAL
             
        #region      DEPOSIT CRUD



        [HttpGet]
        public ActionResult Deposit(string accountNumber = null)
        {
            if (TempData["accNo"] != null)
            {
                accountNumber = TempData["accNo"].ToString();
            }

            ViewBag.CurrencyFmt = CultureInfo.CreateSpecificCulture("NG-NG");

            string Msg = null;

            if (TempData["Msg"] != null)
            {
                Msg += "\n" + TempData["Msg"].ToString();
            }

            List<Deposit> deposits = new List<Deposit>();
            try
            {
                if (accountNumber == null)
                {
                    Msg += "\n" + "Please provide customer account number on the box!";
                    return View();
                }
                else
                {
                    var customerDeposit = con.Deposits.Where(x => x.AccountNumber == accountNumber).ToList();
                    if (customerDeposit.Count > 0)
                    {
                        foreach (var csDep in customerDeposit)
                        {
                            deposits.Add(csDep);
                        }
                        ViewBag.Deposits = deposits;
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

            ViewBag.DepositCount = deposits.Count;
            ViewBag.Msg = Msg;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateDepsit(Deposit model)
        {
            string msg = null;

            if (ModelState.IsValid)
            {
                //GET THE RECORD
                var rec = con.Deposits.FirstOrDefault(x => x.Id == model.Id);
                var account = con.Accounts.FirstOrDefault(x => x.AccountNumber == model.AccountNumber
                                                          && x.IsActive == true);

                if (rec != null && account != null)
                {
                    //UPDATE
                    decimal oldAmount = rec.Amount;
                    account.Balance -= oldAmount;
                    //con.SaveChanges();

                    rec.Amount = model.Amount;
                    account.Balance += model.Amount;

                    int isSaved = con.SaveChanges();
                    if (isSaved > 0)
                    {
                        //CREATE LEDGER HERE
                        TransactionLedger transactionLedger = new TransactionLedger()
                        {
                            TransactionType = "Deposit Update",
                            TransactionId = rec.Id
                        };

                        con.TransactionLedgers.Add(transactionLedger);

                        rec.Status = true;
                        msg = "Deposit record has been updated successfully!";
                        rec.Remark = msg + " - Update on " + DateTime.UtcNow;
                        con.SaveChanges();
                    }
                    else
                    {
                        msg = "Deposit record was not updated successfully!";
                    }
                }
            }
            TempData["accNo"] = model.AccountNumber;
            TempData["Msg"] = msg;
            return RedirectToAction("Deposit");
        }

        [HttpGet]
        public ActionResult NewDeposit()
        {
            if (TempData["Msg"] != null)
            {
                ViewBag.Msg = TempData["Msg"].ToString();
            }
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Deposit(Deposit model)
        {
            string msg = "";


            var account = con.Accounts.FirstOrDefault(x => x.AccountNumber == model.AccountNumber
                                                         && x.IsActive == true);


            try
            {
                // JUST UPDATE

                if (account != null)
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
                        Amount = model.Amount,
                        Status = false
                    };

                    string id = objDeposit.Id;

                    account.Balance += model.Amount;

                    msg = "Deposit transaction has been completed successfully!";

                    //var dbObjx = con.Deposits.SingleOrDefault(o => o.Id == id);
                    var dbObjAcc = con.Accounts.SingleOrDefault(o => o.AccountNumber == model.AccountNumber && o.IsActive == true);

                    objDeposit.Remark = msg;
                    if (msg.Contains("successfully"))
                    {
                        objDeposit.Status = true;

                    }
                    else
                    {
                        objDeposit.Status = false;
                        dbObjAcc.Balance -= model.Amount;


                    }
                    con.Entry(account).State = EntityState.Modified;
                    con.Entry(objDeposit).State = EntityState.Added;
                    //CREATE LEDGER HERE
                    TransactionLedger transactionLedger = new TransactionLedger()
                    {
                        TransactionType = "Created Deposit",
                        TransactionId = objDeposit.Id
                    };

                    con.TransactionLedgers.Add(transactionLedger);
                    con.SaveChanges();

                    TempData["Msg"] = msg;
                    return RedirectToAction("NewDeposit");

                }
                else
                {
                    TempData["Msg"] = "Account does not exist anymore!";
                    return RedirectToAction("NewDeposit");

                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    msg = "TRY AGAIN, IF PERSISTED, CONTACT ADMIN! \n" + ex.InnerException.Message.ToString();
                else
                    msg = "TRY AGAIN, IF PERSISTED, CONTACT ADMIN! \n" + ex.Message.ToString();
            }

            TempData["Msg"] = msg;
            return RedirectToAction("NewDeposit");

        }

        [Authorize(Roles = "Admin")]
        public ActionResult AddEditDeposit(string transactionID)
        {
            string msg = "";

            Deposit model = new Deposit();
            try
            {
                if (transactionID != null)
                {
                    //string uId = userID();
                    var obj = con.Deposits.SingleOrDefault(o => o.Id == transactionID);
                    if (obj != null)
                    {
                        model.AccountName = obj.AccountName;
                        model.AccountNumber = obj.AccountNumber;
                        model.Amount = obj.Amount;
                        model.Remark = obj.Remark;
                        model.Status = obj.Status;
                        model.Id = obj.Id;
                        model.DepositorName = obj.DepositorName;
                        model.DepositorPhone = obj.DepositorPhone;
                        model.CashaierID = obj.CashaierID;
                        model.CreatedAt = obj.CreatedAt;
                    }
                    return PartialView("AddEditDeposit", model);

                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    msg = "TRY AGAIN, IF PERSISTED, CONTACT ADMIN! \n" + ex.InnerException.Message.ToString();
                else
                    msg = "TRY AGAIN, IF PERSISTED, CONTACT ADMIN! \n" + ex.Message.ToString();
            }
            TempData["Msg"] = msg;
            return RedirectToAction("Deposit");
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

                    var account = con.Accounts.FirstOrDefault(x => x.AccountNumber == model.SenderAccountNumber
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
                                    AccountID = account.AccountNumber,
                                    CashaierID = con.Users.FirstOrDefault(x => x.Email == accOperator).Id,
                                    Amount = model.Amount
                                };

                                string id = objTranser.Id;

                                con.Transfers.Add(objTranser);
                                account.Balance -= model.Amount;
                                recieverAccount.Balance += model.Amount;

                                int isSaved = con.SaveChanges();

                                var dbObjx = con.Transfers.SingleOrDefault(o => o.Id == id);
                                var dbObjAcc = con.Accounts.SingleOrDefault(o => o.AccountNumber == model.SenderAccountNumber && o.IsActive == true);
                                var dbObjRecAcc = con.Accounts.SingleOrDefault(o => o.AccountNumber == model.RecieverAccountNumber && o.IsActive == true);


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