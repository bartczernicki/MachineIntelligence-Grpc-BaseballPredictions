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
            Console.WriteLine(string.Empty);

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
                    PredictionType = PredictionType.OnHallOfFameBallot,
                    AlgorithmName = AlgorithmName.GeneralizedAdditiveModel,
                    MLBBaseballBatter = mlbBaseballPlayerBatter
                };

                var baseBallPredictionReply = await baseBallPredictionClient.MakeBaseBallBatterPredictionAsync(
                    baseBallPredictionRequest
                    );

                Console.WriteLine("PredictionID: {0}", baseBallPredictionReply.PredictionID);
                Console.WriteLine("Full Player Name: {0}", baseBallPredictionReply.MLBBaseballBatter.FullPlayerName);
                Console.WriteLine("Predicted Probability of {0}: {1}", baseBallPredictionRequest.PredictionType, baseBallPredictionReply.MLBHOFPrediction.Probability);
                Console.WriteLine("#################################");
            }

            Console.WriteLine("Finished running predictions using gRPC Baseball Predictions Client.");
            Console.ReadLine();
        }
    }
}
