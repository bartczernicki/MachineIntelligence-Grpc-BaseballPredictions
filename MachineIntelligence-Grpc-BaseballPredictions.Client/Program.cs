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
            // Temporary workaround for Visual Studio 2022 Preview
            Environment.SetEnvironmentVariable("ASPNETCORE_PREVENTHOSTINGSTARTUP", "true");

            Console.Title = "gRPC Baseball Predictions Client";
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Starting the gRPC Baseball Predictions Client.");
            Console.WriteLine();

            // Retrieve Sample Baseball Data
            var mlbBaseballPlayerBatters =  await BaseballData.GetSampleBaseballData();

            // The port number(5001) must match the port of the gRPC server.
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var baseBallPredictionClient = new BaseballBatterPrediction.BaseballBatterPredictionClient(channel);

            foreach(var mlbBaseballPlayerBatter in mlbBaseballPlayerBatters)
            {
                // Slow down predictions, to see a better representation on the Console program
                // Note: You would remove this in a real-world implementation
                await Task.Delay(600);

                // OnHallOfFameBallot Prediction
                var baseBallPredictionRequest = new MLBBaseballBatterPredictionRequest { 
                    PredictionID = Guid.NewGuid().ToString(),
                    PredictionType = PredictionType.OnHallOfFameBallot,
                    AlgorithmName = AlgorithmName.GeneralizedAdditiveModel,
                    UseEnsembleOfAlgorithms = true,
                    MLBBaseballBatter = mlbBaseballPlayerBatter
                };

                var baseBallPredictionReply = 
                    await baseBallPredictionClient.MakeBaseBallBatterPredictionAsync(baseBallPredictionRequest);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(baseBallPredictionReply.MLBBaseballBatter.FullPlayerName);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("On Hall Of Fame Ballot ################################");
                Console.ResetColor();
                Console.WriteLine("PredictionID: {0}", baseBallPredictionReply.PredictionID);
                Console.WriteLine("Predicted Probability of {0}: {1}", baseBallPredictionRequest.PredictionType,
                    Math.Round((Decimal)baseBallPredictionReply.MLBHOFPrediction.Probability, 5, MidpointRounding.AwayFromZero));
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("######################################################");
                Console.WriteLine();

                // InductedToHallOfFame Prediction
                baseBallPredictionRequest = new MLBBaseballBatterPredictionRequest
                {
                    PredictionID = Guid.NewGuid().ToString(),
                    PredictionType = PredictionType.InductedToHallOfFame,
                    AlgorithmName = AlgorithmName.GeneralizedAdditiveModel,
                    UseEnsembleOfAlgorithms = false,
                    MLBBaseballBatter = mlbBaseballPlayerBatter
                };

                baseBallPredictionReply =
                    await baseBallPredictionClient.MakeBaseBallBatterPredictionAsync(baseBallPredictionRequest);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(baseBallPredictionReply.MLBBaseballBatter.FullPlayerName);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Inducted to Hall Of Fame #############################");
                Console.ResetColor();
                Console.WriteLine("PredictionID: {0}", baseBallPredictionReply.PredictionID);
                Console.WriteLine("Predicted Probability of {0}: {1}", baseBallPredictionRequest.PredictionType,
                    Math.Round((Decimal)baseBallPredictionReply.MLBHOFPrediction.Probability, 5, MidpointRounding.AwayFromZero));
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("######################################################");
                Console.WriteLine();
            }

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine();
            Console.WriteLine("Finished the gRPC Baseball Predictions Client.");
            Console.ReadLine();
        }
    }
}
