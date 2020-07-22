# Adding a Skill to Identify Custom Entities with Azure Cognitive Search

## Prerequisites
This tutorial builds on the previously created search index in this tutorial: https://github.com/balakreshnan/MilwaukeeTools/blob/master/TextAnalytics/PortalOnlyTutorial.md
So if you’ve not already done so, please first use this tutorial to create the search skillset, index and indexer.

## Overview
In this tutorial, we’ll be using the Custom Entity skill to identify words and phrases in our documentation and use the results to do analysis in Power BI. The specific words we’ll be looking for in the reviews are:

* Intermittent/Intermittently
* Chuck
* Trigger Interference
* Dead
* Low Power
* Too Hot
* Unrelated

We’ll also be using Postman in this tutorial so if you don’t have it already, you can download it [here](https://www.postman.com/downloads/). 
In the previous tutorial, you created the four critical parts of the Azure Cognitive Search Service: Data Source, Index, Skillset and Indexer. We won’t touch the Data Source for this tutorial, but we will be adding a skill to the skillset, a field to the indexer and updating the indexer to account for our changes.

## Update the Index

Reference Postman calls can be found [here](https://github.com/balakreshnan/MilwaukeeTools/blob/master/TextAnalytics/MTCustomEntity.postman_collection.json), but based on the set up of your existing service, the **PUT** request may need to be modified via the process below.
To begin, open a new request in Postman. This will be a **GET** request and add the following as the URL: `https://{{your-search-service-name}}.search.windows.net/indexes/{{your-index-name}}?api-version=2019-05-06` under the headers section add a key for "api-key" with the api key value of your search service. Similarly, add a key of “Content-Type” and a value of “application/json” The request should look like the image below:

![Image](https://github.com/balakreshnan/MilwaukeeTools/blob/master/TextAnalytics/IndexGETImage1.png)

After adding in the values, send the request and you should see a Status of “200 OK”. Copy the body in the response, click the “body” in the request, change to “raw” and copy the text in the box below (refer to the image if you are unclear where to paste the text).

![Image](https://github.com/balakreshnan/MilwaukeeTools/blob/master/TextAnalytics/HeadersImage2.png)

Now, we must update the body to add a field for the custom entity in the index. To do this, copy the text below into the index body (Note that the name can be whatever you like as long as it’s consistent when updating other parts of the search service.

```
"fields": [
   ...,
   {
	"name": "customentity",
	"type": "Collection(Edm.String)",
	"searchable": true,
	"filterable": true,
	"retrievable": true,
	"sortable": false,
	"facetable": true,
	"key": false,
	"indexAnalyzer": null,
	"searchAnalyzer": null,
	"analyzer": "en.microsoft",
	"synonymMaps": []
   },
   ...
]
```
After adding in the new text, change **GET** to **PUT** and send the request. You should receive a 204 No Content response if your request was successful.

## Update the Skillset

We will follow the same process to update the skillset. First issue a **GET** request to: `https://{{your-search-service-name}}.search.windows.net/skillsets/{{your-skillset-name}}?api-version=2019-05-06-Preview`. Don’t forget to also add the api-key and content-type headers before submitting. You should get a response of 200 OK.

Copy the response and add to the Body in the request as you did before. 

You will update the skillset in three places:

First, Add the Custom entity skill, add another skill to your skillset using the code below. Note that you can add more words by adding more “names” in the skill. Similarly, aliases are treated as Synonyms. Finally the FuzzyEditDistance accounts for how many characters can be different for the word to still be a match. FuzzyEditDistance has a max of 5. 
```
{
            "@odata.type": "#Microsoft.Skills.Text.CustomEntityLookupSkill",
            "name": "customentity",
            "description": null,
            "context": "/document/Reviews/pages/*",
            "defaultLanguageCode": "en",
            "entitiesDefinitionUri": null,
            "inputs": [
                {
                    "name": "text",
                    "source": "/document/Reviews/pages/*",
                    "sourceContext": null,
                    "inputs": []
                }
            ],
            "outputs": [
                {
                    "name": "entities",
                    "targetName": "customentity"
                }
            ],
            "inlineEntitiesDefinition": [
                {
                    "name": "intermittent",
                    "description": null,
                    "type": null,
                    "subtype": null,
                    "id": null,
                    "caseSensitive": null,
                    "accentSensitive": null,
                    "fuzzyEditDistance": 1,
                    "defaultCaseSensitive": null,
                    "defaultAccentSensitive": null,
                    "defaultFuzzyEditDistance": null,
                    "aliases": [
                        {
                            "text": "intermittently",
                            "caseSensitive": false,
                            "accentSensitive": null,
                            "fuzzyEditDistance": null
                        }
                    ]
                },
                {
                    "name": "chuck",
                    "description": null,
                    "type": null,
                    "subtype": null,
                    "id": null,
                    "caseSensitive": null,
                    "accentSensitive": null,
                    "fuzzyEditDistance": 0,
                    "defaultCaseSensitive": null,
                    "defaultAccentSensitive": null,
                    "defaultFuzzyEditDistance": null,
                    "aliases": []
                },
                {
                    "name": "trigger interference",
                    "description": null,
                    "type": null,
                    "subtype": null,
                    "id": null,
                    "caseSensitive": null,
                    "accentSensitive": null,
                    "fuzzyEditDistance": 1,
                    "defaultCaseSensitive": null,
                    "defaultAccentSensitive": null,
                    "defaultFuzzyEditDistance": null,
                    "aliases": []
                },
                {
                    "name": "dead",
                    "description": null,
                    "type": null,
                    "subtype": null,
                    "id": null,
                    "caseSensitive": null,
                    "accentSensitive": null,
                    "fuzzyEditDistance": 0,
                    "defaultCaseSensitive": null,
                    "defaultAccentSensitive": null,
                    "defaultFuzzyEditDistance": null,
                    "aliases": []
                },
                {
                    "name": "low power",
                    "description": null,
                    "type": null,
                    "subtype": null,
                    "id": null,
                    "caseSensitive": null,
                    "accentSensitive": null,
                    "fuzzyEditDistance": 0,
                    "defaultCaseSensitive": null,
                    "defaultAccentSensitive": null,
                    "defaultFuzzyEditDistance": null,
                    "aliases": []
                },
                {
                    "name": "too hot",
                    "description": null,
                    "type": null,
                    "subtype": null,
                    "id": null,
                    "caseSensitive": null,
                    "accentSensitive": null,
                    "fuzzyEditDistance": 0,
                    "defaultCaseSensitive": null,
                    "defaultAccentSensitive": null,
                    "defaultFuzzyEditDistance": null,
                    "aliases": []
                },
                {
                    "name": "unrelated",
                    "description": null,
                    "type": null,
                    "subtype": null,
                    "id": null,
                    "caseSensitive": null,
                    "accentSensitive": null,
                    "fuzzyEditDistance": 0,
                    "defaultCaseSensitive": null,
                    "defaultAccentSensitive": null,
                    "defaultFuzzyEditDistance": null,
                    "aliases": []
                }
            ]
        },
```
The next place you’ll update the skillset will be as part of the “Shaper Skill” the shaper skill allows for complex types in the response. Add the following to the “Pages” section of the Shaper Skill. 
```
                        {
                            "name": "customentity",
                            "source": "/document/Reviews/pages/*/customentity",
                            "sourceContext": null,
                            "inputs": []
                        },
```

Here is the full Shaper Skill for reference:

```
{
            "@odata.type": "#Microsoft.Skills.Util.ShaperSkill",
            "name": "#7",
            "description": null,
            "context": "/document",
            "inputs": [
                {
                    "name": "CREATEDDATE",
                    "source": "/document/CREATEDDATE",
                    "sourceContext": null,
                    "inputs": []
                },
                {
                    "name": "ItemSearchName",
                    "source": "/document/ItemSearchName",
                    "sourceContext": null,
                    "inputs": []
                },
                {
                    "name": "ProductType",
                    "source": "/document/ProductType",
                    "sourceContext": null,
                    "inputs": []
                },
                {
                    "name": "Reviews",
                    "source": "/document/Reviews",
                    "sourceContext": null,
                    "inputs": []
                },
                {
                    "name": "Rating",
                    "source": "/document/Rating",
                    "sourceContext": null,
                    "inputs": []
                },
                {
                    "name": "AzureSearch_DocumentKey",
                    "source": "/document/AzureSearch_DocumentKey",
                    "sourceContext": null,
                    "inputs": []
                },
                {
                    "name": "metadata_storage_content_type",
                    "source": "/document/metadata_storage_content_type",
                    "sourceContext": null,
                    "inputs": []
                },
                {
                    "name": "metadata_storage_size",
                    "source": "/document/metadata_storage_size",
                    "sourceContext": null,
                    "inputs": []
                },
                {
                    "name": "metadata_storage_last_modified",
                    "source": "/document/metadata_storage_last_modified",
                    "sourceContext": null,
                    "inputs": []
                },
                {
                    "name": "metadata_storage_content_md5",
                    "source": "/document/metadata_storage_content_md5",
                    "sourceContext": null,
                    "inputs": []
                },
                {
                    "name": "metadata_storage_name",
                    "source": "/document/metadata_storage_name",
                    "sourceContext": null,
                    "inputs": []
                },
                {
                    "name": "metadata_storage_path",
                    "source": "/document/metadata_storage_path",
                    "sourceContext": null,
                    "inputs": []
                },
                {
                    "name": "metadata_storage_file_extension",
                    "source": "/document/metadata_storage_file_extension",
                    "sourceContext": null,
                    "inputs": []
                },
                {
                    "name": "pages",
                    "source": null,
                    "sourceContext": "/document/Reviews/pages/*",
                    "inputs": [
                        {
                            "name": "sentimentScore",
                            "source": "/document/Reviews/pages/*/sentiment",
                            "sourceContext": null,
                            "inputs": []
                        },
                        {
                            "name": "customentity",
                            "source": "/document/Reviews/pages/*/customentity",
                            "sourceContext": null,
                            "inputs": []
                        },
                        {
                            "name": "languageCode",
                            "source": "/document/language",
                            "sourceContext": null,
                            "inputs": []
                        },
                        {
                            "name": "translatedText",
                            "source": "/document/Reviews/pages/*/translated_text",
                            "sourceContext": null,
                            "inputs": []
                        },
                        {
                            "name": "keyPhrases",
                            "source": "/document/Reviews/pages/*/keyphrases/*",
                            "sourceContext": null,
                            "inputs": []
                        },
                        {
                            "name": "Entities",
                            "source": null,
                            "sourceContext": "/document/Reviews/pages/*/entities/*",
                            "inputs": [
                                {
                                    "name": "Entity",
                                    "source": "/document/Reviews/pages/*/entities/*/name",
                                    "sourceContext": null,
                                    "inputs": []
                                },
                                {
                                    "name": "EntityType",
                                    "source": "/document/Reviews/pages/*/entities/*/type",
                                    "sourceContext": null,
                                    "inputs": []
                                },
                                {
                                    "name": "EntitySubType",
                                    "source": "/document/Reviews/pages/*/entities/*/subType",
                                    "sourceContext": null,
                                    "inputs": []
                                },
                                {
                                    "name": "Url",
                                    "source": "/document/Reviews/pages/*/entities/*/wikipediaUrl",
                                    "sourceContext": null,
                                    "inputs": []
                                }
                            ]
                        },
```

The final place you’ll need to update the skillset is in the Knowledge Store section. Add a new table for your custom entity:

```
                  {
                        "tableName": "{{your-skillset-name}}customentity",
                        "referenceKeyName": null,
                        "generatedKeyName": "customentityid",
                        "source": null,
                        "sourceContext": "/document/tableprojection/pages/*/customentity/*",
                        "inputs": [
                            {
                                "name": "customentity",
                                "source": "/document/tableprojection/pages/*/customentity/*",
                                "sourceContext": null,
                                "inputs": []
                            }
                        ]
                    },
```

You’re now ready to send the request. Change **GET** to **PUT** and you should get a response of 204 No Content.

## Update Indexer

The final step when adding the custom entity skill is to update the Indexer. We will follow the same process. To Begin, issue a **GET** request to `https://{{your-search-service-name}}.search.windows.net/indexers/{{indexer-name}}?api-version=2019-05-06`. Make sure you also include the Headers with api-key and content-type. You should receive a 200 OK status. 

Copy the response and add it to the body as before. Update the “Outfield Mappings” with the text below (*Make sure you update Outfield Mappings not Field Mappings*):

```
        {
            "sourceFieldName": "/document/Reviews/pages/*/customentity/*/name",
            "targetFieldName": "customentity",
            "mappingFunction": null
        },
```
Change **GET** to **PUT** and send the request. You should get a status of 204 No Content in response.

## Re-Run the Indexer

After you’ve finished updating your index, indexer, and skillset, log back into the Azure portal to re-run your index. You can do this by navigating to the Search service overview page, and clicking on the name of your Indexer, located near the middle-bottom of the screen. Click “reset” and then “run”, as shown in the image below. It may take upwards of 30 minutes to rerun the index. Note that even if the indexer runs with a “Warning” you can still proceed; however, if it fails, you’ll need to examine further.

![Image](https://github.com/balakreshnan/MilwaukeeTools/blob/master/TextAnalytics/RunIndexerImage3.png)

## Update Power BI

To add the custom entity data to Power BI you’ll use the same process as before to load the data:

* Once in PowerBI, select **Get Data** from Azure Tables
* Enter the name of the storage account when prompted
* You should see a new table called “{{your-skillset-name}}customentity”. Check the box next to this table
* Click “Transform Data”
* Delete the PartitionKey, RowKey, and TimeStamp columns and expand the content column
* Highlight the content.customentity column then on the Transform tab click **Parse**, **JSON** per the Image below

![Image](https://github.com/balakreshnan/MilwaukeeTools/blob/master/TextAnalytics/CustomEntityImage4.png)

* Expand the column again and delete the “matches” column
* After completing the transformations, click “Close & Apply”
* Before creating visualizations, navigate to the “Model” view on the left-hand side menu
* Change all of the connections to “Both” by right-clicking the arrows between tables

![Image](https://github.com/balakreshnan/MilwaukeeTools/blob/master/TextAnalytics/TableConnectionImage4.png)

* Update visualizations using the same process as the previous tutorial


