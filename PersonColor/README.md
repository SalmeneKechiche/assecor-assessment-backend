# PersonColor Solution

Eine .NET 8 Web API zur Verwaltung von Personen mit Liebingsfarben, implementiert nach Clean Architecture Prinzipien.

## Projekte

| Projekt | Beschreibung |
|---------|-------------|
| **Domain** | Kerngesch�ftslogik, Entit�ten und Interfaces |
| **Application** | Gesch�ftslogik-Services und DTOs |
| **Infrastructure** | Datenzugriff (CSV/SQLite) und Repository-Implementierungen |
| **WebApi** | REST-API-Controller und Konfiguration |
| **PersonColor.Tests** | Unit Tests f�r Controller |

## Features

- **Dual-Datenspeicher**: CSV-Dateien oder SQLite-Datenbank
- **REST-API**: CRUD-Operationen f�r Personen
- **Farbfilterung**: 7 vordefinierte Farben (blau, gr�n, violett, rot, gelb, t�rkis, wei�)
- **Swagger-Dokumentation**: Interaktive API-Dokumentation
- **Clean Architecture**: Klare Schichtentrennung und Dependency Injection

## Schnellstart

### Voraussetzungen
- .NET 8 SDK
- IDE Ihrer Wahl (Visual Studio, VS Code)

### Installation und Ausf�hrung

1. **Repository klonen oder herunterladen**

2. **L�sung �ffnen:**cd PersonColor
   dotnet restore
3. **Anwendung starten:**dotnet run --project WebApi
4. **API aufrufen:**
   - Haupt-API: http://localhost:5123
   - Swagger UI: http://localhost:5123/swagger

### Alternative Profile
F�r HTTPS-Unterst�tzung: dotnet run --project WebApi --launch-profile https
- Haupt-API: https://localhost:7043
- Swagger UI: https://localhost:7043/swagger

## Unit Tests

Das Projekt enth�lt umfassende Unit Tests f�r alle Controller-Endpoints.

### Test-Framework
- **xUnit**: Test-Framework
- **Moq**: Mocking-Framework f�r Dependencies

### Test-Abdeckung
Die Tests decken folgende Szenarien ab:
- Erfolgreiche API-Aufrufe (200/201)
- Fehlerbehandlung (404, 400, 500)
- Validierung von Ein- und Ausgabedaten
- Exception-Handling und Logging
- Edge Cases (negative IDs, leere Strings)

### Tests ausf�hren# Alle Tests ausf�hren
dotnet test

## Konfiguration

`appsettings.json`:
- `UseDatabase`: true f�r SQLite, false f�r CSV
- `CsvFilePath`: Pfad zur CSV-Datei