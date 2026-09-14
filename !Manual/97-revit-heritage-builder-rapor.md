# Revit Heritage Builder – projekt riport

Verzió: 0.1.0. Dátum: 2026-09-14.

A korábbi egyszeri Ayse Mayda Revit-generátorból önálló, telepíthető Revit 2025 bővítmény készült. A fejlesztői munkakönyvtár figyelése és a háttérben futó híd kikerült. A bővítmény a saját telepítési mappájából olvassa a családokat és a geometriai adatot.

A Revit szalagon új Heritage Builder fül jelenik meg. A Create Ayse Mayda House gomb megerősítés után egy új RVT-ben hozza létre a natív elemekből álló mintaházat. Az aktuálisan megnyitott projektet nem módosítja. Az About / Help gomb a hatókörről és a korlátokról tájékoztat.

A csomag Revit 2025 API-val lefordult, 0 figyelmeztetéssel és 0 hibával. Létrejött a megosztható ZIP. A telepítő csak a saját felhasználó AppData Revit Addins 2025 helyére ír. A függőségek és a Revit DLL-ek nem kerültek a csomagba.

A régi generátor Revit 2025-ös futása korábban ellenőrzött volt. Az új szalagos v0.1.0 wrapper futtatását tiszta Revit 2025 meneten a kiadás előtt még külön ellenőrizni kell. Emiatt a jelenlegi csomag előzetes kiadásnak tekintendő.

A tulajdonos Public láthatóságot és MIT licencet választott. A GitHub repository létrejött, a main ág feltöltése sikeres: https://github.com/CyberMacs/revit-heritage-builder. A telepítő ZIP jelenleg a helyi Final mappában van; GitHub Release feltöltése külön kiadási döntés lehet.


A későbbi kérésre elkészült a Final/Portable hordozható mappa is. Tartalmazza a DLL-t, Resources mappát, telepítőt, eltávolítót, magyar kézi telepítési leírást, MIT licencet és SHA-256 ellenőrzőösszegeket. A Final/RevitHeritageBuilder-Portable-v0.1.0.zip ugyanennek továbbadható ZIP-változata.
