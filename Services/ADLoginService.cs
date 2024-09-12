using System.Runtime.InteropServices;

namespace PS_Utils.Services
{
    public class ADLoginService
    {
        private const int LOGON32_LOGON_NETWORK = 3;
        private const int LOGON32_PROVIDER_DEFAULT = 0;

        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern bool LogonUser(string lpszUsername, string lpszDomain, string lpszPassword,
            int dwLogonType, int dwLogonProvider, out IntPtr phToken);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public extern static bool CloseHandle(IntPtr handle);

        public static bool AuthenticateUser(string domain, string username, string password)
        {
            IntPtr tokenHandle = IntPtr.Zero;
            bool isValid = LogonUser(username, domain, password, LOGON32_LOGON_NETWORK, LOGON32_PROVIDER_DEFAULT, out tokenHandle);

            if (isValid && tokenHandle != IntPtr.Zero)
            {
                CloseHandle(tokenHandle);
                return true; // Successfully authenticated
            }

            return false; // Authentication failed
        }
    }
}
