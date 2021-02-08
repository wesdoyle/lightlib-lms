using System;

namespace LightLib.Service.Helpers {
    public static class TimeSpanHumanizer {
        
        public static string GetReadableTimespan(TimeSpan ts) {
            string readableTimespan = "";
            var days = ts.Days;
            var approxMonths = days / 30;
            
            switch (days) {
                case < 1:
                    return "less than a day";
                case > 1 when approxMonths <= 1:
                    return "less than a month";
                default: {
                    switch (approxMonths) {
                        case > 1 and <= 11:
                            return "less than a year";
                        case > 11 and <= 12:
                            return "about a year";
                        case > 12 and <= 24:
                            return "more than a year";
                        case > 24: {
                            var approxYears = approxMonths / 12;
                            return $"about {approxYears} years";
                        }
                    }

                    break;
                }
            }
            
            return readableTimespan;
        }
    } 
}