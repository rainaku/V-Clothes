# Kill existing VClothes process
Get-Process -Name "VClothes" -ErrorAction SilentlyContinue | Stop-Process -Force
Start-Sleep -Milliseconds 500

# Build
dotnet build VClothes -c Debug
if ($LASTEXITCODE -ne 0) { Write-Host "BUILD FAILED" -ForegroundColor Red; exit 1 }

# Run
Start-Process "VClothes\bin\Debug\net8.0-windows\VClothes.exe"

#CHẠY ./R TRONG TERMINAL NHE
