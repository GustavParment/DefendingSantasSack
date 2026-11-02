# Defending Santa's Sack – Projektplan

Ett 2D tower defense-spel med jultema, byggt i **Godot 4 (.NET-versionen) med C#**.

**Start:** 2026-11-01
**Mål:** Spelbar version till julafton 2026-12-24

---

## Spelidé

Nissar, snögubbar och andra julbusar försöker sno julklapparna ur tomtens säck.
Spelaren bygger torn på ett rutnät och formar på så sätt själv **labyrinten** som
fienderna måste ta sig igenom. Ju längre väg man tvingar dem att gå, desto längre
tid får tornen på sig att skjuta.

### Kärnregler
- Banan är ett rutnät. Varje ruta är antingen ledig eller har ett torn.
- Fienderna går från **start** till **tomtens säck** längs kortaste vägen.
- Spelaren får **aldrig blockera vägen helt**. Ett torn som skulle stänga vägen kan inte placeras.
- När ett torn placeras räknar fienderna om sin väg.
- Varje fiende som når säcken tar en julklapp (= ett liv). Slut på klappar = game over.
- Fiender som dör ger **pepparkakor** (valuta) som används för att bygga och uppgradera torn.

### Torn (förslag)
| Torn | Funktion |
|---|---|
| Snöbollskastare | Billigt basstorn, skjuter en fiende i taget |
| Pepparkaksmortel | Långsam men gör skada på ett område |
| Kakaosnurr | Saktar ner fiender i närheten |
| Polkagrisspjut | Lång räckvidd, hög skada, dyrt |

### Fiender (förslag)
| Fiende | Egenskap |
|---|---|
| Busnisse | Vanlig, medelsnabb |
| Snögubbe | Långsam men tål mycket |
| Isälva | Snabb, lite liv |
| Grinchen (boss) | Sista vågen, massor av liv |

---

## Verktyg

| Verktyg | Varför |
|---|---|
| **Godot 4 – .NET-versionen** | Lätt spelmotor som klarar 4 GB RAM och har bra 2D-stöd. Välj .NET-versionen för C#. |
| **.NET SDK** | Redan installerat (.NET 10). Godot behöver det för att bygga C#. |
| **VSCodium** | Kodeditor. Installera tilläggen **C#** (muhammad-sammy, från Open VSX) och **godot-tools**. |
| **Renderare: Compatibility** | Använder OpenGL och passar Intel HD 5000 bäst. Väljs när projektet skapas. |

### Godot-begrepp som är bra att kunna
| Begrepp | Vad det är |
|---|---|
| **Node** | Byggsten. Allt i spelet är noder (sprite, kamera, timer, ljud…) |
| **Scene** | Ett träd av noder som sparas som en fil (`.tscn`). En fiende, ett torn eller en hel bana är var sin scen. |
| **Instansiera** | Skapa en kopia av en scen i koden (`PackedScene.Instantiate()`), t.ex. en ny fiende |
| **Signal** | Ett sätt för noder att säga till när något händer, som events i C# |
| **Autoload** | Ett skript som alltid finns laddat, bra för `GameManager` |
| **`_Ready()` / `_Process(delta)`** | Körs när noden startar / varje bildruta |

---

## Teknisk översikt

| Del | Lösning i Godot |
|---|---|
| Rutnät | `TileMapLayer` för marken, plus en egen `GridManager` som håller koll på lediga och upptagna rutor |
| Pathfinding | Egen **BFS** (bredden-först-sökning) i C#, för att lära sig algoritmen. (`AStarGrid2D` finns inbyggt som reserv.) |
| Fiender | Scen `Enemy.tscn` (`Node2D` + `Sprite2D`) med ett skript som följer en lista med rutor |
| Vågor | `WaveSpawner`-nod med en `Timer` som instansierar fiender |
| Torn | Scen `Tower.tscn` med en `Area2D` för räckvidd som hittar fiender och skjuter |
| Projektiler | Scen `Projectile.tscn` som flyger mot målet och gör skada |
| Ekonomi och liv | `GameManager` som Autoload, med signaler när pengar eller liv ändras |
| UI | `CanvasLayer` med `Control`-noder (`Label`, `Button`, `HBoxContainer`) |
| Snö | `CPUParticles2D` (fungerar bäst med Compatibility-renderaren) |

---

## Veckoplan

### Vecka 1 (1–8 nov): Installation och grunder
- [ ] Ladda ner **Godot 4 .NET** (Linux x86_64) från godotengine.org och packa upp den
- [ ] Installera C#- och godot-tools-tilläggen i VSCodium
- [ ] Ställ in VSCodium som extern editor i Godot (*Editor Settings → Dotnet → Editor*)
- [ ] Skapa projektet i `~/Projects/DefendingSantasSack/Game` med renderaren **Compatibility**
- [ ] Lär dig editorn: Scene-panelen, Inspector, FileSystem, 2D-vyn
- [ ] Första C#-skriptet: få en `Sprite2D` att röra sig med piltangenterna

### Vecka 2 (9–15 nov): Rutnät och byggande
- [ ] Skapa en `TileSet` och rita marken med `TileMapLayer`
- [ ] Översätt musens position till en ruta (`LocalToMap`)
- [ ] Klicka för att placera ett block (platshållartorn)
- [ ] Markera start- och målruta (säcken)

### Vecka 3 (16–22 nov): Pathfinding
- [ ] Implementera BFS i C#
- [ ] Rita den beräknade vägen på skärmen (`Line2D`) för att felsöka
- [ ] Neka placering som blockerar vägen
- [ ] Räkna om vägen när ett torn placeras

### Vecka 4 (23–29 nov): Fiender och vågor
- [ ] `Enemy.tscn` som följer vägen
- [ ] Liv på fiender, julklappar (liv) på spelaren
- [ ] `WaveSpawner` med flera vågor
- [ ] Fiender som redan är på väg ska byta väg om labyrinten ändras

### Vecka 5 (30 nov–6 dec): Torn som skjuter
- [ ] `Tower.tscn` med `Area2D` som hittar fiender inom räckvidd
- [ ] Projektiler och skada
- [ ] Pepparkakor när fiender dör, tornen kostar pepparkakor
- [ ] Minst två torntyper

### Vecka 6 (7–13 dec): UI och spelflöde
- [ ] Visa pepparkakor, julklappar och aktuell våg
- [ ] Knappar för att välja torn
- [ ] Startmeny, game over och vinstskärm (separata scener)
- [ ] Sälja och uppgradera torn

### Vecka 7 (14–20 dec): Jul!
- [ ] Byt platshållarna mot julgrafik (egen eller fria assets, t.ex. från itch.io / Kenney.nl)
- [ ] Snöfall med `CPUParticles2D`
- [ ] Ljudeffekter och julmusik (`AudioStreamPlayer`)
- [ ] Balansera svårighetsgraden

### Vecka 8 (21–24 dec): Finputs och release
- [ ] Buggfixar
- [ ] Exportera en körbar version för Linux och Windows (*Project → Export*)
- [ ] Låt någon testspela 🎄

---

## Om tiden räcker (bonus)
- Flera banor
- Fler torn och fiender
- Highscore (sparas med `FileAccess` / `ConfigFile`)
- Snabbspola-knapp (`Engine.TimeScale`)

> **Obs:** Webbexport (spela i webbläsaren) fungerar i nuläget inte med C# i Godot 4.
> Att dela en Linux- eller Windows-version (t.ex. på itch.io) fungerar däremot bra.

## Om tiden inte räcker
Prioritera i den här ordningen: **rutnät → pathfinding → fiender → torn → UI → grafik**.
Ett spel med fyrkanter som fungerar är bättre än ett snyggt spel som inte går att spela.

---

## Mappstruktur
```
DefendingSantasSack/
├── PLAN.md              ← den här filen
└── Game/                ← Godot-projektet
    ├── project.godot
    ├── scenes/          ← .tscn-filer (Main, Enemy, Tower, Projectile, UI…)
    ├── scripts/         ← .cs-filer
    └── assets/          ← grafik, ljud, typsnitt
```
