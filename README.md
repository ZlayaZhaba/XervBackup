XervBackup
==========

XervBackup for Backup and DR

Andrey: 1. open project in VisualStudio Express
2. Build release
3. cd to Src\Installer and launch VS Express build.bat 4. if it fail to finish before final package build:
 
4a cd to Src\Installer\bin\Release\XervBackup and try to launch XevBackup.exe - if it start without problem "good", if not - need to investigate 
4b return back to Src\Installer and launch WixProjBuilder.exe WixInstaller.wxiproj

