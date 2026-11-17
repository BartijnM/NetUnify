# API-contract v1 (concept)

Ik bevries de gedeelde afspraken vóór parallelle bouw. Routes hieronder beginnen met /api/v1. Tijdstempels zijn ISO 8601 in UTC. Ik beschrijf DTO’s en voorbeelden in OpenAPI zodra de review de velden bevestigt.

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

Een bbox wordt als vier getallen opgegeven: west, zuid, oost, noord. Ik valideer grenzen en maximale omvang. De limieten, cursorsemantiek en lege resultaten worden vóór de implementatie vastgelegd. Het kaartscherm stuurt pas een vraag na een afgeronde kaartbeweging en past alleen het antwoord op de recentste aanvraag toe.

Voorgestelde statussen: queued → running → succeeded, failed of cancelled. Een verzoek om te annuleren kan intern pending zijn. Ik leg bij het contract vast wanneer annulering al te laat is. Tellingen onderscheiden gelezen, geaccepteerd en in quarantaine. Na volledig verwerken geldt gelezen = geaccepteerd + quarantaine. Bij vroegtijdige stop zijn tellingen voorlopig.

Fouten krijgen Problem Details met een stabiele code. Voorbeelden: 400 voor ongeldige invoer, 401 voor ontbrekende authenticatie, 403 voor onvoldoende rechten, 404 voor onbekende job en 409 voor een actie die niet past bij de huidige status. Interne stacktraces blijven op de server. Voor CI en UI-ontwikkeling maak ik kleine fictieve voorbeelden van een lopende job, een afgeronde job, een fout en een leeg kaartresultaat.
