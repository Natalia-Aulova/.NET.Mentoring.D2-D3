var powerManagementService = new ActiveXObject("UnmanagedCode.PowerManagement.PowerManagementService");

var args = WScript.Arguments;

if (!args.length) {
    WScript.Echo("The parameter responsible for reservation of hibernation file is missing.");
    WScript.Quit(1);
}

var reserveHiberFile;

switch (args(0).toLowerCase()) {
    case "true":
        reserveHiberFile = true;
        break;
    case "false":
        reserveHiberFile = false;
        break;
    default:
        WScript.Echo("The parameter should be true or false.");
        WScript.Quit(1);
}

var success = powerManagementService.ReserveHiberFile(reserveHiberFile);

if (success) {
    var action = reserveHiberFile ? "reserved." : "removed.";
    WScript.Echo("Hibernation file has been " + action);
} else {
    WScript.Echo("An error occurred.");
}
