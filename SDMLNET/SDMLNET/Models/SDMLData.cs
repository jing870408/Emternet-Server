using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.ML.Data;

namespace SDMLNET.Models
{
    class SDMLData
    {
        [LoadColumn(11)]
        public float Sleep { get; set; }

        [LoadColumn(12)]
        public float Sleeptime { get; set; }

        [LoadColumn(13)]
        public float AvgHeartRate { get; set; }

        [LoadColumn(14)]
        public float SocialApp { get; set; }

        [LoadColumn(15)]
        public float GameApp { get; set; }

        [LoadColumn(16)]
        public float FunApp { get; set; }

        [LoadColumn(17)]
        public float OtherApp { get; set; }

        [LoadColumn(18)]
        public float Temperture { get; set; }

        [LoadColumn(19)]
        public float Humidity { get; set; }

        [LoadColumn(20)]
        public float Sunshine { get; set; }

        [LoadColumn(21)]
        public float Airquality { get; set; }

        [LoadColumn(10)]
        public string Score { get; set; }
    }
    class EmoPredict
    {
        [ColumnName("PredictedLabel")]
        public string Score { get; set; }
    } 
}
