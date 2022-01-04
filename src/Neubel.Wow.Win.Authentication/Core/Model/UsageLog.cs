using System;

namespace Neubel.Wow.Win.Authentication.Core.Model
{
    public class UsageLog
    {
        public int OrgId { get; set; }

        public string PerformedAction { get; set; }

        public string IPAddress { get; set; }

        public DateTime UsageDate { get; set; }

        public string DeviceName { get; set; }

        public string LinkedId { get; set; }

        public string LinkedText { get; set; }

        public string LinkedRemarks { get; set; }
    }
}
