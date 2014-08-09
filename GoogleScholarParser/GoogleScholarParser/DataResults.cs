using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace GoogleScholarParser
{
    public struct Data 
    {
        public string autors;
        public string year;
        public string publishing;
    }

    class DataResults
    {
        public DataResults()
        {

        }

        public Data GetData(string input)
        {
            Data data = new Data();
            data.autors = Autors(ref input);
            data.year = Year(ref input);
            data.publishing = input;
            return data;
        }

        private string Year(ref string input)
        {
            if (!String.IsNullOrEmpty(input))
            {
                Regex reg = new Regex(@"[0-9]{4}");
                Match match = reg.Match(input);
                if (match.Success)
                {
                    input = input.Replace(match.Value, "");
                    return match.Value;
                }
            }
            return "-";
        }

        private string Autors(ref string input)
        {
            if (!String.IsNullOrEmpty(input))
            {
                Regex reg = new Regex(@"[А-Я]{1,2}[ ][А-Я][а-я]+|[A-Z]{1,2}[ ][A-Z][a-z]+");
                MatchCollection matches = reg.Matches(input);
                string match = "";
                for(int i=0; i < matches.Count; i++)
                {
                    input = input.Replace(matches[i].Value, "");
                    match += matches[i].Value + ", ";
                }
                return match.Remove(match.Length - 1);
            }
            return "-";
        }
    }
}
