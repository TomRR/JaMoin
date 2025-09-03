# Lizenzkonzept 

## Anwendungsfall
- Kunde hat eine Desktop-Software von uns entwickeln lassen
- diese ist sehr teuer und soll sicher lizensiert werden

## Rahmenbedingung
- einmal lizensiert, soll die Software auch offline laufen (d.h. ein bei Programmstart erzwungener Server-Request geht nicht)
- es muss verhindert werden, dass der Kunde eine Lizenz auf mehreren Rechnern verwendet
- die Lizenz soll beim Kunden irgendwie "lesbar" sein. D.h. der Mitarbeiter, der die Software verwendet, soll sehen "Lizensiert für ..."
- es soll kryptografisch so sicher wie möglich sein, aber es geht hier nicht um Atomraketen. 
=> Der "Gegner" ist kein FBI sondern Systemadministratoren von knausrigen Kunden.
- wir haben Server und Datenbanken in der Cloud, wo wir eine Lizenz-API/Server einspielen können

## Aufgabe
schreibe ein Konzept, wie wir das umsetzen können. Kurz und knapp.
- so dass der Kunde versteht, warum das sicher ist
- so dass der Entwickler versteht, was und wie er das umsetzen kann.

## Idee
Kunde bekommt nach kauf signierte Lizenzdatei, z.b. .lic datei mit RSA-Private-Key Signierung. Kunde kann sichs angucken aber nicht faelschen.
Inhalt z.b. eine Lizenz-Id (unsere LizenzId), Kundendaten (Name der Firma, Person), Ablaufdatum + weitere infos
Alternative legt der installer die .lic an



Beim allerersten Start (falls Internet verfügbar) kann die Software die Lizenz-ID einmalig gegen unseren Lizenzserver prüfen (z. B. „noch nicht aktiviert“ → aktivieren und als „verbraucht“ markieren).
Dadurch kann der Kunde nicht dieselbe Lizenz mehrfach aktivieren.
Falls kein Internet → es gibt einen Offline-Aktivierungscode (Lizenzserver generiert Datei basierend auf Hardware-ID).