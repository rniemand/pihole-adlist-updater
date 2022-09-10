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
	`SeenCount` INT(11) NOT NULL DEFAULT '1',
	`Domain` VARCHAR(256) NOT NULL DEFAULT '' COLLATE 'utf8mb4_general_ci',
	INDEX `SuspiciousList` (`SuspiciousList`) USING BTREE,
	INDEX `AdvertisingList` (`AdvertisingList`) USING BTREE,
	INDEX `TrackingList` (`TrackingList`) USING BTREE,
	INDEX `MaliciousList` (`MaliciousList`) USING BTREE,
	INDEX `AdultList` (`AdultList`) USING BTREE,
	INDEX `OtherList` (`OtherList`) USING BTREE,
	INDEX `SpamList` (`SpamList`) USING BTREE
)
COLLATE='utf8mb4_general_ci'
ENGINE=InnoDB
;
