using System;
using Microsoft.AspNetCore.Mvc;
using Bookkeeper.Models;
using Bookkeeper.Data;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Bookkeeper.Controllers
{
    public class RecordingController : Controller
    {
        private readonly BookkeeperContext dbContext;
        private readonly UserManager<IdentityUserExtended> userManager;
        private readonly ITransactionUtils transactionUtils;
        private readonly IUserInfoUtils userInfoUtils;
        public RecordingController(BookkeeperContext _dbContext, ITransactionUtils _transactionUtils,
            UserManager<IdentityUserExtended> _userManager,
            IUserInfoUtils _userInfoUtils)
        {
            dbContext = _dbContext;
            transactionUtils = _transactionUtils;
            userManager = _userManager;
            userInfoUtils = _userInfoUtils;
        }

        public IActionResult Overview()
        {
            return View();
        }

        [HttpGet]
        public IActionResult RecordTransaction()
        {
            TransactionViewModel journal = new TransactionViewModel()
            {
                // Pass current datetime as default value
                DefaultEntryDate = DateTime.Now,
            };
            return View(journal);
        }

        [HttpPost]
        public async Task<IActionResult> RecordTransaction(TransactionViewModel journal)
        {
            if (!ModelState.IsValid)
            {
                return View(journal);
            }
            IdentityUserExtended currentUser = null;
            if (User.Identity.IsAuthenticated)
            {
                currentUser = await userManager.GetUserAsync(User);
            }
            switch (journal.Action)
            {
                case JournalAction.AddHeader:
                    return View(journal);
                case JournalAction.EditHeader:
                    return View(journal);
                case JournalAction.AddItem:
                    #region [ Add Item ]
                    // Check that journalLineitem exists and that debit/credit amounts do not both contian values and do not both contain nothing
                    if (journal.JournalLineItem != null
                        && !(journal.JournalLineItem.DebitAmount != null && journal.JournalLineItem.CreditAmount != null)
                        && !(journal.JournalLineItem.DebitAmount == null && journal.JournalLineItem.CreditAmount == null)
                        && journal.JournalLineItem.AccountName != null)
                    {
                        journal.PreviousEntries.Add(journal.JournalLineItem);
                    }
                    return View(journal);
                #endregion [ Add Item ]
                case JournalAction.EditItem:
                    #region [ Edit Item ]
                    {
                        // Should not occur unless tampered with on client side 
                        // (could make this more robust in future by adding arbitrary amount to i on front-end before passing)
                        int index = -1;
                        for (int i = 0; i < journal.PreviousEntries.Count; ++i)
                        {
                            if (journal.PreviousEntries[i].ActionItemIndex == 111)
                            {
                                index = i;
                            }
                        }
                        if (index == -1)
                        {
                            return RedirectToAction("Index", "Home");
                        }

                        return View(journal);
                    }
                #endregion [ Edit Item ]
                case JournalAction.CompleteEdit:
                    return View(journal);
                case JournalAction.DeleteItem:
                    #region [ Delete item ]
                    {
                        int index = -1;
                        for (int i = 0; i < journal.PreviousEntries.Count; ++i)
                        {
                            if (journal.PreviousEntries[i].ActionItemIndex == 111)
                            {
                                index = i;
                            }
                        }
                        if (index == -1)
                        {
                            return RedirectToAction("Index", "Home");
                        }
                        journal.PreviousEntries.RemoveAt(index);
                        return View(journal);
                    }
                #endregion [ Delete item ]
                case JournalAction.CommitTransaction:
                    #region [ Commit Transaction ]
                    journal.UserID = (currentUser != null) ? currentUser.UserInfoID : 0;
                    int transactionID = transactionUtils.CommitTransaction(journal);
                    if (transactionID != -1)
                    {
                        userInfoUtils.IncrementTotalTransactions(currentUser.UserInfoID);
                        return RedirectToAction("TransactionCommitted", new { tranID = transactionID });
                    }
                    return RedirectToAction("Index", "Home");
                #endregion [ Commit Transaction ]
                case JournalAction.EditTransaction:
                    return View(journal);
                default:
                    return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        public IActionResult TransactionCommitted(int tranID)
        {
            TransactionSummaryViewModel summary = transactionUtils.RetrieveLastUserCommitSummary(tranID);
            if (summary == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View(summary);
        }

        [HttpGet]
        public async Task<IActionResult> Journal()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return View(null);
            }
            IdentityUserExtended currentUser = await userManager.GetUserAsync(User);
            JournalViewModel journal = transactionUtils.RetrieveAllTransactionsByUser(currentUser.UserInfoID);
            return View(journal);
        }

        [HttpPost]
        public IActionResult Journal(JournalViewModel input)
        {
            int validID = input.TransactionIDs.Find(x => x > -1);
            if (validID != 0)
            {
                switch (input.Action)
                {
                    case JournalAction.EditTransaction:
                        TransactionViewModel model = transactionUtils.ConvertToTransactionViewModel(validID);
                        if (model != null)
                        {
                            model.Action = JournalAction.EditTransaction;
                            return View("RecordTransaction", model);
                        }
                        return RedirectToAction("Journal");
                    case JournalAction.DeleteTransaction:
                        transactionUtils.DeleteTransaction(validID);
                        return RedirectToAction("Journal");
                }
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public async Task<IActionResult> GeneralLedger()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return View(null);
            }
            IdentityUserExtended currentUser = await userManager.GetUserAsync(User);
            GeneralLedgerViewModel accounts = new GeneralLedgerViewModel()
            {
                AllAccounts = transactionUtils.Post(currentUser.UserInfoID)
            };
            return View(accounts);
        }
    }
}