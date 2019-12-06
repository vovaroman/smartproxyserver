using System;
using System.Text.RegularExpressions;

namespace SmartHttpProxy
{
    public static class Symbols
    {
        public static readonly char[] SemiSplit = new char[] { ';' };
        public static readonly char[] EqualSplit = new char[] { '=' };
        public static readonly String[] ColonSpaceSplit = new string[] { ": " };
        public static readonly char[] SpaceSplit = new char[] { ' ' };
        public static readonly char[] CommaSplit = new char[] { ',' };
        public static readonly Regex CookieSplitRegEx = new Regex(@",(?! )");
    }
    public static class Sol
    {
        public const string CetrificatePath = @"https://api.ipify.org";

        public static int ListeningPort = 5000;

        public static string LinkToGetIp { get; internal set; }
    }
}
