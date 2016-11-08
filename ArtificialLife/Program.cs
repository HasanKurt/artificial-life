using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace ArtificialLife
{
    class Program
    {
        static void Main(string[] args)
        {
            uint numSteps = 0;

            if (args.Length < 2 || !uint.TryParse(args[0], out numSteps))
            {
                Console.WriteLine("Usage: ./" + Assembly.GetExecutingAssembly().GetName().Name + ".exe NUMBER_OF_STEPS \"CODE1\" [\"CODE2\"...]");
                Console.WriteLine("For example: ./" + Assembly.GetExecutingAssembly().GetName().Name + ".exe 10000 \"^1^b,>bz\" \"^1^b0,>bz\"");
                Environment.Exit(-1);
            }

            List<string> codes = new List<string>(args);
            codes.RemoveAt(0);

            World w = new World(codes);
            for (int i = 0; i < numSteps; i++)
            {
                w.Step();
            }

            Console.WriteLine();
            w.PrintScores();

            using (var f = new FileInfo("result.svg").Open(FileMode.Create))
            {
                using (var sw = new StreamWriter(f))
                {
                    sw.Write(w.Draw());
                }
            }

            Console.WriteLine();
            Console.WriteLine("Graphical output written to result.svg (view in web browser)");
            Console.WriteLine();
        }
    }
}