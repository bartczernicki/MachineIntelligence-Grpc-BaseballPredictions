**gRPC Baseball Predictions - Server & Client**

This solution is a client-server model implementation in .NET Core.  THe .NET client communicates with a server using gRPC.  The server hosts in-memory Machine Learning models, that are surfaced for prediction inference.

**The application has the following features:**
* Two projects - client & server communicating with gRPC
* Nested Procotol Buffer classes exchanging complex messages for Machine Learning inference
* Dozen ML.NET models loaded into memory for rapid inference
* Individual prediction or ensemble model predictions

**Requirements:**
* Visual Studio 2019 v4.0, .NET Core 3.1, gRPC, ML.NET v1.4
