#!/bin/bash
sudo "mv" ../builds/server/Game/ ~
if [ $? -ne 0 ]
then
        exit 1
fi
sudo "mv" ../builds/server/MM/ ~
if [ $? -ne 0 ]
then
        exit 1
fi
sudo "cp" ../services/gameinstance@.service /etc/systemd/system/gameinstance@.service
if [ $? -ne 0 ]
then
        exit 1
fi
sudo "cp" ../services/matchmaking.service /etc/systemd/system/matchmaking.service
if [ $? -ne 0 ]
then
        exit 1
fi
sudo systemctl "enable" gameinstance@.service
if [ $? -ne 0 ]
then
        exit 1
fi
sudo systemctl "enable" matchmaking.service
if [ $? -ne 0 ]
then
        exit 1
fi
sudo sytemctl daemon-reload
if [ $? -ne 0 ]
then
        exit 1
fi
