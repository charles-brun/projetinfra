#!/bin/bash

configDir=/home/ubuntu/.config/unity3d

#Suppression du dossier config
if [ -d $configDir ]
then
	rm -r $configDir 2>/dev/null
	if [ $? -eq 0 ]
	then
		echo "$configDir a bien été supprimé."
	else
		echo "Problème inconnu lors de la suppression de $configDir."
	fi
else
	echo "Dossier \"$configDir\" introuvable."
fi

#Fermeture des instances
kill $(ps aux | grep "/home/ubuntu/Game/Game.x86_64" | grep -v grep | awk '{print $2}') 2>/dev/null
if [ $? -eq 0 ]
then
	echo "Toutes les instances ont été fermées."
else
	echo "Problème inconnu lors de la fermeture des instances."
fi
