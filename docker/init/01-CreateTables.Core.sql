CREATE TABLE `Domains` (
	`DateAdded` DATE NOT NULL DEFAULT curdate(),
	`DateLastSeen` DATE NOT NULL DEFAULT curdate(),
	`SuspiciousList` BIT(1) NOT NULL DEFAULT b'0',
	`AdvertisingList` BIT(1) NOT NULL DEFAULT b'0',
	`TrackingList` BIT(1) NOT NULL DEFAULT b'0',
	`MaliciousList` BIT(1) NOT NULL DEFAULT b'0',
	`AdultList` BIT(1) NOT NULL DEFAULT b'0',
	`OtherList` BIT(1) NOT NULL DEFAULT b'0',
	`SeenCount` INT(11) NOT NULL DEFAULT '1',
	`Domain` VARCHAR(256) NOT NULL DEFAULT '' COLLATE 'utf8mb4_general_ci'
)
COLLATE='utf8mb4_general_ci'
ENGINE=InnoDB
;
