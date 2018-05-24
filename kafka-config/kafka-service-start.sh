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
path_data="/var/lib/kafka"
server1="server"
server2="server-1"
topic1="broker-replicated"
topic2="broker-replicated2"
port="2181"
port1="9092"
port2="9093"
echo "clean kafka data"
#sudo rm -rf ${path_data}/data
#sudo mkdir ${path_data}/data
echo "start kafka nodes & producers/consumers"
sudo ${path_kafka}/bin/kafka-server-start.sh ${path_kafka}/config/${server1}.properties &
sudo ${path_kafka}/bin/kafka-server-start.sh ${path_kafka}/config/${server2}.properties &
sudo ${path_kafka}/bin/kafka-console-producer.sh --broker-list localhost:${port1} --topic ${topic1} &
sudo ${path_kafka}/bin/kafka-console-producer.sh --broker-list localhost:${port2} --topic ${topic2} &
sudo ${path_kafka}/bin/kafka-console-consumer.sh --zookeeper localhost:${port} --from-beginning --topic ${topic1} &
sudo ${path_kafka}/bin/kafka-console-consumer.sh --zookeeper localhost:${port} --from-beginning --topic ${topic2}

#sudo /opt/kafka/bin/kafka-server-start.sh /opt/kafka/config/server.properties &
#sudo /opt/kafka/bin/kafka-server-start.sh /opt/kafka/config/server-1.properties &
#sudo /opt/kafka/bin/kafka-console-producer.sh --broker-list localhost:9092 --topic broker-replicated &
#sudo /opt/kafka/bin/kafka-console-producer.sh --broker-list localhost:9093 --topic broker-replicated2 &
#sudo /opt/kafka/bin/kafka-console-consumer.sh --zookeeper localhost:2181 --from-beginning --topic broker-replicated &
#sudo /opt/kafka/bin/kafka-console-consumer.sh --zookeeper localhost:2181 --from-beginning --topic broker-replicated2

