using System;
using System.Collections.Generic;

namespace TimeConversion
{
    class Program
    {
        static void Main(string[] args)
        {
            // Current UTC Time.
            var currentDateTime = DateTime.UtcNow;
            Console.WriteLine("Current UTC DateTime --> {0}", currentDateTime);
            // Tokyo Peek Business Hours are mocked.
            var downTimeTokyo = new Dictionary<DayOfWeek, List<PeekDuration>>();
            downTimeTokyo.Add(DayOfWeek.Sunday, new List<PeekDuration> { new PeekDuration { StartTime = "22:00:00", EndTime="23:59:00" } });
            downTimeTokyo.Add(DayOfWeek.Monday, new List<PeekDuration> { new PeekDuration { StartTime = "00:00:00", EndTime = "08:28:00" }, new PeekDuration { StartTime = "22:00:00", EndTime = "23:59:00" } });
            downTimeTokyo.Add(DayOfWeek.Tuesday, new List<PeekDuration> { new PeekDuration { StartTime = "00:00:00", EndTime = "08:28:00" }, new PeekDuration { StartTime = "22:00:00", EndTime = "23:59:00" } });
            downTimeTokyo.Add(DayOfWeek.Wednesday, new List<PeekDuration> { new PeekDuration { StartTime = "00:00:00", EndTime = "08:28:00" }, new PeekDuration { StartTime = "22:00:00", EndTime = "23:59:00" } });
            downTimeTokyo.Add(DayOfWeek.Thursday, new List<PeekDuration> { new PeekDuration { StartTime = "00:00:00", EndTime = "08:28:00" }, new PeekDuration { StartTime = "22:00:00", EndTime = "23:59:00" } });
            downTimeTokyo.Add(DayOfWeek.Friday, new List<PeekDuration> { new PeekDuration { StartTime = "00:00:00", EndTime = "08:28:00" } });
            
            DisplayTokyoPeekDurations(currentDateTime, downTimeTokyo);
            DisplayLondonPeekDurations(currentDateTime,downTimeTokyo);
            Console.ReadLine();
        }

        /// <summary>
        /// Displays Today's Peek Business Hours in Tokyo.
        /// </summary>
        /// <param name="currentDateTime">UTC Time</param>
        /// <param name="downTime">Mocked Tokyo Hours</param>
        private static void DisplayTokyoPeekDurations(DateTime currentDateTime, Dictionary<DayOfWeek, List<PeekDuration>> downTime)
        {
            var peekDurationsToday = new List<PeekDuration>();
            downTime.TryGetValue(currentDateTime.DayOfWeek, out peekDurationsToday);

            var peekDurations = new List<String>();
            foreach (var peekDuration in peekDurationsToday)
            {
                peekDurations.Add(string.Format("{2} {0}-{1}", peekDuration.StartTime, peekDuration.EndTime, currentDateTime.DayOfWeek));
            }
            var tokyoPeekDurations = string.Join(", ", peekDurations.ToArray());

            Console.WriteLine("Today, Peek Hours in Tokyo are --> {0}", tokyoPeekDurations);
        }

        /// <summary>
        /// Displays Today's Peek Business Hours in London.
        /// </summary>
        /// <param name="currentDateTime">UTC Time</param>
        /// <param name="downTime">Mocked Tokyo Hours</param>
        private static void DisplayLondonPeekDurations(DateTime currentDateTime, Dictionary<DayOfWeek, List<PeekDuration>> downTime)
        {
            var peekDurations = new List<string>();
            foreach (var peekDuration in GetLondonTime(currentDateTime, downTime))
            {
                peekDurations.Add(string.Format("{2} {0}-{1}", peekDuration.StartTime, peekDuration.EndTime,peekDuration.StartDay));
            }
            var londonPeekDurations = string.Join(", ", peekDurations.ToArray());

            Console.WriteLine("Today, Peek Hours in London are --> {0}", londonPeekDurations);
        }

        /// <summary>
        /// Gets Today's Peek Business Hours in London from the corresponding Tokyo Time based on the day (Today).
        /// </summary>
        /// <param name="currentDateTime"></param>
        /// <param name="downTime"></param>
        /// <returns></returns>
        private static List<PeekDuration> GetLondonTime(DateTime currentDateTime,Dictionary<DayOfWeek, List<PeekDuration>> downTime)
        {
            var peekDurationsToday = new List<PeekDuration>();
            var peekDurationsTomorrow = new List<PeekDuration>();
            var peekLondonDurations = new List<PeekDuration>();
            switch (currentDateTime.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    downTime.TryGetValue(DayOfWeek.Sunday, out peekDurationsToday);
                    downTime.TryGetValue(DayOfWeek.Monday, out peekDurationsTomorrow);
                    peekLondonDurations.AddRange(GetPeekHoursLondon(peekDurationsToday, peekDurationsTomorrow, DayOfWeek.Sunday));
                    break;
                case DayOfWeek.Monday:
                    downTime.TryGetValue(DayOfWeek.Monday, out peekDurationsToday);
                    downTime.TryGetValue(DayOfWeek.Tuesday, out peekDurationsTomorrow);
                    peekLondonDurations.AddRange(GetPeekHoursLondon(peekDurationsToday, peekDurationsTomorrow, DayOfWeek.Monday));
                    break;
                case DayOfWeek.Tuesday:
                    downTime.TryGetValue(DayOfWeek.Tuesday, out peekDurationsToday);
                    downTime.TryGetValue(DayOfWeek.Wednesday, out peekDurationsTomorrow);
                    peekLondonDurations.AddRange(GetPeekHoursLondon(peekDurationsToday, peekDurationsTomorrow,DayOfWeek.Tuesday));
                    break;
                case DayOfWeek.Wednesday:
                    downTime.TryGetValue(DayOfWeek.Wednesday, out peekDurationsToday);
                    downTime.TryGetValue(DayOfWeek.Thursday, out peekDurationsTomorrow);
                    peekLondonDurations.AddRange(GetPeekHoursLondon(peekDurationsToday, peekDurationsTomorrow, DayOfWeek.Wednesday));
                    break;
                case DayOfWeek.Thursday:
                    downTime.TryGetValue(DayOfWeek.Thursday, out peekDurationsToday);
                    downTime.TryGetValue(DayOfWeek.Friday, out peekDurationsTomorrow);
                    peekLondonDurations.AddRange(GetPeekHoursLondon(peekDurationsToday, peekDurationsTomorrow, DayOfWeek.Thursday));
                    break;
                case DayOfWeek.Friday:
                    downTime.TryGetValue(DayOfWeek.Friday, out peekDurationsToday);
                    peekLondonDurations.AddRange(GetPeekHoursLondon(peekDurationsToday, peekDurationsTomorrow, DayOfWeek.Friday));
                    break;
            }
            return peekLondonDurations;
        }

        /// <summary>
        /// Manipulates the Time based on days.
        /// </summary>
        /// <param name="peekDurationsToday"></param>
        /// <param name="peekDurationsTomorrow"></param>
        /// <param name="day"></param>
        /// <Example>
        /// In Tokyo
        /// On Tuesday, the Business Peek Hours are 00:00-08:28,22:00-23:59 
        /// On Wednesday, the Business Peek Hours are 00:00-08:28,22:00-23:59 
        /// In London
        /// On Tuesday, the Business Peek Hours are 13:00-14:59 (Tuesday 22:00-23:59 Tokyo Standard Time),15:00-23:28  (Wednesday 00:00-08:28 Tokyo Standard Time)
        /// </Example>
        /// <Result>
        /// In Tokyo
        /// On Tuesday, the Business Peek Hours are 00:00-08:28,22:00-23:59 
        /// In London
        /// On Tuesday, the Business Peek Hours are 13:00-14:59,15:00-23:28 
        /// </Result>
        private static List<PeekDuration> GetPeekHoursLondon(List<PeekDuration> peekDurationsToday, List<PeekDuration> peekDurationsTomorrow, DayOfWeek day)
        {
            var britishTimeZone = TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time");
            var peekHoursLondon = new List<PeekDuration>();
            foreach (PeekDuration peekDuration in peekDurationsToday)
            {
                var britishStartTime = TimeZoneInfo.ConvertTime(DateTime.Parse(peekDuration.StartTime), britishTimeZone);
                var britishEndTime = TimeZoneInfo.ConvertTime(DateTime.Parse(peekDuration.EndTime), britishTimeZone);
                if (britishStartTime.DayOfWeek == day || britishEndTime.DayOfWeek == day)
                    peekHoursLondon.Add(new PeekDuration { StartTime = britishStartTime.TimeOfDay.ToString(), StartDay = britishStartTime.DayOfWeek, EndTime = britishEndTime.TimeOfDay.ToString(), EndDay = britishEndTime.DayOfWeek });
            }
            foreach (PeekDuration peekDuration in peekDurationsTomorrow)
            {
                var britishStartTime = TimeZoneInfo.ConvertTime(DateTime.Parse(peekDuration.StartTime).AddDays(1), britishTimeZone);
                var britishEndTime = TimeZoneInfo.ConvertTime(DateTime.Parse(peekDuration.EndTime).AddDays(1), britishTimeZone);
                if (britishStartTime.DayOfWeek == day || britishEndTime.DayOfWeek == day)
                    peekHoursLondon.Add(new PeekDuration { StartTime = britishStartTime.TimeOfDay.ToString(), StartDay = britishStartTime.DayOfWeek, EndTime = britishEndTime.TimeOfDay.ToString(), EndDay = britishEndTime.DayOfWeek });
            }
            return peekHoursLondon;
        }
    }

    /// <summary>
    /// PeekDuration.
    /// </summary>
    public class PeekDuration
    {
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public DayOfWeek StartDay { get; set; }
        public DayOfWeek EndDay { get; set; }
    }
}
