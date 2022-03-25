#!/bin/bash

dir="/home/ubuntu/.config/unity3d/DefaultCompany/Snake D. Arena"

first_port=65000
last_port=0

if [ -d "$dir" ]
then
        cd "$dir"
        for f in *
        do
                if [ "${f#*.}" = state ]
                then
                        nb_port=${f%_*}
                        if [ $nb_port -lt $first_port ]
                        then
                              first_port=$nb_port
                        fi

                        if [ $nb_port -gt $last_port ]
                        then
                                last_port=$nb_port
                        fi
                fi
        done
fi

if [ $last_port -eq 0 ]
then
        echo "Aucune instance en cours."
else

        for (( port=$first_port; port<=$last_port; port++ ))
        do
                echo "Statut du port $port :"
                sudo systemctl "status" gameinstance@$port
        done
fi