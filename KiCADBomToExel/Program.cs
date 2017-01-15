using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace KiCADBomToExel
{
    static class Program
    {
       
        static int Main(string[] args)
        {
            if (args.Length == 0)
            {
                System.Console.WriteLine("Please enter inputfile.");
                return 1;
            }
            if (args.Length == 1)
            {
                System.Console.WriteLine("Please enter outputfile.");
                return 1;
            }
            if (args.Length == 2)
            {
                System.Console.WriteLine("Opening" + args[0]);
                try
                {
                   
                    ParseXML(args[0], args[1]);
                }
                catch
                {
                    System.Console.WriteLine("Failed");
                    return 1;
                }
            }
            Console.ReadKey();
            return 0;
        }

        static void ParseXML(string inputfile, string outputfile)
        {
            List<string> Inputtextlist = new List<string>();
            List<Part> PartList = new List<Part>();
            Part P = new Part();

            StreamReader sr = File.OpenText(inputfile);
            string s;
            while ((s = sr.ReadLine()) != null)
            {
                

                if(s.Contains("<comp ref="))
                {

                    P = new Part();

                    s = s.Replace("<comp ref=", "");
                    s = s.Replace("\"", "");
                    s = s.Replace(">", "");
                    s = RemoveWhitespace(s);
                    P.Ref = s;
                }
                if (s.Contains("<value>"))
                {
                    s = s.Replace("<value>", "");
                    s = s.Replace("\"", "");
                    s = s.Replace("</value>", "");
                    s = RemoveWhitespace(s);
                    P.Value = s;    
                }
                if (s.Contains("<footprint>"))
                {
                   

                    s = s.Replace("<footprint>", "");
                    s = s.Replace("\"", "");
                    s = s.Replace("</footprint>", "");
                    s = RemoveWhitespace(s);
                    P.Footprint = s;
                }

                if (s.Contains("<libsource "))
                {
                    
                    string[] SA;
                    s = s.Replace("<libsource lib=", "");
                    s = s.Replace("\"", "");
                    s = s.Replace("/>", "");
                    s = RemoveWhitespace(s);
                    SA = s.Split('=');
                    
                    P.LibSource = SA[0];
                    P.PartName = SA[1];
                }
                if (s.Contains("</comp>"))
                {
                    PartList.Add(P);
                }

                //System.Threading.Thread.Sleep(1);
            }
            int i = 0;
            using (StreamWriter writetext = new StreamWriter(outputfile))
            {
                foreach (Part Pa in PartList)
                {
                    if (Pa.LibSource == null)
                    {
                        Pa.LibSource = "";
                    }
                    i++;
                    Console.WriteLine(i + "  " + Pa.Ref + "  " + Pa.PartName + "  " + Pa.Value + "  " + Pa.LibSource + "  " + Pa.Footprint);


                    writetext.WriteLine(i+ "\t" + Pa.Ref + "\t" + Pa.PartName + "\t" + Pa.Value + "\t" + Pa.LibSource + "\t" + Pa.Footprint);

                }
            }

        }
        static string RemoveWhitespace(this string input)
        {
            return new string(input.ToCharArray()
                .Where(c => !Char.IsWhiteSpace(c))
                .ToArray());
        }
    }
}
