# Infrastructure

Das Infrastructure-Layer stellt Datenzugriff und externe Ressourcen bereit.

## Inhalt

- **Repositories**: Datenimplementierungen (CSV und Database)
- **Data**: Entity Framework DbContext
- **Migrations**: Datenbankschema-Verwaltung

## Unterstützte Datenquellen

- CSV-Dateien (CsvPersonRepository)
- SQLite-Datenbank (DatabasePersonRepository)

## Konfiguration

Datenquelle wird über `UseDatabase` in appsettings.json gesteuert.