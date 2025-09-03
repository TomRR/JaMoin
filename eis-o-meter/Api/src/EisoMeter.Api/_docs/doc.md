
## Idee
Das Ziel ist eine kleine MicroApp mit **Frontend** und **Backend**.
- **Frontend**: keine Business-Logik, nur UI/Anzeige
- **Backend**: stellt alle benötigten Funktionen bereit

Kernidee:
- Ein Benutzer kann ein Eis „claimen“, sobald die Temperatur über einem bestimmten Wert liegt.
- Jeder Benutzer darf pro Tag nur **ein Eis** erhalten.
- Sobald ein Benutzer sein Eis erhalten hat, soll keine Benachrichtigung mehr erscheinen.

---

## Zeitaufwand (Schätzung)
- **Dev Setup**
    - Aspire-Apphost → 30 min
    - API-Design → 30 min
    - Frontend → 1,5 h (generieren + anpassen)
    - Gateway & Clerk Setup → 1,5 h
    - Backend → mind. 2 h
        - Setup: 30 min
        - Routes: 30 min
        - Infrastruktur: 1–1,5 h
        - Cleanup: 1–2 h
    - **Summe**: ca. 6–8 h

→ Für MVP zu aufwendig → **Vereinfachung**:
- **Aspire gestrichen**
- **Gateway + Auth gestrichen** → stattdessen gibt der User nur seine **E-Mail** ein
- **Background-Service gestrichen** → API wird bei jeder Anfrage aufgerufen

---

## Technologie-Stack
- **Frontend**: React (LLM- und Generator-freundlich)
- **Backend**: .NET API
- **Datenbank**: SQLite (leichtgewichtig, keine externe Abhängigkeit)

---

## Drittanbieter-APIs (für Wetterdaten)
- [OpenWeatherMap](https://openweathermap.org/api)
- [BrightSky](https://brightsky.dev/docs/#/) ✅ (funktioniert ohne API-Key, liefert Temperatur zur Location)
- [WeatherAPI](https://www.weatherapi.com/)

**Entscheidung:** BrightSky → einfacher Einstieg ohne API-Key

---

## Anforderungen / Kriterien
- UI zeigt aktuelle Temperatur an
- Wenn Temperatur > Schwellwert, darf User ein Eis claimen
- **Nur ein Eis pro User pro Tag**
- Benachrichtigung erscheint, sobald Temperatur erreicht
- Wenn Eis bereits geclaimt → keine weitere Benachrichtigung

---

## API-Design

### Endpunkte

#### ✅ Claim-Status abrufen
```http
GET /api/v1/claims/status?email={email}
Accept: application/json
```

Antwort: Gibt zurück, ob ein User für den aktuellen Tag bereits ein Eis erhalten hat.

```http
POST /api/v1/claims
Accept: application/json
Content-Type: application/json

{
  "email": "user@company.com"
}

```
Antwort: Erfolg oder Fehler (z. B. wenn User schon geclaimt hat).

## Hintergrundprozess

- **TemperatureBackgroundService** ruft periodisch die Wetter-API auf

- Speichert den höchsten Temperaturwert pro Tag in **TemperatureStatus**

- Frontend greift auf diesen Wert zu, anstatt ständig externe API anzufragen

**gestreichen in aktueller version**
## Datenbankmodell

### Tabellen

**Users**

|Feld|Typ|Besonderheit|
|---|---|---|
|Id|PK|AUTOINCREMENT|
|Email|TEXT|UNIQUE|
**UserClaim**

| Feld          | Typ  | Besonderheit  |
| ------------- | ---- | ------------- |
| Id            | PK   | AUTOINCREMENT |
| UserId        | FK   | → Users(Id)   |
| LastClaimDate | TEXT |               |
**TemperatureStatus**

| Feld               | Typ  | Besonderheit  |
| ------------------ | ---- | ------------- |
| Id                 | PK   | AUTOINCREMENT |
| Date               | TEXT |               |
| HighestTemperature | REAL |               |
## Umsetzung (realistisch)


- Planung ~1h
- Setup: ~1h (Api+UI)
- API-Design + Endpunkte: ~1 h
- Handler ~1h
- Datenbank + Infrastruktur: ~3 h
- Frontend-Integration: ~2 h

- **Gesamt ~9h**
