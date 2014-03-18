#!/bin/bash
export LD_LIBRARY_PATH="/usr/lib/XervBackup${LD_LIBRARY_PATH:+:$LD_LIBRARY_PATH}"
export MONO_PATH=$MONO_PATH:/usr/lib/XervBackup

EXE_FILE=/usr/lib/XervBackup/XervBackup.CommandLine.exe
APP_NAME=XervBackup.CommandLine

exec -a "$APP_NAME" mono "$EXE_FILE" "$@"
