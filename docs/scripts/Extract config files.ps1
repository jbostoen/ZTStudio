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

$ZipFiles = Get-ChildItem -Recurse -Include "*.ztd" -path $Path


$Filters = @("ai", "cfg", "lyt", "scn", "uca", "ucb", "ucs")

     
$Filter = "(.*\." + ( $Filters -Join "|.*\." ) + ")"


$ZipFiles | ForEach-Object { 

    $ZipFile = $_ 

    # Open ZIP archive for reading
    $zip = [System.IO.Compression.ZipFile]::OpenRead($ZipFile)

    $zip.Entries | Where-Object { $_.FullName -match $Filter -and $_.Length -ne 0 } | Sort-Object $_.FullName | ForEach-Object {

    

        $FinalName = "$($ProjectPath)" + ($ZipFile.FullName -Replace  [System.Text.RegularExpressions.Regex]::Escape($Path), "") + "\" + "$($_.FullName)" -replace "\.(" + ($Filters -Join "|") + ")`$", ($_.LastWriteTime.ToString("yyyyMMddHHmmss") + ".$1")
        $FinalPath = (Split-Path -Path $FinalName)
        

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