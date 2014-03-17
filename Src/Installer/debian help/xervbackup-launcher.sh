#!/bin/bash
export LD_LIBRARY_PATH="/usr/lib/XervBackup${LD_LIBRARY_PATH:+:$LD_LIBRARY_PATH}"
export MONO_PATH=$MONO_PATH:/usr/lib/XervBackup

EXE_FILE=/usr/lib/XervBackup/XervBackup.exe
APP_NAME=XervBackup

exec -a "$APP_NAME" mono "$EXE_FILE" "$@"
