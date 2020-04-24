FROM mcr.microsoft.com/dotnet/core/sdk:3.1-alpine as build

# copy csproj and restore 
WORKDIR /app
COPY ./*.sln .
COPY ./src/KafkaSpy/*.csproj ./src/KafkaSpy/
COPY ./test/KafkaSpy.Tests/*.csproj ./test/KafkaSpy.Tests/

RUN dotnet restore

# copy everything else and build app
COPY ./src/KafkaSpy ./src/KafkaSpy
COPY ./test/KafkaSpy.Tests ./test/KafkaSpy.Tests
WORKDIR /app/src/KafkaSpy
RUN dotnet build

FROM build AS testrunner
WORKDIR /app/test/KafkaSpy.Tests
ENTRYPOINT ["dotnet", "test", "--logger:trx"]

# FROM build AS test
# WORKDIR /app/test/KafkaSpy.Tests
# RUN dotnet test

FROM build AS publish
WORKDIR /app/src/KafkaSpy
RUN dotnet publish -c Release -p:PublishTrimmed=true -r linux-x64 -o out


FROM mcr.microsoft.com/dotnet/core/runtime:3.1-alpine AS runtime
WORKDIR /app
COPY --from=publish /app/src/KafkaSpy/out ./
ENTRYPOINT ["dotnet", "KafkaSpy.core.dll"]


