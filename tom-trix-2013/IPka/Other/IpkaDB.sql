-- --------------------------------------------------------
-- Host:                         localhost
-- Server version:               5.5.25a - MySQL Community Server (GPL)
-- Server OS:                    Win32
-- HeidiSQL version:             6.0.0.4024
-- Date/time:                    2012-07-18 18:58:34
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!40014 SET FOREIGN_KEY_CHECKS=0 */;

-- Dumping database structure for ipkadb
DROP DATABASE IF EXISTS `ipkadb`;
CREATE DATABASE IF NOT EXISTS `ipkadb` /*!40100 DEFAULT CHARACTER SET latin1 */;
USE `ipkadb`;


-- Dumping structure for table ipkadb.calc_d_events
DROP TABLE IF EXISTS `calc_d_events`;
CREATE TABLE IF NOT EXISTS `calc_d_events` (
  `pk` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `text` text NOT NULL,
  `days_before_deadline` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`pk`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- Dumping data for table ipkadb.calc_d_events: ~0 rows (approximately)
DELETE FROM `calc_d_events`;
/*!40000 ALTER TABLE `calc_d_events` DISABLE KEYS */;
/*!40000 ALTER TABLE `calc_d_events` ENABLE KEYS */;


-- Dumping structure for table ipkadb.calc_t_conjuncts
DROP TABLE IF EXISTS `calc_t_conjuncts`;
CREATE TABLE IF NOT EXISTS `calc_t_conjuncts` (
  `pk` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `ek_operand1` bigint(11) unsigned NOT NULL DEFAULT '0',
  `operation` char(2) NOT NULL DEFAULT '0',
  `ek_operand2` bigint(11) unsigned NOT NULL DEFAULT '0',
  `value` varchar(50) DEFAULT '0',
  `parameter` varchar(10) DEFAULT '0',
  PRIMARY KEY (`pk`),
  KEY `ek_operand1` (`ek_operand1`),
  KEY `ek_operand2` (`ek_operand2`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;

-- Dumping data for table ipkadb.calc_t_conjuncts: ~0 rows (approximately)
DELETE FROM `calc_t_conjuncts`;
/*!40000 ALTER TABLE `calc_t_conjuncts` DISABLE KEYS */;
/*!40000 ALTER TABLE `calc_t_conjuncts` ENABLE KEYS */;


-- Dumping structure for table ipkadb.calc_t_events
DROP TABLE IF EXISTS `calc_t_events`;
CREATE TABLE IF NOT EXISTS `calc_t_events` (
  `pk` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `ek_calc_main` bigint(11) unsigned NOT NULL DEFAULT '0',
  `ek_calc_events` bigint(11) unsigned NOT NULL DEFAULT '0',
  `value` int(11) NOT NULL DEFAULT '0',
  `is_done` bit(1) NOT NULL,
  PRIMARY KEY (`pk`),
  KEY `ek_calc_main` (`ek_calc_main`),
  KEY `ek_calc_events` (`ek_calc_events`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- Dumping data for table ipkadb.calc_t_events: ~0 rows (approximately)
DELETE FROM `calc_t_events`;
/*!40000 ALTER TABLE `calc_t_events` DISABLE KEYS */;
/*!40000 ALTER TABLE `calc_t_events` ENABLE KEYS */;


-- Dumping structure for table ipkadb.calc_t_main
DROP TABLE IF EXISTS `calc_t_main`;
CREATE TABLE IF NOT EXISTS `calc_t_main` (
  `pk` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `ek_main` bigint(11) unsigned NOT NULL DEFAULT '0',
  `start_date` date NOT NULL,
  `paydate` date NOT NULL,
  `year_number` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`pk`),
  KEY `ek_main` (`ek_main`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- Dumping data for table ipkadb.calc_t_main: ~0 rows (approximately)
DELETE FROM `calc_t_main`;
/*!40000 ALTER TABLE `calc_t_main` DISABLE KEYS */;
/*!40000 ALTER TABLE `calc_t_main` ENABLE KEYS */;


-- Dumping structure for table ipkadb.calc_t_operands
DROP TABLE IF EXISTS `calc_t_operands`;
CREATE TABLE IF NOT EXISTS `calc_t_operands` (
  `pk` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `text` varchar(30) NOT NULL,
  `sqltext` varchar(100) NOT NULL,
  `is_system` bit(1) NOT NULL,
  PRIMARY KEY (`pk`),
  UNIQUE KEY `text` (`text`),
  KEY `is_system` (`is_system`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- Dumping data for table ipkadb.calc_t_operands: ~0 rows (approximately)
DELETE FROM `calc_t_operands`;
/*!40000 ALTER TABLE `calc_t_operands` DISABLE KEYS */;
/*!40000 ALTER TABLE `calc_t_operands` ENABLE KEYS */;


-- Dumping structure for table ipkadb.calc_t_rules2conjuncts
DROP TABLE IF EXISTS `calc_t_rules2conjuncts`;
CREATE TABLE IF NOT EXISTS `calc_t_rules2conjuncts` (
  `pk` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `ek_calc_rules` bigint(20) unsigned NOT NULL DEFAULT '0',
  `ek_calc_conjuncts` bigint(20) unsigned NOT NULL DEFAULT '0',
  PRIMARY KEY (`pk`),
  KEY `ek_calc_rules` (`ek_calc_rules`),
  KEY `ek_calc_conjuncts` (`ek_calc_conjuncts`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- Dumping data for table ipkadb.calc_t_rules2conjuncts: ~0 rows (approximately)
DELETE FROM `calc_t_rules2conjuncts`;
/*!40000 ALTER TABLE `calc_t_rules2conjuncts` DISABLE KEYS */;
/*!40000 ALTER TABLE `calc_t_rules2conjuncts` ENABLE KEYS */;


-- Dumping structure for table ipkadb.clac_t_rules
DROP TABLE IF EXISTS `clac_t_rules`;
CREATE TABLE IF NOT EXISTS `clac_t_rules` (
  `pk` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `order_number` int(11) NOT NULL DEFAULT '0',
  `reason` varchar(90) NOT NULL DEFAULT '0',
  `caption` varchar(90) NOT NULL DEFAULT '0',
  `colour` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`pk`),
  UNIQUE KEY `order_number` (`order_number`),
  KEY `reason` (`reason`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- Dumping data for table ipkadb.clac_t_rules: ~0 rows (approximately)
DELETE FROM `clac_t_rules`;
/*!40000 ALTER TABLE `clac_t_rules` DISABLE KEYS */;
/*!40000 ALTER TABLE `clac_t_rules` ENABLE KEYS */;


-- Dumping structure for table ipkadb.d_correspondence
DROP TABLE IF EXISTS `d_correspondence`;
CREATE TABLE IF NOT EXISTS `d_correspondence` (
  `pk` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `corrname` varchar(50) NOT NULL,
  `corrshortname` varchar(20) NOT NULL,
  `is_incoming` bit(1) NOT NULL,
  `is_only` bit(1) NOT NULL,
  `template` varchar(50) NOT NULL,
  `months_deadline` int(10) unsigned DEFAULT NULL,
  `ik_deactivator` int(10) unsigned DEFAULT NULL,
  PRIMARY KEY (`pk`),
  UNIQUE KEY `corrname` (`corrname`),
  UNIQUE KEY `corrshortname` (`corrshortname`),
  KEY `is_incoming` (`is_incoming`),
  KEY `ik_deactivator` (`ik_deactivator`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- Dumping data for table ipkadb.d_correspondence: ~0 rows (approximately)
DELETE FROM `d_correspondence`;
/*!40000 ALTER TABLE `d_correspondence` DISABLE KEYS */;
/*!40000 ALTER TABLE `d_correspondence` ENABLE KEYS */;


-- Dumping structure for table ipkadb.d_icodes
DROP TABLE IF EXISTS `d_icodes`;
CREATE TABLE IF NOT EXISTS `d_icodes` (
  `pk` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `codename` varchar(50) NOT NULL DEFAULT '0',
  `codeshortname` varchar(8) NOT NULL DEFAULT '0',
  `usercode` varchar(2) NOT NULL DEFAULT '0',
  PRIMARY KEY (`pk`),
  UNIQUE KEY `codename` (`codename`),
  UNIQUE KEY `usercode` (`usercode`),
  KEY `codeshortname` (`codeshortname`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- Dumping data for table ipkadb.d_icodes: ~0 rows (approximately)
DELETE FROM `d_icodes`;
/*!40000 ALTER TABLE `d_icodes` DISABLE KEYS */;
/*!40000 ALTER TABLE `d_icodes` ENABLE KEYS */;


-- Dumping structure for table ipkadb.d_requisites
DROP TABLE IF EXISTS `d_requisites`;
CREATE TABLE IF NOT EXISTS `d_requisites` (
  `pk` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `requisname` varchar(50) NOT NULL DEFAULT '0',
  `is_legal` bit(1) NOT NULL,
  PRIMARY KEY (`pk`),
  UNIQUE KEY `requisname_is_legal` (`requisname`,`is_legal`),
  KEY `requisname` (`requisname`),
  KEY `is_legal` (`is_legal`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- Dumping data for table ipkadb.d_requisites: ~0 rows (approximately)
DELETE FROM `d_requisites`;
/*!40000 ALTER TABLE `d_requisites` DISABLE KEYS */;
/*!40000 ALTER TABLE `d_requisites` ENABLE KEYS */;


-- Dumping structure for table ipkadb.t_correspondence
DROP TABLE IF EXISTS `t_correspondence`;
CREATE TABLE IF NOT EXISTS `t_correspondence` (
  `pk` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `ek_main` bigint(20) unsigned NOT NULL DEFAULT '0',
  `ek_corr` bigint(20) unsigned NOT NULL DEFAULT '0',
  `is_sent` bit(1) NOT NULL,
  `value` date DEFAULT NULL,
  `comment` text,
  PRIMARY KEY (`pk`),
  KEY `ek_main` (`ek_main`),
  KEY `ek_corr` (`ek_corr`),
  KEY `value` (`value`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- Dumping data for table ipkadb.t_correspondence: ~0 rows (approximately)
DELETE FROM `t_correspondence`;
/*!40000 ALTER TABLE `t_correspondence` DISABLE KEYS */;
/*!40000 ALTER TABLE `t_correspondence` ENABLE KEYS */;


-- Dumping structure for table ipkadb.t_main
DROP TABLE IF EXISTS `t_main`;
CREATE TABLE IF NOT EXISTS `t_main` (
  `pk` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `project_type` char(2) NOT NULL DEFAULT '0',
  `projectname` varchar(50) NOT NULL DEFAULT '0',
  `is_legal` bit(1) NOT NULL,
  `colour` int(11) NOT NULL DEFAULT '0',
  `client_code` int(11) DEFAULT '0',
  `request_number` varchar(30) DEFAULT '0',
  `date_created` date DEFAULT NULL,
  `patent_number` varchar(30) DEFAULT '0',
  `date_received` date DEFAULT NULL,
  `is_archive` bit(1) DEFAULT NULL,
  PRIMARY KEY (`pk`),
  UNIQUE KEY `projectname` (`projectname`),
  KEY `client_code` (`client_code`),
  KEY `is_legal` (`is_legal`),
  KEY `project_type` (`project_type`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- Dumping data for table ipkadb.t_main: ~0 rows (approximately)
DELETE FROM `t_main`;
/*!40000 ALTER TABLE `t_main` DISABLE KEYS */;
/*!40000 ALTER TABLE `t_main` ENABLE KEYS */;


-- Dumping structure for table ipkadb.t_query
DROP TABLE IF EXISTS `t_query`;
CREATE TABLE IF NOT EXISTS `t_query` (
  `pk` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `parameter` varchar(40) NOT NULL DEFAULT '0',
  `text` varchar(140) DEFAULT '0',
  PRIMARY KEY (`pk`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- Dumping data for table ipkadb.t_query: ~0 rows (approximately)
DELETE FROM `t_query`;
/*!40000 ALTER TABLE `t_query` DISABLE KEYS */;
/*!40000 ALTER TABLE `t_query` ENABLE KEYS */;


-- Dumping structure for table ipkadb.t_requisites
DROP TABLE IF EXISTS `t_requisites`;
CREATE TABLE IF NOT EXISTS `t_requisites` (
  `pk` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `client_code` int(10) unsigned NOT NULL DEFAULT '0',
  `ek_requis` bigint(20) unsigned NOT NULL DEFAULT '0',
  PRIMARY KEY (`pk`),
  UNIQUE KEY `client_code_ek_requis` (`client_code`,`ek_requis`),
  KEY `ek_requis` (`ek_requis`),
  KEY `client_code` (`client_code`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- Dumping data for table ipkadb.t_requisites: ~0 rows (approximately)
DELETE FROM `t_requisites`;
/*!40000 ALTER TABLE `t_requisites` DISABLE KEYS */;
/*!40000 ALTER TABLE `t_requisites` ENABLE KEYS */;
/*!40014 SET FOREIGN_KEY_CHECKS=1 */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
