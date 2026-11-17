# Datasheet en startscope

Ik begin met **elektriciteit, bij voorkeur middenspanningsverbindingen**, en één kleine bounding box in het verzorgingsgebied van de eerste bevestigde bron. Een bbox is een rechthoek met west-, zuid-, oost- en noordgrens. Ik kies de grenzen nadat ik heb gezien waar de bron records bevat. Voor de drie bronnen mogen aparte gebieden nodig zijn. Ik gebruik dezelfde bbox voor de vergelijking tussen brondata en uniforme data van dezelfde import.

| Bron | Openbare aanwijzing | Nog vast te leggen uit een sample |
| --- | --- | --- |
| Liander | De officiële pagina vermeldt elektra via ArcGIS en WFS. Eerste kandidaat voor de bronproef. | Exacte laag en URL, querylimiet en paginering, CRS, geometrie, bron-ID, spanning, materiaal, peildatum en voorwaarden. |
| Enexis | De casus noemt GeoServer of SHP+CSV. De officiële open-datapagina bevestigt de genoemde dataset voor assetligging niet. | De concrete dataset en toegangslink bevestigen vóór het ontwerp van de connector. Verbruiksdata is een andere dataset. |
| Stedin | De officiële liggingspagina biedt SHP-bestanden per netvlak. De bronpagina vermeldt EPSG:28992 voor sommige bestanden. | Kies het bestand voor MS-verbindingen. Controleer de werkelijke CRS in metadata of .prj, DBF-velden, geometrie, peildatum en voorwaarden. |

Officiële bronnen: [Liander open data](https://www.liander.nl/over-ons/open-data) · [Enexis open data](https://www.enexis.nl/over-ons/waar-staan-wij-voor/open-data) · [Stedin liggingsdata](https://www.stedin.net/zakelijk/open-data/liggingsdata-kabels-en-leidingen).

## In te vullen per dataset

- Datasetnaam, bronlink, endpoint of download, datum van raadplegen, bronversie en bronvermelding.
- Formaat en eventuele bestandsonderdelen. Noteer CRS en asvolgorde op basis van metadata en controleer een voorbeeldgeometrie.
- Gebied of bbox, aantal records in de controleproef, selectiecriteria en geometriesoorten.
- Werkelijke veldnamen, typen, eenheden, null-percentages en voorbeeldwaarden. Leg per uniform veld de omzetting vast.
- Beschikbaarheid en stabiliteit van de bron-ID, bekende hiaten, voorwaarden, limieten en toegestane export.

### Liander — Laagspanningskabel (112)

**Bronpagina gecontroleerd.** Ik heb de ArcGIS-laag `Laagspanningskabel`
(ID `112`) uit `Liander Open Data Elektra` geraadpleegd op 17 november 2026.
Het ArcGIS-item vermeldt CC BY 4.0.

**Sample gecontroleerd.** Ik heb via ArcGIS REST drie records opgehaald
als Esri JSON, gesorteerd op `OBJECTID`, zonder bbox. De laag en het
queryantwoord gebruiken beide RD New (`EPSG:28992`, `x, y` in meters).
Het antwoord geeft `esriGeometryPolyline`. In record `7836116` heb ik
een lijnpad met meerdere verschillende coördinaatparen gezien.

[Bekijk de ArcGIS-query voor de drie Liander-records](https://services1.arcgis.com/v6W5HAVrpgSg3vts/arcgis/rest/services/Liander_Open_Data_Elektra/FeatureServer/112/query?where=1%3D1&orderByFields=OBJECTID%20ASC&resultRecordCount=3&outFields=%2A&returnGeometry=true&f=pjson)

| Bronveld | Type | Voorbeeld | Null in sample |
| --- | --- | --- | --- |
| `OBJECTID` | `esriFieldTypeOID` | `7836116` | 0/3 |
| `Shape__Length` | `esriFieldTypeDouble` | 66,03 m | 0/3 |

**Open.** Ik moet de peildatum en de stabiliteit van `OBJECTID` tussen
publicaties nog bevestigen.

### Enexis — Middenspanningskabels (laag 2)

**Bronpagina gecontroleerd.** Ik heb op 17 november 2026 de ArcGIS-laag
`EnexisElektriciteitMiddenspanningskabel` geraadpleegd. De provincie
Groningen publiceert deze Enexis-data en beschrijft het gebied als
Noord-Nederland. De laagmetadata vermeldt maximaal 2.000 records per query
en ondersteuning voor paginering. De provincie vermeldt geen
gebruiksbeperkingen.

[Laagmetadata](https://geoservices.provinciegroningen.nl/server/rest/services/BasisdataExtern/EnergieEnexis/MapServer/2) · [Gebruiksvoorwaarden](https://geoservices.provinciegroningen.nl/server/rest/services/BasisdataExtern/EnergieEnexis/MapServer/2/iteminfo)

**Sample gecontroleerd.** Ik heb via ArcGIS REST drie records opgehaald
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

**Mappingvoorbeeld.** Ik kan voor deze drie records `voltageKv` berekenen
door de waarde in volt door 1.000 te delen. Dat geeft `20` kV.

**Open.** Ik moet de peildatum, de exacte geografische dekking en de
stabiliteit van `OBJECTID` tussen publicaties nog bevestigen.

### Stedin — Middenspanningsverbindingen

**Bronpagina gecontroleerd.** Ik heb op 17 november 2026 de shapefile
`Middenspanningsverbindingen` geraadpleegd. De ZIP bevat `.shp`, `.shx`,
`.dbf`, `.prj` en `.cpg`. Stedin vermeldt dat de liggingsdata geen
KLIC-melding vervangt.

[Stedin-bronpagina en disclaimer](https://www.stedin.net/zakelijk/open-data/liggingsdata-kabels-en-leidingen/) · [Download Middenspanningsverbindingen](https://www.stedin.net/-/media/project/online/files/zakelijk/branches/liggingsdata/middenspanningsverbindingen.zip?rev=96e71fcc2b1b4373961f8fceb169e0aa)

**Sample gecontroleerd.** De `.prj` geeft RD New (`EPSG:28992`, `x, y`
in meters). De `.shp`-header geeft type `3` (`PolyLine`). Ik heb de eerste
drie actieve DBF-rijen bekeken, in bestandsvolgorde en zonder bbox.

| Bronveld | DBF-type | Waarde in alle drie de rijen | Leeg in sample |
| --- | --- | --- | --- |
| `Id` | `C` (tekst) | `239753` | 0/3 |
| `Naam` | `C` (tekst) | `N172 - N35 ()` | 0/3 |

**Mapping.** Ik gebruik MS als spanningsklasse omdat Stedin deze download als middenspanningsverbindingen aanbiedt. Stedin definieert MS als meer dan 400 V en minder dan 50 kV. De
DBF bevat geen apart veld voor de exacte spanning in kV. Omdat `Id`
in de steekproef herhaald wordt, kan ik het niet als unieke sleutel
per rij gebruiken.

**Open.** De peildatum, de betekenis en stabiliteit van `Id` en de
voorwaarden voor herpublicatie via NetUnify zijn nog te bevestigen.

Ik gebruik drie statussen voor bevindingen: **bronpagina gecontroleerd**, **sample gecontroleerd** en **open**.
