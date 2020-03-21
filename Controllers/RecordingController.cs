using System;
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
                // Pass current datetime as default value
                DefaultEntryDate = DateTime.Now,
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

            switch (journal.Action)
            {
                case JournalAction.AddHeader:
                    return View(journal);
                case JournalAction.EditHeader:
                    return View(journal);
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