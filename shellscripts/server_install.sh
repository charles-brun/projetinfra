#!/bin/bash
sudo "mv" -r ../builds/server/Game/ ~
if [ $? -eq 0 ]
then
        exit 1
fi
sudo "mv" -r ../builds/server/MM/ ~
if [ $? -eq 0 ]
then
        exit 1
fi
sudo "cp" ../services/gameinstance@.service /etc/systemd/system/gameinstance@.service
if [ $? -eq 0 ]
then
        exit 1
fi
sudo "cp" ../services/matchmaking.service /etc/systemd/system/matchmaking.service
if [ $? -eq 0 ]
then
        exit 1
fi
sudo systemctl enable /etc/systemd/system/gameinstance@.service
if [ $? -eq 0 ]
then
        exit 1
fi
sudo systemctl enable /etc/systemd/system/matchmaking.service
if [ $? -eq 0 ]
then
        exit 1
fi
sudo sytemctl daemon-reload
if [ $? -eq 0 ]
then
        exit 1
fi