#!/bin/bash

if [ $1 -gt 0 ]
then
	sudo ufw deny $1
	if [ $? -eq 0 ]
	then
		echo "Port $1 fermé avec succès."
	else
		echo "Erreur lors de la tentative de fermeture du port $1."
	fi	
else
	echo "Numéro de port invalide."
fi
