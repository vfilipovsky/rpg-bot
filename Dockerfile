FROM mcr.microsoft.com/dotnet/sdk:5.0 AS development

RUN dotnet tool install --global dotnet-ef

ENV PATH="$PATH:/root/.dotnet/tools"

WORKDIR /app

COPY RpgBot/*.csproj .

RUN dotnet restore

WORKDIR /app/RpgBot

CMD ["dotnet", "watch", "run"]

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS test

WORKDIR /app

COPY RpgBotUnitTests/*.csproj .

RUN dotnet restore

COPY . .

CMD ["dotnet", "test", "RpgBotUnitTests"]