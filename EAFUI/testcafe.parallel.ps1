# Get list of commands

$testcmds = @(
    'Sidebar-Menu-Component',
    'Default Component',
    'Default Landing Page',
    'Data management',
    'Data Management Users'
)

function start-jobhere([scriptblock]$block) {
    Start-Job -Init ([ScriptBlock]::Create("Set-Location '$pwd'")) -Script $block
}

#run all jobs, using multi-threading, in background
ForEach($testcmd in $testcmds){
  start-jobhere ([ScriptBlock]::Create("testcafe -f `"$testcmd`""))
}

#Wait for all jobs
Get-Job | Wait-Job

#Get all job results
Get-Job | Receive-Job
