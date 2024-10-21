# SnakeAI 1.1 Erweiterung
Autor: Sergei Richter

Dieses Dokument beschreibt die Erweiterung 1.1 von dem "SnakeAI" Program. SnakeAI ist die Umsetzung eines simplen Snake Spiels in einer Konsole.

Die Erweiterung hat dabei die folgenden Kernziele (Priorität in absteigender Reihenfolge):
- Hinzufügen einer künstlichen Intelligenz welche die Schlange steuert.
- Aufsetzen einer neuen, verbesserten Oberfläche welche mehrere Konsolenfenster verwaltet und mit der bequemer gearbeitet werden kann
- Hinzufügen eines Multiplayer-Modus für zumindest 2 Spieler (Multiplayer am selben PC)

Für die Umsetzung der künstlichen Intelligenz werden aus pädagogischen Gründen keine externen Libraries verwendet (für speichern von Daten, Matrixmultiplikation, etc. allerdings schon).

# K.I.

Die K.I. wird als neuronales Netzwerk implementiert. Für die Matrixberechnung und Zufallsgeneratoren wird die externe Bibliothek <em>MathNet</em> verwendet, aber ansonsten werden alle Algorithmen und Objekte (feed forward network, Backpropagation) selbst entwickelt.

Es soll später möglich sein, mehrere unterschieddliche K.I. Modelle/Strategien dem System hinzuzufügen, aber zunächst wird ein Model umgesetzt: die <em>MimicStrategy</em>

## MimicStrategy

# ConsoleArea und -Window Erweiterung

# Multipayer Modus

# Backlog

# Änderungen während der Entwicklung