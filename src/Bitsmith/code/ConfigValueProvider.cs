﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Bitsmith
{
    public static class ConfigValueProvider
    {
        public static double ShellHeight{ get {return GetConfigValueAs<double>("Shell.Height",550); }}

        public static double ShellWidth { get { return GetConfigValueAs<double>("Shell.Width", 900); } }

        public static string ShellTitle { get { return GetConfigValueAs<string>("Shell.Title", "Bitsmith CMS"); } }

        internal static bool TryGetConfigValueAs<T>(string key, out T t) where T : IConvertible
        {
            bool b = false;
            t = default(T);
            string s = ConfigurationManager.AppSettings[key];
            if (!string.IsNullOrWhiteSpace(s))
            {
                try
                {
                    t = Parse<T>(s);
                }
                catch (Exception ex)
                {
                }
            }

            return b;
        }
        internal static T GetConfigValueAs<T>(string key, T defaultValue) where T : IConvertible
        {
            T t = defaultValue;
            string s = ConfigurationManager.AppSettings[key];
            if (!String.IsNullOrEmpty(s))
            {
                t = Parse<T>(s);
            }
            return t;
        }

        internal static T GetConfigValueAs<T>(string key) where T : IConvertible
        {
            T t = default(T);
            string s = ConfigurationManager.AppSettings[key];
            if (!String.IsNullOrEmpty(s))
            {
                t = Parse<T>(s);
            }
            return t;
        }

        private static T Parse<T>(string valueToConvert) where T : IConvertible
        {
            return (T)Convert.ChangeType(valueToConvert, typeof(T));
        }


    
    }
}
