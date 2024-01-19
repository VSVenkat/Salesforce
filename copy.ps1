# Source and destination folder paths
#$sourceFolder = "C:\Users\vvallams\Downloads\Test\Source"
#$destinationFolder = "C:\Users\vvallams\Downloads\Test\Destination"

param (
    [string]$sourceFolder = "C:\Users\vvallams\Downloads\Test\Source",
    [string]$destinationFolder="C:\Users\vvallams\Downloads\Test\Destination"
    
)

# Check if the source folder exists
if (Test-Path $sourceFolder -PathType Container) {
    # Check if the destination folder exists, if not, create it
    if (-not (Test-Path $destinationFolder -PathType Container)) {
        New-Item -ItemType Directory -Path $destinationFolder
    }

    # Copy all files from the source folder to the destination folder
    Get-ChildItem -Path $sourceFolder | ForEach-Object {
        Copy-Item $_.FullName -Destination $destinationFolder -Force
        Write-Host "Copied: $($_.FullName)"
    }

    Write-Host "All files copied successfully."
} else {
    Write-Host "Source folder does not exist."
}
https://github.com/tigran201198/learnit_rest_sharp/tree/main/08BaseTestHt
