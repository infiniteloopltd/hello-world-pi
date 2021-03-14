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
            if (ms.Length == 0)
            {
                string error = proc.StandardError.ReadToEnd();
                throw new Exception(error);
            }
            proc.WaitForExit();
            Console.WriteLine(ms.Length + " bytes written");
            File.WriteAllBytes("hello.pdf",ms.ToArray());
        }
    }
}
