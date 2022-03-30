#!/bin/bash

dir="/home/ubuntu/.config/unity3d/DefaultCompany/Snake D. Arena"

#Launch X instances
if [ $1 = "--launch" ] || [ $1 = "-l" ]
then
	if [ $# -eq 2 ]
	then
		nb_instances=$2
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
	else
		echo "Vous devez préciser le nombre d'instances à ouvrir."
		exit 0	
	fi
fi
	
#Close all instances
if [ $1 = "--close" ] || [ $1 = "-c" ]
then
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
        	sudo systemctl "stop" gameinstance@$port
                	echo "Instance au port $port stoppée."
                	sleep 0.5
        	done
        	rm -r "$dir"
	fi
fi

#Check all instances status
if [ $1 = "--status" ] || [ $1 = "-s" ]
then
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
fi
