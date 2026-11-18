# NetUnify

NetUnify zet open liggingsdata van Liander, Enexis en Stedin om naar één assetmodel. De backend levert importjobs, kwaliteitsinzichten en exports via een C# / ASP.NET Core API. De frontend toont jobs, inzichten en een ArcGIS-kaart.

**Liggingsdata voor analyse en demonstratie. Geen KLIC en geen informatie voor graafveiligheid.** Deze grens geldt ook voor de UI.

## Repo-indeling

Backend en frontend worden door afzonderlijke ontwikkelaars binnen één repo uitgewerkt. Taken, voortgang en technische documentatie staan bij de betreffende werkstroom. De algemene map `docs/` bevat uitsluitend gedeelde afspraken.

| Locatie | Inhoud |
| --- | --- |
| [backend/](backend/README.md) | Backendwerkstroom, taken en voortgang |
| [backend/docs/datasheet.md](backend/docs/datasheet.md) | Bronproeven, startscope, mapping en kwaliteitsregels |
| [backend/docs/architectuur.md](backend/docs/architectuur.md) | Backendarchitectuur en infoschema |
| `frontend/` | Bestemd voor frontendimplementatie en frontenddocumentatie; er is nog geen frontendwerk toegevoegd |
| [docs/api-contract.md](docs/api-contract.md) | Gedeeld API-voorstel |
| [docs/scope.md](docs/scope.md) | Gedeelde scope en integratievragen |


