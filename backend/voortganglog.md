# Backendvoortgang

De historische onderdelen beschrijven de toenmalige stand. De meest recente stand staat onderaan.

## Ontwerpstart (historisch)

De casuseisen zijn vertaald naar een backendwerkstroom en een frontendwerkstroom binnen één repo. Een voorlopig datasheet, infoschema, API-contract en wireframes zijn opgesteld. De officiële bronpagina's zijn geraadpleegd. Bronrecords, CRS per dataset, importgedrag en kaartweergave waren op dat moment nog niet getest.

D01 was de volgende stap: een Liander-elektriciteitslaag kiezen en een klein sample inspecteren, een concrete Enexis-bron bevestigen en een Stedin-bestand onderzoeken. Reviewvorm en beschikbare Azure- en ArcGIS-omgeving waren nog niet bevestigd.

## Bronproeven afgerond (historisch)

Voor Liander, Enexis en Stedin is een kleine bronproef uitgevoerd. De uitkomsten zijn in het datasheet vastgelegd. D02 was de volgende stap: één kabelcategorie en één klein gebied kiezen voor de eerste import.

## 18 november 2026 — D02 en samenwerking

Backend en frontend worden door afzonderlijke ontwikkelaars binnen dezelfde repo uitgewerkt. Dit vervangt de eerdere beschrijving als individuele uitvoering. Er is nog geen frontendwerk toegevoegd; de frontendontwikkelaar heeft nog geen toegang tot de repo. De oude frontendschetsen en frontend-README zijn verwijderd.

Voor D02 is Liander-laagspanning (laag 112) gekozen. De RD-bbox is [210650, 467800, 210850, 468000], met intersects en behoud van volledige geometrie. De bronquery gaf 23 records. Handmatige controles bevestigen 23 ingevulde en unieke bron-ID's en niet-lege paden met minimaal twee verschillende x/y-punten. Het gecontroleerde veldschema bevat alleen OBJECTID en Shape__Length.

De mapping, onbekende kenmerken, bron-ID- en geometrieafkeur, vijf kwaliteitsmaten en het onderscheid tussen peildatum en ophaalmoment zijn vastgesteld. De regels en beperkingen staan in het [datasheet](docs/datasheet.md). D02 is als ontwerptaak afgerond; automatische validatie, coördinatentransformatie en importgedrag zijn nog niet getest. Er is geen featurecode toegevoegd.

De volgende stap is **D03**: backendarchitectuur en API-contract reviewen, integratie met de frontend afstemmen en docent-review bevestigen. Hosting, database, API-bbox-CRS, interne geometrieopslag, mappingversie, bron-ID-stabiliteit en beschikbare Azure-/ArcGIS-omgeving blijven te beoordelen.

De eerdere Git-publicatie en eventuele merge zijn niet gecontroleerd. Bij de documentatiebewerkingen zijn geen Git-commando's uitgevoerd.

## 18 november 2026 — Documentatie opgeschoond

Backendtaken en voortgang staan uitsluitend onder `backend/`. Het datasheet en de architectuur staan onder `backend/docs/`. Alleen gedeelde documenten staan rechtstreeks onder `docs/`: het API-contract en de scope. De oude doorverwijzingsbestanden en lege documentatiesubmappen zijn verwijderd. De root-README biedt een overzicht van de repo. Alle documentatie is neutraal geformuleerd, zonder persoonlijk perspectief. D01 en D02 blijven afgevinkt; D03 en implementatietaken blijven open.

## 18 november 2026 — Goedkeuring beperkte lokale B01-proef

De SDK-inventarisatie is doorgegeven: 9.0.319 en 10.0.400 zijn geïnstalleerd. Een aanvullende controle vanuit de backendmap meldde .NET 10 als geselecteerde versie. Deze meldingen komen uit de handmatig uitgevoerde terminalcontroles; er is nog geen project gebouwd.

Voor de eerste proef is één ASP.NET Core Web API-project op .NET 10 gekozen, onder `backend/NetUnify.Api/`. HTTP-afhandeling komt in een JobsController, fictieve gegevens en sortering in een JobService en antwoordmodellen in Contracts. Het jobs-antwoord, de lege lijst en het onderscheid tussen onbekende en beschikbare tellingen zijn besproken en akkoord. De volledige beschrijving staat in [B01: lokale jobsproef](docs/b01-jobsproef.md); de JSON-afspraken staan in het [API-contract](../docs/api-contract.md).

Er is expliciet toestemming gegeven om deze beperkte lokale proef te bouwen terwijl D03 en frontendafstemming nog openstaan. De proef bevat alleen fictieve jobs, zonder database, bronimport, echte jobverwerking of geïmplementeerde rolbeveiliging. Deze toestemming bevestigt geen afgeronde volledige docent- of gezamenlijke frontendreview. D03 en B01 blijven als totale taken open.

De documentatie is bijgewerkt met uitleg voor nieuwe lezers en met onderscheid tussen gecontroleerde feiten, huidige ontwerpafspraken, voorstellen, open vragen en nog te bouwen gedrag. Ontwerpkeuzes mogen tijdens de ontwikkeling worden herzien; reden, gevolgen en eventuele wijzigingen aan API-afspraken moeten dan worden bijgewerkt. De historische voortgang blijft als momentopname behouden.

Er zijn bij deze stap alleen documenten aangepast. Het API-project, endpoint en tests zijn nog niet aangemaakt. De volgende uitvoerstap is het lege API-project voorbereiden en vervolgens build en lokale start controleren, steeds in kleine stappen.

## 24 november 2026 — Lokale B01-jobsproef gecontroleerd

De lokale ASP.NET Core Web API op .NET 10 is gebouwd. JobsController, JobService, JobDto, JobCountsDto en JobsResponseDto zijn toegevoegd en de service is met AddScoped geregistreerd. De klasse JobsController staat in Controllers/JobController.cs. De bestaande WeatherForecast-voorbeeldbestanden zijn aanwezig.

De laptopcontrole trof SDK 8.0.428, 9.0.319 en 10.0.400 aan; 10.0.400 werd geselecteerd. De ontwikkelaar bevestigde build, lokale start op http://localhost:5216 en het WeatherForecast-antwoord. Daarna zijn de drie fictieve jobs met het afgesproken antwoordobject en HTTP 200 gecontroleerd.

De herzienbare typekeuzes zijn Guid voor jobId, DateTimeOffset in UTC voor submittedAt, string voor operator/dataset/status en long voor tellingen. Counts is een optioneel JobCountsDto. Het antwoord gebruikt IReadOnlyList<JobDto> en een optionele tekstcursor. Bij gelijke submittedAt is jobId oplopend gekozen om de volgorde vast te maken. De gecontroleerde volgorde is …002, …003, …001. De jobs-API is hiermee de huidige lokale contractbasis; gezamenlijke frontendafstemming volgt.

De JSON-controle bevestigt counts null bij de wachtende job, 0/0/0 bij de fictieve afgeronde lege verwerking en 23/21/2 bij de andere afgeronde job. De balans 23 = 21 + 2 sluit. Deze waarden zijn demonstratiegegevens. Op 24 november bevestigde de ontwikkelaar ook de tijdelijk lege lijst met HTTP 200. Daarna zijn de fictieve jobs en sortering hersteld en opnieuw gecontroleerd.

De controles zijn handmatig door de ontwikkelaar uitgevoerd en in de chat gemeld. Voor deze documentatie-update is de actuele broncode gelezen. De twee B01-subtaken voor project/build/start en jobs-antwoord zijn afgevinkt. De readmes, proefbeschrijving, architectuur en gedeelde scope en het API-contract zijn bijgewerkt. Historische logregels behouden hun toenmalige stand.

B01 blijft open voor frontendafstemming en daadwerkelijke frontendkoppeling. D03 blijft open voor de volledige review. De volgende stap is de relevante D03-reviewpunten behandelen voor verdere bronimport. Er zijn nog geen automatische tests, bronimport, database, echte jobuitvoering of rolbeveiliging aangetoond. De casusgrens blijft geen KLIC en geen informatie voor graafveiligheid.

## 24 november 2026 — Lokale B02-proef en geometriekeuzes gecontroleerd

Na de B01-proef zijn RD New (EPSG:28992) als intern CRS, MultiLineString en NetTopologySuite voor geometrieverwerking besproken en goedgekeurd. Voor kaartuitvoer is een getransformeerde kopie afgesproken; ETRS89 wordt voor visualisatie gelijkgesteld aan WGS84. De ontwikkelaar controleerde in de NSGI-tool RD [210650,467800] met als ETRS89-resultaat breedte 52.195506749 en lengte 6.201141586 graden. De toekomstige uitvoervolgorde is [6.201141586,52.195506749]. Dit bevestigt het referentieresultaat; de eigen transformatie is nog te bouwen en te testen.

De ontwikkelaar heeft vervolgens stapsgewijs de lokale Liander-proef uitgevoerd. NetTopologySuite 2.6.0, het Asset-model, LianderConnector, geometrie- en assetmapper, ImportResult met QuarantineRecord en LianderImportProcessor zijn toegevoegd. De connector is geregistreerd met AddHttpClient. De vaste D02-bbox gebruikt intersects en volledige geometrie. De ontwikkelroute /dev/liander/sample is alleen in Development beschikbaar.

De live controle gaf 23 bronrecords, 23 MultiLineStrings in RD, 23 unieke bron-ID's en asset-ID's en de afgesproken mappingwaarden. De processor keurt ontbrekende/ongeldige ID's, alle dubbele ID-voorkomens en onbruikbare lijnstructuren af. Mappingversie liander-lv-v1 is opgenomen in het importresultaat en het lokale controleantwoord. De laatste teruggekoppelde uitkomst was 23 gelezen, 23 geaccepteerd, 0 in quarantaine, countsBalance true en errors []. Het controle-endpoint gebruikt een vast fictief job-ID en bewaart het resultaat in geheugen.

Het xUnit-project NetUnify.Api.Tests bevat 15 vaste tests. De ontwikkelaar bevestigde 15 geslaagd, 0 mislukt en 0 overgeslagen. De tests dekken mapping, padvolgorde, onbekende optionele waarden, ID-afkeur, lijnstructuurafkeur, duplicaatvoorrang op geometrieafkeur, lege selectie, sluitende tellingen, mappingversie en behoud van een 300 meter lange lijn buiten de selectiegrens. De tests voeren geen live bronquery uit en bewijzen de serverselectie met intersects niet onafhankelijk.

Voor deze documentatie-update zijn de actuele implementatie en tests gelezen. De testuitvoering en live antwoorden zijn door de ontwikkelaar uitgevoerd en teruggekoppeld; ze zijn voor deze tekstupdate niet opnieuw uitgevoerd. B02 is afgevinkt volgens de bestaande criteria voor de lokale vaste RD-selectie. B01 blijft open voor frontendintegratie en D03 voor de volledige review.

De readmes, TODO, het datasheet en de architectuur zijn bijgewerkt. De volgende stap is de WGS84-transformatie uitwerken en tegen het NSGI-referentiepunt testen, inclusief een gekozen tolerantie. Paginering, CRS-metadata-controle in de connector, databaseopslag, duurzame jobuitvoering, snapshots, herstel, export en beveiliging zijn vervolgwerk. Historische logregels behouden hun toenmalige stand. De casusgrens blijft geen KLIC en geen informatie voor graafveiligheid.

## 26 november 2026 — RD-naar-WGS84-transformer en documentatiecontrole

Werkstroom: backend. Taken: WGS84-vervolg op B02, transformatiekeuze binnen D03 en documentatieonderhoud onder I03. De repo is op de huidige pc aangetroffen onder `C:\xampp\htdocs\Educom\Cases\NetUnify`. SDK 9.0.319 en 10.0.400 zijn aanwezig; vanuit de repo wordt 10.0.400 geselecteerd. De nieuwe gegevens- en bestandsoverzichten en de eerder beschreven Liander-bestanden zijn lokaal aanwezig.

ProjNET 2.1.0 en `Mapping/RdToWgs84Transformer.cs` zijn toegevoegd. De transformer maakt met een RD-WKT-definitie en een zevenparameter-datumtransformatie een nieuwe MultiLineString met SRID 4326, lengtegraad vóór breedtegraad. De interne RD-geometrie blijft behouden. De benadering is bedoeld voor visualisatie en is geen volledige RDNAPTRANS-implementatie.

De ontwikkelaar bevestigde drie transformatietests: het NSGI-punt en behoud van de oorspronkelijke lijn, meerdere paden met puntaantallen en richtingcontroles, en afkeur van een verkeerd invoer-SRID. Voor RD [210650,467800] is uitvoer [6.201141413686673,52.19550675508516] teruggekoppeld. Het verschil met de vastgelegde referentie is respectievelijk -1.723133271624988E-07 en 6.085159043323074E-09 graad. Beide vallen binnen de voorlopige tolerantie van 0.00001 graad per as. Dit bewijst geen gebiedsbrede nauwkeurigheid.

De tijdelijke diagnostische Assert.Fail is verwijderd. De ontwikkelaar bevestigde daarna 18 geslaagde tests, 0 mislukt en 0 overgeslagen. De actuele implementatie, configuratie, projectbestanden, tests en documentatie zijn voor deze update gelezen. Tests, lokale HTTP-routes en de live bronquery zijn tijdens deze documentatiecontrole niet opnieuw uitgevoerd. De API roept de transformer nog niet aan; GeoJSON-uitvoer is niet gebouwd.

De readmes, TODO, het datasheet, de architectuur, de bestands- en gegevensoverzichten zijn bijgewerkt. De B01-beschrijving verduidelijkt dat de automatische tests Liander en transformatie betreffen; het API-contract bevat een gecorrigeerde interne tekstverwijzing. Historische logregels blijven momentopnamen. D03, B01 en I03 blijven als volledige taken open. Alleen de gecontroleerde afzonderlijke transformatiesubtaak is afgevinkt. De code is bij deze documentatie-update niet gewijzigd en er zijn geen Git-commando's uitgevoerd.

Voorstel voor de volgende stap: de getransformeerde uitvoerkopie via een Development-controleroute inspecteren en daarna GeoJSON-uitvoer uitwerken. Verdere nauwkeurigheidscontrole, paginering, CRS-metadata-controle, opslag, duurzame jobs en frontendafstemming blijven vervolgwerk. De casusgrens blijft geen KLIC en geen informatie voor graafveiligheid.

## 26 november 2026 — GeoJSON-mapper en Development-routes gecontroleerd

Werkstroom: backend. Taken: lokaal GeoJSON-vervolg op B02, relevante D03-uitwerking en documentatieonderhoud onder I03. De ontwikkelaar heeft de transformer aangesloten op /dev/liander/sample en de GeoJSON-opbouw verplaatst naar Mapping/AssetGeoJsonMapper.cs. De sample-route toont nu naast de tellingen maximaal één RD/WGS84-coördinatenvoorbeeld en een GeoJSON-collectie. De nieuwe /dev/liander/geojson-route levert rechtstreeks een FeatureCollection met application/geo+json. Beide routes staan binnen de Development-controle.

De ontwikkelaar bevestigde bij de sample-route 23 geaccepteerde assets, 23 features, CountsMatch true en uitsluitend MultiLineStrings. Voor de afzonderlijke GeoJSON-route zijn HTTP 200, application/geo+json en na expliciete tekstomzetting een FeatureCollection met 23 features teruggekoppeld. Het oorspronkelijke PowerShell-inhoudstype is niet bevestigd. Iedere route doet een nieuwe bronaanroep en gebruikt het vaste voorbeeld-job-ID; asset-ID's worden per verwerking opnieuw gegenereerd.

Twee vaste tests zijn toegevoegd in NetUnify.Api.Tests/AssetGeoJsonMapperTest.cs, met klasse AssetGeoJsonMapperTests. Ze controleren de lege collectie en een geserialiseerde feature met bronproperties, asset-ID, padstructuur, referentiepunt en behoud van RD-geometrie. De ontwikkelaar bevestigde na de nieuwe route 20 geslaagde tests, 0 mislukt en 0 overgeslagen. De routes zijn handmatig gecontroleerd; automatische HTTP-integratietests zijn niet toegevoegd.

Program.cs, de GeoJSON-mapper en de mappertests zijn voor deze documentatie-update gelezen. De readmes, TODO, architectuur, het datasheet, de bestands- en gegevensoverzichten en de B01-testbeschrijving zijn bijgewerkt. Tests en live bronaanroepen zijn tijdens deze documentatie-update niet opnieuw uitgevoerd. Er is alleen documentatie gewijzigd; er zijn geen Git-commando's uitgevoerd.

De lokale GeoJSON-subtaak is als gecontroleerd vastgelegd. D03, B01 en I03 blijven als volledige taken open. Het publieke assets-endpoint, frontendkoppeling, ruimere nauwkeurigheidscontrole, CRS-metadata-controle, opslag en duurzame exports volgen later. Voorstel voor de volgende kleine stap: de bron-CRS-metadata valideren. De casusgrens blijft geen KLIC en geen informatie voor graafveiligheid.

## 26 november 2026 - Connector-CRS-controle en 34 tests bevestigd

Werkstroom: backend. Taken: CRS-vervolg onder B02 en documentatieonderhoud onder I03. De ontwikkelaar controleerde het live bronantwoord met dezelfde query als LianderConnector: wkid en latestWkid zijn beide 28992; geometryType is esriGeometryPolyline. De SDK-inventarisatie op deze computer bevestigde daarnaast SDK 8.0.428, 9.0.319 en 10.0.400, met 10.0.400 geselecteerd.

De ontwikkelaar voegde in LianderConnector de controle toe op spatialReference.wkid 28992 en op latestWkid 28992 indien aanwezig, voor het uitlezen van features. Ontbrekende, ongeldige of afwijkende metadata stopt de volledige aanroep. De build is door de ontwikkelaar bevestigd. LianderConnectorTests.cs bevat twee geldige en twaalf ongeldige scenario's met vaste HTTP-antwoorden. De ontwikkelaar bevestigde daarna 34 geslaagde tests: 15 verwerking, 3 transformatie, 2 GeoJSON-mapper en 14 connector-CRS-testgevallen.

De actuele connector en testcode zijn voor deze documentatie-update gelezen. De live Development-routes zijn na de wijziging nog niet opnieuw gecontroleerd. De readmes, TODO, het datasheet, de architectuur, de bestands- en gegevensoverzichten en de B01-testbeschrijving zijn bijgewerkt. D03, B01 en I03 blijven open; alleen de CRS-subtaak is als gecontroleerd afgevinkt. Historische voortgangsregels behouden hun toenmalige stand. Tijdens deze documentatie-update zijn geen applicatiecode of tests gewijzigd en geen Git-commando's uitgevoerd. Tests zijn niet door de assistent opnieuw uitgevoerd.

Voorstel voor de volgende kleine stap: de live /dev/liander/sample-route opnieuw controleren met de gewijzigde connector. De casusgrens blijft geen KLIC en geen informatie voor graafveiligheid.

## 26 november 2026 - Live sample na CRS-wijziging bevestigd

Werkstroom: backend. Taken: live controle onder B02 en documentatieonderhoud onder I03. De ontwikkelaar bevestigde na de connectorwijziging via /dev/liander/sample mappingVersion liander-lv-v1, 23 gelezen, 23 geaccepteerd, 0 in quarantaine, countsBalance true en errors []. De live sample-subtaak is daarmee afgevinkt. De teruggekoppelde selectie bevat geen afzonderlijke GeoJSON-controle.

De readmes, TODO, architectuur, bestands- en gegevensoverzichten en het datasheet zijn bijgewerkt. De eerdere logregels behouden hun toenmalige stand. Er zijn alleen documenten gewijzigd; geen applicatiecode, tests of Git-commando's. Tests en bronaanroepen zijn niet door de assistent opnieuw uitgevoerd. D03, B01 en I03 blijven open. Voorstel voor de volgende kleine stap: de afzonderlijke /dev/liander/geojson-route opnieuw controleren na de CRS-wijziging. Geen KLIC en geen informatie voor graafveiligheid.

## 26 november 2026 - Live GeoJSON na CRS-wijziging bevestigd

Werkstroom: backend. Taken: laatste live controle van het CRS-vervolg onder B02 en documentatieonderhoud onder I03. De ontwikkelaar bevestigde voor /dev/liander/geojson HTTP 200, application/geo+json, FeatureCollection en 23 features. De betreffende TODO-subtaak is afgevinkt. Samen met de eerder herhaalde sample-controle en 34 geslaagde tests is het afgebakende CRS-vervolg afgerond.

De readmes, TODO, architectuur, bestands- en gegevensoverzichten en het datasheet zijn bijgewerkt. Er zijn alleen documenten gewijzigd. Applicatiecode en tests zijn niet gewijzigd; er zijn geen Git-commando's uitgevoerd. De controles zijn door de ontwikkelaar uitgevoerd en teruggekoppeld; tests en live aanroepen zijn niet door de assistent herhaald. Historische logregels behouden hun toenmalige stand. D03, B01 en I03 blijven open, evenals paginering, opslag, duurzame jobs, het publieke assets-endpoint en de overige vastgelegde vervolgpunten. Geen KLIC en geen informatie voor graafveiligheid.
