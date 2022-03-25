#!/bin/bash

dir="/home/ubuntu/.config/unity3d/DefaultCompany/Snake D. Arena"

nb_instances=$1
first_port=7778

if [ -d "$dir" ]
then
	cd "$dir"
	for f in *
	do
		if [ "${f#*.}" = state ]
		then
			nb_port=${f%_*}
			if [ $nb_port -gt $first_port ]
			then
				first_port=$(($nb_port+1))
			fi
		fi

	done
fi

last_port=$(($first_port+$nb_instances))

for (( port=$first_port; port<$last_port; port++ ))
do
	sudo systemctl "start" gameinstance@$port
	echo "Instance au port $port démarrée."
	sleep 0.5
done
