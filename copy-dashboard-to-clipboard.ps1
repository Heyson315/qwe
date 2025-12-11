# SharePoint Dashboard - Copy to Clipboard Script
# This script copies the entire dashboard HTML to your clipboard
# Then you can paste it directly into SharePoint's Embed web part

Write-Host "================================================" -ForegroundColor Cyan
Write-Host "  SharePoint Dashboard - Copy to Clipboard" -ForegroundColor Cyan
Write-Host "================================================" -ForegroundColor Cyan
Write-Host ""

# Path to the dashboard file
$dashboardPath = "sharepoint-dashboard.html"

# Check if file exists
if (-not (Test-Path $dashboardPath)) {
    Write-Host "Error: Dashboard file not found!" -ForegroundColor Red
    Write-Host "Looking for: $dashboardPath" -ForegroundColor Yellow
    Write-Host ""
    Write-Host "Make sure you're in the correct directory:" -ForegroundColor Yellow
    Write-Host "E:\source\Heyson315\DjangoWebProject1\Heyson315\qwe\" -ForegroundColor White
    exit 1
}

Write-Host "Reading dashboard file..." -ForegroundColor Yellow

# Read the HTML content
$htmlContent = Get-Content -Path $dashboardPath -Raw

Write-Host "Copying to clipboard..." -ForegroundColor Yellow

# Copy to clipboard
Set-Clipboard -Value $htmlContent

Write-Host ""
Write-Host "✓ SUCCESS! Dashboard HTML copied to clipboard!" -ForegroundColor Green
Write-Host ""
Write-Host "================================================" -ForegroundColor Cyan
Write-Host "  Next Steps:" -ForegroundColor Cyan
Write-Host "================================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "1. Go to SharePoint:" -ForegroundColor White
Write-Host "   https://rahmanfinanceandaccounting.sharepoint.com/" -ForegroundColor Gray
Write-Host ""
Write-Host "2. Create a new page:" -ForegroundColor White
Write-Host "   - Click 'New' → 'Page'" -ForegroundColor Gray
Write-Host "   - Choose 'Blank' template" -ForegroundColor Gray
Write-Host "   - Name it 'Project Dashboard'" -ForegroundColor Gray
Write-Host ""
Write-Host "3. Add Embed web part:" -ForegroundColor White
Write-Host "   - Click '+' icon" -ForegroundColor Gray
Write-Host "   - Search for 'Embed'" -ForegroundColor Gray
Write-Host "   - Select 'Embed' web part" -ForegroundColor Gray
Write-Host ""
Write-Host "4. Paste the HTML:" -ForegroundColor White
Write-Host "   - Press Ctrl+V in the embed code box" -ForegroundColor Gray
Write-Host "   - The entire dashboard HTML is now pasted!" -ForegroundColor Gray
Write-Host ""
Write-Host "5. Publish:" -ForegroundColor White
Write-Host "   - Click outside the embed box" -ForegroundColor Gray
Write-Host "   - Click 'Publish' (top right)" -ForegroundColor Gray
Write-Host ""
Write-Host "================================================" -ForegroundColor Cyan
Write-Host "✓ Dashboard HTML is in your clipboard!" -ForegroundColor Green
Write-Host "✓ Ready to paste into SharePoint!" -ForegroundColor Green
Write-Host "================================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "Press any key to exit..." -ForegroundColor Gray
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
