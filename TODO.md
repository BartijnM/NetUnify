# TODO

## Fase 0 en 1 — Data en ontwerp

- [x] D01. Haal één klein sample per bron op. Leg dataset, formaat, CRS, velden, volledigheid en gebruiksvoorwaarden vast. Bevestig de concrete Enexis-bron.
- [ ] D02. Kies discipline en bbox voor de eerste import en teken de mapping naar het infoschema. Leg toetsbare kwaliteitsmaten en bekende hiaten vast.
- [ ] D03. Loop architectuur, contract en drie schermschetsen door. Bevestig de keuzes, open vragen en docent-review voordat ik featurecode schrijf.

## Backend

- [ ] B01. Maak de eerste integratieslice met een jobs-endpoint en fictieve data in het afgesproken DTO.
- [ ] B02. Bouw de eerste connector, mappingversie en validatie. Test een geldige asset, een ontbrekend veld, een ongeldige geometrie en een gebiedsgrens.
- [ ] B03. Voeg de andere twee connectors toe en toon herleidbare assets uit alle drie bronnen in hetzelfde model.
- [ ] B04. Voeg begrensde, annuleerbare jobs, quarantaine, retry, export en audit toe. Test dubbele verwerking, falen en herstel.
- [ ] B05. Lever kwaliteit, materiaal-/spanningsmix en dichtheid of vergelijking via de API. Test lege selecties en onbekende waarden.

## Frontend

- [ ] F01. Verbind een dun jobsoverzicht met B01. Toon ook laden, geen jobs en API-fout.
- [ ] F02. Maak jobdetails, acties op basis van rol en status, foutoverzicht en exports bruikbaar via het contract.
- [ ] F03. Toon drie inzichten met herleidbare filters, noemers en datasetversie.
- [ ] F04. Toon uniforme API-data in ArcGIS. Voeg lagen, filters en vergelijking of hotspot toe. Voeg MV/gas toe zodra de data en scope dat toelaten en leg de dekking vast.

## Cloud en oplevering

- [ ] I01. Richt Entra Analyst/Admin, App Service, database, managed identity en Key Vault in. Bewijs dat een Analyst geen import mag starten en een Admin wel.
- [ ] I02. Voeg build en tests als PR-checks toe en maak deployment vanaf main reproduceerbaar. Leg reviewvorm en publicatiemoment vast.
- [ ] I03. Werk lokale start, Azure-URL, ArcGIS-sleutel/licentie, bronvermelding en beperkingen bij. Leg verificatie vast in de voortgang.
- [ ] I04. Demonstreer in 5–8 minuten import → kwaliteit → kaart, met drie bronnen, foutpad, export, mappingversie, rollen en groene pipeline.

Pas na de oplevering beoordeel ik eventuele AI-uitbreidingen afzonderlijk.
