# GPT-tapasztalat – Revit Heritage Builder v0.1.0

A korábbi Revit-megoldás egy ideiglenes IExternalApplication híd volt, amely a fejlesztői Work mappában lévő kérésfájlt figyelte. Ez nem megosztható termékforma. Az új változat szalagos Revit add-in: IExternalApplication hozza létre a Ribbon panelt, IExternalCommand indítja a generálást.

A megoszthatóság kulcsa: a DLL melletti Resources könyvtárból olvassa a JSON-t és az RFA-kat; a saját írások a LocalApplicationData RevitHeritageBuilder mappába kerülnek. Így nincs D: meghajtóra vagy fejlesztői mappára mutató futásidejű útvonal.

A régi kód az ablak- és ajtócsaládot futás közben akarta generálni angol családsablonokból. A hordozható változat a már ellenőrzött RFA-kat csomagolja, így nyelvi sablonfüggőség nem marad a családoknál. A projekt létrehozásához a TemplateLocator a telepített Revit 2025 sablonok között keres DefaultMetric.rte fájlt, majd bármilyen megtalált RTE-re esik vissza.

Fordításkor az API-hivatkozás elsőre hibás volt, mert a környezeti változó és DLL-név között eltűnt a visszaperjel. Külön RevitApiPath tulajdonság javította. A csomag lefordult 0 hibával. A PowerShell Package script önmagában MSB3277 Revit függőségi figyelmeztetéseket adott, míg a közvetlen build NoWarn kapcsolóval tiszta volt; ezt a scriptben is célszerű elnyomni.

A csomagolás tartalmaz telepítő scriptet, de a Revitben megjelenő szalag és teljes generálás újbóli futtatási tesztje még szükséges tiszta Revit 2025 meneten. Ezt a kiadás előtt nem szabad készre tesztelt állapotként kommunikálni.

GitHub feltöltéshez a pi-technika-github-uploader skill releváns: doktor-scan, .gitignore, dokumentáció, helyi commit, majd láthatóság/licenc döntés és csak utána remote létrehozás. A GitHub CLI hitelesítve volt, de tokent nem szabad forrásba, naplóba vagy válaszba írni.


Portable csomag: a Package.ps1 eredetileg nem másolta a LICENSE fájlt, ezért ezt hozzá kellett adni, majd újraépíteni. A Final/Portable a kész buildből készült, így a DLL, a Resources és a telepítési manifest egy verzióból származik. A telepítő és eltávolító PowerShell-parser ellenőrzése hibamentes volt. A SHA-256 fájl a Portable tartalmát védi átvitelkor; a checksum fájl maga szándékosan nincs önmagába beleírva.
