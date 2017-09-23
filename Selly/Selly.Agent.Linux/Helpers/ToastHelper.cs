using System.Diagnostics;

namespace Selly.Agent.Linux
{
    public class ToastHelper
    {
        public static void PopToast(string message)
        {
            // Must run as a user (not root) otherwise the popup does not display correctly
            // Have to use sudo to drop permissons as manually selecting the user account
            // is not implemented in .NET Core (proc.StartInfo.UserName)
            Process proc = new Process();
            proc.StartInfo.FileName = "/usr/bin/sudo";
            proc.StartInfo.Arguments = $"-u \"user\" /usr/bin/kdialog --title \"{message}\" 5 --passivepopup \" \"";
            proc.Start();
        }
    }
}
