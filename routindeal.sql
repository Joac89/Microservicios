-- MySQL dump 10.13  Distrib 5.7.22, for Linux (x86_64)
--
-- Host: localhost    Database: routingdeal
-- ------------------------------------------------------
-- Server version	5.7.22-0ubuntu0.16.04.1

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `tbl_deal`
--

DROP TABLE IF EXISTS `tbl_deal`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `tbl_deal` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `name` varchar(50) CHARACTER SET utf8 DEFAULT NULL,
  `invoiceref` varchar(50) CHARACTER SET utf8 DEFAULT NULL,
  `invoicekey` varchar(50) CHARACTER SET utf8 DEFAULT NULL,
  `state` bit(1) DEFAULT NULL,
  `url` varchar(500) CHARACTER SET utf8 DEFAULT NULL,
  `template` varchar(500) CHARACTER SET utf8 DEFAULT NULL,
  `type` varchar(50) CHARACTER SET utf8 DEFAULT NULL,
  `requesttemplate` varchar(500) CHARACTER SET utf8 DEFAULT NULL,
  `numrequest` int(11) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tbl_deal`
--

LOCK TABLES `tbl_deal` WRITE;
/*!40000 ALTER TABLE `tbl_deal` DISABLE KEYS */;
INSERT INTO `tbl_deal` VALUES (1,'Servicio Agua - consultar','--','1001','','http://35.185.30.69:6060/servicios/pagos/v1/payments','JsonTemplates/json-1001.json','REST-GET','--',0),(3,'Servicio Luz - consultar','--','2001','','http://35.185.30.69:7070/w1-soap-svr/PagosServiceService#consultar','JsonTemplates/json-2001.json','SOAP-GET','SoapTemplates/request-2001.xml',1),(5,'Servicio Agua - pagar','--','1002','','http://35.185.30.69:6060/servicios/pagos/v1/payments','JsonTemplates/json-1002.json','REST-POST','JsonTemplates/request-1002.json',2),(6,'Servicio Luz - pagar','--','2002','','http://35.185.30.69:7070/w1-soap-svr/PagosServiceService#pagar','JsonTemplates/json-2002.json','SOAP-POST','SoapTemplates/request-2002.xml',2),(9,'Servicio Luz - compensar','--','2003','','http://35.185.30.69:7070/w1-soap-svr/PagosServiceService#compensar','JsonTemplates/json-2003.json','SOAP-POST','SoapTemplates/request-2003.xml',2),(10,'Servicio Agua - compensar','--','1003','','http://35.185.30.69:6060/servicios/pagos/v1/payments','JsonTemplates/json-1003.json','REST-DELETE','JsonTemplates/request-1003.json',2);
/*!40000 ALTER TABLE `tbl_deal` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2018-05-22 21:54:09
