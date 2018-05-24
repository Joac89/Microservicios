FROM java:openjdk-8-jre

ENV DEBIAN_FRONTEND noninteractive
ENV SCALA_VERSION 2.11
ENV KAFKA_VERSION 0.10.1.0
ENV KAFKA_HOME /opt/kafka_"$SCALA_VERSION"-"$KAFKA_VERSION"

# Install Kafka, Zookeeper and other needed things
RUN apt-get update && \
    apt-get install wget \
    apt-get install -y zookeeper wget supervisor dnsutils && \
    rm -rf /var/lib/apt/lists/* && \
    apt-get clean && \
    wget -q http://apache.mirrors.spacedump.net/kafka/"$KAFKA_VERSION"/kafka_"$SCALA_VERSION"-"$KAFKA_VERSION".tgz -O /tmp/kafka_"$SCALA_VERSION"-"$KAFKA_VERSION".tgz && \
    tar xfz /tmp/kafka_"$SCALA_VERSION"-"$KAFKA_VERSION".tgz -C /opt && \
    rm /tmp/kafka_"$SCALA_VERSION"-"$KAFKA_VERSION".tgz

ADD scripts/start-kafka.sh /usr/bin/start-kafka.sh

# Supervisor config
ADD supervisor/kafka.conf supervisor/zookeeper.conf /etc/supervisor/conf.d/

# 2181 is zookeeper, 9092 is kafka
EXPOSE 2181 9092

CMD ["supervisord", "-n"]






#FROM openjdk:8u151-jre-alpine
#COPY kafka-service-start.sh /opt/kafka/kafka-service-start.sh
#COPY server.properties /opt/kafka/config/server-2.properties
#COPY server-1.properties /opt/kafka/config/server-1.properties
#CMD ["/opt/kafka/kafka-service-start.sh"]

#FROM openjdk:8u151-jre-alpine
#ARG kafka_version=1.1.0
#ARG scala_version=2.12
#ARG glibc_version=2.27-r0
#ENV KAFKA_VERSION=$kafka_version \
#    SCALA_VERSION=$scala_version \
#    KAFKA_HOME=/opt/kafka \
#    GLIBC_VERSION=$glibc_version

#ENV PATH=${PATH}:${KAFKA_HOME}/bin

#COPY download-kafka.sh start-kafka.sh broker-list.sh create-topics.sh /tmp/

#RUN apk add --no-cache bash curl jq docker \
# && mkdir /opt \
# && chmod a+x /tmp/*.sh \
# && mv /tmp/start-kafka.sh /tmp/broker-list.sh /tmp/create-topics.sh /usr/bin \
# && sync && /tmp/download-kafka.sh \
# && tar xfz /tmp/kafka_${SCALA_VERSION}-${KAFKA_VERSION}.tgz -C /opt \
# && rm /tmp/kafka_${SCALA_VERSION}-${KAFKA_VERSION}.tgz \
# && ln -s /opt/kafka_${SCALA_VERSION}-${KAFKA_VERSION} /opt/kafka \
# && rm /tmp/* \
# && wget https://github.com/sgerrand/alpine-pkg-glibc/releases/download/${GLIBC_VERSION}/glibc-${GLIBC_VERSION}.apk \
# && apk add --no-cache --allow-untrusted glibc-${GLIBC_VERSION}.apk \
# && rm glibc-${GLIBC_VERSION}.apk

#COPY overrides /opt/overrides

#VOLUME ["/kafka"]

## Use "exec" form so that it runs as PID 1 (useful for graceful shutdown)
#CMD ["start-kafka.sh"]