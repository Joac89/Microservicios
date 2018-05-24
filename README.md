El taller está orientado a la implementación de una arquitectura de microservicios por eventos. Los archivos Dockerfile y Dockercompose del proyecto se encuentran en el repositorio Github.

El objetivo del ejercicio es permitir que una empresa de pagos, pueda agregar y retirar convenios de pagos sin afectar la funcionalidad del cliente y sin tener que reiniciar el sistema.

## Concepto
- El concepto de arquitectura de la solución actual, es una arquitectura Inventory Enterprise. A futuro, podría cambiar a Inventory Domain pues si el modelo de negocios se expande, tendríamos diferentes dominios dentro de la empresa y cada uno podría tener su inventario de servicios, y tambien un inventario de servicios empresarial.

## Arquitectura
En la arquitectura del proyecto se hace uso de los patrones Inventory Enterprise (inicialmente), Service Contract y Publisher an Subscriber principalmente
* [Patrones SOA](http://soapatterns.org/design_patterns/enterprise_inventory)

### Servicios implementados
1. Servicio UserInterface
2. Servicio Routing
3. Dispatcher
4. Apache kafka (Producers & Consumers)

### Flujo
Los clientes consumen el servicio **UserInterface**, que expone los métodos para consultar el valor de una factura, el pago y la compensación de la misma. Éste servicio, notifica a **kafka** la nueva solicitud de consulta, pago o compensación de factura para que **Dispatcher** tome la solicitud, consulte en el servicio **Routing** a quien corresponde, permita la **transformación** de datos para envío y recibo y consuma los servicios externos de los diferentes **convenios** como: Agua, Luz, Teléfono entre otros.

#### Transformación
La transformación de datos de envío y recibo, se implementa por medio de esquemas JSON y por XML string para el remplazo de variables sobre templates.
* [JSON Schema](http://json-schema.org/)

### Tecnología
* [Aspnet Core dotnet](https://www.c-sharpcorner.com/article/creating-web-api-with-asp-net-core-using-visual-studio-code/)
* [Visual Studio Code](https://code.visualstudio.com/download)
* [JUST for .Net Core 2](https://www.nuget.org/packages/JUST.NETCore/)
* [Apache kafka](https://kafka.apache.org/)

## Implementación
### Github
Descargue los proyectos localizados en el repositorio, y abralos con **visual studio code**. Una vez abiertos, debe iniciar kafka para poder enviar mensajes entre los servicios.

### Puesta en marcha
#### Iniciar kafka
Para configurar kafka, siga los siguientes manuales que le permitirán instalarlo y tenerlo listo para el uso en la implementación
* [How to install kafka on ubuntu](https://hevodata.com/blog/how-to-install-kafka-on-ubuntu/)
* [Apache kafka multiple brokers](http://www.benniehaelen.com/big-data/apache-kafka-part-iii-multiple-brokers/)
* [Apache kafka quickstart](https://kafka.apache.org/quickstart)

Luego de instalar y configurar kafka, se siguen los siguientes pasos:

* Copiar los archivos **server.properties** y **server-1.properties**
Los archivos se encuentran en el repositorio, estos archivos están configurados para los dos nodos que se usarán en kafka. El nodo 1 va por el puerto **9092** y el nodo 2 por el puerto **9093**

la configuración de los archivos se puede encontrar en el siguiente manual online: [Apache kafka multiple brokers](http://www.benniehaelen.com/big-data/apache-kafka-part-iii-multiple-brokers/)

* Se deben ejecutar los siguientes comandos del archivo [kafka-service-start.sh](https://github.com/Joac89/Microservicios/blob/master/kafka-config/kafka-service-start.sh) ubicado en el repositorio. Puede ejecutar los comandos paso a paso en una terminal de ubuntu, o puede ejecutar el archivo (.sh) desde una terminal (ejecuta todos los comandos automáticamente).

Ejecutar el archivo (.sh)
```
$ /path_file/kafka-service-start.sh
```
donde **path_file** es la ruta donde se guarda el archivo de comandos.

Si desea ejecutarlos paso a paso, escriba los siguientes comandos:

Nodos:
```
$ sudo /path_kafka_installation/bin/kafka-server-start.sh /opt/kafka/config/server.properties 
$ sudo /path_kafka_installation/bin/kafka-server-start.sh /opt/kafka/config/server-1.properties 
```
Productores:
```
$ sudo /path_kafka_installation/bin/kafka-console-producer.sh --broker-list localhost:9092 --topic broker-replicated 
$ sudo /path_kafka_installation/bin/kafka-console-producer.sh --broker-list localhost:9093 --topic broker-replicated2 
```
Consumidores:
```
$ sudo /path_kafka_installation/bin/kafka-console-consumer.sh --zookeeper localhost:2181 --from-beginning --topic broker-replicated 
$ sudo /path_kafka_installation/bin/kafka-console-consumer.sh --zookeeper localhost:2181 --from-beginning --topic broker-replicated2
```
donde **path_kafka_installation** es la ruta donde se instaló el apache kafka. En éste caso, se instaló en **/opt/kafka/**.
Recuerde ejecutar los comandos en diferentes consolas para poder detallar el funcionamientos de los consumidores y productores de kafka.

### Productores y consumidores
En kafka, los productores y consumidores, se utilizan para la escucha de las solicitudes enviadas desde el servicio de negocio, a los servicios que son orquestados para dar una respuesta del consumo de los servicios de convenios.

#### Uso
El servicio de negocio **UserInterface**, sirve de productor kafka para enviar los mensajes a la cola de eventos y sirve de consumidor a la respuesta a los eventos generados. **Dispatcher** sirve de productor para enviar las respuestas recibidas y transformadas desde los servicios de convenios, y como consumidor para detectar los eventos generados desde el cliente. En **Dispatcher**, tambien se realiza la transformación de datos y el enrutamiento al servicio de convenio específico a utilizar.

### Ejecución
Luego de descargar los proyectos desde el repositorio, se abren con Visual Studio Code y se ejecutan para poner en marcha el objetivo del taller.

Se ejecutan:
* Apache kafka (comandos para lanzar nodos, productores y consumidores)
* Servicios de AspnetCore (UserInterface, RoutingDeal, Dispatcher)

#### Visual Studio Code
Luego de clonar el repositorio desde github, Visual Studio Code pedirá restaurar paquetes nuget para cada proyecto, si ésto no sucede, se procede a ejecutar el comando **dotnet restore** desde la carpeta donde se encuentra el proyecto clonado
```
$ /path_proyecto/dotnet restore
```

### Transformación de datos en Dispatcher
- Para la transformación de datos, en Dispatcher se cuenta con plantillas JSON, que permitirán transformar los datos de respuesta de los diferentes servicios de convenios en un esquema estandar. Para ésto, consultar el manual **JUST for .Net Core 2** presentado anteriormente. JUST funciona de manera similar a las plantillas XSLT pero orientado a JSON exclusivamente.

### Ventajas de la solución
El taller expone las siguientes ventajas para el proceso de negocio propuesto
* Agregar un nuevo servicio de convenios a la arquitectura sin detener el sistema
* Utilizar herramientas de transformación de datos para estandarizar las respuestas por medio de plantillas
* Enrutador que permite transparencia entre servicios.
* Permite interactuar con servicios REST y SOAP (bajo parámetros de configuración establecidos)
* Servicios separados que pueden agregarse en contenedores

### Desventajas
* Costo de rendimiento en la transformación de datos de envío y recibo.

### Docker
La solución cuenta con los archivos **Dockerfile** y **Dockercompose*** que permiten agregar en contenedores los servicios utilizados

#### Advertencia!
Pendiente de implementar la comunicación entre los contenedores de los servicios y **Kafka** local. Se presentaron inconvenientes al momento de la comunicación entre los contenedores y el host (máquina local) donde se estaba ejecutando el kafka.

### Conclusión
Podriamos concluir, que la solución es parte de los primeros pasos para la construcción de sistemas orientados a microservicios, flexibles y próximos a ser más frecuentes en el cámpo tecnológico de hoy en día

