CREATE TABLE `Domains` (
	`DateAdded` DATETIME NOT NULL DEFAULT current_timestamp(),
	`DateLastSeen` DATETIME NOT NULL DEFAULT current_timestamp(),
	`SuspiciousList` BIT(1) NOT NULL DEFAULT b'0',
	`AdvertisingList` BIT(1) NOT NULL DEFAULT b'0',
	`TrackingList` BIT(1) NOT NULL DEFAULT b'0',
	`MaliciousList` BIT(1) NOT NULL DEFAULT b'0',
	`AdultList` BIT(1) NOT NULL DEFAULT b'0',
	`OtherList` BIT(1) NOT NULL DEFAULT b'0',
	`SpamList` BIT(1) NOT NULL DEFAULT b'0',
	`CombinedList` BIT(1) NOT NULL DEFAULT b'0',
	`SeenCount` INT(11) NOT NULL DEFAULT '1',
	`Domain` VARCHAR(256) NOT NULL DEFAULT '' COLLATE 'utf8mb4_general_ci',
	INDEX `SuspiciousList` (`SuspiciousList`) USING BTREE,
	INDEX `AdvertisingList` (`AdvertisingList`) USING BTREE,
	INDEX `TrackingList` (`TrackingList`) USING BTREE,
	INDEX `MaliciousList` (`MaliciousList`) USING BTREE,
	INDEX `AdultList` (`AdultList`) USING BTREE,
	INDEX `OtherList` (`OtherList`) USING BTREE,
	INDEX `SpamList` (`SpamList`) USING BTREE,
	INDEX `CombinedList` (`CombinedList`) USING BTREE
)
COLLATE='utf8mb4_general_ci'
ENGINE=InnoDB
;

CREATE TABLE `AdLists` (
	`AdListId` INT(11) NOT NULL AUTO_INCREMENT,
	`Enabled` BIT(1) NOT NULL DEFAULT b'1',
	`AdListType` INT(11) NOT NULL DEFAULT '0',
	`AdListSource` TINYINT(4) NOT NULL DEFAULT '1',
	`ListUrl` VARCHAR(256) NOT NULL DEFAULT '' COLLATE 'utf8mb4_general_ci',
	`ProjectUrl` VARCHAR(256) NOT NULL DEFAULT '' COLLATE 'utf8mb4_general_ci',
	`Maintainer` VARCHAR(50) NOT NULL DEFAULT '' COLLATE 'utf8mb4_general_ci',
	PRIMARY KEY (`AdListId`) USING BTREE,
	INDEX `Enabled` (`Enabled`) USING BTREE,
	INDEX `AdListSource` (`AdListSource`) USING BTREE
)
COLLATE='utf8mb4_general_ci'
ENGINE=InnoDB
;

CREATE TABLE `Whitelists` (
	`EntryId` INT(11) NOT NULL DEFAULT '0',
	`Enabled` BIT(1) NOT NULL DEFAULT b'1',
	`IsRegex` BIT(1) NOT NULL DEFAULT b'0',
	`Order` SMALLINT(6) NOT NULL DEFAULT '255',
	`Expression` VARCHAR(128) NOT NULL DEFAULT '' COLLATE 'utf8mb4_general_ci',
	PRIMARY KEY (`EntryId`) USING BTREE,
	INDEX `Enabled` (`Enabled`) USING BTREE,
	INDEX `IsRegex` (`IsRegex`) USING BTREE
)
COLLATE='utf8mb4_general_ci'
ENGINE=InnoDB
;
