CREATE TABLE `Domains` (
	`DateAdded` DATE NOT NULL DEFAULT curdate(),
	`Restrictive` BIT(1) NOT NULL DEFAULT b'0',
	`Domain` VARCHAR(256) NOT NULL DEFAULT '' COLLATE 'utf8mb4_general_ci',
	`ListName` VARCHAR(32) NOT NULL DEFAULT '' COLLATE 'utf8mb4_general_ci',
	`SeenCount` INT(11) NOT NULL DEFAULT '1',
	INDEX `Restrictive` (`Restrictive`) USING BTREE,
	INDEX `ListName` (`ListName`) USING BTREE
)
COLLATE='utf8mb4_general_ci'
ENGINE=InnoDB
AUTO_INCREMENT=0;
