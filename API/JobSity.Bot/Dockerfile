#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["JobSity.Bot/JobSity.Bot.csproj", "JobSity.Bot/"]
COPY ["JobSity.Model/JobSity.Model.csproj", "JobSity.Model/"]
RUN dotnet restore "JobSity.Bot/JobSity.Bot.csproj"
COPY . .
WORKDIR "/src/JobSity.Bot"
RUN dotnet build "JobSity.Bot.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "JobSity.Bot.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "JobSity.Bot.dll"]