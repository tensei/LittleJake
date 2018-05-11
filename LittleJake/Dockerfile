FROM microsoft/dotnet:2.0-runtime 

WORKDIR /app
COPY  /bin/release/netcoreapp2.0/publish/ .
ENTRYPOINT ["dotnet", "LittleJake.dll"]