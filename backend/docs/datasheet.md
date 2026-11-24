# Datasheet en startscope

De eerste import betreft **elektriciteit: Liander-laagspanningskabels, laag 112**. Deze bron is gekozen vanwege de toegankelijke ArcGIS-query. De eerdere voorkeur voor middenspanning is hiermee voor de eerste import vervangen. Enexis en Stedin blijven verkennende MS-bronproeven; een passende Liander-MS-laag of LS-datasets voor de andere bronnen zijn nog niet onderzocht.

Een bbox is een rechthoek met west-, zuid-, oost- en noordgrens. Voor andere bronnen kunnen aparte gebieden nodig zijn. Een bronvergelijking gebruikt dezelfde selectie voor brondata en uniforme data van die import.

| Bron | Onderzochte categorie | Status |
| --- | --- | --- |
| Liander | Laagspanningskabel, laag 112 | Eerste importbron; D01/D02 handmatig onderzocht. Lokale B02-verwerking met 23 records en 15 automatische tests gecontroleerd. |
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
| `geometry` | Alle `geometry.paths` naar één `MultiLineString`, met behoud van paden en puntvolgorde | RD-mapping geïmplementeerd en getest; WGS84-transformatie nog open |
| `jobId` | Verwijzing naar de verwerkende importjob | Context uit de toekomstige import, geen bronveld |

Voor de toekomstige GeoJSON-uitvoer is WGS84 met lengtegraad vóór breedtegraad afgesproken. Intern is RD New (EPSG:28992) gekozen, met NetTopologySuite MultiLineString. Databaseopslag en de concrete transformatiebibliotheek staan nog open. De WGS84-transformatie is nog te bouwen en te testen. Het bron-CRS van deze bbox bepaalt niet automatisch het bbox-CRS in het API-contract.

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

Deze uitkomsten beschrijven de handmatige broncontrole van 18 november. De latere geautomatiseerde lijnstructuurcontroles en lokale verwerking staan onder B02. De coördinatentransformatie staat nog open. De handmatige uitkomsten gelden voor deze selectie op dit controlemoment.

### Afgesproken validatie en quarantaine (lokaal geïmplementeerd in B02)

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
- De afgesproken ID- en lijnstructuurvalidatie is lokaal geïmplementeerd en met vaste tests gecontroleerd. Transformatie, duurzame verwerking, opslag en herstel zijn vervolgwerk. De connector vraagt RD-uitvoer aan, maar valideert de CRS-metadata van het antwoord nog niet afzonderlijk.
- De bestaande Enexis- en Stedin-proeven betreffen middenspanning; vergelijkbaarheid met de eerste LS-import is niet aangetoond.
- Voor Enexis en Stedin blijven de datasetgebonden open vragen uit D01 van toepassing.

Deze data is uitsluitend bestemd voor analyse en demonstratie: **geen KLIC en geen informatie voor graafveiligheid**.

## B02 — Lokale Liander-verwerking gecontroleerd op 24 november 2026

### Huidige implementatie en ontwerpafspraken

`LianderConnector` haalt de vaste D02-bbox op met intersects, inSR/outSR 28992, alle attributen, volledige geometrie en sortering op OBJECTID. Het verzoek vraagt maximaal 100 records. Een HTTP-fout, ArcGIS-foutantwoord of gemelde overschrijding van de transferlimiet stopt de aanroep. Paginering is nog te bouwen; deze foutpaden zijn in de huidige tests niet afzonderlijk gesimuleerd.

`LianderAssetMapper` gebruikt mappingversie `liander-lv-v1`. OBJECTID wordt als een geheel 64-bits getal gelezen en als cultuuronafhankelijke tekst bewaard. Iedere geaccepteerde asset krijgt een nieuwe Guid; herkenning tussen importpogingen blijft een open vraag. Operator, dataset, discipline, assettype en spanningsklasse volgen de D02-mapping. Onbekende exacte spanning en materiaal blijven null.

`LianderGeometryMapper` maakt met NetTopologySuite 2.6.0 een MultiLineString met SRID 28992. Alle paden en hun x/y-puntvolgorde blijven behouden. De beschreven controles betreffen de afgesproken lijnstructuur. Bredere topologische controles zijn hiermee niet aangetoond.

`LianderImportProcessor` leest eerst de bron-ID's en keurt alle voorkomens van dubbele ID's af vóór geometrieomzetting. Ontbrekende of ongeldige bron-ID's en onbruikbare geometrieën leveren een QuarantineRecord op. Het record bevat jobId, een 1-gebaseerd recordnummer in het antwoord, sourceRecordId indien beschikbaar en een reasonCode. Geometriefouten bij een dubbel ID krijgen de duplicaatcode. Het resultaat bevat jobId, mappingVersion, gelezen aantal, assets en afkeurmeldingen. Bij volledige verwerking geldt gelezen = assets + quarantaine.

De lokale ontwikkelroute `/dev/liander/sample` gebruikt vast job-ID `00000000-0000-0000-0000-000000000010`. Dit is een controle in geheugen; opslag, een jobworker en koppeling met het fictieve jobsoverzicht zijn vervolgwerk.

### Gecontroleerde resultaten

De ontwikkelaar heeft het live antwoord teruggekoppeld met mappingVersion `liander-lv-v1`, 23 gelezen, 23 geaccepteerd, 0 in quarantaine, countsBalance true en errors []. Eerdere controles op dezelfde proef bevestigden 23 unieke bron-ID's en asset-ID's en de gekozen vaste mappingwaarden. De geometriestap gaf 23 MultiLineStrings met SRID 28992.

De ontwikkelaar meldde 15 geslaagde xUnit-tests, 0 mislukt en 0 overgeslagen. De code staat in `backend/NetUnify.Api.Tests/UnitTest1.cs`. De vaste tests omvatten correcte mapping en padvolgorde, onbekende optionele waarden, ontbrekend/null/ongeldig bron-ID, ontbrekende/lege geometrie, te korte paden, ontbrekende/niet-numerieke/niet-eindige coördinaten, identieke punten, volledige recordafkeur bij één ongeldig pad, dubbele ID's inclusief een ongeldige geometrie, een lege selectie en behoud van volledige geometrie buiten de bbox. Ieder scenario controleert mappingversie en sluitende tellingen. De grensproef bewaart een lijn van 300 meter die aan beide kanten buiten de bbox doorloopt; de selectie door Liander zelf wordt hiermee niet onafhankelijk getest.

### CRS-keuze en referentie voor de volgende stap

Intern wordt RD New (EPSG:28992) gebruikt. Voor kaartuitvoer is afgesproken een kopie te transformeren, met behoud van de interne RD-geometrie. Voor de visualisatie wordt ETRS89 aan WGS84 gelijkgesteld. De transformatiebibliotheek, uitvoering en acceptatietolerantie staan nog open.

De ontwikkelaar controleerde op 24 november in de NSGI-rekentool het volgende punt, zonder hoogte:

| Stelsel | Waarde |
| --- | --- |
| RD x / y | 210650 / 467800 meter |
| ETRS89 breedte | 52.195506749 graden |
| ETRS89 lengte | 6.201141586 graden |
| Verwachte volgorde voor kaartuitvoer volgens de gekozen gelijkstelling | [6.201141586, 52.195506749] |

Dit is een extern referentieresultaat voor een toekomstige test. Een eigen geslaagde WGS84-transformatie is hiermee nog niet aangetoond. Eén punt controleert een concrete uitkomst en asvolgorde; het bewijst geen nauwkeurigheid over het hele brongebied.

[NSGI-rekentool](https://www.nsgi.nl/coordinatenstelsels-en-transformaties/tools/online-coordinatentransformatie) · [Geonovum-handreiking voor de gelijkstelling bij visualisatie](https://docs.geostandaarden.nl/crs/def-hr-crs-20250211/)
