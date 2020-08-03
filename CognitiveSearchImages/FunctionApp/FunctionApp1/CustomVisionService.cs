using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Training;
using System;
using System.Collections.Generic;
using System.Text;

namespace FunctionApp1
{
    public class CustomVisionService
    {
        public CustomVisionService()
        {
            string trainingKey = "<your-training-key>";
            string ENDPOINT = "<your-end-point>";

            CVTrainingClient = new CustomVisionTrainingClient()
            {
                ApiKey = trainingKey,
                Endpoint = ENDPOINT
            };
        }

        public CustomVisionTrainingClient CVTrainingClient { get; }
    }
}
