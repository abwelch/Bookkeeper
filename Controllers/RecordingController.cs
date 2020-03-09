using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Bookkeeper.Models;
using Bookkeeper.Data;

namespace Bookkeeper.Controllers
{
    public class RecordingController : Controller
    {
        private readonly BookkeeperContext dbContext;

        public RecordingController(BookkeeperContext _dbContext) 
        {
            dbContext = _dbContext;
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
                JournalHeader = new JournalHeaderViewModel(),
                JournalLineItem = new JournalLineItemViewModel(),
                PreviousEntries = new List<JournalLineItemViewModel>(),
                DefaultEntryDate = DateTime.Now,
                Action = -1,
                ActionItemIndex = -1
            };
            return View(journal);
        }

        [HttpPost]
        public IActionResult RecordTransaction(TransactionViewModel journal)
        {
            if (!ModelState.IsValid)
            {
                return View(journal);
            }
            // Determine action to take
            switch ((JournalAction)journal.Action)
            {
                case JournalAction.AddHeader:
                    if (journal.JournalHeader == null)
                    {
                        return RedirectToAction("Index", "Home");

                    }
                    break;
                case JournalAction.EditHeader:

                    break;
                case JournalAction.AddItem:

                    break;
                case JournalAction.EditItem:

                    break;
                case JournalAction.DeleteItem:

                    break;
                case JournalAction.CommitTransaction:

                    break;
                default:
                    return RedirectToAction("Index", "Home");
            }
            

            return View(journal);
        }
    }
}