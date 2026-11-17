# Wireframes voor ontwerp-review

Ik gebruik deze tekstschetsen om scherminhoud en API-koppelingen te bespreken.

```text
JOBS
[Bron v] [Discipline v] [Gebied] [Import starten]
Job | Status | Gelezen | Geaccepteerd | Quarantaine | Details
Details: bron, mappingversie, tijden, audit
[Fouten] [Exporteer assets] [Exporteer fouten] [Retry] [Annuleren]
```

Ik toon actiestatus en foutmeldingen naast de job. Rol en jobstatus bepalen welke acties beschikbaar zijn. Ik laat de UI zien wat er gebeurt als ophalen of starten faalt.

```text
INZICHTEN
[Bron v] [Gebied] [Discipline v] [Datasetversie v]
1. Datakwaliteit per bron: veldvolledigheid en geometrie
2. Mix: materiaal en spanning, inclusief onbekend
3. Dichtheid per cel of vergelijking bron versus uniform
```

Ik toon bij elk inzicht de gebruikte dataset, selectie en noemer. Dichtheid is een beschrijvende maat. Ik gebruik het niet als veiligheidsbeoordeling. Voor vergelijking toon ik per bbox de aantallen en verschillen in velden of geometrie.

```text
KAART
[Bron v] [Assettype v] [Spanningsklasse v] [Lagen]
|                     ArcGIS-kaart                     |
|   Selectie: bron, assettype, peildatum, kenmerken    |
Legenda | Laadstatus | Geen KLIC / geen graafveiligheid
```

Ik maak lege selectie, laden, verlopen sessie en API-fout zichtbaar. De eerste UI-slice gebruikt de jobsgegevens uit de API. De interactieve kaart volgt na de API- en Azure-basis.
