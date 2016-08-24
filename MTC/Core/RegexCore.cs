using System;
using System.Text.RegularExpressions;

namespace MTC.Core
{
    public static class RegexCore
    {
        public static MatchCollection GetMatches(String pattern, String source) 
        {            
            return new Regex(pattern, RegexOptions.Multiline).Matches(source);
        }
    }
}
