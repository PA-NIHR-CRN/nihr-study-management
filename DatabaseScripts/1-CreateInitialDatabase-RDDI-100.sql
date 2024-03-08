CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
    `MigrationId` varchar(150) CHARACTER SET utf8mb4 NOT NULL,
    `ProductVersion` varchar(32) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK___EFMigrationsHistory` PRIMARY KEY (`MigrationId`)
) CHARACTER SET=utf8mb4;

START TRANSACTION;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20240308110904_01-Initial') THEN

    ALTER DATABASE CHARACTER SET utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20240308110904_01-Initial') THEN

    CREATE TABLE `personRole` (
        `id` int NOT NULL AUTO_INCREMENT,
        `type` varchar(45) CHARACTER SET utf8mb4 NOT NULL,
        `description` varchar(150) CHARACTER SET utf8mb4 NULL,
        `created` datetime(6) NOT NULL,
        CONSTRAINT `PK_personRole` PRIMARY KEY (`id`)
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20240308110904_01-Initial') THEN

    CREATE TABLE `personType` (
        `id` int NOT NULL AUTO_INCREMENT,
        `description` varchar(45) CHARACTER SET utf8mb4 NOT NULL,
        `created` datetime(6) NOT NULL,
        CONSTRAINT `PK_personType` PRIMARY KEY (`id`)
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20240308110904_01-Initial') THEN

    CREATE TABLE `researchInitiativeIdentifierType` (
        `id` int NOT NULL AUTO_INCREMENT,
        `description` varchar(45) CHARACTER SET utf8mb4 NOT NULL,
        `created` datetime(6) NOT NULL,
        CONSTRAINT `PK_researchInitiativeIdentifierType` PRIMARY KEY (`id`)
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20240308110904_01-Initial') THEN

    CREATE TABLE `researchInitiativeType` (
        `id` int NOT NULL AUTO_INCREMENT,
        `description` varchar(45) CHARACTER SET utf8mb4 NULL,
        `created` datetime(6) NOT NULL,
        CONSTRAINT `PK_researchInitiativeType` PRIMARY KEY (`id`)
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20240308110904_01-Initial') THEN

    CREATE TABLE `sourceSystem` (
        `id` int NOT NULL AUTO_INCREMENT,
        `code` varchar(45) CHARACTER SET utf8mb4 NOT NULL,
        `description` varchar(45) CHARACTER SET utf8mb4 NOT NULL,
        `created` datetime(6) NOT NULL,
        CONSTRAINT `PK_sourceSystem` PRIMARY KEY (`id`)
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20240308110904_01-Initial') THEN

    CREATE TABLE `person` (
        `id` int NOT NULL AUTO_INCREMENT,
        `personType_id` int NOT NULL,
        `created` datetime(6) NOT NULL,
        CONSTRAINT `PK_person` PRIMARY KEY (`id`),
        CONSTRAINT `fk_person_type` FOREIGN KEY (`personType_id`) REFERENCES `personType` (`id`)
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20240308110904_01-Initial') THEN

    CREATE TABLE `researchInitiative` (
        `id` int NOT NULL AUTO_INCREMENT,
        `researchInitiativeType_id` int NULL,
        `created` datetime(6) NOT NULL,
        CONSTRAINT `PK_researchInitiative` PRIMARY KEY (`id`),
        CONSTRAINT `fk_researchInitiative_type` FOREIGN KEY (`researchInitiativeType_id`) REFERENCES `researchInitiativeType` (`id`)
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20240308110904_01-Initial') THEN

    CREATE TABLE `researchInitiativeIdentifier` (
        `int` int NOT NULL AUTO_INCREMENT,
        `sourceSystem_id` int NOT NULL,
        `value` varchar(150) CHARACTER SET utf8mb4 NOT NULL,
        `researchInitiativeIdentifierType_id` int NOT NULL,
        `created` datetime(6) NOT NULL,
        CONSTRAINT `PRIMARY` PRIMARY KEY (`int`),
        CONSTRAINT `fk_researchInitiativeIdentifier_sourceSystem` FOREIGN KEY (`sourceSystem_id`) REFERENCES `sourceSystem` (`id`),
        CONSTRAINT `fk_researchInitiativeIdentifier_type` FOREIGN KEY (`researchInitiativeIdentifierType_id`) REFERENCES `researchInitiativeIdentifierType` (`id`)
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20240308110904_01-Initial') THEN

    CREATE TABLE `personName` (
        `id` int NOT NULL AUTO_INCREMENT,
        `person_id` int NOT NULL,
        `family` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
        `given` varchar(10) CHARACTER SET utf8mb4 NOT NULL,
        `email` varchar(150) CHARACTER SET utf8mb4 NOT NULL,
        `created` datetime(6) NOT NULL,
        CONSTRAINT `PK_personName` PRIMARY KEY (`id`),
        CONSTRAINT `fk_personName_person` FOREIGN KEY (`person_id`) REFERENCES `person` (`id`)
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20240308110904_01-Initial') THEN

    CREATE TABLE `researcher` (
        `id` int NOT NULL AUTO_INCREMENT,
        `person_id` int NOT NULL,
        `created` datetime(6) NOT NULL,
        CONSTRAINT `PK_researcher` PRIMARY KEY (`id`),
        CONSTRAINT `fk_researcher_person` FOREIGN KEY (`person_id`) REFERENCES `person` (`id`)
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20240308110904_01-Initial') THEN

    CREATE TABLE `griResearchStudy` (
        `id` int NOT NULL AUTO_INCREMENT,
        `researchInitiative_id` int NOT NULL,
        `gri` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
        `shortTitle` varchar(150) CHARACTER SET utf8mb4 NOT NULL,
        `requestSourceSystem_id` int NOT NULL,
        `created` datetime(6) NOT NULL,
        CONSTRAINT `PK_griResearchStudy` PRIMARY KEY (`id`),
        CONSTRAINT `fk_griResearchStudy_researchInitiative` FOREIGN KEY (`researchInitiative_id`) REFERENCES `researchInitiative` (`id`),
        CONSTRAINT `fk_griResearchStudy_sourceSystem` FOREIGN KEY (`requestSourceSystem_id`) REFERENCES `sourceSystem` (`id`)
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20240308110904_01-Initial') THEN

    CREATE TABLE `griMapping` (
        `id` int NOT NULL AUTO_INCREMENT,
        `griResearchStudy_id` int NOT NULL,
        `sourceSystem_id` int NOT NULL,
        `researchInitiativeIdentifier_id` int NOT NULL,
        `created` datetime(6) NOT NULL,
        CONSTRAINT `PK_griMapping` PRIMARY KEY (`id`),
        CONSTRAINT `fk_griMapping_griResearchStudy` FOREIGN KEY (`griResearchStudy_id`) REFERENCES `griResearchStudy` (`id`),
        CONSTRAINT `fk_griMapping_researchInitiativeIdentifier` FOREIGN KEY (`researchInitiativeIdentifier_id`) REFERENCES `researchInitiativeIdentifier` (`int`),
        CONSTRAINT `fk_griMapping_sourceSystem` FOREIGN KEY (`sourceSystem_id`) REFERENCES `sourceSystem` (`id`)
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20240308110904_01-Initial') THEN

    CREATE TABLE `researchStudyTeamMember` (
        `id` int NOT NULL AUTO_INCREMENT,
        `griMapping_id` int NOT NULL,
        `researcher_id` int NOT NULL,
        `personRole_id` int NOT NULL,
        `effective_from` datetime(6) NOT NULL,
        `effective_to` datetime(6) NULL,
        `created` datetime(6) NOT NULL,
        CONSTRAINT `PK_researchStudyTeamMember` PRIMARY KEY (`id`),
        CONSTRAINT `fk_researchStudyTeamMember_griResearch` FOREIGN KEY (`griMapping_id`) REFERENCES `griResearchStudy` (`id`),
        CONSTRAINT `fk_researchStudyTeamMember_personRol` FOREIGN KEY (`personRole_id`) REFERENCES `personRole` (`id`),
        CONSTRAINT `researchStudyTeamMember_researcher` FOREIGN KEY (`researcher_id`) REFERENCES `researcher` (`id`)
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20240308110904_01-Initial') THEN

    CREATE TABLE `griResearchStudyStatus` (
        `id` int NOT NULL AUTO_INCREMENT,
        `GriMappingId` int NOT NULL,
        `code` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
        `FromDate` datetime(6) NOT NULL,
        `ToDate` datetime(6) NULL,
        `created` datetime(6) NOT NULL,
        CONSTRAINT `PK_griResearchStudyStatus` PRIMARY KEY (`id`),
        CONSTRAINT `fk_griResearchStudyStatus_griMapping` FOREIGN KEY (`GriMappingId`) REFERENCES `griMapping` (`id`)
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20240308110904_01-Initial') THEN

    INSERT INTO `personRole` (`id`, `created`, `description`, `type`)
    VALUES (1, TIMESTAMP '2024-03-08 11:09:04', 'A Chief investigator role', 'CHIEF_INVESTIGATOR');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20240308110904_01-Initial') THEN

    INSERT INTO `personType` (`id`, `created`, `description`)
    VALUES (1, TIMESTAMP '2024-03-08 11:09:04', 'RESEARCHER');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20240308110904_01-Initial') THEN

    INSERT INTO `researchInitiativeIdentifierType` (`id`, `created`, `description`)
    VALUES (1, TIMESTAMP '2024-03-08 11:09:04', 'PROJECT');
    INSERT INTO `researchInitiativeIdentifierType` (`id`, `created`, `description`)
    VALUES (2, TIMESTAMP '2024-03-08 11:09:04', 'PROTOCOL');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20240308110904_01-Initial') THEN

    INSERT INTO `researchInitiativeType` (`id`, `created`, `description`)
    VALUES (1, TIMESTAMP '2024-03-08 11:09:04', 'STUDY');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20240308110904_01-Initial') THEN

    INSERT INTO `sourceSystem` (`id`, `code`, `created`, `description`)
    VALUES (1, 'EDGE', TIMESTAMP '2024-03-08 11:09:04', 'Edge system');
    INSERT INTO `sourceSystem` (`id`, `code`, `created`, `description`)
    VALUES (2, 'IRAS', TIMESTAMP '2024-03-08 11:09:04', 'IRAS system');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20240308110904_01-Initial') THEN

    CREATE INDEX `fk_griMapping_griResearchStudy_idx` ON `griMapping` (`griResearchStudy_id`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20240308110904_01-Initial') THEN

    CREATE INDEX `fk_griMapping_researchInitiativeIdentifier_idx` ON `griMapping` (`researchInitiativeIdentifier_id`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20240308110904_01-Initial') THEN

    CREATE INDEX `fk_griMapping_sourceSystem_idx` ON `griMapping` (`sourceSystem_id`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20240308110904_01-Initial') THEN

    CREATE INDEX `fk_griResearchStudy_researchInitiative_idx` ON `griResearchStudy` (`researchInitiative_id`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20240308110904_01-Initial') THEN

    CREATE INDEX `fk_griResearchStudy_sourceSystem_idx` ON `griResearchStudy` (`requestSourceSystem_id`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20240308110904_01-Initial') THEN

    CREATE INDEX `IX_griResearchStudyStatus_GriMappingId` ON `griResearchStudyStatus` (`GriMappingId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20240308110904_01-Initial') THEN

    CREATE INDEX `fk_person_type_idx` ON `person` (`personType_id`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20240308110904_01-Initial') THEN

    CREATE INDEX `fk_personName_person_idx` ON `personName` (`person_id`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20240308110904_01-Initial') THEN

    CREATE INDEX `fk_researcher_person_idx` ON `researcher` (`person_id`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20240308110904_01-Initial') THEN

    CREATE INDEX `fk_researchInitiative_type_idx` ON `researchInitiative` (`researchInitiativeType_id`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20240308110904_01-Initial') THEN

    CREATE INDEX `fk_researchInitiativeIdentifier_sourceSystem_idx` ON `researchInitiativeIdentifier` (`sourceSystem_id`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20240308110904_01-Initial') THEN

    CREATE INDEX `fk_researchInitiativeIdentifier_type_idx` ON `researchInitiativeIdentifier` (`researchInitiativeIdentifierType_id`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20240308110904_01-Initial') THEN

    CREATE INDEX `fk_researchStudyTeamMember_griResearch_idx` ON `researchStudyTeamMember` (`griMapping_id`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20240308110904_01-Initial') THEN

    CREATE INDEX `fk_researchStudyTeamMember_personRol_idx` ON `researchStudyTeamMember` (`personRole_id`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20240308110904_01-Initial') THEN

    CREATE INDEX `researchStudyTeamMember_researcher_idx` ON `researchStudyTeamMember` (`researcher_id`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20240308110904_01-Initial') THEN

    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20240308110904_01-Initial', '6.0.25');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

COMMIT;

