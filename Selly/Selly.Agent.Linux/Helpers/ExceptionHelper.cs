using System.IO;
using System.Text;

namespace Selly.Agent.Linux
{
    public class ExceptionHelper
    {
        public static void WriteFile(string name, string message)
        {
            using (FileStream file = new FileStream($@"/home/user/Repos/selly/Selly/Selly.Agent.Linux/{name}.txt", FileMode.Create))
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(message);
                byte[] b = System.Text.Encoding.UTF8.GetBytes(sb.ToString());
                file.Write(b, 0, b.Length);
            }
        }
    }
}
