# Process data using a Knowledge Store powered by Azure Cognitive Search and display results in Power BI
A knowledge store contains output from an Azure Cognitive Search enrichment pipeline for later analysis or other downstream processing. An AI-enriched pipeline accepts image files or unstructured text files, indexes them by using Azure Cognitive Search, applies AI enrichments from Cognitive Services (such as image analysis and natural language processing), and then saves the results to a knowledge store in Azure Storage. 

In this tutorial, we will show you how to use Azure Cognitive Search to extract key phrases and analyze sentiment from your METCo Reviews dataset, as well as how to send this data to a knowledge store so that it can be leveraged to create visualizations in Power BI.

## Move Data to Azure
To build the Knowledge Store, your data will need to be in Azure. In this scenario, we are using a blob storage container to hold our csv file, but Azure Cognitive Search can connect to [many different data sources](https://docs.microsoft.com/en-us/azure/search/search-indexer-overview#supported-data-sources).

If your data is not already in Azure, create an Azure Storage Account resource, and upload your data to a blob storage container.*

## Create Azure Resources
Once in the Azure Portal, create the following resources:

* Azure Cognitive Search
* Azure Cognitive Services

**Azure Cognitive Search** will allow you to not only index your data in a format that is searchable and ready for additional analysis, but it will also be the platform for you to add enrichments to your data such as sentiment analysis, language detection and key phrase extraction. Azure Cognitive Search pricing tiers can be found [here](https://docs.microsoft.com/en-us/azure/search/search-sku-tier). A free tier can be used for the purposes of this tutorial, but it can only index a maximum of 50 MB.

**Azure Cognitive Services** is a suite of pre-built AI models that Azure Cognitive Search will leverage for the enrichments. 
Before navigating away from these resources, use a link on the left navigation pane to open the Keys page. Copy the primary admin key value for the Azure Cognitive Search resource and repeat the process for the Azure Cognitive Service resource. You will need both of these keys later.

## Understand the Azure Cognitive Search Model

Azure Cognitive Search is made up of 4 elements: data source, skillset, index and indexer. In the following steps we will create each one of these elements for your use case. The purpose of each is as follows:
* The **Data Source** contains all of the metadata needed to connect to your data. It contains information such as the connection string and the container where your data is.
* The **Index** has information about the search index (and the shape of that index, which fields are sortable, retrievable, etc.) or in this case, the structure of the tables we will use in the Knowledge Store.
* The **Skillset** contains information about each of the enrichment steps and their dependencies. For example, we will be adding enrichments such as key phrase extraction and sentiment analysis which we will specify in the skillset. The skillset is also where you specify your Azure Cognitive Services information to provide the AI to power the skills.
* The **Indexer** brings it all together. It has pointers to the other 3 and is in charge of taking data from the data source, running the skillset and pushing the data into the index.

![Image of Azure Search](https://github.com/balakreshnan/MilwaukeeTools/blob/master/TextAnalytics/SearchStructure.png)

## Connect to a Datastore

Navigate to your Azure Cognitive Search resource in the Azure portal. On the search service landing page, click “Import data” from the menu across the top of the screen.

* First, choose **Azure Blob Storage** from the Data Source menu and configure the settings to match the image below:
* Change parsing mode to **Delimited text**
* Check “First Line Contains Header”
* Connect to data through a connection string on choosing and existing connection
* Once configured, select “Next: Add cognitive skills”
 
![DS](https://github.com/balakreshnan/MilwaukeeTools/blob/master/TextAnalytics/DatasourceConfig.png) 

## Create the Skillset

The next step is to create the skillset, which specifies both the enhancements to be applied and the knowledge store where the results will be stored. First choose to “Attach Cognitive Services” and select an existing Cognitive Services resource. You can create one if you do not have one already, but note that it must be located in the same region as the search service. Add enrichments in the next menu and configure to match the image below.

* Change Source data field to **Reviews**
* Change Enrichment granularity level to **Pages** then add key phrases, detect language, translate text and detect sentiment
* For “Save enrichments to a knowledge store check the boxes associated with Azure Table Projections and attach a blob storage container.
* Click “Next: Customize target index” when complete

![DS](https://github.com/balakreshnan/MilwaukeeTools/blob/master/TextAnalytics/SkillsetConfig.png) 

## Create the Search Index

Create an Azure Cognitive Search index to represent the data that you are interested in searching, filtering, and applying enhancements to. Most of the options here are more relevant for data in a search app, but you can configure the index similar to the image below.

![DS](https://github.com/balakreshnan/MilwaukeeTools/blob/master/TextAnalytics/IndexConfig.png) 

## Create the Indexer

The final step in creating the knowledge store is to create the indexer. The indexer reads the data and activates the skillset. Aside from changing the name you do not need to make any changes here.

## Connect to Power BI

After creating the Knowledge Store, you can use Power BI to create powerful visuals for your data.
1. Start Power BI Desktop and click **Get data**.

2. In the Get Data window, select Azure, and then select **Azure Table Storage**.

3. Click **Connect**.

4. For Account Name or URL, enter in your Azure Storage account name (the full URL will be created for you).

5. If prompted, enter the storage account key.

6. Select the tables containing the data created by the previous walkthrough: 

  - *(YourSkillsetName)Document*
  - *(YourSkillsetName)Pages*
  - *(YourSkillsetName)KeyPhrases*
  
7. Click **Load**.

8. On the top ribbon, click **Transform Data** to open the **Power Query Editor**.

9. Select the *Document* table and then remove the PartionKey, RowKey, and Timestamp columns (as shown in the image below). Click the icon with opposing arrows at the upper right side of the table to expand the **Content**. When the list of columns appears, select all columns, and then deselect columns that start with 'metadata'. Click **OK** to show the selected columns.

![Delete Columns](https://github.com/balakreshnan/MilwaukeeTools/blob/master/TextAnalytics/DeleteColumnsPowerBI.png)

10. Repeat these steps for the remaining tables.*

**Note that you will need to change the data types for Rating and Setiment Score to **Whole Number** and **Decimal Number**, respectively (similar to as shown in the image below).* The Created Date field will also need to be changed to the Date type.

![Change Type](https://github.com/balakreshnan/MilwaukeeTools/blob/master/TextAnalytics/ChangeDataTypePowerBI.png)

11. On the command bar, click **Close and Apply**.

## Create Visualizations
Click on the **Model** tile on the left navigation pane and validate that Power BI shows relationships between all three tables.

Double-click each relationship and make sure that the Cross-filter direction is set to Both. This enables your visuals to refresh when a filter is applied.

![Modify Model](https://github.com/balakreshnan/MilwaukeeTools/blob/master/TextAnalytics/ModifyModelPowerBI.png)

Click on the Report tile on the left navigation pane to explore data through visualizations. For text fields, tables and cards are useful visualizations. You can choose fields from each of the three tables to fill in the table or card.

In addition to the built-in visualizations, there are many interesting visualizations in the Visualizations Marketplace. We used one called Force-Directed-Graph Visualization to build a network map of products and key phrases.

![Visual 1](https://github.com/balakreshnan/MilwaukeeTools/blob/master/TextAnalytics/PowerBIVisual1.JPG)


![Visual 2](https://github.com/balakreshnan/MilwaukeeTools/blob/master/TextAnalytics/Visual2.JPG)


