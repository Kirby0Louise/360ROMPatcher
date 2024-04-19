# 360ROMPatcher

A dead simple ROM Patcher for Xbox 360 XEX/JTAG format games

## Usage:

`360ROMPatcher <PATCH FILE>.360Patch <ROM DIRECTORY>`

## 360Patch Format ##

Start file with 360PATCHERMAGIC

Blank lines and ONLY blank lines can be used freely to organize

[XDELTA PATCH]|[TARGET FILE RELATIVE TO ROM DIRECTORY] to specify a patch

End file with 360PATCHEREOF
