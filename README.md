# NetUnify

NetUnify zet open liggingsdata van Liander, Enexis en Stedin om naar één assetmodel. De backend levert importjobs, kwaliteitsinzichten en exports via een C# / ASP.NET Core API. De frontend toont jobs, inzichten en een ArcGIS-kaart.

**Liggingsdata voor analyse en demonstratie. Geen KLIC en geen informatie voor graafveiligheid.** Deze grens geldt ook voor de UI.

## Repo-indeling

Backend en frontend worden door afzonderlijke ontwikkelaars binnen één repo uitgewerkt. Taken, voortgang en technische documentatie staan bij de betreffende werkstroom. De algemene map `docs/` bevat uitsluitend gedeelde afspraken.

| Locatie | Inhoud |
| --- | --- |
| [backend/](backend/README.md) | Backendwerkstroom, taken en voortgang |
| [backend/docs/datasheet.md](backend/docs/datasheet.md) | Bronproeven, startscope, mapping en kwaliteitsregels |
| [backend/docs/architectuur.md](backend/docs/architectuur.md) | Backendarchitectuur en infoschema |
| `frontend/` | Bestemd voor frontendimplementatie en frontenddocumentatie; er is nog geen frontendwerk toegevoegd |
| [docs/api-contract.md](docs/api-contract.md) | Gedeeld API-voorstel |
| [docs/scope.md](docs/scope.md) | Gedeelde scope en integratievragen |

## Status en leeswijzer

De documentatie beschrijft het doel en de huidige ontwerpafspraken. D01 en D02 zijn uitgewerkt. De beperkte lokale B01-backendproef is gebouwd en handmatig gecontroleerd met fictieve jobs, inclusief sortering, tellingen, lege lijst en HTTP 200. De lokale B02-proef haalt de vaste Liander-LS-selectie op, mapt naar RD-geometrie en verzamelt afkeurmeldingen. De ontwikkelaar bevestigde 23 geaccepteerde records en inmiddels 20 geslaagde automatische tests: 15 voor de Liander-verwerking, 3 voor de RD-naar-WGS84-transformer met ProjNET 2.1.0 en 2 voor de GeoJSON-mapper. Het NSGI-referentiepunt is binnen de voorlopige tolerantie gecontroleerd. De Development-route `/dev/liander/geojson` levert rechtstreeks GeoJSON; HTTP 200, `application/geo+json` en 23 features zijn door de ontwikkelaar gecontroleerd. Verdere nauwkeurigheidscontrole, het publieke assets-endpoint, de volledige D03-review en frontendafstemming zijn vervolgstappen.

Voor een eerste kennismaking: lees [scope en werkwijze](docs/scope.md), vervolgens de [backendintroductie](backend/README.md) en het [API-contract](docs/api-contract.md). De uitwerking en controles van de eerste bouwstap staan in [B01: lokale jobsproef](backend/docs/b01-jobsproef.md).

Ontwerpkeuzes vormen een werkbare basis en kunnen tijdens de ontwikkeling worden aangepast. Gecontroleerde bronfeiten, ontwerpafspraken, voorstellen en nog open vragen worden apart benoemd. Een ontwerpafspraak betekent niet dat de functionaliteit al gebouwd of getest is.
