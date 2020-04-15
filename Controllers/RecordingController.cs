using System;
using Microsoft.AspNetCore.Mvc;
using Bookkeeper.Models;
using Bookkeeper.Data;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Bookkeeper.Controllers
{
    public class RecordingController : Controller
    {
        private readonly BookkeeperContext dbContext;
        private readonly UserManager<IdentityUserExtended> userManager;
        private readonly ITransactionUtils transactionUtils;
        public RecordingController(BookkeeperContext _dbContext, ITransactionUtils _transactionUtils,
            UserManager<IdentityUserExtended> _userManager)
        {
            dbContext = _dbContext;
            transactionUtils = _transactionUtils;
            userManager = _userManager;
        }

        public IActionResult Index()
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
        public async Task<IActionResult> RecordTransactionAsync(TransactionViewModel journal)
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
                        return RedirectToAction("TransactionCommitted", new { tranID = transactionID });
                    }
                    return RedirectToAction("Index", "Home");
                #endregion [ Commit Transaction ]
                default:
                    return RedirectToAction("Index", "Home");
            }
        }

        public IActionResult TransactionCommitted(int tranID)
        {
            TransactionSummaryViewModel summary = transactionUtils.RetrieveLastUserCommitSummary(tranID);
            if (summary == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View(summary);
        }
    }
}