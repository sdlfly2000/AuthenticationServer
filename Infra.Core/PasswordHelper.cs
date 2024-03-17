using Microsoft.Extensions.Configuration;
using System.Text;

namespace Infra.Core
{
    public class PasswordHelper
    {
        private static int ALLOW_DELAY_IN_SEC = 2;

        public PasswordHelper(IConfiguration configuration)
        {
            ALLOW_DELAY_IN_SEC = int.Parse(configuration.GetSection("AuthServiceConfigure:AllowMaxDelayInSec").Value!);
        }
        
        public static string? ExtractPwdWithTimeVerification(string orginalPwd)
        {
            var pwdCompounds = ConvertBase64ToString(orginalPwd).Split('|');
            var password = pwdCompounds[0];
            var datetimeSent = long.Parse(pwdCompounds[1]) / 1000;

            if (((DateTime.UtcNow - DateTime.UnixEpoch).TotalSeconds - datetimeSent) > ALLOW_DELAY_IN_SEC)
            {
                return null;
            }

            return password;
        }

        #region Private Methods

        private static string ConvertBase64ToString(string base64)
        {
            if (base64.Length % 4 != 0)
                base64 += new String('=', 4 - base64.Length % 4);

            return Encoding.UTF8.GetString(
                Convert.FromBase64String(base64));
        }

        #endregion
    }
}
