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
