# Mac Integration

## Die Situation
- wir haben eine Anwendung mit .NET Core API, EF.Core, MS SQL Datenbank
- drei Umgebungen sind vorhanden
-- local Development (geht auf localDB unter localhost, wird ja mit VS mit geliefert)
-- Staging Datenbank mit Staging API Server läuft in Azure, damit der Kunde die nächsten Features testen kann
-- Production Datenbank mit Production API Server läuft in Azure

## Problem
- es soll ein neuer Mitarbeiter aufs Projekt. Der benutzt aber Mac und das soll so auch bleiben
- was müssen wir tun, damit der neue Mitarbeiter auch einfach `git checkout` machen kann und seine localhost test-Umgebung läuft?

## Aufgabe
Skizziere, was geändert werden muss bzw was gegeben sein muss, damit 
- Entwickler mit Windows und mit Mac das Projekt direkt nach dem `git checkout` compilieren und testen können


## Antwort
anstatt VS can Rider als offensichtlichste alternative verwendet werden. anstonsten irgendein editor je nach preferenz (vs code, vim, cursor)
Ausserdem sollte das Team eine editorconfig anlegen wo die Team-Regeln Definiert sind damit in MR's nicht immer zu Whitespace und oder syntax dissusionen kommt.
1. Project Setup
1.1 Containerization
Um eine plattformunabhängige Entwicklungsumgebung zu ermöglichen, sollten API und Datenbank in Containern laufen. 
Dafür bietet sich ein Dockerfile pro Service und eine zentrale docker-compose.yml (oder alternativ Podman) an. 
So kann PLattformunabhankig entwickelt werden und nach einem einfachen 'git clone' und 'docker-compose up -d' ist ne lokale Testumgebung starten. 
Anstelle von LocalDB, die nur unter Windows funktioniert, wird ein SQL Server Container oder eine andere relationale Datenbank eingesetzt.
'docker-compose.override.yml' fuer locale entwicklung damit nicht die fuer staging und prod ueberschrieben werden muss.
1.2 Aspire
Auch sehr nice, um die lokale Entwicklung von API und Datenbank noch stärker zu standardisieren. 
Mit Aspire wird die Anwendung im AppHost konfiguriert, sodass Abhängigkeiten wie Datenbank oder Secrets direkt von dort eingebunden werden. 
Voraussetzung ist ein installiertes Docker oder Podman. Darüber hinaus übernimmt Aspire die Verwaltung und das Zusammenspiel der einzelnen Services.
Zentraler einstiegspunkt fuer Abhängigkeiten wie DB oder Secrets

2. Configuration
Die Konfiguration der Anwendung sollte konsequent über appsettings.json und appsettings.Development.json erfolgen. 
Dazu dann auch die nutzung des Options-Patterns, um Konfigurationen typensicher in die Services zu injecten. 
Harte Codierungen von ConnectionStrings oder API-Keys sollten vermieden werden (grosses nono). 
Für sensible Daten dann den 'dotnet user-secrets'-Mechanismus benutzen, sodass lokale Secrets nicht im Code oder im Repository landen.


3. Local Testing / Requests
Für manuelle Tests lassen sich .http oder .rest Dateien direkt im Projekt einchecken, die Requests enthalten. am besten mind. einen pro endpoint. 
Alternativ kann auch eine Postman-Collection gepflegt und im Team geteilt werden. 
Für automatisierte Tests empfiehlt sich der Einsatz von Testcontainers, sodass Integrationstests ebenfalls mit einer Container-Datenbank laufen können. 
Damit wird vermieden, dass Entwickler:innen eine lokale Datenbank manuell starten müssen. 
In Kombination mit klassischen Unit Tests ergibt sich so eine durchgängige, reproduzierbare Testumgebung für alle Teammitglieder – unabhängig vom Betriebssystem.


Nice-To-Have:
Seed-Script fuer Datenbank.
Z.b. direct in Aspire (bevorzugt), in der API (#If Development) oder autoimatisch im Container-Start einbindne via 'dotnet ef database update' 


Die größte Baustelle ist die LocalDB-Abhängigkeit. 
Wenn wir die Datenbank in ein Docker-Setup packen, ist die Cross-Plattform-Entwicklung gelöst. 
Alles andere ist „nur“ Standardisierung von Config, Secrets und Dev-Workflow :)



P.S.
Was alles zusammenhalten kann ist ein "Lebendige"-Dokumentation und eine standardisierung ueber Projecte hinweg