# Backend

De backend is het serverdeel van NetUnify. Deze ontvangt API-verzoeken en gaat brondata verwerken tot uniforme assets. De beoogde functies zijn bronimport, mapping, importjobs, opslag, kwaliteitsinzichten, exports, beveiliging en tests. De frontend wordt afzonderlijk ontwikkeld en zal de API gebruiken.

## Documentatie

- [Backend-TODO](TODO.md): taken en acceptatiecriteria.
- [Backendvoortgang](voortganglog.md): uitgevoerde controles en besluiten per moment.
- [B01: lokale jobsproef](docs/b01-jobsproef.md): uitleg van de eerste bouwstap, projectindeling en controlepunten.
- [Datasheet en mapping](docs/datasheet.md): bronfeiten, importscope, omzettingsregels en bekende hiaten.
- [Backendarchitectuur en infoschema](docs/architectuur.md): huidige technische richting en open ontwerpvragen.
- [Gedeeld API-contract](../docs/api-contract.md): berichten tussen backend en frontend.
- [Gedeelde scope en werkwijze](../docs/scope.md): projectgrens, samenwerking en omgaan met wijzigingen.

## Huidige fase

D01 en D02 zijn uitgewerkt. De Liander-LS-bronproef bevat een selectie van 23 handmatig onderzochte records. De lokale jobs-API is gebouwd en handmatig gecontroleerd. De lokale B02-proef haalt de vaste Liander-selectie op en verwerkt deze in geheugen tot assets en afkeurmeldingen. De ontwikkelaar bevestigde 23 gelezen, 23 geaccepteerd en 0 in quarantaine, met mappingversie `liander-lv-v1`.

De beperkte lokale B01-proef is gebouwd en handmatig gecontroleerd: één ASP.NET Core Web API-project op .NET 10, met een GET-endpoint dat fictieve jobs teruggeeft. De proef bevat geen database, bronimport, echte jobuitvoering of geïmplementeerde rolbeveiliging. Frontendafstemming volgt later. D03 blijft als volledige review open; deze toestemming betreft alleen de beschreven proef.

Historisch zijn op de laptop SDK 8.0.428, 9.0.319 en 10.0.400 aangetroffen. Op de huidige pc zijn op 26 november 2026 SDK 9.0.319 en 10.0.400 aangetroffen; vanuit de repo wordt 10.0.400 geselecteerd. De projectbuild, lokale start en HTTP-controles zijn eerder door de ontwikkelaar uitgevoerd en bevestigd.

## Lokaal starten

Voer vanuit de reporoot uit met een .NET 10 SDK:

```powershell
dotnet build backend/NetUnify.Api/NetUnify.Api.csproj
dotnet run --project backend/NetUnify.Api/NetUnify.Api.csproj --launch-profile http
```

Het gecontroleerde HTTP-profiel luistert op `http://localhost:5216`. GET `http://localhost:5216/api/v1/jobs` geeft drie fictieve jobs met HTTP 200. Stop de applicatie met `Ctrl+C`.

De controles van de lege lijst en het herstel van de fictieve jobs staan in [B01: lokale jobsproef](docs/b01-jobsproef.md).

## Lokale Liander-proef en tests

GET `http://localhost:5216/dev/liander/sample` voert in Development de vaste Liander-bboxquery uit en verwerkt het antwoord in geheugen. Het antwoord bevat een vast fictief job-ID, mappingversie, tellingen, balanscontrole, afkeurmeldingen, maximaal één coördinatenvoorbeeld onder `coordinateSamples` en de GeoJSON-collectie onder `geoJson`. De vaste bbox en de uitkomsten staan in het [datasheet](docs/datasheet.md#b02--lokale-liander-verwerking-gecontroleerd-op-24-november-2026). Dit is een ontwikkelcontrole met een live bronaanroep. `/api/v1/jobs` levert nog steeds de afzonderlijke fictieve jobs.

Voer vanuit de reporoot de automatische mapping-, validatie- en transformatietests uit:

```powershell
dotnet test backend/NetUnify.Api.Tests/NetUnify.Api.Tests.csproj
```

De ontwikkelaar bevestigde op 26 november 2026 20 geslaagde tests, 0 mislukt en 0 overgeslagen: 15 voor Liander, 3 voor de transformatie en 2 voor de GeoJSON-mapper. De tests gebruiken vaste gegevens en roepen Liander niet aan. De bronquery is eerder afzonderlijk handmatig gecontroleerd. Het interne CRS is RD New (`EPSG:28992`), met NetTopologySuite 2.6.0 en `MultiLineString`. `Mapping/RdToWgs84Transformer.cs` gebruikt ProjNET 2.1.0 en maakt een nieuwe MultiLineString met SRID 4326, lengtegraad vóór breedtegraad. De oorspronkelijke RD-geometrie blijft behouden. Het NSGI-referentiepunt, meerdere paden en een verkeerd invoer-SRID zijn getest. De voorlopige tolerantie is 0.00001 graad per as. De zevenparameterbenadering is geen volledige RDNAPTRANS-implementatie; nauwkeurigheid over het hele gebied is niet aangetoond. De transformer is via `Mapping/AssetGeoJsonMapper.cs` aangesloten op de twee Liander-ontwikkelroutes. GET `http://localhost:5216/dev/liander/geojson` geeft rechtstreeks een FeatureCollection terug met inhoudstype `application/geo+json`. De ontwikkelaar bevestigde HTTP 200 en 23 MultiLineString-features, gelijk aan de 23 geaccepteerde assets. Beide routes staan binnen de Development-controle; het publieke `/api/v1/assets`-endpoint is nog niet gebouwd. Zie het [datasheet](docs/datasheet.md#26-november-2026--eerste-transformatiecontrole) voor waarden en beperkingen.

## Voorlopige keuzes

Azure App Service en Azure SQL zijn voorstellen. Frontendtechniek, hosting, authenticatie, API-bbox-CRS en verschillende import- en opslagdetails blijven te beoordelen. De huidige keuzes zijn een basis voor stapsgewijze ontwikkeling en mogen met vastlegging van reden en gevolgen worden herzien.

**Geen KLIC en geen informatie voor graafveiligheid.**

Bron voor de opdracht is de aangeleverde Educom-casus NetUnify, p. 1–2.

## GeoJSON lokaal controleren

Laat de API draaien en voer in een tweede PowerShell-terminal uit:

```powershell
$response = Invoke-WebRequest -Uri 'http://localhost:5216/dev/liander/geojson'
$jsonText = if ($response.Content -is [byte[]]) {
    [System.Text.Encoding]::UTF8.GetString($response.Content)
} else {
    [string]$response.Content
}
$geoJson = ConvertFrom-Json -InputObject $jsonText
[pscustomobject]@{
    StatusCode = $response.StatusCode
    ContentType = $response.Headers['Content-Type'] -join ', '
    Type = $geoJson.type
    FeatureCount = @($geoJson.features | Where-Object { $null -ne $_ }).Count
}
```

Gecontroleerd door de ontwikkelaar: HTTP 200, `application/geo+json`, `FeatureCollection` en 23 features. De expliciete tekstomzetting verwerkt zowel tekst als eventuele UTF-8-bytes; het oorspronkelijke PowerShell-inhoudstype is niet teruggekoppeld. Iedere routeaanroep haalt de bron opnieuw op, waardoor aantallen kunnen veranderen. Assets krijgen per verwerking nieuwe GUID's; dit is nog geen stabiele gepubliceerde dataset of duurzame export.
