using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.ML.Data;

namespace SDServer.DataModels
{
    public class EmoData
    {
        [LoadColumn(11)]
        public int Sleep { get; set; }

        [LoadColumn(12)]
        public int Sleeptime { get; set; }

        [LoadColumn(13)]
        public int AvgHeartRate { get; set; }

        [LoadColumn(14)]
        public float SocialApp { get; set; }

        [LoadColumn(15)]
        public float GameApp { get; set; }

        [LoadColumn(16)]
        public float FunApp { get; set; }

        [LoadColumn(17)]
        public float OtherApp { get; set; }

        [LoadColumn(18)]
        public int Temperture { get; set; }

        [LoadColumn(19)]
        public int Humidity { get; set; }

        [LoadColumn(20)]
        public int Sunshine { get; set; }

        [LoadColumn(21)]
        public int Airquality { get; set; }

        [LoadColumn(10)]
        public string Score { get; set; }
    }

}
   

