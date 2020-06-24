# ML Assited insights from images with bounding box object detection

## Architecture

![alt text](https://github.com/balakreshnan/MilwaukeeTools/blob/master/images/MilwaukeeToolsresearchAIArch.jpg "Architecture")

- Move data from Source
- Create Azure ML service
- Create a Data Store pointing to where images where moved
- Create a new data labelling project
- Label bounding box
- wait for validation and inference
- Export the JSON
- Move to Cognitive Search
- Connect knowledge store to Power BI