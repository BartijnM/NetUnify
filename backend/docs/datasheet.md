# Datasheet en startscope

De eerste import betreft **elektriciteit: Liander-laagspanningskabels, laag 112**. Deze bron is gekozen vanwege de toegankelijke ArcGIS-query. De eerdere voorkeur voor middenspanning is hiermee voor de eerste import vervangen. Enexis en Stedin blijven verkennende MS-bronproeven; een passende Liander-MS-laag of LS-datasets voor de andere bronnen zijn nog niet onderzocht.

Een bbox is een rechthoek met west-, zuid-, oost- en noordgrens. Voor andere bronnen kunnen aparte gebieden nodig zijn. Een bronvergelijking gebruikt dezelfde selectie voor brondata en uniforme data van die import.

| Bron | Onderzochte categorie | Status |
| --- | --- | --- |
| Liander | Laagspanningskabel, laag 112 | Eerste importbron; D01 en de D02-selectie handmatig onderzocht. |
| Enexis | Middenspanningskabel, provinciale publicatie, laag 2 | Werkende bronquery en drie records gecontroleerd; dekking en peildatum nog open. |
| Stedin | Middenspanningsverbindingen, shapefile | Bestandsstructuur, CRS, geometriesoort en drie DBF-rijen gecontroleerd; bron-ID en herpublicatievoorwaarden nog open. |

Officiële bronnen: [Liander open data](https://www.liander.nl/over-ons/open-data) · [Enexis open data](https://www.enexis.nl/over-ons/waar-staan-wij-voor/open-data) · [Stedin liggingsdata](https://www.stedin.net/zakelijk/open-data/liggingsdata-kabels-en-leidingen).

## Checklist per dataset (niet alles is al bevestigd)

- Datasetnaam, bronlink, endpoint of download, datum van raadplegen, bronversie en bronvermelding.
- Formaat en eventuele bestandsonderdelen. Noteer CRS en asvolgorde op basis van metadata en controleer een voorbeeldgeometrie.
- Gebied of bbox, aantal records in de controleproef, selectiecriteria en geometriesoorten.
- Werkelijke veldnamen, typen, eenheden, null-percentages en voorbeeldwaarden. Leg per uniform veld de omzetting vast.
- Beschikbaarheid en stabiliteit van de bron-ID, bekende hiaten, voorwaarden, limieten en toegestane export.

### Liander — Laagspanningskabel (112)

**Bronpagina gecontroleerd.** De ArcGIS-laag `Laagspanningskabel`
(ID `112`) uit `Liander Open Data Elektra` is geraadpleegd op 17 november 2026.
Het ArcGIS-item vermeldt CC BY 4.0.

**Sample gecontroleerd.** Via ArcGIS REST zijn drie records opgehaald
als Esri JSON, gesorteerd op `OBJECTID`, zonder bbox. De laag en het
queryantwoord gebruiken beide RD New (`EPSG:28992`, `x, y` in meters).
Het antwoord geeft `esriGeometryPolyline`. In record `7836116` is
een lijnpad met meerdere verschillende coördinaatparen aangetroffen.

[Bekijk de ArcGIS-query voor de drie Liander-records](https://services1.arcgis.com/v6W5HAVrpgSg3vts/arcgis/rest/services/Liander_Open_Data_Elektra/FeatureServer/112/query?where=1%3D1&orderByFields=OBJECTID%20ASC&resultRecordCount=3&outFields=%2A&returnGeometry=true&f=pjson)

| Bronveld | Type | Voorbeeld | Null in sample |
| --- | --- | --- | --- |
| `OBJECTID` | `esriFieldTypeOID` | `7836116` | 0/3 |
| `Shape__Length` | `esriFieldTypeDouble` | 66,03 m | 0/3 |

**Open.** De peildatum en de stabiliteit van `OBJECTID` tussen
publicaties zijn nog niet bevestigd.

### Enexis — Middenspanningskabels (laag 2)

**Bronpagina gecontroleerd.** Op 17 november 2026 is de ArcGIS-laag
`EnexisElektriciteitMiddenspanningskabel` geraadpleegd. De provincie
Groningen publiceert deze Enexis-data en beschrijft het gebied als
Noord-Nederland. De laagmetadata vermeldt maximaal 2.000 records per query
en ondersteuning voor paginering. De provincie vermeldt geen
gebruiksbeperkingen.

[Laagmetadata](https://geoservices.provinciegroningen.nl/server/rest/services/BasisdataExtern/EnergieEnexis/MapServer/2) · [Gebruiksvoorwaarden](https://geoservices.provinciegroningen.nl/server/rest/services/BasisdataExtern/EnergieEnexis/MapServer/2/iteminfo)

**Sample gecontroleerd.** Via ArcGIS REST zijn drie records opgehaald
als Esri JSON, gesorteerd op `OBJECTID`, zonder bbox. De laag en het
queryantwoord gebruiken RD New (`EPSG:28992`, `x, y` in meters).
Het antwoord geeft `esriGeometryPolyline` en de records bevatten lijnpaden
onder `geometry.paths`.

[Bekijk de query voor de drie Enexis-records](https://geoservices.provinciegroningen.nl/server/rest/services/BasisdataExtern/EnergieEnexis/MapServer/2/query?where=1%3D1&orderByFields=OBJECTID%20ASC&resultRecordCount=3&outFields=OBJECTID%2Cnominalvoltage%2Cnominalvoltageuom&returnGeometry=true&f=pjson)

| Bronveld | Type | Waarden in de steekproef | Null in sample |
| --- | --- | --- | --- |
| `OBJECTID` | `esriFieldTypeOID` | `886645`, `886646`, `886647` | 0/3 |
| `nominalvoltage` | `esriFieldTypeDouble` | `20000.0` in alle drie | 0/3 |
| `nominalvoltageuom` | `esriFieldTypeString` | `V` in alle drie | 0/3 |

**Mappingvoorbeeld.** Voor deze drie records wordt `voltageKv` berekend
door de waarde in volt door 1.000 te delen. Dat geeft `20` kV.

**Open.** De peildatum, de exacte geografische dekking en de
stabiliteit van `OBJECTID` tussen publicaties zijn nog niet bevestigd.

### Stedin — Middenspanningsverbindingen

**Bronpagina gecontroleerd.** Op 17 november 2026 is de shapefile
`Middenspanningsverbindingen` geraadpleegd. De ZIP bevat `.shp`, `.shx`,
`.dbf`, `.prj` en `.cpg`. Stedin vermeldt dat de liggingsdata geen
KLIC-melding vervangt.

[Stedin-bronpagina en disclaimer](https://www.stedin.net/zakelijk/open-data/liggingsdata-kabels-en-leidingen/) · [Download Middenspanningsverbindingen](https://www.stedin.net/-/media/project/online/files/zakelijk/branches/liggingsdata/middenspanningsverbindingen.zip?rev=96e71fcc2b1b4373961f8fceb169e0aa)

**Sample gecontroleerd.** De `.prj` geeft RD New (`EPSG:28992`, `x, y`
in meters). De `.shp`-header geeft type `3` (`PolyLine`). De eerste
drie actieve DBF-rijen zijn bekeken, in bestandsvolgorde en zonder bbox.

| Bronveld | DBF-type | Waarde in alle drie de rijen | Leeg in sample |
| --- | --- | --- | --- |
| `Id` | `C` (tekst) | `239753` | 0/3 |
| `Naam` | `C` (tekst) | `N172 - N35 ()` | 0/3 |

**Mapping.** MS wordt als spanningsklasse gebruikt omdat Stedin deze download als middenspanningsverbindingen aanbiedt. Stedin definieert MS als meer dan 400 V en minder dan 50 kV. De
DBF bevat geen apart veld voor de exacte spanning in kV. Omdat `Id`
in de steekproef herhaald wordt, is dit veld niet geschikt als unieke sleutel
per rij.

**Open.** De peildatum, de betekenis en stabiliteit van `Id` en de
voorwaarden voor herpublicatie via NetUnify zijn nog te bevestigen.

Bevindingen hebben drie statussen: **bronpagina gecontroleerd**, **sample gecontroleerd** en **open**.


## D02 — Eerste import: Liander-laagspanning

### Vastgestelde ontwerpkeuzes

De brondataset is `Liander_Open_Data_Elektra/112`. De bronquery selecteert met `intersects` in RD New (EPSG:28992, x/y in meters):

| Grens | Waarde |
| --- | ---: |
| West / xmin | 210650 |
| Zuid / ymin | 467800 |
| Oost / xmax | 210850 |
| Noord / ymax | 468000 |

Dit gebied is 200 × 200 meter. De volledige geometrie van geselecteerde kabels blijft behouden en wordt niet afgeknipt. Een kabel kan daardoor buiten de bbox doorlopen. De volledige kabellengte is geen maat voor de lengte binnen de bbox.

[Telling voor de D02-selectie](https://services1.arcgis.com/v6W5HAVrpgSg3vts/arcgis/rest/services/Liander_Open_Data_Elektra/FeatureServer/112/query?where=1%3D1&geometry=210650%2C467800%2C210850%2C468000&geometryType=esriGeometryEnvelope&inSR=28992&spatialRel=esriSpatialRelIntersects&returnCountOnly=true&f=pjson)

### Mapping naar het infoschema

| Uniform veld | Bron of regel | Status |
| --- | --- | --- |
| `operator` | `Liander` | Ontwerpkeuze |
| `sourceDataset` | `Liander_Open_Data_Elektra/112` | Ontwerpkeuze |
| `sourceRecordId` | `OBJECTID` als tekst, bijvoorbeeld `"7836116"` | Bronveld gecontroleerd; mapping gekozen |
| `assetId` | Eigen door NetUnify gegenereerde ID | Ontwerpkeuze; herkenning tussen publicaties niet bewezen |
| `discipline` | `electricity` | Afgeleid uit de dataset |
| `assetType` | `cable` | Afgeleid uit de laag |
| `voltageClass` | `LV` | Afgeleid uit Laagspanningskabel; interne code gekozen |
| `voltageKv` | `null` | Geen veld voor exacte spanning in het gecontroleerde veldschema |
| `material` | `null` | Geen materiaalveld in het gecontroleerde veldschema |
| `geometry` | Alle `geometry.paths` naar één `MultiLineString`, met behoud van paden en puntvolgorde | Ontwerpkeuze; transformatie nog niet getest |
| `jobId` | Verwijzing naar de verwerkende importjob | Context uit de toekomstige import, geen bronveld |

Aan de API-grens wordt geometrie omgezet naar WGS84, lengtegraad vóór breedtegraad. Het interne opslag-CRS en de transformatiewijze worden in D03 beoordeeld. Het bron-CRS van deze bbox bepaalt niet automatisch het bbox-CRS in het API-contract.

`Shape__Length` is beschikbaar als bronattribuut, maar krijgt in het huidige Asset-model geen eigen veld. Voor latere lengteberekeningen moet een geschikte metrische of geodetische methode worden gekozen en de gebiedsselectie expliciet worden gemaakt.

Voor `DatasetSnapshot` blijft `peildatum` null zolang geen betrouwbare bronpeildatum beschikbaar is. `opgehaaldOp` wordt het werkelijke ophaalmoment van de import in UTC. Dit ophaalmoment is geen vervangende peildatum.

### Handmatig gecontroleerd op 18 november 2026

| Controle | Uitkomst |
| --- | --- |
| Aantal treffers in de bbox | 23 |
| Ingevuld `OBJECTID` | 23/23 (100%) |
| Verschillende bron-ID's | 23; geen dubbele ID's |
| Aanwezige, niet-lege `geometry.paths` | 23/23 |
| Ieder pad bevat minimaal twee coördinaatparen met x/y | Bevestigd voor alle 23 records |
| Ieder pad bevat minimaal twee verschillende punten | Bevestigd voor alle 23 records |
| Veldschema met `outFields=*` | Alleen `OBJECTID` en `Shape__Length`; geometrie staat afzonderlijk in het antwoord |

Deze uitkomsten zijn handmatige broncontroles, geen uitgevoerde import of volledige geometrievalidatie. Eindige numerieke waarden en de coördinatentransformatie zijn nog niet automatisch getest. De uitkomsten gelden alleen voor deze selectie op dit controlemoment.

### Afgesproken validatie en quarantaine (nog te bouwen)

Een record met een ontbrekend bron-ID gaat in quarantaine. Bij dubbele bron-ID's binnen één import van dezelfde dataset gaan alle records met dat ID in quarantaine; er wordt geen willekeurig exemplaar gekozen.

Het volledige record gaat ook in quarantaine wanneer `geometry.paths` ontbreekt of leeg is, een pad minder dan twee coördinaatparen heeft, een x/y ontbreekt of geen eindig getal is, of een pad geen twee verschillende punten bevat. Losse ondeugdelijke paden worden niet verwijderd om een record alsnog te accepteren. Overige records gaan door naar de overige controles.

Onbekend materiaal en onbekende exacte spanning zijn bekende bronhiaten en geen afkeurreden. In kwaliteitsuitkomsten blijven deze kenmerken herkenbaar als onbekend.

### Toetsbare kwaliteitsmaten (ontwerp)

| Maat | Berekening |
| --- | --- |
| ID-volledigheid | Records met ingevuld bron-ID / gelezen records |
| Dubbele bron-ID's | Aantal extra voorkomens van hetzelfde bron-ID binnen de datasetselectie |
| Bruikbare lijnstructuur | Records die alle afgesproken geometriecontroles doorstaan / gelezen records |
| Bekende exacte spanning | Geaccepteerde records met bekende `voltageKv` / geaccepteerde records |
| Bekend materiaal | Geaccepteerde records met bekend `material` / geaccepteerde records |

Bij een noemer van nul wordt 'niet van toepassing' gerapporteerd. Het aantal extra dubbele ID-voorkomens is niet gelijk aan het aantal afgekeurde records: bij twee records met hetzelfde ID is er één extra voorkomen, maar gaan beide records in quarantaine.

### Bekende hiaten en open vragen

- Liander: peildatum en stabiliteit van `OBJECTID` tussen publicaties blijven open. De volledige laag, querylimieten en paginering zijn met deze selectie niet bewezen.
- Exacte spanning en materiaal zijn niet beschikbaar als eigen bronveld. Waarden worden niet op basis van aannames ingevuld.
- Automatische validatie, transformatie, importgedrag en herstel zijn nog niet geïmplementeerd of getest.
- De bestaande Enexis- en Stedin-proeven betreffen middenspanning; vergelijkbaarheid met de eerste LS-import is niet aangetoond.
- Voor Enexis en Stedin blijven de datasetgebonden open vragen uit D01 van toepassing.

Deze data is uitsluitend bestemd voor analyse en demonstratie: **geen KLIC en geen informatie voor graafveiligheid**.
