# Gegevensoverzicht NetUnify

Dit document beschrijft de gegevens in de huidige lokale backendproeven, gecontroleerd aan de broncode op 24 november 2026. De tabellen geven per veld de betekenis, herkomst en een voorbeeld. Voor de precieze importregels en broncontroles geldt het [datasheet](datasheet.md). De gedeelde API-afspraken staan in het [API-contract](../../docs/api-contract.md).

**Open liggingsdata voor analyse en demonstratie. Geen KLIC en geen informatie voor graafveiligheid.**

## Overzicht van de gegevensstromen

| Gegevens | Betekenis | Huidige herkomst en status |
| --- | --- | --- |
| Bronrecords | Kabels met bronkenmerken en ligging | Liander-laag 112 wordt via een vaste ArcGIS-query opgehaald |
| Assets | Kabelrecords omgezet naar het uniforme NetUnify-model | LianderAssetMapper en LianderGeometryMapper maken deze in geheugen |
| Importresultaat | Gelezen aantal, geaccepteerde assets, afkeurmeldingen en mappingversie | LianderImportProcessor levert dit voor de lokale controle |
| Quarantaine | Registratie van records die de validatie afkeurt | De processor bewaart per afgekeurd record een verwijzing en foutcode in geheugen |
| Jobs | Samenvattingen van verwerkingsopdrachten | JobService levert drie vaste fictieve voorbeelden via GET /api/v1/jobs |

De lokale Liander-controle en het fictieve jobsoverzicht werken afzonderlijk. De koppeling naar echte jobs, opslag en snapshots is vervolgwerk.

## Bronnen en selectie

| Bron | Onderzochte gegevens | Huidige verwerking |
| --- | --- | --- |
| Liander | Laagspanningskabels, Liander_Open_Data_Elektra/112 | Connector, mapping en validatie voor de vaste selectie gebouwd en gecontroleerd |
| Enexis | Middenspanningskabels via de provincie Groningen, laag 2 | Drie records onderzocht tijdens D01; connector nog te bouwen |
| Stedin | Middenspanningsverbindingen als shapefile | Bestandsstructuur en drie attribuutrijen onderzocht tijdens D01; connector nog te bouwen |

| Selectie-eigenschap Liander | Huidige waarde | Betekenis |
| --- | --- | --- |
| Bron- en uitvoer-CRS van de query | EPSG:28992, RD New | x/y-coördinaten in meters |
| Bbox | [210650, 467800, 210850, 468000] | West, zuid, oost, noord; gebied van 200 × 200 meter |
| Ruimtelijke selectie | intersects | Een kabel die het gebied raakt of doorsnijdt kan worden geselecteerd |
| Geometriebehoud | Volledige geometrie | Geselecteerde kabels kunnen buiten de bbox doorlopen |
| Aantal bij de laatste gemelde live controle | 23 | Broninhoud kan bij een latere aanroep veranderen |
| Gevraagde recordlimiet | 100 | De huidige connector stopt bij een gemelde transferlimiet; paginering volgt later |

## Velden uit het Liander-antwoord

Een bronrecord staat onder features. Kenmerken staan onder attributes; de geometrie staat onder geometry. De voorbeelden hieronder illustreren het formaat. OBJECTID 7836116 en de lengte van circa 66,03 meter komen uit de eerdere D01-steekproef.

| Bronveld of JSON-pad | Gegevenstype | Betekenis | Voorbeeld en gebruik |
| --- | --- | --- | --- |
| attributes.OBJECTID | Geheel getal; gelezen als long | Identificatie binnen de bronlaag | 7836116; wordt SourceRecordId als tekst |
| attributes.Shape__Length | Decimaal getal, meters | Lengteattribuut van de brongeometrie | Circa 66,03; heeft momenteel geen eigen veld in Asset |
| geometry.paths | Lijst van lijnpaden | Elk pad bevat opeenvolgende coördinaatparen | [[[210650,467800],[210750,467800]]]; illustratief |
| geometry.paths[pad][punt][0] | Eindig getal, meters | RD-x, oostwaartse coördinaat | 210650 |
| geometry.paths[pad][punt][1] | Eindig getal, meters | RD-y, noordwaartse coördinaat | 467800 |

De laagmetadata en eerdere bronproef bevestigen RD New. De connector vraagt outSR=28992 aan. Een afzonderlijke automatische controle van de CRS-metadata van ieder antwoord is vervolgwerk.

## Uniform assetmodel

Een asset stelt één geaccepteerd kabelrecord voor. Het C#-model staat in Models/Asset.cs. De volgende namen zijn de interne C#-propertynamen. De kaartuitvoer moet later als GeoJSON worden uitgewerkt.

| Veld | C#-type | Betekenis en herkomst | Voorbeeld in de Liander-mapping |
| --- | --- | --- | --- |
| AssetId | Guid | Door NetUnify gegenereerde identificatie; nieuw bij iedere mapping | Een nieuwe GUID |
| Operator | string | Netbeheerder, gekozen op basis van de bron | Liander |
| SourceDataset | string | Exacte verwijzing naar bronservice en laag | Liander_Open_Data_Elektra/112 |
| SourceRecordId | string | OBJECTID als cultuuronafhankelijke tekst | "7836116" |
| Discipline | string | Soort netwerk, afgeleid uit de dataset | electricity |
| AssetType | string | Soort asset, afgeleid uit de laag | cable |
| VoltageClass | string | Spanningscategorie, afgeleid uit Laagspanningskabel | LV, laagspanning |
| VoltageKv | double? | Exacte spanning in kilovolt, indien bekend | null; ontbreekt als bronveld |
| Material | string? | Materiaal van de kabel, indien bekend | null; ontbreekt als bronveld |
| Geometry | MultiLineString | Alle lijnpaden, met behoud van puntvolgorde, intern in RD | SRID 28992, één of meer lijnpaden |
| JobId | Guid | Verwijzing naar de verwerkende poging | De lokale proef gebruikt vast ID …0010 |

Een null betekent dat een optioneel kenmerk onbekend is. SourceRecordId en SourceDataset leggen samen de bronverwijzing vast. De stabiliteit van OBJECTID tussen bronpublicaties is nog een open vraag. AssetId wordt bij een volgende aanroep opnieuw gegenereerd.

## Importresultaat en lokale controlesamenvatting

Het interne model staat in Models/ImportResult.cs. De processor verwerkt één opgehaalde selectie volledig in geheugen.

| Intern veld | C#-type | Betekenis | Laatst gecontroleerde waarde |
| --- | --- | --- | --- |
| JobId | Guid | Identificatie van de verwerkingspoging | 00000000-0000-0000-0000-000000000010, vast voor de lokale proef |
| MappingVersion | string | Versie van de gebruikte mapping- en validatieregels | liander-lv-v1 |
| Read | int | Aantal gelezen bronrecords | 23 |
| Assets | IReadOnlyList<Asset> | Geaccepteerde uniforme assets | 23 assets |
| Quarantine | IReadOnlyList<QuarantineRecord> | Afkeurmeldingen voor bronrecords | Lege lijst |

GET /dev/liander/sample is beschikbaar in Development en toont de onderstaande samenvatting. De assets worden tijdens de aanroep intern opgebouwd.

| JSON-veld | Betekenis | Laatst gecontroleerde waarde |
| --- | --- | --- |
| jobId | JobId uit het importresultaat | Vast lokaal voorbeeld-ID, eindigend op 0010 |
| mappingVersion | MappingVersion uit het importresultaat | liander-lv-v1 |
| counts.read | Aantal gelezen records | 23 |
| counts.accepted | Aantal assets | 23 |
| counts.quarantined | Aantal afkeurmeldingen | 0 |
| countsBalance | Controle op read = accepted + quarantined | true |
| errors | Lijst met afkeurmeldingen | [] |

## Quarantaine en foutcodes

| Veld | C#-type | Betekenis |
| --- | --- | --- |
| JobId | Guid | Verwerkingspoging waarin het record is afgekeurd |
| RecordNumber | int | Positie in het opgehaalde antwoord, beginnend bij 1 |
| SourceRecordId | string? | Bron-ID indien beschikbaar; bij ontbrekend of ongeldig ID is dit null |
| ReasonCode | string | Herkenbare reden van afkeur |

| ReasonCode | Betekenis |
| --- | --- |
| SOURCE_ID_MISSING | Het record bevat geen beschikbaar OBJECTID |
| SOURCE_ID_INVALID | OBJECTID is geen geheel getal dat als long kan worden gelezen |
| SOURCE_ID_DUPLICATE | Hetzelfde bron-ID komt meermaals voor in deze selectie; alle voorkomens worden afgekeurd |
| GEOMETRY_PATHS_MISSING | De geometrie bevat geen bruikbare, niet-lege paths-array |
| GEOMETRY_PATH_TOO_SHORT | Een pad is geen array of bevat minder dan twee punten |
| GEOMETRY_COORDINATE_INVALID | Een punt bevat geen twee eindige numerieke x/y-waarden |
| GEOMETRY_POINTS_IDENTICAL | Een pad bevat minder dan twee verschillende x/y-punten |

Bij een dubbel bron-ID krijgt de duplicaatcode voorrang op een eventuele geometriefout. Eén ongeldig pad leidt tot afkeur van het volledige record.

## Jobs via GET /api/v1/jobs

Een job stelt een opdracht tot gegevensverwerking voor. De huidige drie jobs zijn vaste demonstratiegegevens uit Services/JobService.cs. De status en tellingen daarvan worden in de code ingevuld. Het endpoint voert geen bronimport uit.

| JSON-veld | C#-type | Betekenis | Voorbeeld |
| --- | --- | --- | --- |
| items | IReadOnlyList<JobDto> | De lijst met jobs | Drie fictieve jobs, of [] bij een lege lijst |
| nextCursor | string? | Verwijzing naar een volgende pagina | null; echte paginering volgt later |
| items[].jobId | Guid | Unieke identificatie van een job | 00000000-0000-0000-0000-000000000002 |
| items[].operator | string | Netbeheerder waarop de job betrekking heeft | Liander |
| items[].dataset | string | Brondataset waarop de job betrekking heeft | Liander_Open_Data_Elektra/112 |
| items[].status | string | Verwerkingsstatus volgens de API-afspraak | succeeded of queued in de huidige voorbeelden |
| items[].submittedAt | DateTimeOffset | Tijdstip waarop de opdracht is ingediend, in UTC | 2026-11-19T10:00:00+00:00 |
| items[].counts | JobCountsDto? | Beschikbare tellingen, of null als onbekend | {"read":23,"accepted":21,"quarantined":2} |
| items[].counts.read | long | Aantal gelezen records | 23 |
| items[].counts.accepted | long | Aantal geaccepteerde records | 21 |
| items[].counts.quarantined | long | Aantal afgekeurde records | 2 |

De jobs zijn gesorteerd op submittedAt van nieuw naar oud, gevolgd door jobId oplopend. De onderstaande drie regels tonen de huidige uitvoervolgorde. Alle tijden zijn op 19 november 2026 in UTC; de GUID-prefix is 00000000-0000-0000-0000-.

| Job-ID-suffix | Tijd | Status | Tellingen | Betekenis van het voorbeeld |
| --- | --- | --- | --- | --- |
| 000000000002 | 10:00 | succeeded | 0 / 0 / 0 | Volledig verwerkte lege selectie |
| 000000000003 | 10:00 | queued | counts null | Wachtende opdracht met onbekende tellingen |
| 000000000001 | 09:00 | succeeded | 23 / 21 / 2 | Afgeronde voorbeeldverwerking met twee afgekeurde records |

De API-afspraak voorziet ook running, failed en cancelled. Die statussen hebben momenteel geen voorbeeldrecord. Beschikbare tellingen tijdens verwerking of na een vroegtijdige stop zijn voorlopig. Bij volledig verwerken geldt read = accepted + quarantined. Een bekend aantal nul en een onbekende telling null hebben elk hun eigen betekenis.

## Vervolg en open gegevensvragen

De WGS84-transformatie en GeoJSON-uitvoer volgen na de huidige RD-proef. Databaseopslag, snapshots met bronpeildatum en ophaalmoment, duurzame jobs en de koppeling tussen importresultaten en het jobsoverzicht zijn vervolgwerk. De bronpeildatum en stabiliteit van bron-ID's blijven open vragen. Het [datasheet](datasheet.md) bevat de gecontroleerde NSGI-referentie en alle datasetgebonden beperkingen.
