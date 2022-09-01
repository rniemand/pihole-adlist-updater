CREATE TABLE `Domains` (
	`DateAdded` DATE NOT NULL DEFAULT curdate(),
	`DateLastSeen` DATE NOT NULL DEFAULT curdate(),
	`Strict` BIT(1) NOT NULL DEFAULT b'0',
	`Deleted` BIT(1) NOT NULL DEFAULT b'0',
	`Domain` VARCHAR(256) NOT NULL DEFAULT '' COLLATE 'utf8mb4_general_ci',
	`ListName` VARCHAR(32) NOT NULL DEFAULT '' COLLATE 'utf8mb4_general_ci',
	`SeenCount` INT(11) NOT NULL DEFAULT '1',
	INDEX `Strict` (`Strict`) USING BTREE,
	INDEX `ListName` (`ListName`) USING BTREE,
	INDEX `Deleted` (`Deleted`) USING BTREE
)
COLLATE='utf8mb4_general_ci'
ENGINE=InnoDB
AUTO_INCREMENT=0;
