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

Ik gebruik drie statussen voor bevindingen: **bronpagina gecontroleerd**, **sample gecontroleerd** en **open**. Op dit moment zijn alleen de genoemde bronpagina’s gecontroleerd. Ik leg geen bronmetingen vast voordat ik de samples heb geïnspecteerd.
