using System;
using System.Collections.Generic;
using System.Text;

namespace PR3_1920_Q3_WEBG.Extensions
{
    public static class StringExtension
    {

        public static string FloatStringBEFormat(this String str)
        {
            if (String.IsNullOrEmpty(str))
            {
                return str;
            }
            else
            {
                
                return str.Replace(".", ",");
            }
        }

        public static string FloatStringToInt(this string str)
        {
            if (String.IsNullOrEmpty(str))
            {
                return str;
            }
            else
            {
                int position = str.IndexOf(".");
                return str.Substring(0, position);
            }
        }

        public static string SuppresBefor(this string str, char cara)
        {
            if (String.IsNullOrEmpty(str))
            {
                return str;
            }
            else
            {
                int position = str.IndexOf(cara);
                return str.Substring(position+1,str.Length-position-1);
            }
        }


    }

}
