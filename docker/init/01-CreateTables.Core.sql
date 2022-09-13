CREATE TABLE `Domains` (
	`DateAdded` DATETIME NOT NULL DEFAULT current_timestamp(),
	`DateLastSeen` DATETIME NOT NULL DEFAULT current_timestamp(),
	`Suspicious` BIT(1) NOT NULL DEFAULT b'0',
	`Advertising` BIT(1) NOT NULL DEFAULT b'0',
	`Tracking` BIT(1) NOT NULL DEFAULT b'0',
	`Malicious` BIT(1) NOT NULL DEFAULT b'0',
	`Adult` BIT(1) NOT NULL DEFAULT b'0',
	`Other` BIT(1) NOT NULL DEFAULT b'0',
	`Spam` BIT(1) NOT NULL DEFAULT b'0',
	`Combined` BIT(1) NOT NULL DEFAULT b'0',
	`Facebook` BIT(1) NOT NULL DEFAULT b'0',
	`SeenCount` INT(11) NOT NULL DEFAULT '1',
	`Domain` VARCHAR(256) NOT NULL DEFAULT '' COLLATE 'utf8mb4_general_ci',
	INDEX `Facebook` (`Facebook`) USING BTREE,
	INDEX `SuspiciousList` (`Suspicious`) USING BTREE,
	INDEX `AdvertisingList` (`Advertising`) USING BTREE,
	INDEX `TrackingList` (`Tracking`) USING BTREE,
	INDEX `MaliciousList` (`Malicious`) USING BTREE,
	INDEX `AdultList` (`Adult`) USING BTREE,
	INDEX `OtherList` (`Other`) USING BTREE,
	INDEX `SpamList` (`Spam`) USING BTREE,
	INDEX `CombinedList` (`Combined`) USING BTREE
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
