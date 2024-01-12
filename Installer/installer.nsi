# infos sur l'application
!define APPNAME "AutomateDesign"
!define COMPANYNAME "LETSGO"
!define DISPLAYCOMPANYNAME "Let's Go"
!define DESCRIPTION "SAÉ 5"

# version
!define VERSIONMAJOR 1
!define VERSIONMINOR 0
!define VERSIONBUILD 0

# taille en kio du programme installé
# défini automatiquement par deploy-client, décommentez cette ligne si vous déployez manuellement
# !define INSTALLSIZE 47355

# demande les permissions admin
!ifdef USERINSTALL
RequestExecutionLevel user
!else
RequestExecutionLevel admin
!endif

# dossier par défaut d'installation
!ifdef USERINSTALL
InstallDir "$APPDATA\${COMPANYNAME}\${APPNAME}"
!else
InstallDir "$PROGRAMFILES\${COMPANYNAME}\${APPNAME}"
!endif

Name "${APPNAME}"

# icône de l'installateur
!define MUI_ICON "${NSISDIR}\Contrib\Graphics\Icons\nsis3-install.ico"

# page d'accueil
!define MUI_WELCOMEPAGE_TITLE "Installation d'AutomateDesign"
!define MUI_WELCOMEPAGE_TEXT "Ce logiciel à été réalisé dans le cadre de la SAÉ 5 par Loïc BOUCHER, Louis DEVIE, Corentin FEVRE et Benjamin PASQUIER."


# emplacement de l'installateur
!ifdef USERINSTALL
outFile "..\Client\bin\automatedesign_setup_user.exe"
!else
outFile "..\Client\bin\automatedesign_setup.exe"
!endif

!include LogicLib.nsh
!include MUI2.nsh

!insertmacro MUI_PAGE_WELCOME
!insertmacro MUI_PAGE_DIRECTORY
!insertmacro MUI_PAGE_INSTFILES
 
function .onInit
	setShellVarContext all
functionEnd
 
section "install"
	setOutPath $INSTDIR

	# fichiers à inclure
	file /a /r ..\Client\bin\deploy\*.*
	# nom de l'exécutable
	!define ENTRYPOINT "Client.exe"

	writeUninstaller "$INSTDIR\uninstall.exe"
 
	createDirectory "$SMPROGRAMS\${DISPLAYCOMPANYNAME}"
	createShortCut "$SMPROGRAMS\${DISPLAYCOMPANYNAME}\${APPNAME}.lnk" "$INSTDIR\${ENTRYPOINT}" "" "$INSTDIR\${ENTRYPOINT}"

	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${COMPANYNAME} ${APPNAME}" "DisplayName" "${COMPANYNAME} - ${APPNAME} - ${DESCRIPTION}"
	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${COMPANYNAME} ${APPNAME}" "UninstallString" "$\"$INSTDIR\uninstall.exe$\""
	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${COMPANYNAME} ${APPNAME}" "QuietUninstallString" "$\"$INSTDIR\uninstall.exe$\" /S"
	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${COMPANYNAME} ${APPNAME}" "InstallLocation" "$\"$INSTDIR$\""
	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${COMPANYNAME} ${APPNAME}" "DisplayIcon" "$\"$INSTDIR\application-icon.ico$\""
	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${COMPANYNAME} ${APPNAME}" "Publisher" "$\"${COMPANYNAME}$\""
	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${COMPANYNAME} ${APPNAME}" "DisplayVersion" "$\"${VERSIONMAJOR}.${VERSIONMINOR}.${VERSIONBUILD}$\""
	WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${COMPANYNAME} ${APPNAME}" "VersionMajor" ${VERSIONMAJOR}
	WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${COMPANYNAME} ${APPNAME}" "VersionMinor" ${VERSIONMINOR}

	WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${COMPANYNAME} ${APPNAME}" "NoModify" 1
	WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${COMPANYNAME} ${APPNAME}" "NoRepair" 1

	WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${COMPANYNAME} ${APPNAME}" "EstimatedSize" ${INSTALLSIZE}
sectionEnd

function un.onInit
	SetShellVarContext all

	MessageBox MB_OKCANCEL "Voulez-vous vraiment désinstaller ${APPNAME}?" IDOK next
		Abort
	next:
functionEnd
 
section "uninstall"
	delete "$SMPROGRAMS\${DISPLAYCOMPANYNAME}\${APPNAME}.lnk"
	rmDir "$SMPROGRAMS\${DISPLAYCOMPANYNAME}"

	delete $INSTDIR\*
 
	delete $INSTDIR\uninstall.exe
 
	rmDir $INSTDIR
 
	DeleteRegKey HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${COMPANYNAME} ${APPNAME}"
sectionEnd