# Cognitive Search Reporting on ML-Assisted-Data-Labeled Images

This tutorial will show you how to use tag data created by a data labeling project in Azure ML and create a searchable index for reporting and analysis.

## Prerequisites
* Visual Studio code
* Postman

The following resources are assumed to be already deployed and accessible:

1. Machine Learning
    * Data labeling project with tagged images
1. Blob Storage
    * Images stored in a container with **container-level access**

## Create Azure Resources
In your Azure Portal, you will need to create these resources in the same region with their following parameters:

1. Cognitive Search
    * Pricing tier: Standard
1. Cognitive Services
    * Pricing tier: Standard
1. Function App
    * Publish: Code
    * Runtime stack: .NET Core
    * Version: 3.1
1. Storage Account (for Function App storage dependency)
    * Replication: Locally-redundant storage (LRS)

## Export Data Labelling Tags

In your data labelling project, export the tagged data to an Azure ML Dataset as shown.
![image](/CognitiveSearchImages/images/exportAzureMLDataset.png)

## Publish Function App
Modify "BlobStorageService.cs" to include:
1. The connection string to your Blob Storage account
1. The name of your blob container

![image](/CognitiveSearchImages/images/modifyCode.PNG)

Publish the app ([reference doc](https://docs.microsoft.com/en-us/azure/search/cognitive-search-create-custom-skill-example#publish-the-function-to-azure))

1. In **Solution Explorer**, right-click the project and select **Publish**. Choose **Create New** > **Publish**.

1. If you haven't already connected Visual Studio to your Azure account, select **Add an account....**

1. Follow the on-screen prompts. You're asked to specify a unique name for your app service, the Azure subscription, the resource group, the hosting plan, and the storage account you want to use. You can create a new resource group, a new hosting plan, and a storage account if you don't already have these. When finished, select **Create**

1. After the deployment is complete, notice the Site URL. It is the address of your function app in Azure. 

1. In the [Azure portal](https://portal.azure.com), navigate to the Resource Group, and look for the Function you published. Under the Functions section, you should see your listed function. Open it and go to the "Code + Test" blade. Select "Get Function Url" and copy the ***default* host key**. You will need this for creating the skillset in Postman.

![image](/CognitiveSearchImages/images/getFunctionURL.PNG)

## Understand the Azure Cognitive Search Model
Azure Cognitive Search is made up of 4 elements: data source, skillset, index and indexer. In the following steps we will create each one of these elements for your use case. The purpose of each is as follows:

1. **The Data Source** contains all of the metadata needed to connect to your data. It contains information such as the connection string and the container where your data is.
1. **The Index** has information about the search index (and the shape of that index, which fields are sortable, retrievable, etc.) or in this case, the structure of the tables we will use in the Knowledge Store.
1. **The Skillset** contains information about each of the enrichment steps and their dependencies. For example, we will be adding enrichments such as key phrase extraction and sentiment analysis which we will specify in the skillset. The skillset is also where you specify your Azure Cognitive Services information to provide the AI to power the skills.
1. **The Indexer** brings it all together. It has pointers to the other 3 and is in charge of taking data from the data source, running the skillset and pushing the data into the index.

![image](/CognitiveSearchImages/images/SearchStructure.png)

## Setup Azure Cognitive Search through Postman
We will be using Postman to create the Knowledge Store. If you don’t have this tool already, you can download it [here](https://www.postman.com/downloads/). When you create a knowledge store, you must issue four HTTP requests:

* **PUT request to create the index:** This index holds the data that Azure Cognitive Search uses and returns.
* **POST request to create the datasource:** This datasource connects your Azure Cognitive Search behavior to the data and knowledge store's storage account.
* **PUT request to create the skillset:** The skillset specifies the enrichments that are applied to your data and the structure of the knowledge store.
* **PUT request to create the indexer:** Running the indexer reads the data, applies the skillset, and stores the results. *You must run this request last*.

Import the collections of requests "MilwaukeeToolsDemo-Postman.postman_collection.json" from this repo into Postman. For each request fill out the necessary paramters and send the requests in order. For each call, you will modify the headers to include the API-key and the content-type (application/json), as well as modify the body of the request, per the image below.

![image](/CognitiveSearchImages/images/postman-headers-ui.png)

# Connect to PowerBI
1. Start Power BI Desktop and click **Get data**.

1. In the Get Data window, select Azure, and then select **Azure Table Storage**.

1. Click **Connect**

1. For Account Name or URL, enter in your Azure Storage account name (the full URL will be created for you).

1. If prompted, enter the storage account key.

1. Select the *imageLabels* table containing the data created by the previous walkthrough
  
1. Click **Transform Data**

1. Remove all columns except for "*Content*"

1. Expand the "*Content*" column with the selected fields shown:
![image](/CognitiveSearchImages/images/expandColumn.PNG)

1. Close the Power Query Editor window and apply changes

1. For each of the fields *conduitCount*, *headLampCount*, *jobCartCount*, *scissorLiftCount*, and *taskLightingCount* change their **Data Type** to "Whole Number" under the **Column tools** tab

1. Rename the columns as you see fit

# Create Visualizations and Filters
1. Expand the submenu in the Visualizations pane and select "Get more visuals", then browse for "Image Grid" and add it to your report.

1. For the Image Grid, drag "blobUri" from the Fields pane into the Image URL box in the Visualizations pane to add images to view

1. Similarly add the "Smart Filter by OKVIZ" for an easy filtering experience

1. Add charts of your choosing the visualize the data

![image](/CognitiveSearchImages/images/powerbi.PNG)
