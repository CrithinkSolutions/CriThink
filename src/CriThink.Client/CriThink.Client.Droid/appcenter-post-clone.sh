#!/usr/bin/env bash

# Move to AppCenter folder
cd $APPCENTER_SOURCE_DIRECTORY

# Move to project dir
cd $PROJECT_DIR

# Replace base URI
find $API_URL_FILENAME -type f -exec sed -i '' -e 's/'"$LOCAL_API_URL"'/'"$API_URL"'/' {} \;

find MainApplication.cs -type f -exec sed -i '' -e 's/<APPCENTER_API_KEY>/'"$APPCENTER_API_KEY"'/' {} \;

cd Constants/

find DeepLinkConstants.cs -type f -exec sed -i '' -e 's/crithinkdemo.com/'"$SCHEMA_HOST"'/' {} \;

cd ../Properties/
find AndroidManifest.xml -type f -exec sed -i '' -e 's/CriThink Dev/'"$APP_NAME"'/' {} \;
find AndroidManifest.xml -type f -exec sed -i '' -e 's/com.crithink.client.dev/'"$PACKAGE_NAME"'/' {} \;