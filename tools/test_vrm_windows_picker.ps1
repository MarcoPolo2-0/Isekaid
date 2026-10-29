param(
    [Parameter(Mandatory=$true)][int]$PlayerId,
    [Parameter(Mandatory=$true)][string]$AvatarPath,
    [int]$TimeoutSeconds = 120
)
$ErrorActionPreference = 'Stop'
Add-Type -AssemblyName System.Windows.Forms
if (-not [IO.Path]::IsPathRooted($AvatarPath)) { throw 'An absolute local fixture path is required.' }
Add-Type @"
using System;
using System.Runtime.InteropServices;
using System.Text;
public static class AvatarPickerProbe
{
    public delegate bool EnumWindow(IntPtr window, IntPtr data);
    [DllImport("user32.dll")] public static extern bool EnumWindows(EnumWindow callback, IntPtr data);
    [DllImport("user32.dll")] public static extern uint GetWindowThreadProcessId(IntPtr window, out uint process);
    [DllImport("user32.dll", CharSet=CharSet.Unicode)] public static extern int GetWindowText(IntPtr window, StringBuilder text, int length);
    [DllImport("user32.dll")] public static extern bool PostMessage(IntPtr window, int message, IntPtr parameter, IntPtr data);
    [DllImport("user32.dll")] public static extern bool SetForegroundWindow(IntPtr window);
    [DllImport("user32.dll")] public static extern IntPtr GetForegroundWindow();
    [DllImport("user32.dll")] public static extern IntPtr GetDlgItem(IntPtr window, int identifier);
    public static IntPtr Find(int process)
    {
        IntPtr found = IntPtr.Zero;
        EnumWindows((window, data) => {
            uint owner; GetWindowThreadProcessId(window, out owner);
            if (owner != process) return true;
            var text = new StringBuilder(256); GetWindowText(window, text, text.Capacity);
            if (text.ToString().StartsWith("Seleccionar apariencia local", StringComparison.Ordinal) && GetDlgItem(window, 0x47c) != IntPtr.Zero) { found = window; return false; }
            return true;
        }, IntPtr.Zero);
        return found;
    }
}
"@
$timer = [Diagnostics.Stopwatch]::StartNew()
function Wait-Dialog {
    do {
        if ($timer.Elapsed.TotalSeconds -gt $TimeoutSeconds) { throw 'Timed out waiting for the owned file dialog.' }
        if (-not (Get-Process -Id $PlayerId -ErrorAction SilentlyContinue)) { throw 'The test player exited before the file picker checks completed.' }
        $window = [AvatarPickerProbe]::Find($PlayerId)
        if ($window -ne [IntPtr]::Zero) { return $window }
        Start-Sleep -Milliseconds 100
    } while ($true)
}
$first = Wait-Dialog
if (-not [AvatarPickerProbe]::SetForegroundWindow($first) -or [AvatarPickerProbe]::GetForegroundWindow() -ne $first) { throw 'The owned dialog did not receive focus.' }
[System.Windows.Forms.SendKeys]::SendWait('%n')
[System.Windows.Forms.SendKeys]::SendWait('^a')
[System.Windows.Forms.SendKeys]::SendWait([IO.Path]::GetFileName($AvatarPath))
[System.Windows.Forms.SendKeys]::SendWait('{ENTER}')
$marker = Join-Path ([IO.Path]::GetDirectoryName($AvatarPath)) 'picker-selected.ok'
while (-not (Test-Path -LiteralPath $marker)) {
    if ($timer.Elapsed.TotalSeconds -gt $TimeoutSeconds) { throw 'The test player did not confirm the selected path.' }
    if (-not (Get-Process -Id $PlayerId -ErrorAction SilentlyContinue)) { throw 'The test player rejected the selected path.' }
    Start-Sleep -Milliseconds 100
}
$second = Wait-Dialog
[void][AvatarPickerProbe]::PostMessage($second, 0x0111, [IntPtr]2, [IntPtr]::Zero)
'{"dialogsHandled":2,"target":"owned Windows test player only"}'
