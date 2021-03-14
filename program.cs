using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace myApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            
            var proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "wkhtmltopdf",
                    Arguments = "-q - -",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    RedirectStandardInput = true,
                    CreateNoWindow = true
                }
            };
            proc.Start();
            using(var sIn = proc.StandardInput)
            {
                sIn.WriteLine("hello world");
            }
            var ms = new MemoryStream();
            var sOut = proc.StandardOutput.BaseStream;
            sOut.CopyTo(ms);
            string error = proc.StandardError.ReadToEnd();
            if (ms.Length == 0)
            {
                throw new Exception(error);
            }
            proc.WaitForExit();
            Console.WriteLine(Encoding.UTF8.GetString(ms.ToArray()));
            Console.WriteLine(ms.Length + " bytes written");
            File.WriteAllBytes("hello.pdf",ms.ToArray());
        }
    }
}
