using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.ML;
using Microsoft.ML;
using SDServer.DataModels;

namespace SDServer
{
    public class PredictController 
    {
        private readonly PredictionEnginePool<EmoData, EmoPredict> _predictionEnginePool;

        

        public PredictController(PredictionEnginePool<EmoData, EmoPredict> predictionEnginePool)
        {
             _predictionEnginePool = predictionEnginePool;
        }
        public PredictController()
        {

        }

        public string Post([FromBody] EmoData input)
        {
            EmoPredict prediction = _predictionEnginePool.Predict(input);
            
            //string sentiment = Convert.ToBoolean(prediction.PredictionLabel) ? "Positive" : "Negative";

            return prediction.Score;
        }
        

    }
}