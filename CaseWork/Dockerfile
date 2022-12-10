FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["CaseWork/CaseWork.csproj", "CaseWork/"]
RUN dotnet restore "CaseWork/CaseWork.csproj"
COPY . .
WORKDIR "/src/CaseWork"
RUN dotnet build "CaseWork.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CaseWork.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CaseWork.dll"]
