# API-contract v1 (concept)

Dit document beschrijft het gedeelde API-voorstel voor de backend- en frontendwerkstroom. De gezamenlijke contractreview is nog niet afgerond. Routes hieronder beginnen met /api/v1. Tijdstempels zijn ISO 8601 in UTC. DTO’s en voorbeelden worden in OpenAPI uitgewerkt zodra de review de velden bevestigt.

| Methode en route | Invoer en antwoord | Rol |
| --- | --- | --- |
| POST /jobs | operator, dataset, discipline, bbox en mappingVersion. 202 met jobId en Location. | Admin |
| GET /jobs | filters en cursor. Status en samenvatting per job. | Analyst, Admin |
| GET /jobs/{id} | status, mappingversie, tijden, tellingen, retryOf en auditinformatie. | Analyst, Admin |
| POST /jobs/{id}/retry | nieuwe poging voor een mislukte of geannuleerde job. 202 met nieuwe ID. | Admin |
| POST /jobs/{id}/cancel | vraagt annulering voor een wachtende of lopende job. 202. | Admin |
| GET /jobs/{id}/errors | veilig, gepagineerd foutenoverzicht. | Analyst, Admin |
| GET /jobs/{id}/exports/{kind} | assets als GeoJSON of errors als CSV. Voor assets is een bruikbaar resultaat nodig. | Analyst, Admin |
| GET /assets | bbox, filters en cursor. Begrensde GeoJSON FeatureCollection met vervolgcursor via Link-header. | Analyst, Admin |
| GET /insights | bbox, filters en versie. Kwaliteit, mix en dichtheid of bronvergelijking. | Analyst, Admin |

Een bbox wordt als vier getallen opgegeven: west, zuid, oost, noord. De API valideert grenzen en maximale omvang. De limieten, cursorsemantiek en lege resultaten worden vóór de implementatie vastgelegd.

Voorgestelde statussen: queued → running → succeeded, failed of cancelled. Een verzoek om te annuleren kan intern pending zijn. Het contract moet nog vastleggen wanneer annulering al te laat is. Tellingen onderscheiden gelezen, geaccepteerd en in quarantaine. Na volledig verwerken geldt gelezen = geaccepteerd + quarantaine. Bij vroegtijdige stop zijn tellingen voorlopig.

Fouten krijgen Problem Details met een stabiele code. Voorbeelden: 400 voor ongeldige invoer, 401 voor ontbrekende authenticatie, 403 voor onvoldoende rechten, 404 voor onbekende job en 409 voor een actie die niet past bij de huidige status. Interne stacktraces blijven op de server. Voor CI en frontendontwikkeling zijn kleine fictieve voorbeelden voorzien van een lopende job, een afgeronde job, een fout en een leeg kaartresultaat.


## Nog af te stemmen in D03

- Bbox-CRS en asvolgorde per endpoint. De RD-bbox uit de backendbronproef wordt niet zonder afspraak als API-bbox gebruikt.
- DTO's, null-semantiek, limieten, cursors, mappingversie en lege resultaten.
- Authenticatiestroom, origin/CORS of proxy en de koppeling van de eerste jobs-slice.
- De D02-keuzes voor Liander staan in het [backenddatasheet](../backend/docs/datasheet.md#d02--eerste-import-liander-laagspanning). Ze betekenen geen afgeronde gezamenlijke contractreview.

Schermontwerp en frontendtaken horen bij de frontendwerkstroom; er is nog geen frontenddocumentatie aan deze repo toegevoegd. De gedeelde casusgrens staat in [scope en samenwerking](scope.md).
