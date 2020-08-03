using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Training;
using Azure.Storage.Blobs.Models;
using Flurl;

namespace FunctionApp1
{
    public class Function1
    {
        private readonly CustomVisionService _cvService;
        private readonly BlobStorageService _blobService;

        public Function1(CustomVisionService cvService, BlobStorageService blobService)
        {
            _cvService = cvService;
            _blobService = blobService;
        }

        [FunctionName("Function1")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log,
            ExecutionContext executionContext)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string skillName = executionContext.FunctionName;
            IEnumerable<WebApiRequestRecord> requestRecords = WebApiSkillHelpers.GetRequestRecords(req);
            if (requestRecords == null)
            {
                return new BadRequestObjectResult($"{skillName} - Invalid request record array.");
            }

            //Custom Vision
            //var projects = _cvService.CVTrainingClient.GetProjects();

            //Blob Storage
            var container = _blobService.blobContainerClient;
            var containerUri = container.Uri.AbsoluteUri;

            WebApiSkillResponse response = WebApiSkillHelpers.ProcessRequestRecords(skillName, requestRecords,
            (inRecord, outRecord) => {
                int headLampCount = 0;
                int jobCartCount = 0;
                int scissorLiftCount = 0;
                int taskLightingCount = 0;
                int conduitCount = 0;

                //array of labels for image
                JArray labels = (JArray)inRecord.Data["label"];

                //loop through labels
                foreach (JObject o in labels.Children<JObject>())
                {
                    if ((string)o.Property("label") == "head lamps") headLampCount++;
                    else if ((string)o.Property("label") == "Job Carts") jobCartCount++;
                    else if ((string)o.Property("label") == "Scissor Lifts") scissorLiftCount++;
                    else if ((string)o.Property("label") == "Task Lighting") taskLightingCount++;
                    else if ((string)o.Property("label") == "Conduit") conduitCount++;
                }

                //get blob uri
                string image_url = (string)inRecord.Data["image_url"];
                string parentFolder = Path.GetFileName(Path.GetDirectoryName(image_url));
                string fileName =  Path.GetFileName(image_url);
                string blobLocation = parentFolder + '/' + fileName;
                string blobUri = Flurl.Url.Combine(
                        container.Uri.AbsoluteUri,
                        blobLocation
                    );

                outRecord.Data["blobUri"] = blobUri;
                outRecord.Data["headLampCount"] = headLampCount;
                outRecord.Data["jobCartCount"] = jobCartCount;
                outRecord.Data["scissorLiftCount"] = scissorLiftCount;
                outRecord.Data["taskLightingCount"] = taskLightingCount;
                outRecord.Data["conduitCount"] = conduitCount;

                return outRecord;
            });

            return new OkObjectResult(response);
        }
    }
}
