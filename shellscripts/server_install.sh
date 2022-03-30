#!/bin/bash

#activer le firewall
sudo ufw allow 22 2>/dev/null
sudo ufw "enable" 2>/dev/null

#déplacer les builds dans USER
if [ -e ../builds/server/Game ]
then
	sudo "mv" ../builds/server/Game/ ~
fi
if [ $? -ne 0 ]
then
	exit 1
fi
if [ -e ../builds/server/MM ]
then
	sudo "mv" ../builds/server/MM/ ~
fi
if [ $? -ne 0 ]
then
	exit 1
fi


#ajouter le nom du USER dans les services
#déplacer les services dans system
if [ -e ../services/gameinstance@.service ]
then
	sudo python3 change_user.py gameinstance@.service $USER
	sudo "mv" ../services/gameinstance@.service /etc/systemd/system/gameinstance@.service
fi
if [ $? -ne 0 ]
then
	exit 1
fi
if [ -e ../services/matchmaking.service ]
then
	sudo python3 change_user.py matchmaking.service $USER
	sudo "mv" ../services/matchmaking.service /etc/systemd/system/matchmaking.service
fi
if [ $? -ne 0 ]
then
	exit 1
fi

#activer les services
sudo systemctl "enable" /etc/systemd/system/gameinstance@.service
if [ $? -ne 0 ]
then
	exit 1
fi
sudo systemctl "enable" /etc/systemd/system/matchmaking.service
if [ $? -ne 0 ]
then
	exit 1
fi
sudo systemctl daemon-reload
if [ $? -ne 0 ]
then
	exit 1
fi

mv ../shellscripts ~
