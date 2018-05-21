#!/bin/bash
#########################################################################################
#	INICIO DE SERVICIO KAFKA CON PRODUCTORES Y CONSUMIDORES				                #
#										                                                #
#	Integrantes: 									                                    #
#		Jesus Aguilar								                                    #
#		Franklin Ruiz								                                    #
#		Luis Macea								                                        #
#########################################################################################
path_kafka="/opt/kafka"
server1="server"
server2="server-1"
topic1="broker-replicated"
topic2="broker-replicated2"
port="2181"
port1="9092"
port2="9093"
echo "start kafka nodes & producers/consumers"
sudo ${path_kafka}/bin/kafka-server-start.sh ${path_kafka}/config/${server1}.properties &
sudo ${path_kafka}/bin/kafka-server-start.sh ${path_kafka}/config/${server2}.properties &
sudo ${path_kafka}/bin/kafka-console-producer.sh --broker-list localhost:${port1} --topic ${topic1} &
sudo ${path_kafka}/bin/kafka-console-producer.sh --broker-list localhost:${port2} --topic ${topic2} &
sudo ${path_kafka}/bin/kafka-console-consumer.sh --zookeeper localhost:${port} --from-beginning --topic ${topic1} &
sudo ${path_kafka}/bin/kafka-console-consumer.sh --zookeeper localhost:${port} --from-beginning --topic ${topic2}

