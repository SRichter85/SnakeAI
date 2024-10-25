# SnakeAI 1.2 Erweiterung
Autor: Sergei Richter

# Kontext und Ziel
SnakeAI ist eime Umsetzung des bekannten Snake Spiels in der Konsole, welches für eine Umschulung bei Lutz&Grub entwickelt wird. Das Program existiert bereits in der Version 1.1.

Die Version 1.2 soll dabei die grafische Oberfläche erweitern. Die wichtigsten Ziele sind wie folgt:

<b>Funktionale Ziele und Features</b>
- Der Benutzer soll die Möglichkeit haben, seinen Namen einzutragen
- Die aktuellen Einstellungen sollen direkt im Menü sichtbar sein, teilweise z.B. als Slider
- Anstatt dem Highscore-View sollen auch andere Ansichten dargestellt werden
- Die Spielregeln und Programinfo soll als Fenster über allen anderen Fenstern angezeigt werden

<b>Visuelle Verbesserungen</b>
- Eine sterbende Schlange soll einpaar mal Blinken für visuelles Feedback (quasi eine Animation)
- Wenn eine Schlange ein Futter ißt, so soll das Futter die Schlange durch den Mange "hinabrutschen"


<b>Nicht-Funktionale Ziele und Features</b>
- Es gibt immer wieder Bugs wenn Elemente sich gegenseitig überzeichnen (z.B. wenn eine Schlange ein Futter ist), dies soll durch eine einheitliche Logik geändert werden. Insbesondere muss es ein Element bestimmen können, ob an einer Position ein leeres Zeichen gezeichnet werden soll, oder ob ein zu dem Objekt gehörendes Zeichen entfernt werden soll.
- Die Anordnung der Elemente soll automatisch erfolgen. So soll es z.B. möglich sein, einfach eine Art Liste zu erstellen welche automatisch die XY Position der Elemente darstellt.
  
# Auflistung der Anforderungen
| ID | Beschreibung |
|----|--------------|
| | 

 # Design

 ## Initial Testing
 Es wurden folgende allgemeine Tests durchgeführt. Dabei wurde jeweils ein Array der Grösse 140x120, bzw. ein Console-Fenster der Grösse 140x120, befüllt. Es wurde im Debug Modus getestet.

 | ID | TEST | DAUER (ticks) |
 |----|------|-------|
 | 00 | Befüllen des Arrays mit fixen Daten, besteht aus jeweils 2 ConsoleColor und einem char | 0.09.341
 | 01 | Befüllen des Arrays mit zufälligen Daten, besteht aus jeweils 2 ConsoleColor und einem char | 0.012.412
 | 02 | Hinausschreiben von 140x120 einzelnen chars. Vor jedem Aufruf wird SetCursorPosition gesetzt, und es werden die zufälligen Farben gesetzt| 8.479.288
 | 03 | Hinausschreiben von 140x120 einzelnen chars. Vor jedem Aufruf wird SetCursorPosition gesetzt, aber Farben werden nicht geändert| 3.635.606
 | 04 | Hinausschreiben von 140x120 einzelnen chars. Vor jeder Zeile wird SetCursorPosition gesetzt, aber Farben werden nicht geändert| 2.189.905
 | 05 | Hinausschreiben von 140x120 einzelnen chars. Weder wird SetCursorPosition() aufgerufen, noch werden die Farben gesetzt.| 1.909.744
 | 06 | Hinausschreiben von 140x120 einzelnen chars. SetCursorPosition() wird nicht aufgerufen, die Farben werden gesetzt - aber immer die gleichen.| 7.303.949
 | 07 | Hinausschreiben von 120 strings der Länge 140. Vor jedem Aufruf wird SetCursorPosition gesetzt. Es werden keine Farben gesetzt | 0.079.949
 | 08 | Hinausschreiben von 120 strings der Länge 140. SetCursorPosition() wird nicht verwendet und es werden keine Farben gesetzt. | 0.055.933
 | 09 | Hinausschreiben von 120 strings der Länge 140. SetCursorPosition() wird nicht verwendet, aber Farben werden gesetzt (pro Linie). | 0.117.119
 
 
 Dabei entsprechen 10.000 ticks einer Millisekunde. In der fertigen Software wollen wir 50 Frames pro Sekunde erreichen, d.h. 20 Millisekunden pro Frame oder 200.000 ticks pro Frame.

 Auf die Reihenfolge der Tests kommt es nicht an. Es ist also egal, ob man die Console mit Daten beschrieben hat und dann dieselben Buchstaben nochmals hinausschreibt.

<b>FOLGERUNGEN AUS DEM TEST:</b>
| AUFRUF | DAUER (ticks) |
|--------|---------------|
| Zugriff auf Array Variable | 0.000,5
| Write(c), c ist ein char | 0.113,6
| 140xWrite(c), c ist ein char | 15.914,5
| Write(s), s ist ein string mit s.Length = 120 | 0.466,1
| SetCursorPosition() | 0.102,7 - 2.334,6
| BackgroundColor = ConsoleColor.Black | 0.590,8

Zusammenfassung:
- Alle Aufrufe an die Console sind sehr teuer und es ist viel effektiver den zu schreiben Wert vorher zu berechnen
- SetCursorPosition() sollte vermieden werden
- Es ist viel besser Write(s) [s ist string] als mehrfach Write(c) [c ist char] zu benutzen
- Mehrfache Aufrufe an Background.Color und Foreground.Color sollten ebenfalls vermieden werden.

Logische Schlussfolgerung für einen Algorithmus:
- 'Tracke' welche Änderungen notwendig sind
- Gruppiere die Änderungen nach BackgroundColor, dannach nach ForegroundColor
- Fasse die Änderungen zu strings zusammen
- Schreibe die strings in die Console hinaus






