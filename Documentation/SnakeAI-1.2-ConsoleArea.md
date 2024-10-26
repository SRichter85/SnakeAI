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

 # Design, Vorbemerkungen

 ## Merging Algorithm (Testing)
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

## Gewünschte Schnittstelle aus Programmierersicht

<b>Platzieren der Elemente</b>: Man würde gewisse, vorgefertige GUI Elemente zur Verfügung haben wie "Button", "DockPanel", etc. Diese hätten Properties welche die Platzierung steuern, wie z.B. ein Dimensions-Objekt welches die Grösse speichert. Die GUI Elemente könnte man dann zu einander hinzufügen, wie z.B:
```csharp
ConsoleWindow wnd = new ConsoleWindow();
wnd.Dimensions = new Dimensions() { Width = 140, Height = 120 };

DockPanel dp = new DockPanel();
dp.Dimensions = new Dimensions() { MaxWidth = 40, MinWidth = 20,  };
dp.Padding = new Distances() { Left = 2, Top = 2, Bottom = 2, Right = 3 };

Button btn = new Button();
btn.Alignment.Vertical = Alignment.Center;
btn.Alignment.Horizontal = Alignment.Stretch;

wnd.Add(dp, 0, 0);
dp.Add(btn, Dock.Top);
```

<b>Entwickeln neuer Elemente</b>: Es sollte eine Basis-Klasse (z.B. ConsoleFrame) existieren, von dem alle anderen Klassen abgeleitet werden.
```nomnoml
[<abstract>ConsoleFrame|
    + Visible : bool
    + Dimension : Dimension
    + Margin : Distances
    + Padding : Distances
    + ZPosition : int|
    # Write(x : int, y : int, text : string) : void
    # SetColor(x : int, y : int, length : int, color : Theme) : void
    # <virtual> OnInitializing() : void
    # <virtual> OnRefreshing() : void]

[ConsoleFrame] <:- [<abstract>AbstractFrame|
    # AdditionalProperty|
    # AdditionalFunction()]

[AbstractFrame] <:- [AnotherConcreteGuiElement|
    |
    # <override> OnInitialized() : void
    # <override> OnRefreshing() : void]
    
[ConsoleFrame] <:- [ConcreteGuiElement|
    |
    # <override> OnInitialized() : void
    # <override> OnRefreshing() : void]
```

Die Basis-Klasse <b>ConsoleFrame</b> übernimmt die gesamte Berechnung, an welcher eigentlichen Stelle etwas geschrieben wird, wie gross das Control ist, etc. Es bietet zwei Funktionen zum überschreiben an:
| Funktion | Aufruf-Zeitpunkt |
|----------|------------------|
| OnInitialized() : void | Wird nach der Berechnung der Grösse und Position aufgerufen (1x)
| OnRefreshing() : void | Wird periodisch aufgerufen, jedesmal wenn ein neuer Output berechnet werden soll

Weiterhin bietet die Klasse mehrere Funktionen an, um Werte tatsächlich zu schreiben. Wichtig ist, dass alle Positionsangaben relativ zu dem ConcreteControl geschehen - und zwar NACHDEM Margin, Padding, etc. berechnet wurden. Als Beispiel werden einige aufgeführt, aber nach Bedarf können noch weitere dazuprogrammiert werden:
| Funktion | Verhalten |
|----------|------------------|
| Write(x : int, y : int, text : string) : void | Schreibe den Text an die Position (x,y)
| SetColor(x : int, y : int, length : int, color : Theme) : void | Setze die Farbe der <em>length</em>-nächsten Zeichen, beginnend von <em>(x,y)</em>
| SetColor(line : int, color : Theme) | Setze die Farbe für die Linie
| Clear() : void | Lösche die gesamte Ausgabe vom Control
| ... | ...

Von dem ConsoleFrame können bereits konkrete GuiElemente abgeleitet werden. Aber es können auch weitere, abstrakte Klassen zwischen dem ConsoleFrame und dem konkreten GuiElement sein.

## Benötigte Custom-Control
Es folgt eine Liste aller Controls, welche umgesetzt werden sollen. Die Control werden in mehrere Gruppen unterteilt:
- ArrangementControls : Besitzen mehrere Kinder-Controls und die Hauptaufgabe ist es, diese Kinder anzuordnen
- InteractiveControls : Steuerelemente, welche eine gewisse Funktionalität beinhalten. Das Besondere an dieser Gruppe ist es, dass die GUI einen "Fokus" auf dieses Element legen kann wodurch gesteuert wird auf welches Element Benutzereingaben (also Tastendrücke) weitergereicht werden
- DisplayControls : Controls, welche nur der Darstellung dienen.
- Views: Die oberen Klassen weisen eine allgemeine Funktionalität auf. Diese werden benutzt, um <b>Views</b> zu erzeugen. Ein <b>View</b> ist dabei einfach nur eine Bezeichnung für ein UserControl, welches speziell für die Darstellung des Spiels, der AI, etc. verantwortlich ist (und in der Regel aus mehreren allgemeinen Controls besteht):

<b>ArrangementConrols</b>:
| Control | Hauptfunktion | Anwendung
|---------|---------------|----------
| StackPanel | Ordnet die Kind-Elemente untereinander an | Menü-Darstellung
| Grid | Ordnet Kind-Elemente in einem Gitter an | Allgemeiner Aufbau der GUI
| ZFrame | Zeichnet Kind-Elemente an die angegebene Position, nach einem Z-Wert sortiert an | Darstellung eines Info-Fensters, evtl. PopUps'

<b>InteractiveControls</b>:
| Control | Hauptfunktion | Anwendung
|---------|---------------|----------
| EditBox | Erlaubt dem Benutzer, Werte einzugeben | Eingabe von Benutzernamen
| Button | Kann vom Benutzer betätigt werden | Ruft Funktionen im Program auf
| Slider | Zeit Werte auf einem Balken von Links nach Rechts an | Eingabe von Spielgeschwindigkeit
| Table | Zeigt Werte in einer Tabelle an. Es können einzelne Zeilen ausgewählt werden | Highscore, Info's über die KI

<b>DisplayControls</b>:
| Control | Hauptfunktion | Anwendung
|---------|---------------|----------
| Label | Zeigt einen Text an, welcher sich ändern kann | Z.B. für die FrameCounter, FPS, Punktewerte, etc.

<b>Views</b>:
| Control | Hauptfunktion
|---------|--------------
| MenuView | Gibt dem Benutzer die Möglichkeit, das Spiel zu verwalten. Ändern der Settings, starten von einem Spiel, etc.
| GameView | Darstellung des Spiels
| GameStatusView | Darstellung von FPS, FrameCounter, Punkte, etc.
| HighscoreView | Darstellung des Highscores in einer Tabelle 






