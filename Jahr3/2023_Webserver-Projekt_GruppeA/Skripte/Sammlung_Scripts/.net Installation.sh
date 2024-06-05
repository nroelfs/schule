#!/bin/bash

wget https://dot.net/v1/dotnet-install.sh -O dotnet-install.sh #Downlaod .Net
chmod +x ./dotnet-install.sh #Vergiebt die berechtigung um das Script zu starten
./dotnet-install.sh --channel 6.0 #Installiert die Version 6.0
./dotnet-install.sh --channel 6.0   --runtime aspnetcore
rm dotnet-install.sh #Entfernt das script