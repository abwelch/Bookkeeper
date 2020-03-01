using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bookkeeper.Models
{
    public partial class UserInfo
    {
        public UserInfo()
        {
            JournalTransactions = new HashSet<JournalTransaction>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserID { get; set; }
        public DateTime AccountCreation { get; set; }
        public DateTime LastActivity { get; set; }
        public int TotalCurrentTransactions { get; set; }
        public int TotalStatements { get; set; }

        public virtual ICollection<JournalTransaction> JournalTransactions { get; set; }
    }
}
