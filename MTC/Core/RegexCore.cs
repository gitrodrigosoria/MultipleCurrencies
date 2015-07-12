using System;
using System.Text.RegularExpressions;

namespace MTC.Core
{
    public static class RegexCore
    {
        public static MatchCollection getMatches(String pattern, String source) 
        {            
            return new Regex(pattern, RegexOptions.Multiline).Matches(source);
        }
    }
}