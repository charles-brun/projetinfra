#!/bin/bash

chmod +x ~/Game/Game.x86_64
~/Game/Game.x86_64 -batchmode -nographics

if [ $? -eq 0 ]
then
	echo "Lancement du serveur réussi."
else
	echo "Erreur inconnue lors de l'exécution."
fi
