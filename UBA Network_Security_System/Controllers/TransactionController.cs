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
    [Authorize(Roles ="Admin, Accountant")]
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
                    var customerWithdrawals = con.Withdrawals.Where(x => x.AccountNumber == accountNumber).ToList();
                    if (customerWithdrawals.Count > 0)
                    {
                        foreach (var csDep in customerWithdrawals)
                        {
                            withdrawals.Add(csDep);
                        }
                        ViewBag.Withdraws =  withdrawals.OrderBy(x => x.CreatedAt).ToList();
                    }
                    else
                    {
                        Msg += "\n" + "Sorry, No transaction found with provided account number!";
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
                    else if (account.SecurePass != model.SecurePass)
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
                        ViewBag.Deposits = deposits.OrderBy(x => x.CreatedAt).ToList();
                    }
                    else
                    {
                        Msg += "\n" + "Sorry, No transaction found with provided account number!";
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
        public ActionResult Transfer(string rAccNumber = null, string sAccNumber = null)
        {
            if (TempData["rAccNo"] != null && TempData["sAccNo"] != null)
            {
                rAccNumber = TempData["raccNo"].ToString();
                sAccNumber = TempData["sAccNo"].ToString();
            }

            ViewBag.CurrencyFmt = CultureInfo.CreateSpecificCulture("NG-NG");

            string Msg = null;

            if (TempData["Msg"] != null)
            {
                Msg += "\n" + TempData["Msg"].ToString();
            }

            List<Transfer> transfers = new List<Transfer>();
            try
            {
                if (rAccNumber == null && sAccNumber == null)
                {
                    Msg += "\n" + "Please provide customers account number on the boxes!";
                    return View();
                }
                else
                {
                    var customerTransfers = con.Transfers.Where(x => x.SenderAccountNumber == sAccNumber &&
                                                          x.RecieverAccountNumber == rAccNumber).ToList();
                    if (customerTransfers.Count > 0)
                    {
                        foreach (var csTrans in customerTransfers)
                        {
                            transfers.Add(csTrans);
                        }
                        ViewBag.Transfers = transfers.OrderBy(x => x.CreatedAt).ToList();
                    }
                    else
                    {
                        Msg += "\n" + "Sorry, No transaction found with provided account number!";
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

            ViewBag.TransferCount = transfers.Count;
            ViewBag.Msg = Msg;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateTransfer(Transfer model)
        {
            string msg = null;

            if (ModelState.IsValid)
            {
                //GET THE RECORD
                var rec = con.Transfers.FirstOrDefault(x => x.Id == model.Id);
                var accountR = con.Accounts.FirstOrDefault(x => x.AccountNumber == model.RecieverAccountNumber
                                                          && x.IsActive == true);

                var accountS = con.Accounts.FirstOrDefault(x => x.AccountNumber == model.SenderAccountNumber
                                                          && x.IsActive == true);

                if (rec != null && accountR != null && accountS != null)
                {

                    decimal oldAmount = rec.Amount;
                    decimal recieverOldBalance = accountR.Balance;
                    decimal senderOldBalance = accountS.Balance;


                    accountR.Balance -= oldAmount;
                    accountS.Balance += oldAmount;
                  

                    rec.Amount = model.Amount;
                    //DEBIT SENDER
                    accountS.Balance -= model.Amount;

                    //CREDIT RECIEVER
                    accountR.Balance += model.Amount;

                    //  CONFIRM THAT THE WITHDRAWING AMOUNT IS NOT GREATER THAN THE BALANCE
                    if (model.Amount > accountS.Balance)
                    {
                        msg = "SORRY, TRANSACTION FAILED DUE TO INSUFFICIENT BALANCE!";
                        TempData["sAccNo"] = model.SenderAccountNumber;
                        TempData["rAccNo"] = model.RecieverAccountNumber;
                        TempData["Msg"] = msg;
                        return RedirectToAction("Transfer");
                    }

                    int isSaved = con.SaveChanges();
                    if (isSaved > 0)
                    {

                        //CREATE LEDGER HERE
                        TransactionLedger transactionLedger = new TransactionLedger()
                        {
                            TransactionType = "Transfer Update",
                            TransactionId = rec.Id
                        };

                        con.TransactionLedgers.Add(transactionLedger);

                        rec.Status = true;
                        msg = "Transfer record has been updated successfully!" + "\n" +
                            "Recorde updated on " + DateTime.UtcNow.ToString();
                        rec.Remark = msg;
                        con.SaveChanges();
                    }


                    else
                    {
                        msg = "Withdrawal record was not updated successfully!";
                    }
                }
            }
            TempData["rAccNo"] = model.RecieverAccountNumber;
            TempData["sAccNo"] = model.SenderAccountNumber;
            TempData["Msg"] = msg;
            return RedirectToAction("Transfer");
        }

        [HttpGet]
        public ActionResult NewTransfer()
        {
            if (TempData["Msg"] != null)
            {
                ViewBag.Msg = TempData["Msg"].ToString();
            }

            if (TempData["Model"] != null)
            {
                return View((Transfer)TempData["Model"]);
            }

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Transfer(Transfer model)
        {
            string msg = "";




            try
            {

                //GET THE RECORD
                var accountR = con.Accounts.FirstOrDefault(x => x.AccountNumber == model.RecieverAccountNumber
                                                          && x.IsActive == true);

                var accountS = con.Accounts.FirstOrDefault(x => x.AccountNumber == model.SenderAccountNumber
                                                          && x.IsActive == true);

                var accountant = User.Identity.Name;

                // JUST UPDATE

                if (accountR != null && accountS != null)
                {

                    //  CONFIRM THAT THE WITHDRAWING AMOUNT IS NOT GREATER THAN THE BALANCE
                    if (model.Amount > accountS.Balance)
                    {
                        msg = "SORRY, TRANSACTION FAILED DUE TO INSUFFICIENT BALANCE!";
                        TempData["sAccNo"] = model.SenderAccountNumber;
                        TempData["rAccNo"] = model.RecieverAccountNumber;
                        TempData["Msg"] = msg;
                        TempData["Model"] = model;
                        return RedirectToAction("NewTransfer");
                    }
                    else if (accountS.SecurePass != model.SecurePass)
                    {
                        msg = "SORRY, AUTHENTICATION FAILED!";
                        TempData["sAccNo"] = model.SenderAccountNumber;
                        TempData["rAccNo"] = model.RecieverAccountNumber;
                        TempData["Msg"] = msg;
                        TempData["Model"] = model;
                        return RedirectToAction("NewTransfer");
                    }

                    // CREATE NEW RECORD

                    Transfer objTranser = new Transfer()
                    {
                        RecieverAccountName = accountR.SurName + " " +
                                      accountR.MiddleName +
                                      accountR.FirstName,

                        RecieverAccountNumber = model.RecieverAccountNumber,
                        SenderAccountNumber = model.SenderAccountNumber,
                        SenderPhone = accountS.Phone,
                        SecurePass = accountS.SecurePass,
                        CashaierID = con.Users.FirstOrDefault(x => x.Email == accountant).Id,
                        Amount = model.Amount
                    };

                    string id = objTranser.Id;

                    con.Transfers.Add(objTranser);
                    accountS.Balance -= model.Amount;
                    accountR.Balance += model.Amount;

                    int isSaved = con.SaveChanges();

                    var dbObjx = con.Transfers.SingleOrDefault(o => o.Id == id);
                    var dbObjAcc = con.Accounts.SingleOrDefault(o => o.AccountNumber == model.SenderAccountNumber && o.IsActive == true);
                    var dbObjRecAcc = con.Accounts.SingleOrDefault(o => o.AccountNumber == model.RecieverAccountNumber && o.IsActive == true);


                    if (isSaved > 0)
                    {
                        dbObjx.Status = true;
                        msg = "Transfer record has been updated successfully!";
                        dbObjx.Remark = msg;
                        con.SaveChanges();


                        //CREATE LEDGER HERE
                        TransactionLedger transactionLedger = new TransactionLedger()
                        {
                            TransactionType = "Created Transfer",
                            TransactionId = dbObjx.Id
                        };

                        con.TransactionLedgers.Add(transactionLedger);
                        con.SaveChanges();

                        TempData["Msg"] = msg;
                        return RedirectToAction("NewTransfer");

                    }
                }
                else
                {
                    TempData["Msg"] = "Please make sure both accounts are correct and try again later!";
                    return RedirectToAction("NewTransfer");

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
            return RedirectToAction("NewTransfer");

        }

        [Authorize(Roles = "Admin")]
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
                    }
                    return PartialView("AddEditTransfer", model);

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
            return RedirectToAction("Transfer");
        }

        #endregion  -    END OF TRANSFER

        #region     - ALL TRANSACTIONS
        [Authorize(Roles ="Admin")]
        public ActionResult Transactions()
        {
            return View(con.TransactionLedgers.OrderBy(x => x.CreatedAt).ToList());
        }
        #endregion  - END OF ALL TRANSACTIONS

    }
}