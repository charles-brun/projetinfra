#!/bin/bash
<<<<<<< HEAD

#activer le firewall
sudo ufw allow 22 2>/dev/null
sudo ufw "enable" 2>/dev/null

#déplacer les builds dans USER
if [ -e ../builds/server/Game ]
=======
sudo "mv" ../builds/server/Game/ ~
if [ $? -ne 0 ]
>>>>>>> aaf61fbbd58e6b4696a1d61f0d6d8baf169b7e4a
then
	sudo "mv" ../builds/server/Game/ ~
fi
<<<<<<< HEAD
=======
sudo "mv" ../builds/server/MM/ ~
>>>>>>> aaf61fbbd58e6b4696a1d61f0d6d8baf169b7e4a
if [ $? -ne 0 ]
then
	exit 1
fi
<<<<<<< HEAD
if [ -e ../builds/server/MM ]
=======
sudo "cp" ../services/gameinstance@.service /etc/systemd/system/gameinstance@.service
if [ $? -ne 0 ]
>>>>>>> aaf61fbbd58e6b4696a1d61f0d6d8baf169b7e4a
then
	sudo "mv" ../builds/server/MM/ ~
fi
<<<<<<< HEAD
=======
sudo "cp" ../services/matchmaking.service /etc/systemd/system/matchmaking.service
>>>>>>> aaf61fbbd58e6b4696a1d61f0d6d8baf169b7e4a
if [ $? -ne 0 ]
then
	exit 1
fi
<<<<<<< HEAD


#ajouter le nom du USER dans les services
#déplacer les services dans system
if [ -e ../services/gameinstance@.service ]
=======
sudo systemctl "enable" gameinstance@.service
if [ $? -ne 0 ]
>>>>>>> aaf61fbbd58e6b4696a1d61f0d6d8baf169b7e4a
then
	sudo python3 change_user.py gameinstance@.service $USER
	sudo "mv" ../services/gameinstance@.service /etc/systemd/system/gameinstance@.service
fi
<<<<<<< HEAD
=======
sudo systemctl "enable" matchmaking.service
>>>>>>> aaf61fbbd58e6b4696a1d61f0d6d8baf169b7e4a
if [ $? -ne 0 ]
then
	exit 1
fi
<<<<<<< HEAD
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
=======
sudo sytemctl daemon-reload
if [ $? -ne 0 ]
then
        exit 1
>>>>>>> aaf61fbbd58e6b4696a1d61f0d6d8baf169b7e4a
fi
