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

            string modelPathInductedToHallOfFameStochasticGradientDescentCalibrated = Path.Combine(Environment.CurrentDirectory, "Models", "InductedToHallOfFame-StochasticGradientDescentCalibrated.mlnet");
            string modelPathInductedToHallOfFameFastTree = Path.Combine(Environment.CurrentDirectory, "Models", "InductedToHallOfFame-FastTree.mlnet");
            string modelPathInductedToHallOfFameGeneralizedAdditiveModel = Path.Combine(Environment.CurrentDirectory, "Models", "InductedToHallOfFame-GeneralizedAdditiveModel.mlnet");
            string modelPathInductedToHallOfFameLogisticRegression = Path.Combine(Environment.CurrentDirectory, "Models", "InductedToHallOfFame-LogisticRegression.mlnet");
            string modelPathInductedToHallOfFameLightGbm = Path.Combine(Environment.CurrentDirectory, "Models", "InductedToHallOfFame-LightGbm.mlnet");

            string modelPathOnHallOfFameBallotStochasticGradientDescentCalibrated = Path.Combine(Environment.CurrentDirectory, "Models", "OnHallOfFameBallot-StochasticGradientDescentCalibrated.mlnet");
            string modelPathOnHallOfFameBallotFastTree = Path.Combine(Environment.CurrentDirectory, "Models", "OnHallOfFameBallot-FastTree.mlnet");
            string modelPathOnHallOfFameBallotGeneralizedAdditiveModel = Path.Combine(Environment.CurrentDirectory, "Models", "OnHallOfFameBallot-GeneralizedAdditiveModel.mlnet");
            string modelPathOnHallOfFameBallotLogisticRegression = Path.Combine(Environment.CurrentDirectory, "Models", "OnHallOfFameBallot-LogisticRegression.mlnet");
            string modelPathOnHallOfFameBallotLightGbm = Path.Combine(Environment.CurrentDirectory, "Models", "OnHallOfFameBallot-LightGbm.mlnet");

            services.AddPredictionEnginePool<MLBBaseballBatter, MLBHOFPrediction>()
                .FromFile("InductedToHallOfFame-StochasticGradientDescentCalibrated", modelPathInductedToHallOfFameStochasticGradientDescentCalibrated)
                .FromFile("InductedToHallOfFame-FastTree", modelPathInductedToHallOfFameFastTree)
                .FromFile("InductedToHallOfFame-GeneralizedAdditiveModel", modelPathInductedToHallOfFameGeneralizedAdditiveModel)
                .FromFile("InductedToHallOfFame-LogisticRegression", modelPathInductedToHallOfFameLogisticRegression)
                .FromFile("InductedToHallOfFame-LightGbm", modelPathInductedToHallOfFameLightGbm)
                .FromFile("OnHallOfFameBallot-StochasticGradientDescentCalibrated", modelPathOnHallOfFameBallotStochasticGradientDescentCalibrated)
                .FromFile("OnHallOfFameBallot-FastTree", modelPathOnHallOfFameBallotFastTree)
                .FromFile("OnHallOfFameBallot-GeneralizedAdditiveModel", modelPathOnHallOfFameBallotGeneralizedAdditiveModel)
                .FromFile("OnHallOfFameBallot-LogisticRegression", modelPathOnHallOfFameBallotLogisticRegression)
                .FromFile("OnHallOfFameBallot-LightGbm", modelPathOnHallOfFameBallotLightGbm);
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
