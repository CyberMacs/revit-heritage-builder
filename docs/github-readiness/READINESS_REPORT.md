# GitHub Readiness Report

Generated: 2026-09-14T18:30:06+03:00
Project: `D:\Ai Apps\ChatGPT Codex\CAD\2026-09-14-revit-plugin-heritage-builder`
Git repository: yes
Branch: `main`
Detected stack: .NET
Files scanned: 92

## Findings

- **WARN** `secret-scan-skipped` `src/RevitHeritageBuilder/bin/Release/net8.0-windows/Resources/house_semantic.json`: Text-like file is 5.5 MiB; regex scan skipped.
- **WARN** `secret-scan-skipped` `src/RevitHeritageBuilder/Resources/house_semantic.json`: Text-like file is 5.5 MiB; regex scan skipped.

## Remotes

- None detected.

## Notes

- Regex and filename checks are heuristic; contextual privacy/security review is still required.
- Current-file secret scan does not prove that Git history is free of hardcoded secrets.
- Git working tree/status is not empty; review intended changes before staging.
