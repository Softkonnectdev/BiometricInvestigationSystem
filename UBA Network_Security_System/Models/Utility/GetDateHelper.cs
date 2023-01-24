using System;

namespace UBA_Network_Security_System.Models.Utility
{
    public class GetDateHelper
    {
        public DateTime GetLocalTime()
        {
            try
            {
                TimeZoneInfo localZone = TimeZoneInfo.Local;
                DateTime correctDateTime = DateTime.UtcNow.AddHours((localZone.BaseUtcOffset.Hours));

                return correctDateTime;
            }
            catch
            {
                return DateTime.Now;
            }
        }
    }
}