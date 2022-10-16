using System.Text;
using System.IO;

namespace Astana.Class.Log
{
    public static class LogClass
    {
        static object loc = new object();
        public static void Write(string logText)
        {
            lock (loc)
            {
                using (StreamWriter sw = new StreamWriter("Log.txt", true, Encoding.UTF8))
                {
                    try
                    {
                        sw.WriteLine(logText);
                    }
                    catch { }
                }
            }
        }
    }
}
