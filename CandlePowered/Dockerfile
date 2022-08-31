FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["CandlePowered/CandlePowered.csproj", "CandlePowered/"]
RUN dotnet restore "CandlePowered/CandlePowered.csproj"
COPY . .
WORKDIR "/src/CandlePowered"
RUN dotnet build "CandlePowered.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CandlePowered.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CandlePowered.dll"]
