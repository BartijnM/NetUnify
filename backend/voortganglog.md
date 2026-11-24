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
