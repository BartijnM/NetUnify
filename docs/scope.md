# Gedeelde scope en samenwerking

NetUnify heeft een backendwerkstroom en een frontendwerkstroom binnen één repo, uitgevoerd door afzonderlijke ontwikkelaars. Werkstroomspecifieke taken en documentatie staan bij de betreffende werkstroom. Dit document beschrijft de gedeelde scope en nog af te stemmen integratiepunten. De gezamenlijke review is nog niet afgerond.

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

Het [API-contract](api-contract.md) blijft concept. De D02-keuzes staan in het [backenddatasheet](../backend/docs/datasheet.md). Backendfeaturecode volgt pas na bevestiging van de ontwerpreview.
