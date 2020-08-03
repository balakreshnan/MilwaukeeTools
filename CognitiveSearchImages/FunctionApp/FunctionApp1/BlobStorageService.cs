using Azure.Storage.Blobs;
using System;
using System.Collections.Generic;
using System.Text;

namespace FunctionApp1
{
    public class BlobStorageService
    {
        public BlobStorageService()
        {
            string connectionString = "<your-connection-string>";

            blobServiceClient = new BlobServiceClient(connectionString);
            blobContainerClient = blobServiceClient.GetBlobContainerClient("<container-name-with-images>");
        }

        public BlobServiceClient blobServiceClient { get; }

        public BlobContainerClient blobContainerClient { get; }
    }
}
