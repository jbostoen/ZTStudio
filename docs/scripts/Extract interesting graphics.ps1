#requires -Version 5.0

# Note: run as Administrator; also run Set-ExecutionPolicy Unrestricted
# Written for Zoo Tycoon - Complete Collection

# Change these.
$Path = "c:\program files (x86)\Microsoft Games\Zoo Tycoon" # Where is Zoo Tycoon installed?
$ProjectPath = "C:\test" # Where should the files be extracted to?


# change extension filter to a file extension that exists
# inside your ZIP file

Write-Host "Please wait, this may take a while."


if((Test-Path -Path $ProjectPath) -eq $false)
{
  $null = New-Item -Path $ProjectPath -ItemType Directory -Force
}

# Load ZIP methods
Add-Type -AssemblyName System.IO.Compression.FileSystem

$ZipFiles = @(

    # Original Zoo Tycoon
    "$($Path)\objects.ztd",
    "$($Path)\global.ztd",
    
    # Dinosaur Digs
    "$($Path)\xpack1\object01.ztd",
    "$($Path)\xpack1\objects3.ztd",

    # Marine Mania
    "$($Path)\xpack2\animals9.ztd",
    "$($Path)\xpack2\animalsa.ztd",
    "$($Path)\xpack2\object05.ztd",
    "$($Path)\xpack2\objects6.ztd",

    # Complete collection
    "$($Path)\updates\animalsa.ztd",
    "$($Path)\updates\object06.ztd"

);

$Filters = @(
    "^objects\/bamboo\/.*",
    "^objects\/restrant\/.*",
    "^objects\/testbox\/.*",
    "^objects\/temp\/.*",
    "^objects\/showbuoy\/.*",
    "^objects\/asirope\/.*",
    "^animals\/orngutan\/.*",
    "^animals\/dolphin\/.*",
    "^objects\/cinema\/.*",
    "^objects\/swnglog\/.*",
    "^guests\/(man|woman)\/putt.*",
    "^objects\/dinoputt\/putt.*",
    )
     
$Filter = "(" + ( $Filters -Join "|" ) + ")"


$ZipFiles | ForEach-Object { 

    $ZipFile = $_ 

    # Open ZIP archive for reading
    $zip = [System.IO.Compression.ZipFile]::OpenRead($ZipFile)

    $zip.Entries | Where-Object { $_.FullName -match $Filter -and $_.Length -ne 0 } | Sort-Object $_.FullName | ForEach-Object {

        $FinalName = "$($ProjectPath)\$($_.FullName)"
        $FinalPath = Split-Path -Path $FinalName

        If((Test-Path -Path $FinalPath) -eq $false) {
            New-Item -ItemType Directory -Path $FinalPath -Force
        }
        
        [System.IO.Compression.ZipFileExtensions]::ExtractToFile( $_ , $FinalName, $true)

    }

    # close ZIP file
    $zip.Dispose()

}

# Open folder
explorer $ProjectPath 

Write-Host "Finished!"