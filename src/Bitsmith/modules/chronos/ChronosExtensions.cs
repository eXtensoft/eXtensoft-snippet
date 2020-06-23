using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Windows;

namespace Bitsmith.Models
{
    public static class ChronosExtensions
    {
        public static Chronos Default(this Chronos model)
        {
            model.CreatedAt = DateTime.Now;
            return model;
        }

        public static List<TagIdentifier> Activities(this List<TagIdentifier> list)
        {
            list.Add(new TagIdentifier() { Id = Guid.NewGuid().ToString().ToLower(), Display = "Planning", Token = "planning" });
            list.Add(new TagIdentifier() { Id = Guid.NewGuid().ToString().ToLower(), Display = "Design", Token = "Design" });
            list.Add(new TagIdentifier() { Id = Guid.NewGuid().ToString().ToLower(), Display = "Code", Token = "code" });
            list.Add(new TagIdentifier() { Id = Guid.NewGuid().ToString().ToLower(), Display = "Troubleshoot", Token = "troubleshoot" });
            list.Add(new TagIdentifier() { Id = Guid.NewGuid().ToString().ToLower(), Display = "Refactor", Token = "refactor" });
            list.Add(new TagIdentifier() { Id = Guid.NewGuid().ToString().ToLower(), Display = "Unit Test", Token = "unit-test" });
            list.Add(new TagIdentifier() { Id = Guid.NewGuid().ToString().ToLower(), Display = "Testing", Token = "testing" });
            list.Add(new TagIdentifier() { Id = Guid.NewGuid().ToString().ToLower(), Display = "Collaborate", Token = "collaborate" });
            list.Add(new TagIdentifier() { Id = Guid.NewGuid().ToString().ToLower(), Display = "Deploy", Token = "deploy" });
            list.Add(new TagIdentifier() { Id = Guid.NewGuid().ToString().ToLower(), Display = "Sleep", Token = "sleep" });
            return list;
        }

        public static TimeEntry Default(this TimeEntry model)
        {
            model.Id = Guid.NewGuid().ToString().ToLower();
            model.CreatedAt = DateTime.Now;
            var user = Environment.UserName;
            model.Actor = new TagIdentifier() { Display = user, Token = user.ToLower()};
            model.Role = new TagIdentifier() { Display = "Software Engineer", Token = "Software Engineer".ToToken() };
            model.Started = DateTime.Now.Date;           
            return model;
        }

        public static string WeekOfYear(this DateTime target)
        {
            CultureInfo culture = new CultureInfo("en-US");
            Calendar calendar = culture.Calendar;
            CalendarWeekRule weekrule = culture.DateTimeFormat.CalendarWeekRule;
            DayOfWeek firstdayofweek = culture.DateTimeFormat.FirstDayOfWeek;
            return calendar.GetWeekOfYear(target, weekrule, firstdayofweek).ToString("D2"); ;
        }


       

        public static void Save(this TimeEntry model)
        {
            Chronos chronos = null;
            FileInfo info = model.EnsureFile();
            if(!FileSystemDataProvider.TryRead<Chronos>(info.FullName, out chronos, out string message))
            { 
                chronos = new Chronos().Default();
            }
            chronos.Items.Add(model);
            if(!FileSystemDataProvider.TryWrite<Chronos>(chronos,out string writeerror, info.FullName))
            {
                MessageBox.Show(writeerror);
            }
        }

        public static FileInfo EnsureFile(this TimeEntry model)
        {
            DirectoryInfo directory = new DirectoryInfo(AppConstants.ChronosDirectory);
            if (!directory.Exists)
            {
                directory.Create();
            }

            FileInfo info = new FileInfo(Path.Combine(directory.FullName, model.Filename()));
            if (!info.Exists)
            {
                Chronos chronos = new Chronos().Default();
                if(!FileSystemDataProvider.TryWrite(chronos, out string message, info.FullName))
                {
                    MessageBox.Show(message);
                }
            }
            return info;
        }

        private static string Filename(this TimeEntry model)
        {

            string master = !string.IsNullOrWhiteSpace(model.MasterId) ? model.MasterId : "master";
            string filepath = $"{model.Started.ToString("yyyy")}.{model.Started.WeekOfYear()}.{master}.xml";
            return filepath;
        }


    }
}
