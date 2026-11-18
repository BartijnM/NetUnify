# API-contract v1 (concept)

Dit document beschrijft welke berichten backend en frontend beogen uit te wisselen. Een API-contract legt routes, velden en betekenissen vast, zodat beide werkstromen daarop kunnen aansluiten. De gezamenlijke frontendreview is nog niet afgerond.

Voor de beperkte lokale B01-proef is de GET /jobs-antwoordvorm hieronder akkoord. De rest is een voorstel voor het vervolg. Er is nog geen endpoint gebouwd en de rollen uit de routetabel zijn nog niet geïmplementeerd. De proef is uitsluitend lokaal met fictieve gegevens, zonder authenticatie of autorisatie; daarmee is geen productie-API goedgekeurd.

Dit contract is een werkdocument. Afspraken kunnen veranderen door review of implementatie. Een wijziging aan velden, typen of gedrag wordt hier bijgewerkt, met reden en gevolgen in de backendvoortgang. Als de frontend ervan afhankelijk wordt, moet die wijziging gezamenlijk worden afgestemd. Routes hieronder beginnen met /api/v1. Tijdstempels zijn ISO 8601 in UTC. OpenAPI-uitwerking volgt tijdens de bouw.

## Beoogde routes

De tabel beschrijft de beoogde API. Alleen GET /jobs valt binnen de lokale B01-proef.

| Methode en route | Invoer en antwoord | Beoogde rol |
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


## GET /jobs — huidige afspraak voor de lokale B01-proef

Een importjob stelt een verwerkingsopdracht voor. In deze proef zijn alle jobs demonstratiegegevens: de antwoorden bewijzen geen uitgevoerde import. De backend kan hierdoor lokaal worden gecontroleerd zonder frontend, database of werkende connector.

### Antwoordmodel

Een geslaagde GET /api/v1/jobs geeft HTTP 200 met een object met `items` en `nextCursor`.

| Veld | Betekenis in de huidige proef |
| --- | --- |
| `items` | Lijst met jobs; een lege lijst is toegestaan |
| `nextCursor` | Vervolgcursor; `null` wanneer geen volgende pagina bestaat. De kleine fictieve dataset past op één pagina |

Een job in `items` heeft de volgende velden:

| Veld | Betekenis |
| --- | --- |
| `jobId` | Unieke identificatie van de job |
| `operator` | Netbeheerder, bijvoorbeeld `Liander` |
| `dataset` | Brondataset, bijvoorbeeld `Liander_Open_Data_Elektra/112` |
| `status` | `queued`, `running`, `succeeded`, `failed` of `cancelled` |
| `submittedAt` | Aanmaaktijdstip in UTC |
| `counts` | Object met beschikbare tellingen, of `null` wanneer die nog niet beschikbaar zijn |

DTO betekent data transfer object: een antwoordmodel met deze uitwisselingsvelden, los van eventuele toekomstige opslagtabellen. De interne C#-typen en de concrete fictieve job-ID's worden bij de DTO-stap uitgewerkt; ze zijn hier nog niet definitief vastgesteld.

Jobs worden gesorteerd op `submittedAt` van nieuw naar oud. `jobId` is de vaste tweede sorteersleutel bij gelijke tijdstempels. De richting van die tweede sortering moet vóór implementatie nog expliciet worden gekozen.

### Betekenis van tellingen

Wanneer `counts` beschikbaar is, bevat het drie niet-negatieve gehele getallen: `read` (gelezen), `accepted` (geaccepteerd) en `quarantined` (apart gezet wegens afkeur).

| Situatie | Weergave |
| --- | --- |
| Wachtende job | `counts: null`; nog geen verwerkingsuitkomst beschikbaar |
| Lopende job | Beschikbare voorlopige tellingen |
| Volledig verwerkte job | Definitieve tellingen; `read = accepted + quarantined` |
| Mislukte of geannuleerde job | Beschikbare voorlopige tellingen, anders `null` |

Een afgeronde lege import heeft drie nullen: `read: 0`, `accepted: 0`, `quarantined: 0`. Dat verschilt van `counts: null`: nul is een bekend aantal, null betekent onbekend of nog niet beschikbaar. Tellingen bij vroegtijdige stop zijn niet definitief; de eindbalans wordt daar niet zonder meer geëist.

### Voorbeeld met fictieve gegevens

Dit is een illustratie van de afgesproken antwoordvorm, geen gemeten importresultaat. De voorbeeld-ID is geen definitieve keuze voor het interne ID-type.

```json
{
  "items": [
    {
      "jobId": "demo-job-001",
      "operator": "Liander",
      "dataset": "Liander_Open_Data_Elektra/112",
      "status": "succeeded",
      "submittedAt": "2026-11-18T09:00:00Z",
      "counts": {
        "read": 10,
        "accepted": 8,
        "quarantined": 2
      }
    }
  ],
  "nextCursor": null
}
```

Geen jobs is een geslaagd antwoord met HTTP 200:

```json
{
  "items": [],
  "nextCursor": null
}
```

Een lege lijst betekent 'succesvol opgehaald, geen jobs'. Een fout tijdens het verzoek is een ander geval en krijgt volgens het beoogde contract een afzonderlijk foutantwoord. Filters en echte cursorpaginering zijn nog niet gebouwd of volledig ontworpen; `nextCursor: null` bewijst geen werkende paginering.

## Nog af te stemmen in D03

- Bbox-CRS en asvolgorde per endpoint. De RD-bbox uit de backendbronproef wordt niet zonder afspraak als API-bbox gebruikt.
- DTO's en null-semantiek buiten de lokale B01-proef; filters, pagineringslimieten, cursors en mappingversie. De hieronder beschreven jobs-antwoordvorm is al akkoord voor de proef.
- Authenticatiestroom, origin/CORS of proxy en de koppeling van de eerste jobs-slice.
- De D02-keuzes voor Liander staan in het [backenddatasheet](../backend/docs/datasheet.md#d02--eerste-import-liander-laagspanning). Ze betekenen geen afgeronde gezamenlijke contractreview.

Schermontwerp en frontendtaken horen bij de frontendwerkstroom; er is nog geen frontenddocumentatie aan deze repo toegevoegd. De gedeelde casusgrens staat in [scope en samenwerking](scope.md).
