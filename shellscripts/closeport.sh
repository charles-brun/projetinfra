#!/bin/bash

if [ $1 -gt 0 ]
then
	aws ec2 revoke-security-group-ingress --region eu-west-3 --group-id sg-0db0ddb8f24ef1b6e --protocol udp --port $1 --cidr 0.0.0.0/0 2>/dev/null
	if [ $? -eq 0 ]
	then
		echo "Port $1 fermé avec succès."
	else
		echo "Erreur lors de la tentative de fermeture du port $1."
	fi	
else
	echo "Numéro de port invalide."
fi
