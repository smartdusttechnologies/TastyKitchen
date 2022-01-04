namespace Neubel.Wow.Win.Authentication.WebAPI.DTO
{
    /// <summary>
    /// Security parameters details.
    /// </summary>
    public class SecurityParameter
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Minimum capital letters allowed.
        /// </summary>
        public int MinCaps { get; set; }
        /// <summary>
        /// Minimum small charectors allowed.
        /// </summary>
        public int MinSmallChars { get; set; }
        /// <summary>
        /// Minimum special charactors allowed.
        /// </summary>
        public int MinSpecialChars { get; set; }
        /// <summary>
        /// Minimum numbers allowed.
        /// </summary>
        public int MinNumber { get; set; }
        /// <summary>
        /// Minimum length of password.
        /// </summary>
        public int MinLength { get; set; }
        /// <summary>
        /// Allow user name.
        /// </summary>
        public bool AllowUserName { get; set; }
        /// <summary>
        /// Disallow past password.
        /// </summary>
        public int DisAllPastPassword { get; set; }
        /// <summary>
        /// Disallowed charectors.
        /// </summary>
        public string DisAllowedChars { get; set; }
        /// <summary>
        /// Password change interval date.
        /// </summary>
        public int ChangeIntervalDays { get; set; }
        /// <summary>
        /// Organization id.
        /// </summary>
        public int OrgId { get; set; }
    }
}
