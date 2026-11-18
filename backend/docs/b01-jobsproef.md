# B01 — Lokale jobsproef

## Doel en status

Deze proef is de eerste kleine bouwstap van de backend. Een zelfstandig draaiende API moet een lijst fictieve importjobs teruggeven. Daardoor kunnen HTTP-afhandeling en antwoordmodellen worden gecontroleerd zonder frontend, database of bronimport. Een importjob is een opdracht om brongegevens te verwerken; de voorbeeldjobs voeren zelf geen werk uit.

De beperkte lokale proef is op 18 november 2026 goedgekeurd. De keuzes hieronder zijn de huidige basis en mogen tijdens de ontwikkeling met onderbouwing worden aangepast. B01 is nog niet gebouwd. De volledige D03-review en de latere frontendafstemming blijven open.

## Beschikbare omgeving

De handmatig doorgegeven SDK-inventarisatie bevat .NET SDK 9.0.319 en 10.0.400. Vanuit de backendmap is .NET 10 als geselecteerde SDK gemeld. De huidige keuze is ASP.NET Core Web API op .NET 10. De SDK bevat de hulpmiddelen om C#-projecten te maken en te bouwen; alleen een runtime is daarvoor niet voldoende. De eerste projectbuild moet de bruikbaarheid van deze omgeving nog aantonen.

## Projectindeling en verantwoordelijkheden

```text
backend/
├── NetUnify.Api/         nog aan te maken API-project
│   ├── Controllers/     HTTP-afhandeling: JobsController
│   ├── Services/        fictieve jobs en sortering: JobService
│   └── Contracts/       DTO's voor de JSON-antwoorden
├── docs/                technische backenddocumentatie
├── README.md
├── TODO.md
└── voortganglog.md
```

Dit schema toont de geplande indeling, geen reeds aangemaakte bestanden. Eén API-project houdt deze eerste stap overzichtelijk. De frontend heeft geen projectafhankelijkheid op deze applicatie nodig en kan later via HTTP aansluiten.

Een controller ontvangt het HTTP-verzoek en bepaalt het HTTP-antwoord. Een service levert de benodigde gegevens en bevat de logica die niet bij HTTP hoort. Een DTO (data transfer object) beschrijft de gegevens die over de API-grens gaan. Bijvoorbeeld: GET /api/v1/jobs komt bij JobsController binnen, die jobs bij JobService opvraagt en ze in de afgesproken antwoordvorm met HTTP 200 teruggeeft. Geen van deze stappen start een echte import.

## Functionele grens

De proef levert alleen GET /api/v1/jobs met vaste fictieve gegevens, een gesorteerde lijst en de afgesproken lege-lijstweergave. De velden, tellingen en JSON-voorbeelden staan op één gedeelde plek: het [API-contract](../../docs/api-contract.md).

De proef bouwt geen POST /jobs, database, Liander-connector, mapping, worker, retry, export of rolbeveiliging. Analyst/Admin zijn beoogde rollen voor het vervolg; ze worden met deze proef nog niet afgedwongen. De proef wordt uitsluitend lokaal uitgevoerd. Hosting en beveiligde beschikbaarstelling zijn latere stappen en zijn niet met deze toestemming vrijgegeven.

De eerste proef is zelfstandig te controleren. De frontendkoppeling volgt later en blijft onderdeel van de totale B01-integratieslice. Een geslaagde lokale proef is daarom geen bewijs dat de frontendintegratie gereed is.

## Huidige antwoordafspraken

- Antwoordobject met `items` en `nextCursor`; de kleine voorbeeldset past op één pagina.
- Jobsvelden: `jobId`, `operator`, `dataset`, `status`, `submittedAt` en `counts`.
- Nieuwste `submittedAt` eerst; `jobId` als vaste tweede sorteersleutel. De richting van die tweede sortering wordt nog gekozen.
- Geen jobs geeft HTTP 200 met `items: []` en `nextCursor: null`.
- `counts: null` betekent nog geen beschikbare tellingen. Een beschikbaar counts-object bevat `read`, `accepted` en `quarantined`.
- Tijdens verwerking of na vroegtijdige stop zijn tellingen voorlopig. Na volledige verwerking geldt `read = accepted + quarantined`.
- Een volledig verwerkte lege import heeft drie nullen, geen ontbrekend counts-object.

Deze afspraken zijn nog niet door implementatie bevestigd. Filters, echte paginering, de C#-typen en de precieze fictieve dataset worden in gerichte vervolgstappen uitgewerkt. Nieuwe keuzes worden niet stilzwijgend als al goedgekeurd behandeld.

## Geplande uitvoering en gerichte controles

De uitvoering verloopt in kleine stappen. Na iedere stap wordt de uitkomst gecontroleerd voordat de volgende code wordt toegevoegd. Het API-project wordt niet automatisch aangemaakt door deze documentatiestap.

| Stap | Zelf te controleren | Waarom |
| --- | --- | --- |
| Project aanmaken | Project staat onder backend/NetUnify.Api en richt zich op .NET 10 | Voorkomt een verkeerde locatie of doelversie |
| Eerste build en lokale start | Build slaagt; applicatie start en geeft de lokale luister-URL | Toont aan dat project en omgeving samenwerken |
| DTO's en fictieve jobs | JSON heeft de afgesproken velden, UTC-tijden, statuswaarden en counts-betekenis | Controleert het bericht dat later door de frontend wordt gebruikt |
| Jobs-endpoint | GET /api/v1/jobs geeft HTTP 200 en het antwoordobject | Controleert de koppeling van controller, service en DTO |
| Sortering | Nieuwste job eerst; gelijke tijden geven een voorspelbare volgorde | Voorkomt een wisselende volgorde bij herhaalde aanvragen |
| Lege lijst | HTTP 200 met lege items en nextCursor null | Onderscheidt geen gegevens van een verzoekfout |
| Tellingen | Wachtend is null; afgerond leeg is 0/0/0; volledig verwerkt heeft sluitende aantallen | Voorkomt verwarring tussen onbekend, nul en definitieve aantallen |
| Latere frontendintegratie | Frontend verwerkt het afgesproken antwoord en de lege toestand | Toont pas dan aan dat de volledige integratieslice werkt |

De concrete commando's, bestanden en codefragmenten volgen per uitvoerstap. Uitgevoerde controles en resultaten worden daarna in het voortganglog vastgelegd; deze tabel bevat nog geen behaalde resultaten.

## Open punten buiten of na de proef

De volledige D03-review, docent-review, frontendafstemming, authenticatie, hosting, database, API-bbox-CRS, opslag-CRS, mappingversie en herkenning van assets tussen bronpublicaties blijven open. De bronvragen en mapping staan in het [datasheet](datasheet.md). De bredere architectuur staat in het [architectuurdocument](architectuur.md).

**Liggingsdata voor analyse en demonstratie. Geen KLIC en geen informatie voor graafveiligheid.**
