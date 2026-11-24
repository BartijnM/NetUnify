# Backend-TODO

Deze planning betreft uitsluitend de backendwerkstroom. Alleen afgeronde en gecontroleerde taken worden afgevinkt. Gedeelde afstemmingspunten staan in [scope en samenwerking](../docs/scope.md). De uitgevoerde broncontroles en ontwerpkeuzes staan in het [datasheet](docs/datasheet.md).

## Data en ontwerp

- [x] D01. Voer een kleine bronproef per bron uit en leg formaat, CRS, velden, volledigheid, voorwaarden en open vragen vast. Bevestig een werkende concrete Enexis-bron. Open bronvragen blijven zichtbaar in het datasheet.
- [x] D02. Kies discipline en bbox voor de eerste import, leg de mapping naar het infoschema vast en beschrijf toetsbare kwaliteitsmaten en bekende hiaten. Bewijs: Liander-LS-selectie met 23 records, handmatige controles en vastgelegde ontwerpkeuzes in het datasheet. Dit is geen voltooide import of automatische validatietest.
- [ ] D03. Rond de review van backendarchitectuur, infoschema en API-voorstel af. Bevestig open vragen, docent-review en integratieafspraken met de frontend. Alleen de beperkte lokale B01-proef is inmiddels goedgekeurd; dit rond D03 niet als geheel af.

## Toestemming en eerstvolgende stap

De [lokale B01-proef](docs/b01-jobsproef.md) mag starten vóór afronding van de volledige D03-review. Dit is een beperkte wijziging van de eerdere afspraak om alle featurecode te laten wachten. De frontendbouwer wordt later betrokken. Buiten deze proef blijven de relevante open ontwerp- en reviewvragen eerst te behandelen. De lokale jobsproef is gebouwd en handmatig gecontroleerd. De volgende stap is de relevante D03-reviewpunten behandelen voordat de bronimport wordt uitgebreid.

## Backendimplementatie

- [ ] B01. Maak de eerste integratieslice met een jobs-endpoint en fictieve data in het afgesproken DTO. De lokale backendproef is gebouwd en handmatig gecontroleerd; frontendkoppeling en afstemming volgen later.
  - [x] Voorbereiding: SDK-inventarisatie en selectie van .NET 10 gemeld; projectindeling, jobsvelden, antwoordverpakking, tellingen en scheiding controller/service/DTO besproken en akkoord voor de lokale proef.
  - [x] Maak het API-project en controleer build en lokale start. Uitvoering door de ontwikkelaar bevestigd.
  - [x] Bouw en controleer het jobs-antwoord volgens het API-contract: fictieve gegevens, vaste sortering, tellingen en lege lijst. HTTP 200 voor gevuld en leeg antwoord bevestigd; fictieve jobs daarna hersteld.
  - [ ] Stem het voorlopige contract af met de frontendwerkstroom en controleer de daadwerkelijke frontendkoppeling.
- [ ] B02. Bouw de eerste Liander-connector, mappingversie en validatie volgens D02. Test geldige assets, ontbrekende en dubbele bron-ID's, ongeldige geometrie, onbekende optionele waarden en de gebiedsgrens.
- [ ] B03. Voeg de andere twee connectors toe en lever herleidbare assets uit drie bronnen in hetzelfde model. Bevestig eerst passende kabelcategorieën en dekking.
- [ ] B04. Voeg begrensde, annuleerbare jobs, quarantaine, retry, export en audit toe. Test dubbele verwerking, falen en herstel.
- [ ] B05. Lever kwaliteit, materiaal-/spanningsmix en dichtheid of vergelijking via de API. Test lege selecties, nulnoemers en onbekende waarden.

## Backendcloud en oplevering

- [ ] I01. Review App Service en databasekeuze. Richt Entra Analyst/Admin, managed identity en Key Vault in. Bewijs in de API dat een Analyst geen import mag starten en een Admin wel.
- [ ] I02. Voeg backendbuild en tests als PR-checks toe en maak backenddeployment vanaf main reproduceerbaar. Stem gedeelde CI, reviewvorm en publicatiemoment af.
- [ ] I03. Werk backendstartinstructies, API-URL, configuratie, bronvermelding en beperkingen bij na verificatie. Stem API-toegang en de casusgrens af met de frontendwerkstroom.
- [ ] I04. Lever backendbewijs voor de gezamenlijke demo van 5–8 minuten: import, kwaliteit, drie bronnen, foutpad, export, mappingversie, rollen en groene pipeline. Stem de frontendkoppeling en demo af.

Eventuele AI-uitbreidingen worden pas na de oplevering afzonderlijk beoordeeld.
