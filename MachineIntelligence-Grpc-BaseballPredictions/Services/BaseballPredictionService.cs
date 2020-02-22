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

            var prediction = _predictionPool.Predict("InductedToHallOfFameGeneralizedAdditiveModel", request.MLBBaseballBatter);

            var response = new MLBBaseballBatterPredictionResponse
            {
                PredictionID = request.PredictionID,
                PredictionType = request.PredictionType,
                PredictedProbability = prediction.Probability
            };


            return Task.FromResult(response);
        }
    }
}
