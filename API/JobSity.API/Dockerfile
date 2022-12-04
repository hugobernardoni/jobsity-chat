#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["JobSity.API/JobSity.API.csproj", "JobSity.API/"]
COPY ["JobSity.DAO/JobSity.DAO.csproj", "JobSity.DAO/"]
COPY ["JobSity.Model/JobSity.Model.csproj", "JobSity.Model/"]
COPY ["JobSity.Repositories/JobSity.Repositories.csproj", "JobSity.Repositories/"]
RUN dotnet restore "JobSity.API/JobSity.API.csproj"
COPY . .
WORKDIR "/src/JobSity.API"
RUN dotnet build "JobSity.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "JobSity.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "JobSity.API.dll"]