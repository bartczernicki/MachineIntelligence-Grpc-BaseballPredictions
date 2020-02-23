**gRPC Baseball Predictions - Client & Server**

**Background:** gRPC is an emerging open communication standard set of conventions that is as an option for building connected services.  It can be an alternative to REST/SOA based architectures. What makes gRPC compelling for Machine Intelligence (AI, ML) scenarios is it facilitates not only uranary communication, but also bi-directional/streaming as well.  Therefore, services such a IoT or streaming media can extract insights using Machine Intelligence dynamically.

This solution is a client-server model implementation in .NET Core.  The .NET Core client communicates with a server using gRPC conventions.  The server hosts in-memory Machine Learning models, that are surfaced for real-time prediction inference.

![gRPC Client & Server](https://github.com/bartczernicki/MachineIntelligence-Grpc-BaseballPredictions/blob/master/Images/gRPCBaseballServerAndClient.gif)
*In the example above, the server's logs correspond to the client's request logs as the Machine Learning models execute predictions*

**The gRPC example system has the following features:**
* Two projects - client & server communicating with gRPC
* Nested Procotol Buffers (proto) exchanging complex messages for Machine Learning inference
* Dozen ML.NET models loaded into memory for configurable predictions
* Individual prediction or ensemble model predictions

**Project Structure:**
* Visual Studio 2019 v4.0, C#, .NET Core 3.1, gRPC, ML.NET v1.4

Note: This system focuses on gRPC communication and Machine Learning inference.  The models used in this example have been built using a seperate console training job located in a project at this GitHub location:
* https://github.com/bartczernicki/MLDotNet-BaseballClassification
