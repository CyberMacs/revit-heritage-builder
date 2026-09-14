# Átadási jegyzék – v0.1.0

## Létrehozott fájlok

- src/RevitHeritageBuilder: C# Revit 2025 add-in forrása és csomagolt RFA-családai.
- installer: telepítő, eltávolító, manifest-sablon és csomagoló script.
- dist/RevitHeritageBuilder-v0.1.0.zip: fejlesztői kiadási csomag.
- Final/Portable: továbbadható, kézi telepítéshez is használható hordozható csomag.
- Final/RevitHeritageBuilder-Portable-v0.1.0.zip: a Portable mappa ZIP-változata.
- README.md, INSTALL.md, CHANGELOG.md, SECURITY.md: angol GitHub dokumentáció.
- docs: fejlesztői és kiadási ellenőrző lista.
- !Manual: magyar útmutatók.
- GPT-tapasztalat.md: tapasztalatok.
- BackUp: az átvett források és a módosított projektfájlok korábbi állapota.

## Módosított eredeti fájlok

Nincs. A korábbi Revit-projekt forrásait és családjait új projektbe másoltuk.

## Függőségek

Használat: Windows és Autodesk Revit 2025.
Fordítás: .NET 8 SDK, valamint telepített Revit 2025 API.
A kész telepítőcsomagban nincsenek Autodesk DLL-ek.

## Következő lépések

1. Telepítés tesztelése tiszta Revit 2025 meneten.
2. Nyilvános GitHub feltöltés MIT licenccel.
3. GitHub repository elkészült: https://github.com/CyberMacs/revit-heritage-builder. A kiadási ZIP GitHub Release-be töltése szükség szerint külön lépés.

