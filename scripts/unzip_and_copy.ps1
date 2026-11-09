$Environment = $env:COMPUTERNAME
switch ($Environment)
{
    "DEV-UAT-TMS" {
        $zipPath = "C:\vijay\AutoDeploy\B2BCandidateRegistration_deployment.zip"
        $extractPath = "C:\vijay\AutoDeploy\B2BCandidateRegistration\UnzipedFile"
        $finalPath = "C:\vijay\AutoDeploy\B2BCandidateRegistration\AfterUnzip"
        $subFolderToMove = "_PublishedWebsites\B2BCandidateRegistration"
        $deployPath = "C:\inetpub\wwwroot\B2BCandidateRegistration"
        # Backup root folder
        $backupRoot = "C:\vijay\ReleaseBuild\Backup"
    }
    
    default {
        throw "Computer name not found for deployment. Computer/Environment value: '$Environment'"
    }
}

# Create timestamp folder name
$timestamp = "B2BCandidateRegistration_" + (Get-Date -Format "yyyyMMdd_HHmmss")

# Ensure temp and final directories exist
if (!(Test-Path $extractPath)) { New-Item -ItemType Directory -Path $extractPath }
if (!(Test-Path $finalPath)) { New-Item -ItemType Directory -Path $finalPath }

# OPTIONAL: Clean extractPath before unzip
if (Test-Path $extractPath) {
    Write-Host "Clearing $extractPath"
    Remove-Item "$extractPath\*" -Recurse -Force -ErrorAction SilentlyContinue
}

# OPTIONAL: Clean finalPath before moving (if desired)
if (Test-Path $finalPath) {
    Write-Host "Clearing $finalPath"
    Remove-Item "$finalPath\*" -Recurse -Force -ErrorAction SilentlyContinue
}

# Unzip deployment.zip folder
Write-Host "Unzipping $zipPath to $extractPath"
Expand-Archive -Path $zipPath -DestinationPath $extractPath -Force

# Build full path to the subfolder
$subFolderPath = Join-Path $extractPath $subFolderToMove

# Delete web.config if it exists
$webConfigPath = Join-Path $subFolderPath "web.config"
if (Test-Path $webConfigPath) {
    Write-Host "Deleting $webConfigPath"
    Remove-Item $webConfigPath -Force
}

# Move files from unzipped folder to final folder
Write-Host "Moving files to $finalPath"
# Commented on 06/06/2026 Copy-Item -Path "$subFolderPath\*" -Destination $finalPath -Force
robocopy $subFolderPath $finalPath /E /R:2 /W:2
if ($LASTEXITCODE -ge 8)
{
    throw "Deployment copy failed:unzipped folder to final folder. Robocopy exit code: $LASTEXITCODE"
}

# Backup folder for this deployment
$backupPath = Join-Path $backupRoot $timestamp

# Create backup folder
New-Item -ItemType Directory -Path $backupPath -Force | Out-Null

# Backup current deployed files
Write-Host "Backing up current deployment from $deployPath to $backupPath"
# Commented on 06/06/2026 as below is more reliable Copy-Item -Path "$deployPath\*" -Destination $backupPath -Recurse -Force
robocopy $deployPath $backupPath /E /XD "$deployPath\pdffile" /R:2 /W:2
if ($LASTEXITCODE -ge 8)
{
    throw "Deployment copy failed:Backup current deployed files. Robocopy exit code: $LASTEXITCODE"
}

# Move files from After final folder to deploy folder
Write-Host "Moving files to $deployPath"
# Commented on 06/06/2026 as below is more reliable Copy-Item -Path "$finalPath\*" -Destination $deployPath -Recurse -Force
robocopy $finalPath $deployPath /E /R:2 /W:2
if ($LASTEXITCODE -ge 8)
{
    robocopy $backupPath $deployPath /E /R:2 /W:2
    if ($LASTEXITCODE -ge 8)
    {
        throw "Deployment failed during rollback, Manual action required. Copy failed from Backup folder to Deployment folder. Robocopy exit code: $LASTEXITCODE"
    }
    throw "Deployment copy failed:After final folder to deploy folder. Robocopy exit code: $LASTEXITCODE"
}

Write-Host "Deployment completed successfully!!"
