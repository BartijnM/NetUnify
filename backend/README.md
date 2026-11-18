# Backend

De backend omvat de C# / ASP.NET Core API, bronconnectors, mapping, importjobs, opslag, kwaliteitsinzichten, exports, beveiliging en tests.

- [Backend-TODO](TODO.md)
- [Backendvoortgang](voortganglog.md)
- [Datasheet en mapping](docs/datasheet.md)
- [Backendarchitectuur en infoschema](docs/architectuur.md)
- [Gedeeld API-contract](../docs/api-contract.md)
- [Gedeelde scope en integratievragen](../docs/scope.md)

D01 en D02 zijn uitgewerkt. D03 is de volgende stap; featurecode wacht op bevestiging van de ontwerpreview. De eerste implementatieslice levert daarna jobs met fictieve data via het afgesproken DTO. Er zijn nog geen geverifieerde startinstructies of implementatietests.

**Geen KLIC en geen informatie voor graafveiligheid.**

## Huidige fase

D01 en D02 van de backend zijn uitgewerkt. De eerste import betreft Liander-laagspanning in een RD-bbox met 23 handmatig onderzochte records. Er is nog geen werkende import gebouwd. D03 omvat de backendontwerpreview, het gedeelde API-contract en de integratieafstemming. Backendfeaturecode volgt pas na bevestiging van de ontwerpreview.

Azure App Service en Azure SQL blijven ontwerpvoorstellen. De frontendtechniek is nog niet bevestigd; Razor Pages was een eerdere suggestie. Lokale startinstructies, Azure-URL en toegangsafspraken worden toegevoegd na verificatie.

Bron voor de opdracht is de aangeleverde Educom-casus NetUnify, p. 1–2.