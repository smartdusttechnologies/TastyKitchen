using System;

namespace Neubel.Wow.Win.Authentication.Core.Model
{
    public class TransactionLog
    {
        public int OrgId { get; set; }

        public string TransactionName { get; set; }

        public string TransactionType { get; set; }

        public int TransactionTypeId { get; set; }

        public DateTime TransactionDate { get; set; }

        public string LinkedId { get; set; }

        public string LinkedText { get; set; }

        public string LinkedRemarks { get; set; }
    }
}
