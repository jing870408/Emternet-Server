using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.ML.Data;

namespace SDServer.DataModels
{
    public class EmoPredict : EmoData
    {
        [ColumnName("PredictedLabel")]
        public new string Score { get; set; }
    }

}
