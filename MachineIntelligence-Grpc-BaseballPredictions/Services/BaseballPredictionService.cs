using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;


namespace MachineIntelligence_Grpc_BaseballPredictions
{
    public class BaseballPredictionService : BaseballBatterPrediction.BaseballBatterPredictionBase
    {
        private readonly ILogger<BaseballPredictionService> _logger;
        public BaseballPredictionService(ILogger<BaseballPredictionService> logger)
        {
            _logger = logger;
        }

        public override Task<MLBBaseballBatterPredictionResponse> MakeBaseBallBatterPrediction(MLBBaseballBatterPredictionRequest request, ServerCallContext context)
        {
            var response = new MLBBaseballBatterPredictionResponse
            {
                PredictionID = request.PredictionID,
                PredictionType = request.PredictionType,
                PredictedProbability = 0.54f
            };

            return Task.FromResult(response);
        }
    }
}
