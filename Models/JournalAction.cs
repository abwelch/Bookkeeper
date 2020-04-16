using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookkeeper.Models
{
    public static class JournalAction
    {
        public const string AddHeader = "AddHeader";
        public const string EditHeader = "EditHeader";
        public const string AddItem = "AddItem";
        public const string EditItem = "EditItem";
        public const string CompleteEdit = "CompleteEdit";
        public const string DeleteItem = "DeleteItem";
        public const string CommitTransaction = "CommitTransaction";
        public const string EditTransaction = "EditTransaction";
        public const string DeleteTransaction = "DeleteTransaction";
    }
}
