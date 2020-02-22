using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Grpc.Net.Client;
using MachineIntelligence_Grpc_BaseballPredictions;

namespace MachineIntelligence_Grpc_BaseballPredictions.Client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Starting the gRPC Baseball Predictions Client.");

            // Retrieve Sample Baseball Data
            var mlbBaseballPlayerBatters =  await BaseballData.GetSampleBaseballData();

            // The port number(5001) must match the port of the gRPC server.
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var baseBallPredictionClient = new BaseballBatterPrediction.BaseballBatterPredictionClient(channel);

            foreach(var mlbBaseballPlayerBatter in mlbBaseballPlayerBatters)
            {
                await Task.Delay(500);

                var baseBallPredictionRequest = new MLBBaseballBatterPredictionRequest { 
                    PredictionID = Guid.NewGuid().ToString(),
                    PredictionType = "BaseballHOfInduction",
                    ModelName = "LogisticRegression",
                    MLBBaseballBatter = mlbBaseballPlayerBatter
                };

                var baseBallPredictionReply = await baseBallPredictionClient.MakeBaseBallBatterPredictionAsync(
                    baseBallPredictionRequest
                    );
                Console.WriteLine("PredictionID: {0} :::: PredictedProbability: {1}",
                    baseBallPredictionReply.PredictionID,
                    baseBallPredictionReply.PredictedProbability
                    );
            }


            Console.ReadLine();
        }
    }
}
