using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.ML;

namespace MachineIntelligence_Grpc_BaseballPredictions
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddGrpc();

            /* Custom Services */

            string modelPathInductedToHallOfFameStochasticGradientDescentCalibrated = Path.Combine(Environment.CurrentDirectory, "Models", "InductedToHoF-StochasticGradientDescentCalibrated.mlnet");
            string modelPathInductedToHallOfFameFastTree = Path.Combine(Environment.CurrentDirectory, "Models", "InductedToHoF-FastTree.mlnet");
            string modelPathInductedToHallOfFameGeneralizedAdditiveModels = Path.Combine(Environment.CurrentDirectory, "Models", "InductedToHoF-GeneralizedAdditiveModels.mlnet");
            string modelPathInductedToHallOfFameLogisticRegression = Path.Combine(Environment.CurrentDirectory, "Models", "InductedToHoF-LogisticRegression.mlnet");
            string modelPathInductedToHallOfFameLightGbm = Path.Combine(Environment.CurrentDirectory, "Models", "InductedToHoF-LightGbm.mlnet");

            string modelPathOnHallOfFameBallotStochasticGradientDescentCalibrated = Path.Combine(Environment.CurrentDirectory, "Models", "OnHoFBallot-StochasticGradientDescentCalibrated.mlnet");
            string modelPathOnHallOfFameBallotFastTree = Path.Combine(Environment.CurrentDirectory, "Models", "OnHoFBallot-FastTree.mlnet");
            string modelPathOnHallOfFameBallotGeneralizedAdditiveModels = Path.Combine(Environment.CurrentDirectory, "Models", "OnHoFBallot-GeneralizedAdditiveModels.mlnet");
            string modelPathOnHallOfFameBallotLogisticRegression = Path.Combine(Environment.CurrentDirectory, "Models", "OnHoFBallot-LogisticRegression.mlnet");
            string modelPathOnHallOfFameBallotLightGbm = Path.Combine(Environment.CurrentDirectory, "Models", "OnHoFBallot-LightGbm.mlnet");

            services.AddPredictionEnginePool<MLBBaseballBatter, MLBHOFPrediction>()
                .FromFile("InductedToHoF-StochasticGradientDescentCalibrated", modelPathInductedToHallOfFameStochasticGradientDescentCalibrated)
                .FromFile("InductedToHoF-FastTree", modelPathInductedToHallOfFameFastTree)
                .FromFile("InductedToHoF-GeneralizedAdditiveModels", modelPathInductedToHallOfFameGeneralizedAdditiveModels)
                .FromFile("InductedToHoF-LogisticRegression", modelPathInductedToHallOfFameLogisticRegression)
                .FromFile("InductedToHoF-LightGbm", modelPathInductedToHallOfFameLightGbm)
                .FromFile("OnHoFBallot-StochasticGradientDescentCalibrated", modelPathOnHallOfFameBallotStochasticGradientDescentCalibrated)
                .FromFile("OnHoFBallot-FastTree", modelPathOnHallOfFameBallotFastTree)
                .FromFile("OnHoFBallot-GeneralizedAdditiveModels", modelPathOnHallOfFameBallotGeneralizedAdditiveModels)
                .FromFile("OnHoFBallot-LogisticRegression", modelPathOnHallOfFameBallotLogisticRegression)
                .FromFile("OnHoFBallot-LightGbm", modelPathOnHallOfFameBallotLightGbm);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<BaseballPredictionService>();

                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
                });
            });
        }
    }
}
