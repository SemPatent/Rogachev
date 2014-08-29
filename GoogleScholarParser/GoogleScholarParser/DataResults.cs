using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace GoogleScholarParser
{
    public struct DataString
    {
        public string owners;
        public string university;
        public string journal;
        public string volume_journal;
        public string num_journal;
        public int year;
        public string pages;
    }

    class DataResults
    {
        public DataResults()
        {

        }

        public DataString GetData(string input)
        {
            DataString data = new DataString();
            data.owners = Owners(ref input);
            data.year = Year(ref input);
            data.university = University(input);

            data.journal = "-";
            data.volume_journal = "-";
            data.num_journal = "-";
            data.pages = "-";
            return data;
        }

        private int Year(ref string input)
        {
            if (!String.IsNullOrEmpty(input))
            {
                Regex reg = new Regex(@"[0-9]{4}");
                Match match = reg.Match(input);
                if (match.Success)
                {
                    input = input.Replace(match.Value, "");
                    return int.Parse(match.Value);
                }
            }
            return 0;
        }

        private string Owners(ref string input)
        {
            if (!String.IsNullOrEmpty(input))
            {
                Regex reg = new Regex(@"[А-Я]{1,2}[ ][А-Я][а-я]+|[A-Z]{1,2}[ ][A-Z][a-z]+");
                MatchCollection matches = reg.Matches(input);
                string match = "";
                for (int i = 0; i < matches.Count; i++)
                {
                    input = input.Replace(matches[i].Value, "");
                    match += matches[i].Value + ", ";
                }
                if (match.Length == 0)
                {
                    return "-";
                }
                else
                {
                    return match.Remove(match.Length - 1);
                }
            }
            return "-";
        }

        private string University(string input)
        {
            if (!String.IsNullOrEmpty(input))
            {
                
                bool begin = true;
                string output = DeleteShit(input);
                for (int i = 0; i < input.Length && begin == true; i++)
                {
                    if (!Char.IsLetterOrDigit(input[i]))
                    {
                        output = output.Remove(0, 1);
                    }
                    else
                    {
                        begin = false;
                    }
                }
                return output;
            }
            return "-";
        }

        public string DeleteShit(string input)
        {
            input = input.Replace("&quot;", " ");
            input = input.Replace("&laquo;", " ");
            input = input.Replace("&raquo;", " ");
            input = input.Replace("&amp;", " ");
            input = input.Replace("&hellip;", " ");
            input = input.Replace(";", " ");
            return input;
        }
    }
}
