# Overzicht gegevensstroom NetUnify

NetUnify haalt open liggingsgegevens van elektriciteitskabels op bij netbeheerders. De backend zet de verschillende bronformaten om naar één assetmodel, controleert de gegevens en maakt de uitkomsten beschikbaar voor de frontend.

**De gegevens zijn bedoeld voor analyse en demonstratie. Geen KLIC en geen informatie voor graafveiligheid.**

## Bronnen en binnenkomst

| Bron | Hoe komen de gegevens binnen? | Welke gegevens worden verwacht? | Huidige stand |
| --- | --- | --- | --- |
| Liander | De connector doet een HTTP-verzoek aan een ArcGIS REST-service en ontvangt JSON | Bronkenmerken en lijnpaden met coördinaten van kabels | Ophalen en verwerken voor één klein gebied werkt lokaal |
| Enexis | Een onderzochte ArcGIS-service van de provincie Groningen biedt JSON met Enexis-gegevens aan | Kabelgeometrie en kenmerken, waaronder spanning en eenheid | Bronproef uitgevoerd; connector volgt later |
| Stedin | Download als ZIP met samenhangende shapefile-bestanden | Kabelgeometrie in .shp, kenmerken in .dbf en het coördinatenstelsel in .prj | Bronproef uitgevoerd; connector volgt later |

De bronnen gebruiken verschillende bestandsformaten en veldnamen. Een connector leest het formaat van een specifieke bron. Mapping zet de ontvangen gegevens vervolgens om naar dezelfde namen en structuur binnen NetUnify.

## Concrete gegevens die binnenkomen uit de bron

| Gegeven in een Liander-record | Wat stelt het voor? | Wat gebeurt ermee? |
| --- | --- | --- |
| attributes.OBJECTID | Identificatie van het bronrecord | Wordt gecontroleerd op aanwezigheid, geldigheid en dubbele voorkomens; wordt daarna de bron-ID van de asset |
| attributes.Shape__Length | Lengte van de bronlijn in meters | Komt mee uit de bron; heeft momenteel geen eigen veld in het uniforme assetmodel |
| geometry.paths | Eén of meer lijnpaden met x/y-coördinaten in meters, RD New | Wordt gecontroleerd en omgezet naar een MultiLineString met behoud van volledige paden |

De bron levert in deze laag geen eigen velden voor exacte spanning en materiaal. Deze kenmerken blijven onbekend in het assetmodel.

## Concrete gegevens per verwerkte asset

De verwerking maakt voor ieder geaccepteerd bronrecord onderstaande gegevens aan. Deze assets bestaan tijdens de lokale aanroep in het geheugen.

| Intern veld | Huidige inhoud | Betekenis |
| --- | --- | --- |
| AssetId | Nieuw gegenereerde GUID | Eigen identificatie van de asset |
| Operator | Liander | Netbeheerder |
| SourceDataset | Liander_Open_Data_Elektra/112 | Dataset en laag waar het record vandaan komt |
| SourceRecordId | OBJECTID als tekst | Verwijzing naar het oorspronkelijke bronrecord |
| Discipline | electricity | Elektriciteitsnet |
| AssetType | cable | Kabel |
| VoltageClass | LV | Laagspanning, afgeleid uit de gekozen laag |
| VoltageKv | null | Exacte spanning onbekend |
| Material | null | Materiaal onbekend |
| Geometry | MultiLineString, SRID 28992 | Kabelpaden in RD New |
| JobId | 00000000-0000-0000-0000-000000000010 | Vast ID van de lokale controlepoging |

## Exact antwoord van de lokale importcontrole

GET /dev/liander/sample gaf bij de laatste door de ontwikkelaar uitgevoerde controle de volgende waarden terug.

| Veld in het antwoord | Gecontroleerde waarde | Betekenis |
| --- | --- | --- |
| jobId | 00000000-0000-0000-0000-000000000010 | Vast ID van de lokale controlepoging |
| mappingVersion | liander-lv-v1 | Gebruikte versie van de omzettings- en validatieregels |
| counts.read | 23 | Gelezen bronrecords |
| counts.accepted | 23 | Geaccepteerde assets |
| counts.quarantined | 0 | Afgekeurde records |
| countsBalance | true | Gelezen aantal is gelijk aan geaccepteerd plus afgekeurd |
| errors | [] | Leeg afkeuroverzicht |

Het endpoint toont deze samenvatting. De assets uit de vorige tabel worden intern opgebouwd. Een latere live aanroep kan andere aantallen opleveren wanneer de bron verandert.

## Exacte inhoud van de huidige jobs

GET /api/v1/jobs geeft een object met items en nextCursor. Items bevat de onderstaande drie fictieve jobs in deze volgorde. Bij alle drie is operator `Liander` en dataset `Liander_Open_Data_Elektra/112`.

| Veld | Eerste job | Tweede job | Derde job |
| --- | --- | --- | --- |
| jobId | 00000000-0000-0000-0000-000000000002 | 00000000-0000-0000-0000-000000000003 | 00000000-0000-0000-0000-000000000001 |
| status | succeeded | queued | succeeded |
| submittedAt | 2026-11-19T10:00:00+00:00 | 2026-11-19T10:00:00+00:00 | 2026-11-19T09:00:00+00:00 |
| counts | Object met de tellingen hieronder | null | Object met de tellingen hieronder |
| counts.read | 0 | — | 23 |
| counts.accepted | 0 | — | 21 |
| counts.quarantined | 0 | — | 2 |

| Gegeven | Betekenis |
| --- | --- |
| jobId | Identificatie van de opdracht |
| operator en dataset | Bron waarop de opdracht betrekking heeft |
| status | De voorbeeldstatus; succeeded betekent geslaagd en queued betekent wachtend |
| submittedAt | Indieningstijd in UTC |
| counts | Bekende tellingen; null betekent dat de tellingen nog onbekend zijn |
| nextCursor | Is voor dit antwoord null; de drie jobs passen in één antwoord |

Bij de wachtende job zijn de drie tellingsvelden afwezig doordat counts null is. De afgeronde lege job heeft bekende aantallen van nul. Het jobsoverzicht gebruikt vaste voorbeeldwaarden; de actuele Liander-verwerking heeft een eigen controleantwoord hierboven.

## Verwerking van de gegevens

| Stap | Wat gebeurt er? | Wat levert dat op? |   
| --- | --- | --- |
| 1. Selectie bepalen | Een bron, dataset en gebied worden gekozen | Een afgebakende hoeveelheid brondata om te verwerken |
| 2. Ophalen | De connector vraagt de gegevens op of leest de bronbestanden | Bronrecords met kenmerken en geometrie |
| 3. Controleren | De backend controleert bron-ID's, dubbele ID's en de structuur van lijnpaden | Een oordeel per record en een foutcode bij afkeur |
| 4. Uniform maken | De mapper zet bruikbare records om naar het assetmodel | Assets met dezelfde structuur, met behoud van bronverwijzing en geometrie |
| 5. Afkeur verzamelen | Onbruikbare records krijgen een verwijzing en reden in het afkeuroverzicht | Inzicht in welke records zijn afgekeurd en waarom |
| 6. Resultaat samenvatten | De backend telt gelezen, geaccepteerde en afgekeurde records en vermeldt de mappingversie | Een controleerbaar verwerkingsresultaat |
| 7. Beschikbaar stellen | De API levert gegevens aan de frontend voor jobs, inzichten en de kaart | De lokale controlesamenvatting werkt; de verdere frontendkoppeling volgt |

Het uniforme assetmodel bevat onder meer netbeheerder, brondataset, bron-ID, kabelcategorie, geometrie en een verwijzing naar de verwerking. Ontbrekende optionele kenmerken blijven onbekend. Na volledige verwerking geldt **gelezen = geaccepteerd + afgekeurd**.

## Rol van een importjob

Een importjob stelt één opdracht voor om een bronselectie te verwerken. Een job krijgt een eigen ID, zodat bron, voortgang en resultaat bij dezelfde poging kunnen worden teruggevonden.

| Onderdeel van de job | Betekenis |
| --- | --- |
| Opdracht | Welke bron, dataset en gebied moeten worden verwerkt? |
| Status | Wachtend, bezig, geslaagd, mislukt of geannuleerd |
| Tijden | Wanneer is de opdracht ingediend, gestart en afgerond? |
| Mappingversie | Welke omzettings- en validatieregels zijn gebruikt? |
| Tellingen | Hoeveel records zijn gelezen, geaccepteerd en afgekeurd? |
| Resultaat | Welke assets en afkeurmeldingen horen bij deze poging? |

Deze tabel beschrijft de beoogde rol van echte importjobs. Het huidige jobsoverzicht toont vaste fictieve voorbeelden. De lokale Liander-proef haalt al echte brondata op en verwerkt deze direct in geheugen, met een vast job-ID voor de controle. De koppeling tussen die verwerking en echte jobs met statusopvolging en opslag volgt later.

## Wat kan nu worden getoond?

De lokale Liander-proef toont de route van bronverzoek naar mapping, validatie en een resultaat met tellingen en afkeurmeldingen. Het afzonderlijke jobs-endpoint toont hoe de frontend straks een jobsoverzicht kan ontvangen. Automatische tests controleren de huidige mapping- en afkeurregels. Opslag, duurzame jobuitvoering, de andere connectors en kaartuitvoer zijn volgende uitbreidingen.
