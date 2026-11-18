# Backend-TODO

Deze planning betreft uitsluitend de backendwerkstroom. Alleen afgeronde en gecontroleerde taken worden afgevinkt. Gedeelde afstemmingspunten staan in [scope en samenwerking](../docs/scope.md). De uitgevoerde broncontroles en ontwerpkeuzes staan in het [datasheet](docs/datasheet.md).

## Data en ontwerp

- [x] D01. Voer een kleine bronproef per bron uit en leg formaat, CRS, velden, volledigheid, voorwaarden en open vragen vast. Bevestig een werkende concrete Enexis-bron. Open bronvragen blijven zichtbaar in het datasheet.
- [x] D02. Kies discipline en bbox voor de eerste import, leg de mapping naar het infoschema vast en beschrijf toetsbare kwaliteitsmaten en bekende hiaten. Bewijs: Liander-LS-selectie met 23 records, handmatige controles en vastgelegde ontwerpkeuzes in het datasheet. Dit is geen voltooide import of automatische validatietest.
- [ ] D03. Review de backendarchitectuur, infoschema en API-voorstel. Stem de integratie met de frontendwerkstroom af en bevestig open vragen en docent-review voordat backendfeaturecode wordt geschreven.

## Backendimplementatie (na D03)

- [ ] B01. Maak de eerste integratieslice met een jobs-endpoint en fictieve data in het afgesproken DTO; stem de koppeling af met de frontendwerkstroom.
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
