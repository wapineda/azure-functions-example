FROM microsoft/azure-functions-dotnet-core2.0:2.0
ENV AzureWebJobsScriptRoot=/home/site/wwwroot
COPY . /home/site/wwwroot