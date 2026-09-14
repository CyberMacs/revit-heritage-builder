# Release checklist

- [ ] Run dotnet build in Release configuration against Revit 2025.
- [ ] Run installer/Package.ps1.
- [ ] Install the ZIP with Install-CurrentUser.ps1 on a clean or test Windows profile.
- [ ] Start Revit 2025 and confirm the Heritage Builder tab is present.
- [ ] Run Create Ayse Mayda House.
- [ ] Confirm the generated RVT opens and includes walls, hosted windows/door, floors, roofs, railings, stairs, and family assets.
- [ ] Check %LOCALAPPDATA%\RevitHeritageBuilder\Logs\build.log for unexpected failures.
- [ ] Run github_repo_doctor.py with write reports.
- [ ] Review Git status and staged file list before pushing.
- [ ] Choose repository visibility and a license before public release.

