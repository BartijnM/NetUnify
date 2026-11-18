# Gedeelde scope en samenwerking

NetUnify heeft een backendwerkstroom en een frontendwerkstroom binnen één repo, uitgevoerd door afzonderlijke ontwikkelaars. Werkstroomspecifieke taken en documentatie staan bij de betreffende werkstroom. Dit document beschrijft de gedeelde scope en nog af te stemmen integratiepunten. De volledige gezamenlijke review is nog niet afgerond. De beperkte lokale backendproef B01 is afzonderlijk goedgekeurd.

## Verdeling

| Onderdeel | Locatie |
| --- | --- |
| C# / ASP.NET Core API, bronimport, mapping, jobs, kwaliteit, exports, opslag en backendbeveiliging | `backend/`; technische documentatie onder `backend/docs/` |
| Jobs, inzichten, ArcGIS-kaart en frontendimplementatie | `frontend/`; nog geen werk toegevoegd |
| API-grens, gedeelde scope en integratie | `docs/` |

De frontendontwikkelaar heeft nog geen toegang tot de repo. Frontendwerk en frontenddocumentatie zijn nog niet toegevoegd. Razor Pages is een eerdere suggestie en geen vastgelegde frontendkeuze. App Service en Azure SQL blijven voorstellen voor de ontwerpreview.

## Casusgrens

**NetUnify gebruikt open liggingsdata voor analyse en demonstratie. Geen KLIC en geen informatie voor graafveiligheid.** Deze grens blijft zichtbaar in de root-README en moet zichtbaar zijn in de UI.

## Nog gezamenlijk af te stemmen

- API-contract, DTO's, authenticatie, API-bbox-CRS, foutafhandeling en toegang vanuit de frontend.
- Eerste integratieslice: backend levert jobs met fictieve data; frontend gebruikt het afgesproken DTO.
- Hosting, origins/CORS of proxy, omgevingen en eventuele gedeelde CI/deploymentconfiguratie.
- Verantwoordelijkheid voor ArcGIS-toegang, bronvermelding en zichtbaarheid van beperkingen in de UI.
- Reviewvorm, docent-review en de gezamenlijke demo van 5–8 minuten.

Het [API-contract](api-contract.md) blijft concept. De D02-keuzes staan in het [backenddatasheet](../backend/docs/datasheet.md). De beperkte lokale B01-proef mag nu worden gebouwd. Dit vervangt alleen voor die proef de eerdere afspraak dat alle backendfeaturecode op de volledige ontwerpreview wacht. D03 en de latere frontendafstemming blijven open.


## Documentatiestatus en omgaan met wijzigingen

De documentatie groeit mee met het project. Een afgesproken ontwerp is de huidige basis voor een stap en kan later veranderen. Een wijziging maakt een eerder uitgevoerd brononderzoek niet automatisch ongeldig; bronfeiten en ontwerpconclusies blijven daarom gescheiden.

| Status | Betekenis | Voorbeeld |
| --- | --- | --- |
| Gecontroleerd feit | Een concrete waarneming met een afgebakende controle | De Liander-bboxquery gaf 23 treffers tijdens de bronproef |
| Huidige ontwerpafspraak | Besproken en akkoord als basis voor de betreffende stap; nog herzienbaar | Eén API-project op .NET 10 voor B01 |
| Voorstel | Een mogelijke invulling die nog moet worden beoordeeld | Azure App Service en Azure SQL |
| Open vraag | Er is nog geen voldoende onderbouwd antwoord of besluit | Stabiliteit van bron-ID's tussen publicaties |
| Geïmplementeerd en getest | Gebouwd gedrag met vastgelegde uitgevoerde controles | Nog niet van toepassing op B01 bij deze documentatiestap |

Bij een wijziging worden het betreffende document en, waar nodig, TODO en API-contract bijgewerkt. Het backendvoortganglog vermeldt datum, aanleiding, gewijzigde keuze, gevolgen en uitgevoerde controles. Voor gevolgen aan de API-grens wordt afstemming met de frontend vastgelegd zodra die plaatsvindt. Historische logregels blijven momentopnamen; de actuele afspraken staan in de inhoudelijke documenten. Een taak wordt pas afgevinkt wanneer de bijbehorende controle echt is uitgevoerd.

## Goedkeuring lokale B01-proef

Op 18 november 2026 is toestemming bevestigd om de afgebakende lokale backendproef te bouwen: een jobs-endpoint met vaste fictieve gegevens. Database, bronimport, echte jobuitvoering en rolbeveiliging vallen buiten deze proef. Er is nog geen frontendkoppeling aangetoond. De goedkeuring geldt niet als afronding van alle D03-punten of als afzonderlijk bewijs van een volledige docent- en frontendreview.

De backenduitwerking staat in [B01: lokale jobsproef](../backend/docs/b01-jobsproef.md). De huidige berichten staan in het [API-contract](api-contract.md). Een lokale start, build en endpointcontrole moeten nog worden uitgevoerd.
