# SnakeAI 1.1 Erweiterung
Autor: Sergei Richter

Dieses Dokument beschreibt die Erweiterung 1.1 von dem "SnakeAI" Program. SnakeAI ist die Umsetzung eines simplen Snake Spiels in einer Konsole.

Die Erweiterung hat dabei die folgenden Kernziele:
- Hinzufügen eines Multiplayer-Modus für zumindest 2 Spieler (Multiplayer am selben PC) (v1.1)
- Aufsetzen einer neuen, verbesserten Oberfläche welche mehrere Konsolenfenster verwaltet und mit der bequemer gearbeitet werden kann (v1.2)
- Hinzufügen einer künstlichen Intelligenz welche die Schlange steuert. (v1.3)

Für die Umsetzung der künstlichen Intelligenz werden aus pädagogischen Gründen keine externen Libraries verwendet (für speichern von Daten, Matrixmultiplikation, etc. allerdings schon).

# Mehrspieler Modus
Es sollen mehrere Spieler gleichzeitig (an der selben Maschine) spielen können. Dazu müssen konkret folgende Sachen geändert werden:
- Die Game Klasse kann mehrere Snakes verwalten
- Die Controls für die Schlange müssen an die richtige Schlange weitergeleitet werden. Insbesondere ist es wichtig, dass mehrere Spieler gleichzeitig (oder zumindest in sehr kurzen Abständen) eine Taste drücken können
- Die Snakes Klasse muss auch korrekt damit umgehen, dass Schlangen sich gegenseitig essen können.

Dafür wird das Klassendesign wie folgt geändert:
```nomnoml
#direction: right
[Game|
    ...
    + Snakes : SnakePopulation
    ...|
]

[Game] o- [SnakePopulation|
    - _snakes : Snake\[\]
    + NumberSnakes : int|
    ~ SnakePopulation(g : Game)
    + DeactivateSnake(s : Snake) : void
    + ActivateSnake() : Snake?
    ~ CheckCollision(f : Food) : void
    ~ CheckCollision() : void
    ~ UpdatePosition() : void
    ~ UpdateState() : void
]

[SnakePopulation] o- n[Snake|
    ...
    + <get> IsActive : bool
    ...|
]

```

Anstatt ein Objekt vom Typ <b>Snake</b> besitzt das <b>Game</b>-Objekt nun ein Objekt vom Typ <b>SnakePopulation</b>. Eine <b>SnakePopulation</b> wiederum besitzt mehrere <b>Snake</b> Objekte und agiert wie ein Wrapper um alle <b>Snake</b> herum, d.h. es leitet Aufrufe wie <em>CheckCollision(Food f)</em> an die einzelnen Schlangen weiter.

Die <b>Snake</b>-Objekte werden einmal beim Programstart erstellt, die Anzahl ändert sich nicht während der Programausführung nicht. Wenn der Benutzer nun mehrere Schlangen hinzufügen möchte, so wird das <em>IsActive</em>-Flag gesetzt, welches bei den <em>UpdateState(), CheckCollision()</em> Funktionen etc. berücksichtigt wird. Auf diese Weise müssen keine Listen über mehrere Threads synchronisiert werden und die GUI muss beim Zeichnen über das Flag abfragen, ob die Schlange noch aktiv ist.
