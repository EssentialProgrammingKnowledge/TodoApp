CREATE TABLE IF NOT EXISTS `quests` (
  `Id` INT(11) NOT NULL AUTO_INCREMENT,
  `Title` VARCHAR(150) NOT NULL,
  `Description` TEXT NULL,
  `Status` INT(11) NOT NULL,
  `Created` DATETIME NOT NULL,
  `Modified` DATETIME DEFAULT NULL,
  PRIMARY KEY (`Id`),
  INDEX `idx_quests_title_status` (`Title`,`Status`)
)