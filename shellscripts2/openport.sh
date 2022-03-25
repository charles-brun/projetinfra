#!/bin/bash

open_port=$1

if [ $open_port -gt 0 ]
then
	aws ec2 authorize-security-group-ingress --region eu-west-3 --group-id sg-0db0ddb8f24ef1b6e --protocol udp --port $open_port --cidr 0.0.0.0/0 2>/dev/null
	if [ $? -eq 0 ]
	then
		echo "Port $open_port ouvert avec succès."
	else
		echo "Erreur inconnue lors de la tentative d'ouverture du port $open_port."
	fi
else
	echo "Numéro de port invalide."
fi
