CREATE TABLE `Domains` (
	`DomainId` BIGINT(20) NOT NULL AUTO_INCREMENT,
	`DateAdded` DATE NOT NULL DEFAULT curdate(),
	`DateLastSeen` DATE NOT NULL DEFAULT curdate(),
	`DateDeleted` DATE NULL DEFAULT NULL,
	`Deleted` BIT(1) NOT NULL DEFAULT b'0',
	`Restrictive` BIT(1) NOT NULL DEFAULT b'0',
	`SeenCount` INT(11) NOT NULL DEFAULT '0',
	`Domain` VARCHAR(64) NOT NULL DEFAULT '' COLLATE 'utf8mb4_general_ci',
	`ListName` VARCHAR(32) NOT NULL DEFAULT '' COLLATE 'utf8mb4_general_ci',
	PRIMARY KEY (`DomainId`) USING BTREE,
	INDEX `Restrictive` (`Restrictive`) USING BTREE,
	INDEX `Deleted` (`Deleted`) USING BTREE,
	INDEX `ListName` (`ListName`) USING BTREE
)
COLLATE='utf8mb4_general_ci'
ENGINE=InnoDB
AUTO_INCREMENT=0;
