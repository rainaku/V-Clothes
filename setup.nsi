; V-Clothes Installer Script
; Requires NSIS 3.x

!include "MUI2.nsh"

; ============ Compression ============
SetCompressor /SOLID lzma
SetCompressorDictSize 64

; ============ General ============
Name "V Clothes"
OutFile "VClothes-Setup.exe"
InstallDir "$PROGRAMFILES64\VClothes"
InstallDirRegKey HKLM "Software\VClothes" "InstallDir"
RequestExecutionLevel admin
Unicode True

; ============ Version Info ============
VIProductVersion "1.0.0.0"
VIAddVersionKey "ProductName" "V Clothes"
VIAddVersionKey "CompanyName" "V Clothes"
VIAddVersionKey "FileDescription" "V Clothes - He thong quan ly cua hang ao thun"
VIAddVersionKey "FileVersion" "1.0.0.0"
VIAddVersionKey "LegalCopyright" "(c) 2025 V Clothes"

; ============ MUI Settings ============
!define MUI_ABORTWARNING
!define MUI_ICON "${NSISDIR}\Contrib\Graphics\Icons\modern-install.ico"
!define MUI_UNICON "${NSISDIR}\Contrib\Graphics\Icons\modern-uninstall.ico"
!define MUI_FINISHPAGE_RUN "$INSTDIR\VClothes.exe"
!define MUI_FINISHPAGE_RUN_TEXT "Khởi chạy V Clothes"
!define MUI_FINISHPAGE_RUN_NOTCHECKED 0

; ============ Pages ============
!insertmacro MUI_PAGE_WELCOME
!insertmacro MUI_PAGE_DIRECTORY
!insertmacro MUI_PAGE_INSTFILES
!insertmacro MUI_PAGE_FINISH

!insertmacro MUI_UNPAGE_CONFIRM
!insertmacro MUI_UNPAGE_INSTFILES

; ============ Language ============
!insertmacro MUI_LANGUAGE "Vietnamese"

; ============ Install Section ============
Section "Install"
    SetOutPath "$INSTDIR"
    
    ; Copy application (single-file, framework-dependent)
    File "VClothes\bin\publish-small\VClothes.exe"
    
    ; Create uninstaller
    WriteUninstaller "$INSTDIR\Uninstall.exe"
    
    ; Start Menu shortcut
    CreateDirectory "$SMPROGRAMS\V Clothes"
    CreateShortcut "$SMPROGRAMS\V Clothes\V Clothes.lnk" "$INSTDIR\VClothes.exe"
    CreateShortcut "$SMPROGRAMS\V Clothes\Go cai dat.lnk" "$INSTDIR\Uninstall.exe"
    
    ; Desktop shortcut
    CreateShortcut "$DESKTOP\V Clothes.lnk" "$INSTDIR\VClothes.exe"
    
    ; Registry entries for Add/Remove Programs
    WriteRegStr HKLM "Software\VClothes" "InstallDir" "$INSTDIR"
    WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\VClothes" "DisplayName" "V Clothes"
    WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\VClothes" "UninstallString" "$INSTDIR\Uninstall.exe"
    WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\VClothes" "DisplayVersion" "1.0.0"
    WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\VClothes" "Publisher" "V Clothes"
    WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\VClothes" "NoModify" 1
    WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\VClothes" "NoRepair" 1
SectionEnd

; ============ Uninstall Section ============
Section "Uninstall"
    ; Remove files
    Delete "$INSTDIR\VClothes.exe"
    Delete "$INSTDIR\Uninstall.exe"
    RMDir "$INSTDIR"
    
    ; Remove shortcuts
    Delete "$SMPROGRAMS\V Clothes\V Clothes.lnk"
    Delete "$SMPROGRAMS\V Clothes\Go cai dat.lnk"
    RMDir "$SMPROGRAMS\V Clothes"
    Delete "$DESKTOP\V Clothes.lnk"
    
    ; Remove registry
    DeleteRegKey HKLM "Software\VClothes"
    DeleteRegKey HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\VClothes"
SectionEnd
