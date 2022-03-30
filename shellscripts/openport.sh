#!/bin/bash

open_port=$1

if [ $open_port -gt 0 ]
then
	sudo ufw allow $open_port
	if [ $? -eq 0 ]
	then
		echo "Port $open_port ouvert avec succès."
	else
		echo "Erreur inconnue lors de la tentative d'ouverture du port $open_port."
	fi
else
	echo "Numéro de port invalide."
fi
