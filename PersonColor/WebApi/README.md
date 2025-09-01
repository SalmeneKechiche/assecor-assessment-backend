# WebApi

Das WebApi-Layer stellt die REST-API-Endpunkte bereit.

## Inhalt

- **Controllers**: API-Endpunkte (PersonsController)
- **Program.cs**: Anwendungskonfiguration und DI-Setup
- **Swagger**: API-Dokumentation

## Endpoints

- `GET /api/persons` - Alle Personen abrufen
- `GET /api/persons/{id}` - Person nach ID abrufen
- `GET /api/persons/color/{color}` - Personen nach Farbe filtern
- `POST /api/persons` - Neue Person hinzufügen

## Dokumentation

Swagger UI verfügbar unter `/swagger`