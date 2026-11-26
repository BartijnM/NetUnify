# B01 — Lokale jobsproef

## Doel en status

De lokale jobsproef is gebouwd in `backend/NetUnify.Api/` op .NET 10. GET `/api/v1/jobs` geeft vaste fictieve importjobs terug. Een job stelt een verwerkingsopdracht voor; de voorbeeldgegevens worden rechtstreeks in `JobService` samengesteld.

De beperkte proef is op 18 november 2026 goedgekeurd. De ontwikkelaar heeft build, lokale start, antwoordvelden, sortering, tellingen en HTTP 200 handmatig gecontroleerd. Op 24 november zijn ook de lege lijst met HTTP 200 en het herstel van de fictieve jobs bevestigd. Voor het jobs-endpoint zijn geen automatische tests aanwezig; de huidige 34 automatische tests betreffen de Liander-verwerking, de transformatie, de GeoJSON-mapper en de CRS-controle in de connector. B01 als volledige integratieslice en de D03-review staan open.

## Projectindeling

```text
backend/NetUnify.Api/
├── Controllers/JobController.cs   klasse JobsController, HTTP-afhandeling
├── Services/JobService.cs         vaste fictieve jobs en sortering
├── Contracts/JobDto.cs            JobDto en JobCountsDto
├── Contracts/JobsResponseDto.cs   items en nextCursor
└── Program.cs                    controller- en serviceregistratie
```

De standaard WeatherForecast-controller en het bijbehorende model zijn nog aanwezig.

Een controller ontvangt een HTTP-verzoek en levert het HTTP-antwoord. De service levert de gegevens en sortering. De DTO's beschrijven de uitwisselingsvelden. Bij GET `/api/v1/jobs` roept `JobsController` de `JobService` aan en geeft `JobsResponseDto` via `Ok(...)` terug. De service is met `AddScoped` geregistreerd.

## Omgeving en lokale start

Op de laptop zijn SDK 8.0.428, 9.0.319 en 10.0.400 aangetroffen. SDK 10.0.400 werd geselecteerd. De ontwikkelaar heeft de projectbuild en de lokale start bevestigd.

Voer vanuit de reporoot uit:

```powershell
dotnet build backend/NetUnify.Api/NetUnify.Api.csproj
dotnet run --project backend/NetUnify.Api/NetUnify.Api.csproj --launch-profile http
```

Het HTTP-profiel luistert op `http://localhost:5216`. Open `/api/v1/jobs` voor het antwoord. Controleer de status vanuit een tweede terminal:

```powershell
(Invoke-WebRequest -Uri 'http://localhost:5216/api/v1/jobs').StatusCode
```

De gecontroleerde status is `200`. Stop de API met `Ctrl+C` voordat deze opnieuw wordt gestart na een codewijziging.

## Antwoord en typekeuzes

De JSON-velden en betekenissen staan in het [gedeelde API-contract](../../docs/api-contract.md). De huidige C#-keuzes zijn `Guid` voor job-ID, `DateTimeOffset` met UTC-offset nul voor submittedAt, `string` voor operator/dataset/status en `long` voor de drie tellingen. `JobCountsDto?` maakt onbekende tellingen expliciet met `null`. De antwoordlijst gebruikt `IReadOnlyList<JobDto>` en `nextCursor` gebruikt `string?`.

Deze keuzes zijn herzienbaar. De types zijn gekozen om identificatie, tijdstip en onbekende tellingen expliciet te maken. De kleine fictieve dataset past op één pagina; `nextCursor` is `null`. Filters en echte cursorpaginering zijn vervolgvragen.

## Gecontroleerde fictieve dataset

Alle drie jobs gebruiken operator `Liander` en dataset `Liander_Open_Data_Elektra/112`. De ID's hieronder hebben prefix `00000000-0000-0000-0000-` en vaste tijden op 19 november 2026 in UTC.

| Volgorde | ID-suffix | Tijd UTC | Status | read / accepted / quarantined |
| --- | --- | --- | --- | --- |
| 1 | `000000000002` | 10:00 | `succeeded` | 0 / 0 / 0 |
| 2 | `000000000003` | 10:00 | `queued` | `counts: null` |
| 3 | `000000000001` | 09:00 | `succeeded` | 23 / 21 / 2 |

De sortering is submittedAt aflopend, gevolgd door jobId oplopend via `ThenBy`. De voorbeeldgegevens controleren beide sorteersleutels. `counts: null` betekent onbekend; 0/0/0 betekent een bekende lege verwerking. De overige afgeronde voorbeeldjob heeft sluitende tellingen, 23 = 21 + 2. Deze aantallen zijn fictief.

De controle van de lege lijst gebruikte tijdelijk `return Array.Empty<JobDto>();` in `JobService.GetJobs()`. Na herstart is HTTP 200 met `items: []` en `nextCursor: null` bevestigd. Daarna is het oorspronkelijke return-statement met sortering hersteld, de API herstart en de volgorde …002, …003, …001 opnieuw bevestigd.

## Uitgevoerde controles

| Controle | Vastgelegde uitkomst |
| --- | --- |
| Project en doelversie | Aanwezig onder backend/NetUnify.Api; net10.0 |
| Build en lokale start | Succes door ontwikkelaar bevestigd |
| Eerste HTTP-verzoek | WeatherForecast geeft records terug |
| Jobs-antwoord | Drie items, afgesproken velden en nextCursor null |
| HTTP-status jobs | 200 bevestigd via Invoke-WebRequest |
| Sortering | Nieuwste tijd eerst; bij gelijke tijd jobId oplopend |
| Tellingen | Onbekend null, bekende lege verwerking 0/0/0, eindbalans 23 = 21 + 2 |
| Lege lijst | items [], nextCursor null en HTTP 200 bevestigd |
| Herstel na lege-lijstcontrole | Drie jobs terug in de afgesproken volgorde |
| Frontendkoppeling | Nog uit te voeren |

De HTTP- en inhoudscontroles zijn handmatig door de ontwikkelaar uitgevoerd en in de chat teruggekoppeld. De broncode is voor deze documentatie-update gelezen. Statuswaarden en tellingen zijn op de voorbeeldgegevens gecontroleerd; afdwingen van de regels bij toekomstige invoer vraagt verdere implementatie.

## Functionele grens en vervolg

De lokale proef bevat fictieve jobs. Bronimport, opslag, echte jobuitvoering, authenticatie en rolbeveiliging zijn vervolgonderdelen. De volgende stap is de relevante D03-reviewpunten behandelen voor verdere bronimport. Frontendafstemming en de daadwerkelijke koppeling zijn nog onderdeel van B01. Hosting en Azure SQL blijven voorstellen.

Open ontwerpvragen staan in het [architectuurdocument](architectuur.md), bronvragen in het [datasheet](datasheet.md) en integratievragen in de [gedeelde scope](../../docs/scope.md).

**Liggingsdata voor analyse en demonstratie. Geen KLIC en geen informatie voor graafveiligheid.**
