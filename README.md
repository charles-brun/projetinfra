# SNAKE D. ARENA - Serveur

Cette solution permet d'implémenter un mode multijoueur en ligne pour le jeu Snake D. Arena. Il s'agit d'un système de matchmaking, redirigeant les clients vers des instances de deux joueurs.

## Mise en place de la solution

### Prérequis système
- 1 machine serveur
- OS ubuntu
- Processeur x86_64
- RAM 1Go
- Stockage 1Go

### Avant l'installation
- UFW installé
- Python 3 installé
- Ports 7777 à 7777+n (pour n instances) ouverts côté routeur

*Si vous souhaitez gérer les ports du routeur de façon dynamique, vous pouvez ajouter les règles de son firewall aux scripts /shellscripts/openport.sh et /shellscripts/closeport.sh.
La règle devra être renseignée à la suite de la commande UFW, et prendra la variable $1 comme numéro de port.*

### Installation
- git clone https://github.com/charles-brun/projetinfra
- cd /projetinfra/builds/shellscripts
- chmod +x *
- ./server_install.sh

### Build client

L'adresse IP des serveurs est renseignée dans les builds clients. Si un nouveau serveur est créé, une mise à jour doit être effectuée chez les clients pour implémenter son adresse IP.

## Utilisation de la solution

### Utilisation du serveur
#### Gestion du matchmaking : 
	>sudo systemctl <CMD> matchmaking.service
avec <CMD> :
	"start"	Lancer le matchmaking
	"status"	Consulter l'état du matchmaking
	"stop"	Arrêter le matchmaking

#### Gestion des instances : 
	>$HOME/shellscripts/server_instances.sh <ARG>
avec <ARG> :
	"--launch <N>" ou "-l <N>"	Lancer <N> instances
	"--close" ou "-c"			Fermer toutes les instances
	"--status" ou "-s			Consulter l'état des instances

### Utilisation du client

- Lancer l'exécutable
- Cliquer sur "Jouer"

## Schéma du fonctionnement du matchmaking

## Monitoring

## Backup

