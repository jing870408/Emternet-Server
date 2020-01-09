using System;
using System.Collections.Generic;

namespace SDServer.Models
{
    public partial class OriVarData
    {
        public int Idx { get; set; }
        public TimeSpan? Sleep { get; set; }
        public double? Sleeptime { get; set; }
        public int? AvgHeartRate { get; set; }
        public TimeSpan? SocialApp { get; set; }
        public TimeSpan? GameApp { get; set; }
        public TimeSpan? FunApp { get; set; }
        public TimeSpan? OtherApp { get; set; }
        public double? Temperture { get; set; }
        public double? Humidity { get; set; }
        public double? Rain { get; set; }
        public double? AirQ { get; set; }
        public string Account { get; set; }
        public DateTime Date { get; set; }
        public int Pros { get; set; }
    }
}
