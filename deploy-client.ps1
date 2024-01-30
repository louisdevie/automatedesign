param (
    [Switch]$Help,
    [Switch]$InstallerOnly
)

$ErrorActionPreference = "Stop"

$BuildOutputDir = "Client\bin\deploy"
$InstallerOutputFile = "Client\bin\automatedesign_setup.exe"

$HelpMessage = @"
Ce script permets de construire l'application de bureau AutomateDesign et son installateur. 

Utilisation: deploy-client [OPTIONS...]

Options:
   -Help            Affiche ce message et quitte
   -InstallerOnly   Garde le projet déjà publié et reconstruit seulement l'installateur.
"@

if ($Help) {
    Write-Host $HelpMessage
    exit
}

$ExpectedSolutionPath = Join-Path -Path $PSScriptRoot -ChildPath "AutomateDesign.sln"
if (-not(Test-Path -Path $ExpectedSolutionPath))
{
    Write-Host "Ce script doit être exécuté à la racine du projet."
    exit 1
}

function TryRemove-Item
{
    param ([String[]]$Path, [Switch]$Recurse)
    
    if (Test-Path -Path $Path) {
        Remove-Item -Path $Path -Recurse:$Recurse.IsPresent -Force
    }
}

function Test-CommandExists
{
    param ([String]$Command)
    
    $Result = $false
    
    try {
        if(Get-Command $Command)
        {
            $Result = $true
        }
    }
    catch {  } # juste ignorer les erreurs
    
    return $Result
}

$MakeNSISCommands = @("makensis.exe", "C:\Program Files (x86)\NSIS\makensis.exe")

foreach ($Command in $MakeNSISCommands)
{
    if (Test-CommandExists $Command)
    {
        $MakeNSISCommandFound = $Command
    }
}

if ($MakeNSISCommandFound -eq $null)
{
    Write-Host "Impossible de trouver le compilateur NSIS. Vérifiez qu'il est bien installé et que makensis.exe se trouve sur le PATH."
    exit 1
}

# clean up

Write-Host "Nettoyage..."

if (-not $InstallerOnly)
{
    TryRemove-Item -Path $BuildOutputDir -Recurse
}
TryRemove-Item -Path $InstallerOutputFile


# build

if ($InstallerOnly)
{
    if (Test-Path -Path $BuildOutputDir)
    {
        Write-Host "Construction du projet ignorée"        
    }
    else
    {
        Write-Host "Le projet n'a pas encore été construit. Appelez d'abord cette commande sans -InstallerOnly."
        exit 1
    }
}
else
{
    Write-Host "Construction du projet..."

    dotnet publish Client -c Release -r win-x64 --self-contained true -o $BuildOutputDir
}

# check size

$TotalInstalledSizeInBytes = (Get-ChildItem -Path $BuildOutputDir -Recurse | Measure-Object -Property Length -Sum).Sum
$TotalInstalledSizeInKB = [math]::Truncate($TotalInstalledSizeInBytes / 1KB)


# invoke NSIS

Write-Host "Construction de l'installateur..."

& $MakeNSISCommandFound /V1 "/DINSTALLSIZE=$TotalInstalledSizeInKB" Installer/installer.nsi

Write-Host "Terminé."