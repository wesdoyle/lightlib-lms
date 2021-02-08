using System;
using System.Collections.Generic;
using LightLib.Data.Models;

namespace LightLib.Service.Helpers {
    public static class DataHelpers {
        
        public static IEnumerable<string> HumanizeBusinessHours(IEnumerable<BranchHours> branchHours) { 
            var hours = new List<string>();

            foreach (var time in branchHours) {
                var day = HumanizeDayOfWeek(time.DayOfWeek);
                var openTime = HumanizeTime(time.OpenTime);
                var closeTime = HumanizeTime(time.CloseTime);
                var timeEntry = $"{day} {openTime} to {closeTime}";
                hours.Add(timeEntry);
            }

            return hours;
        }

        private static string HumanizeDayOfWeek(int number) => Enum.GetName(typeof(DayOfWeek), number);

        private static string HumanizeTime(int time) => TimeSpan.FromHours(time).ToString("hh':'mm");
    }
}