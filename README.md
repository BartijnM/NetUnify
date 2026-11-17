# NetUnify

Ik bouw een webapp die open liggingsdata van Liander, Enexis en Stedin naar één assetmodel omzet. Ik kan daarmee importjobs volgen, de datakwaliteit per bron beoordelen en uniforme assets op een ArcGIS-kaart bekijken.

**Liggingsdata voor analyse en demonstratie. Geen KLIC en geen informatie voor graafveiligheid.**

## Werkstromen

| Werkstroom | Verantwoordelijkheid |
| --- | --- |
| Backend | C# en ASP.NET Core API, connectors, mapping, importjobs, opslag, export, beveiliging en tests |
| Frontend | ASP.NET Core Razor Pages als ontwerpvoorstel, jobs en inzichten, en ArcGIS Maps SDK for JavaScript voor de kaart |
| Integratie | Scope, uniform model, API-contract, Azure, review en demo |

Ik houd beide werkstromen in één repo. De frontend gebruikt de API en de backend blijft zelfstandig testbaar. De eerste integratieslice laat één scherm gegevens uit de API tonen.

## Documentatie

- [Datasheet en startscope](docs/datasheet.md)
- [Architectuur en infoschema](docs/architectuur.md)
- [API-contract](docs/api-contract.md)
- [Wireframes](docs/wireframes.md)
- [TODO en acceptatiecriteria](TODO.md)
- [Voortgang](voortganglog.md)

## Huidige fase

Ik zit in de dataverkenning en het ontwerp. De documentatie bevat voorstelkeuzes voor de review. De geplande bronproef is een kleine selectie van elektriciteitsassets uit één bron. Na vastlegging van het datasheet en de ontwerpreview volgt de eerste integratieslice. Ik voeg lokale startinstructies, Azure-URL en informatie over ArcGIS-toegang toe zodra ik die heb geverifieerd.

Bron voor de opdracht is de aangeleverde Educom-casus NetUnify, p. 1–2.
