CREATE SCHEMA `salon` ;
USE salon;

CREATE TABLE `bookingrecord` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `ContactID` int NOT NULL,
  `TimeSlotID` int NOT NULL,
  `Date` datetime NOT NULL,
  `Description` varchar(255) DEFAULT NULL,
  `CreatedDate` datetime NOT NULL,
  `Cancel` bit(1) DEFAULT NULL,
  PRIMARY KEY (`ID`)
);

CREATE TABLE `contact` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(255) NOT NULL,
  `Phone` varchar(100) NOT NULL,
  `Email` varchar(100) DEFAULT NULL,
  `CreatedDate` datetime NOT NULL,
  PRIMARY KEY (`ID`)
);
