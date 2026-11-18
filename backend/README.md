# Backend

De backend is het serverdeel van NetUnify. Deze ontvangt API-verzoeken en gaat brondata verwerken tot uniforme assets. De beoogde functies zijn bronimport, mapping, importjobs, opslag, kwaliteitsinzichten, exports, beveiliging en tests. De frontend wordt afzonderlijk ontwikkeld en zal de API gebruiken.

## Documentatie

- [Backend-TODO](TODO.md): taken en acceptatiecriteria.
- [Backendvoortgang](voortganglog.md): uitgevoerde controles en besluiten per moment.
- [B01: lokale jobsproef](docs/b01-jobsproef.md): uitleg van de eerste bouwstap, projectindeling en controlepunten.
- [Datasheet en mapping](docs/datasheet.md): bronfeiten, importscope, omzettingsregels en bekende hiaten.
- [Backendarchitectuur en infoschema](docs/architectuur.md): huidige technische richting en open ontwerpvragen.
- [Gedeeld API-contract](../docs/api-contract.md): berichten tussen backend en frontend.
- [Gedeelde scope en werkwijze](../docs/scope.md): projectgrens, samenwerking en omgaan met wijzigingen.

## Huidige fase

D01 en D02 zijn uitgewerkt. De Liander-LS-bronproef bevat een selectie van 23 handmatig onderzochte records. Er is nog geen werkende import of API gebouwd en er zijn nog geen implementatietests uitgevoerd.

De beperkte lokale B01-proef is goedgekeurd: één ASP.NET Core Web API-project op .NET 10, met een GET-endpoint dat fictieve jobs teruggeeft. De proef bevat geen database, bronimport, echte jobuitvoering of geïmplementeerde rolbeveiliging. Frontendafstemming volgt later. D03 blijft als volledige review open; deze toestemming betreft alleen de beschreven proef.

Volgens de gemelde SDK-controle zijn SDK 9.0.319 en 10.0.400 geïnstalleerd. Vanuit de backendmap is .NET 10 als geselecteerde SDK gemeld. Een projectbuild en lokale start zijn nog niet gecontroleerd. Startinstructies worden toegevoegd zodra ze zijn uitgevoerd en geverifieerd.

## Voorlopige keuzes

Azure App Service en Azure SQL zijn voorstellen. Frontendtechniek, hosting, authenticatie, API-bbox-CRS en verschillende import- en opslagdetails blijven te beoordelen. De huidige keuzes zijn een basis voor stapsgewijze ontwikkeling en mogen met vastlegging van reden en gevolgen worden herzien.

**Geen KLIC en geen informatie voor graafveiligheid.**

Bron voor de opdracht is de aangeleverde Educom-casus NetUnify, p. 1–2.
