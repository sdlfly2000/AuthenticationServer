using Microsoft.Extensions.Configuration;
using Serilog;
using System.Text;

namespace Infra.Core
{
    public class PasswordHelper
    {
        private const int DEFAULT_ALLOW_DELAY_IN_SEC = 5;
        private static int AllowDelayInSec;
      
        public static string? ExtractPwdWithTimeVerification(string orginalPwd)
        {
            var pwdCompounds = ConvertBase64ToString(orginalPwd).Split('|');
            var password = pwdCompounds[0];
            var datetimeSent = long.Parse(pwdCompounds[1]) / 1000;

            var currentDatetime = DateTime.UtcNow;

            AllowDelayInSec = GetConfiguration().GetValue<int>("AuthServiceConfigure:AllowMaxDelayInSec", DEFAULT_ALLOW_DELAY_IN_SEC);

            Log.Information($"{nameof(PasswordHelper)}: CurrentDatetime: {currentDatetime}, UnixEpoch: {DateTime.UnixEpoch}, DatetimeSent: {datetimeSent}, ALLOW_DELAY_IN_SEC: {AllowDelayInSec}, result: {(currentDatetime - DateTime.UnixEpoch).TotalSeconds - datetimeSent}");

            if (((currentDatetime - DateTime.UnixEpoch).TotalSeconds - datetimeSent) > AllowDelayInSec)
            {
                return null;
            }

            return password;
        }

        public static string EncryptoPassword(string password)
        {
            // Get Hash Code
            unchecked
            {
                int hash1 = (5381 << 16) + 5381;
                int hash2 = hash1;

                for (int i = 0; i < password.Length; i += 2)
                {
                    hash1 = ((hash1 << 5) + hash1) ^ password[i];
                    if (i == password.Length - 1)
                        break;
                    hash2 = ((hash2 << 5) + hash2) ^ password[i + 1];
                }

                return (hash1 + (hash2 * 1566083941)).ToString();
            }            
        }

        #region Private Methods

        private static string ConvertBase64ToString(string base64)
        {
            if (base64.Length % 4 != 0)
                base64 += new String('=', 4 - base64.Length % 4);

            return Encoding.UTF8.GetString(
                Convert.FromBase64String(base64));
        }

        private static IConfiguration GetConfiguration()
        {
            return new ConfigurationManager().AddJsonFile("appsettings.json").Build();
        }

        #endregion
    }
}
