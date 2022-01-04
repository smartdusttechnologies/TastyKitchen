using System;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Formatting;
using System.Timers;
using Newtonsoft.Json;
using System.IO;

namespace Neubel.Wow.Win.Authentication.Common
{
    public static class GenericUtil
    {
        // This size of the IV (in bytes) must = (keysize / 8).  Default keysize is 256, so the IV must be
        // 32 bytes long.  Using a 16 character string here gives us 32 bytes when converted to a byte array.
        //private const string initVector = "pemgail9uzpgzl88";
        private const String initVector = "neubelatomvngt14";
        private const String passPhrase = "guru";
        // This constant is used to determine the keysize of the encryption algorithm
        private const int keysize = 256;
        //Encrypt
        public static string EncryptString(string plainText)
        {
            byte[] initVectorBytes = Encoding.UTF8.GetBytes(initVector);
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, null);
            byte[] keyBytes = password.GetBytes(keysize / 8);
            RijndaelManaged symmetricKey = new RijndaelManaged();
            symmetricKey.Mode = CipherMode.CBC;
            ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
            cryptoStream.FlushFinalBlock();
            byte[] cipherTextBytes = memoryStream.ToArray();
            memoryStream.Close();
            cryptoStream.Close();
            return Convert.ToBase64String(cipherTextBytes);
        }
        //Decrypt
        public static string DecryptString(string cipherText)
        {
            byte[] initVectorBytes = Encoding.UTF8.GetBytes(initVector);
            byte[] cipherTextBytes = Convert.FromBase64String(cipherText);
            PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, null);
            byte[] keyBytes = password.GetBytes(keysize / 8);
            RijndaelManaged symmetricKey = new RijndaelManaged();
            symmetricKey.Mode = CipherMode.CBC;
            ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);
            MemoryStream memoryStream = new MemoryStream(cipherTextBytes);
            CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            byte[] plainTextBytes = new byte[cipherTextBytes.Length];
            int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
            memoryStream.Close();
            cryptoStream.Close();
            return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
        }

        //public static String getConnStr()
        //{
        //    return System.Configuration.ConfigurationManager.ConnectionStrings["KromeDBAsync"].ConnectionString;
        //}

        public static String NewRefreshToken = Guid.NewGuid().ToString();

        public static string NewAccessToken()
        {
            int size = 64;
            char[] chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890@#$*=!~-+_".ToCharArray();
            byte[] data = new byte[size];
            using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
            {
                crypto.GetBytes(data);
            }
            StringBuilder result = new StringBuilder(size);
            foreach (byte b in data)
            {
                result.Append(chars[b % (chars.Length)]);
            }
            return result.ToString();
        }

        //Todo: Revisit this section
        public enum ResponseCodes
        {
            Success = 1,
            InvalidParameters = 2,
            LoginFailed = 3,
            UserValidationPending = 4,
            UnknownError = 5,
            UserNotAvailable = 6,
            InvalidMobileOTP = 7,
            InvalidMobileNumber = 8,
            InvalidEmailId = 9,
            UserDisabled = 10,
            SessionExpired = 11,
            InvalidToken = 12,
            DuplicateEmail = 13,
            DuplicateMobile = 14,
            OTPValidationPending = 15,
            DBError = 16,
            PasswordNotSet = 17,
            ServiceNotAvailableForCountry = 18,
            UserAlreadyExists = 19,
            InvalidRefreshToken = 20,
            PasswordAlreadyExists = 21,
            Unauthorized = 22,
            AlreadyLatest = 23,
            ForeignKeyError = 24
        }

        public static List<T> ToNonNullList<T>(this IEnumerable<T> obj)
        {
            return obj == null ? new List<T>() : obj.ToList();
        }

        public static int GenerateOTP()
        {

            char[] charArr = "0123456789".ToCharArray();
            string strrandom = string.Empty;
            Random random = new Random();
            int noofchars = Convert.ToInt32(6);
            for (int i = 0; i < noofchars; i++)
            {
                //It will not allow repitition of Numbers
                int pos = random.Next(1, charArr.Length);
                if (!strrandom.Contains(charArr.GetValue(pos).ToString()))
                    strrandom += charArr.GetValue(pos);
                else
                    i--;
            }
            return Convert.ToInt32(strrandom);
        }

        public enum OTPTransType
        {
            NewRegistration = 0,
            PasswordReset = 1,
            HomeShareNewUser = 2,
            HomeShareExistingUser = 3,
            ResetEmail = 4,
            ResetMobile = 5
        }

        public static String GetPasswordHash(String pPwd)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(pPwd));

            //get hash result after compute it  
            byte[] result = md5.Hash;

            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                //change it into 2 hexadecimal digits for each byte  
                strBuilder.Append(result[i].ToString("x2"));
            }

            return strBuilder.ToString();
        }
    }

    public class KromeEmail
    {
        public KromeEmail() { }

        String[] tMessage = { " is the OTP for your Krome Automation account registration <br><br> - Krome Home Automation",
            " is the OTP to reset your Krome Password <br><br>- Krome Home Automation",
            "You have access to new Krome Residence. Please download Krome application from appstore and register to use it",
            "You have access to new Krome Residence. New Home will appear in your Krome Home Automation Application",
            " to Verify and Reset your email Id for your Krome Account. <br><br> - Krome Home Automation",
            " to Verify and Reset your Mobile Number for your Krome Account. <br><br> - Krome Home Automation"};
        String[] tSubject = { "KROME OTP", "KROME OTP", "Access to New Krome Residence", "Access to New Krome Residence",
                "Change Krome Account Email address", "Change Krome Account Mobile Number" };
        Timer sendTimer = new Timer();
        MailMessage mailMsg = new MailMessage();

        public void SendEmail(String toAddress, String pOTP, GenericUtil.OTPTransType pType)
        {
            mailMsg.IsBodyHtml = true;
            if ((int)pType == 2)
            {
                mailMsg.Body = tMessage[(int)pType] + "<br />New Password - " + pOTP;
            }
            else
            {
                mailMsg.Body = pOTP + tMessage[(int)pType];
            }
            mailMsg.Subject = tSubject[(int)pType];
            mailMsg.To.Add(new MailAddress(toAddress));
            mailMsg.Bcc.Add(new MailAddress("guru@neubeltech.com"));
            mailMsg.From = new MailAddress("support@neubeltech.com");

            sendTimer.Elapsed += new ElapsedEventHandler(TimerTrigger);
            sendTimer.Interval = 100;
            sendTimer.Enabled = true;
        }

        private async void TimerTrigger(object sender, ElapsedEventArgs e)
        {
            sendTimer.Enabled = false;
            await SendEmailAsync();
        }

        private async Task SendEmailAsync()
        {
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.UseDefaultCredentials = true;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.Credentials = new System.Net.NetworkCredential("support@neubeltech.com", "N3u6nt*up!");// Enter senders User name and password
            smtp.EnableSsl = true;
            String MsgTo = mailMsg.To.ToString();
            await smtp.SendMailAsync(mailMsg);
        }
    }

    //ToDo: User different sms message type (authkey) for general messages
    public class KromeSMS
    {
        public KromeSMS() { }

        String[] tMessage = { " is the OTP for your Krome Automation account registration - Krome Home Automation",
            " is the OTP to reset your Krome Password - Krome Home Automation",
            "You have access to new Krome Residence. Please download Krome application from appstore and register to use it",
            "You have access to new Krome Residence. New Home will appear in your Krome Home Automation Application",
            "Email Message has been sent to your mobile to verify. Please check your EMail.",
            " is the OTP to reset your registered Krome MObile Number - Krome Home Automation ",
            "<a href="+"https://play.google.com/store/apps/details?id=neubel.krome.android >"+"Click Here</a> to download Krome App"};

        string MobileNo = ""; // + mobileno;
        String Message = "";
        String OTP = "";
        string pUrl = "http://control.msg91.com";
        string pPath = "/api/sendotp.php";
        Timer sendTimer = new Timer();

        public void SendSMS(String pMobileNumber, String pOTP, GenericUtil.OTPTransType pType)
        {

            MobileNo = pMobileNumber;
            Message = pOTP + tMessage[(int)pType];
            OTP = pOTP;
            sendTimer.Elapsed += new ElapsedEventHandler(TimerTrigger);
            sendTimer.Interval = 100;
            sendTimer.Enabled = true;
        }

        private async void TimerTrigger(object sender, ElapsedEventArgs e)
        {
            sendTimer.Enabled = false;
            await TriggerSMS();
        }

        private async Task TriggerSMS()
        {
            SendSMSJson sContent = new SendSMSJson();
            sContent.message = Message;
            sContent.mobile = "+91" + MobileNo;
            sContent.otp = OTP;

            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(pUrl);
                var request = new HttpRequestMessage(HttpMethod.Post, pPath);

                try
                {
                    request.Content = sContent.getSMSContent();
                    var response = await client.SendAsync(request);
                    Console.WriteLine(response.ToString());
                }
                catch (Exception ex)
                {
                    String c = ex.Message;
                }
            }
            catch (Exception ex)
            {
                string b = ex.Message;
            }
        }
    }

    public class SendSMSJson
    {
        public SendSMSJson() { }
        public String sender { get; set; } = "KROMEA";
        public String message { get; set; }
        public string mobile { get; set; }
        public string authkey { get; set; } = "182007A7u2YtbJ259fbf869";
        public string otp { get; set; }

        public FormUrlEncodedContent getSMSContent()
        {
            var keyValues = new List<KeyValuePair<string, string>>();
            keyValues.Add(new KeyValuePair<string, string>("sender", sender));
            keyValues.Add(new KeyValuePair<string, string>("message", message));
            keyValues.Add(new KeyValuePair<string, string>("mobile", mobile));
            keyValues.Add(new KeyValuePair<string, string>("authkey", authkey));
            keyValues.Add(new KeyValuePair<string, string>("otp", otp));

            return new FormUrlEncodedContent(keyValues);
        }
    }
}