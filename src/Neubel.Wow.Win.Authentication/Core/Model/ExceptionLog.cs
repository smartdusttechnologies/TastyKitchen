using System;

namespace Neubel.Wow.Win.Authentication.Core.Model
{
    public class ExceptionLog
    {
        public int OrgId { get; set; }
        public string ExceptionMsg { get; set; }
        public int ExceptionTypeId { get; set; }
        public string ExceptionType { get; set; }
        public string ExceptionSource { get; set; }
        public string ExceptionURL { get; set; }
        public string FullException { get; set; }
        public string IPAddress { get; set; }
        public DateTime ExceptionDate { get; set; }
        public string LinkedText { get; set; }
        public string LinkedRemarks { get; set; }
        public string LinkedId { get; set; }
    }
}
