using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ML;

namespace MachineIntelligence_Grpc_BaseballPredictions
{
    public class BaseballPredictionService : BaseballBatterPrediction.BaseballBatterPredictionBase
    {
        private readonly ILogger<BaseballPredictionService> _logger;
        private readonly PredictionEnginePool<MLBBaseballBatter, MLBHOFPrediction> _predictionPool;

        public BaseballPredictionService(ILogger<BaseballPredictionService> logger,
            PredictionEnginePool<MLBBaseballBatter, MLBHOFPrediction> predictionEnginePool
            )
        {
            _logger = logger;
            _predictionPool = predictionEnginePool;
        }

        public override Task<MLBBaseballBatterPredictionResponse> MakeBaseBallBatterPrediction(MLBBaseballBatterPredictionRequest request, ServerCallContext context)
        {
            _logger.LogInformation("Making prediction for {0}", request.MLBBaseballBatter.FullPlayerName);

            MLBHOFPrediction prediction;

            // If using ensemble, iterate through the different algorithms used in the models
            if (request.UseEnsembleOfAlgorithms)
            {
                List<MLBHOFPrediction> allModelAlgorithmPredictions = new List<MLBHOFPrediction>();

                foreach (AlgorithmName algorithmName in Enum.GetValues(typeof(AlgorithmName)))
                {
                    var tempmodelNameForPrediction = string.Format("{0}-{1}", request.PredictionType, algorithmName);
                    var tempPrediction = _predictionPool.Predict(tempmodelNameForPrediction, request.MLBBaseballBatter);
                    allModelAlgorithmPredictions.Add(tempPrediction);
                }

                prediction = new MLBHOFPrediction
                {
                    Probability = (allModelAlgorithmPredictions.Sum(a => a.Probability) / allModelAlgorithmPredictions.Count()),
                    Prediction = (allModelAlgorithmPredictions.Sum(a => a.Probability) / allModelAlgorithmPredictions.Count()) >= 0.5 ? true : false,
                    Score = 0 // Score is meaningless, as each algorithm's processes use different scales
                };

            }
            // Else, perform a simple prediction based on the source algorithm of the model
            else
            {
                var modelNameForPredictions = string.Format("{0}-{1}", request.PredictionType, request.AlgorithmName);
                prediction = _predictionPool.Predict(modelNameForPredictions, request.MLBBaseballBatter);
            }

            var response = new MLBBaseballBatterPredictionResponse
            {
                PredictionID = request.PredictionID,
                PredictionType = request.PredictionType,
                MLBBaseballBatter = request.MLBBaseballBatter,
                MLBHOFPrediction = prediction
            };


            return Task.FromResult(response);
        }
    }
}
