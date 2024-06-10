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
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20240607091645_01-Initial') THEN

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
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20240607091645_01-Initial') THEN

    CREATE TABLE `organisation` (
        `id` int NOT NULL AUTO_INCREMENT,
        `code` varchar(250) CHARACTER SET utf8mb4 NOT NULL,
        `description` varchar(250) CHARACTER SET utf8mb4 NOT NULL,
        `created` datetime(6) NOT NULL,
        CONSTRAINT `PK_organisation` PRIMARY KEY (`id`)
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
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20240607091645_01-Initial') THEN

    CREATE TABLE `person` (
        `id` int NOT NULL AUTO_INCREMENT,
        `created` datetime(6) NOT NULL,
        CONSTRAINT `PK_person` PRIMARY KEY (`id`)
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
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20240607091645_01-Initial') THEN

    CREATE TABLE `researchStudyIdentifierType` (
        `id` int NOT NULL AUTO_INCREMENT,
        `description` varchar(250) CHARACTER SET utf8mb4 NOT NULL,
        `created` datetime(6) NOT NULL,
        CONSTRAINT `PK_researchStudyIdentifierType` PRIMARY KEY (`id`)
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
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20240607091645_01-Initial') THEN

    CREATE TABLE `roleType` (
        `id` int NOT NULL AUTO_INCREMENT,
        `code` varchar(250) CHARACTER SET utf8mb4 NOT NULL,
        `description` varchar(250) CHARACTER SET utf8mb4 NULL,
        `created` datetime(6) NOT NULL,
        CONSTRAINT `PK_roleType` PRIMARY KEY (`id`)
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
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20240607091645_01-Initial') THEN

    CREATE TABLE `sourceSystem` (
        `id` int NOT NULL AUTO_INCREMENT,
        `code` varchar(250) CHARACTER SET utf8mb4 NOT NULL,
        `description` varchar(250) CHARACTER SET utf8mb4 NOT NULL,
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
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20240607091645_01-Initial') THEN

    CREATE TABLE `personName` (
        `id` int NOT NULL AUTO_INCREMENT,
        `person_id` int NOT NULL,
        `family` varchar(250) CHARACTER SET utf8mb4 NOT NULL,
        `given` varchar(250) CHARACTER SET utf8mb4 NOT NULL,
        `email` varchar(250) CHARACTER SET utf8mb4 NOT NULL,
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
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20240607091645_01-Initial') THEN

    CREATE TABLE `practitioner` (
        `id` int NOT NULL AUTO_INCREMENT,
        `person_id` int NOT NULL,
        `created` datetime(6) NOT NULL,
        CONSTRAINT `PK_practitioner` PRIMARY KEY (`id`),
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
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20240607091645_01-Initial') THEN

    CREATE TABLE `researchStudy` (
        `id` int NOT NULL AUTO_INCREMENT,
        `shortTitle` varchar(250) CHARACTER SET utf8mb4 NOT NULL,
        `sourceSystem_id` int NOT NULL,
        `created` datetime(6) NOT NULL,
        CONSTRAINT `PK_researchStudy` PRIMARY KEY (`id`),
        CONSTRAINT `fk_researchStudy_sourceSystem` FOREIGN KEY (`sourceSystem_id`) REFERENCES `sourceSystem` (`id`)
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
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20240607091645_01-Initial') THEN

    CREATE TABLE `practitionerRole` (
        `id` int NOT NULL AUTO_INCREMENT,
        `researchStudy_id` int NOT NULL,
        `practitioner_id` int NOT NULL,
        `roleType_id` int NOT NULL,
        `organization_id` int NULL,
        `effective_from` datetime(6) NOT NULL,
        `effective_to` datetime(6) NULL,
        `created` datetime(6) NOT NULL,
        CONSTRAINT `PK_practitionerRole` PRIMARY KEY (`id`),
        CONSTRAINT `fk_researchStudyTeamMember_organization` FOREIGN KEY (`organization_id`) REFERENCES `organisation` (`id`),
        CONSTRAINT `fk_researchStudyTeamMember_personRole` FOREIGN KEY (`roleType_id`) REFERENCES `roleType` (`id`),
        CONSTRAINT `fk_researchStudyTeamMember_practitioner` FOREIGN KEY (`practitioner_id`) REFERENCES `practitioner` (`id`),
        CONSTRAINT `fk_researchStudyTeamMember_researchStudy` FOREIGN KEY (`researchStudy_id`) REFERENCES `researchStudy` (`id`)
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
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20240607091645_01-Initial') THEN

    CREATE TABLE `researchStudyIdentifier` (
        `id` int NOT NULL AUTO_INCREMENT,
        `researchStudy_id` int NOT NULL,
        `sourceSystem_id` int NOT NULL,
        `value` varchar(250) CHARACTER SET utf8mb4 NOT NULL,
        `IdentifierTypeId` int NOT NULL,
        `created` datetime(6) NOT NULL,
        CONSTRAINT `PK_researchStudyIdentifier` PRIMARY KEY (`id`),
        CONSTRAINT `fk_researchStudyIdentifier_researchStudy` FOREIGN KEY (`researchStudy_id`) REFERENCES `researchStudy` (`id`),
        CONSTRAINT `FK_researchStudyIdentifier_researchStudyIdentifierType_Identifi~` FOREIGN KEY (`IdentifierTypeId`) REFERENCES `researchStudyIdentifierType` (`id`) ON DELETE CASCADE,
        CONSTRAINT `fk_researchStudyIdentifier_sourceSystem` FOREIGN KEY (`sourceSystem_id`) REFERENCES `sourceSystem` (`id`)
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
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20240607091645_01-Initial') THEN

    CREATE TABLE `researchStudyIdentifierStatus` (
        `id` int NOT NULL AUTO_INCREMENT,
        `ResearchStudyIdentifierId` int NOT NULL,
        `code` varchar(250) CHARACTER SET utf8mb4 NOT NULL,
        `FromDate` datetime(6) NOT NULL,
        `ToDate` datetime(6) NULL,
        `created` datetime(6) NOT NULL,
        CONSTRAINT `PK_researchStudyIdentifierStatus` PRIMARY KEY (`id`),
        CONSTRAINT `fk_researchStudyIdentifierStatus_researchStudy` FOREIGN KEY (`ResearchStudyIdentifierId`) REFERENCES `researchStudyIdentifier` (`id`)
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
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20240607091645_01-Initial') THEN

    INSERT INTO `organisation` (`id`, `code`, `created`, `description`)
    VALUES (1, 'org01', TIMESTAMP '2024-06-07 10:16:44', 'Development organisation');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20240607091645_01-Initial') THEN

    INSERT INTO `researchStudyIdentifierType` (`id`, `created`, `description`)
    VALUES (1, TIMESTAMP '2024-06-07 10:16:44', 'PROJECT');
    INSERT INTO `researchStudyIdentifierType` (`id`, `created`, `description`)
    VALUES (2, TIMESTAMP '2024-06-07 10:16:44', 'PROTOCOL');
    INSERT INTO `researchStudyIdentifierType` (`id`, `created`, `description`)
    VALUES (3, TIMESTAMP '2024-06-07 10:16:44', 'BUNDLE');
    INSERT INTO `researchStudyIdentifierType` (`id`, `created`, `description`)
    VALUES (4, TIMESTAMP '2024-06-07 10:16:44', 'GRIS ID');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20240607091645_01-Initial') THEN

    INSERT INTO `roleType` (`id`, `code`, `created`, `description`)
    VALUES (1, 'CHF_INV@2.16.840.1.113883.2.1.3.8.5.2.3.5', TIMESTAMP '2024-06-07 10:16:44', 'Chief Investigator');
    INSERT INTO `roleType` (`id`, `code`, `created`, `description`)
    VALUES (2, 'STDY_CRDNTR@2.16.840.1.113883.2.1.3.8.5.2.3.5', TIMESTAMP '2024-06-07 10:16:44', 'Study Coordinator');
    INSERT INTO `roleType` (`id`, `code`, `created`, `description`)
    VALUES (3, 'RSRCH_ACT_CRDNTR@2.16.840.1.113883.2.1.3.8.5.2.3.5', TIMESTAMP '2024-06-07 10:16:44', 'Research Activity Coordinator');
    INSERT INTO `roleType` (`id`, `code`, `created`, `description`)
    VALUES (4, 'PRNCPL_INV@2.16.840.1.113883.2.1.3.8.5.2.3.5', TIMESTAMP '2024-06-07 10:16:44', 'Principal Investigator');
    INSERT INTO `roleType` (`id`, `code`, `created`, `description`)
    VALUES (5, 'CMPNY_RP@2.16.840.1.113883.2.1.3.8.5.2.3.5', TIMESTAMP '2024-06-07 10:16:44', 'Company Representative');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20240607091645_01-Initial') THEN

    INSERT INTO `sourceSystem` (`id`, `code`, `created`, `description`)
    VALUES (1, 'EDGE', TIMESTAMP '2024-06-07 10:16:44', 'Edge system');
    INSERT INTO `sourceSystem` (`id`, `code`, `created`, `description`)
    VALUES (2, 'IRAS', TIMESTAMP '2024-06-07 10:16:44', 'IRAS system');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20240607091645_01-Initial') THEN

    CREATE INDEX `idx_personName_id` ON `personName` (`person_id`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20240607091645_01-Initial') THEN

    CREATE INDEX `fk_researcher_person_idx` ON `practitioner` (`person_id`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20240607091645_01-Initial') THEN

    CREATE INDEX `idx_researchStudyTeamMember_PractitionerId` ON `practitionerRole` (`practitioner_id`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20240607091645_01-Initial') THEN

    CREATE INDEX `idx_researchStudyTeamMember_researchStudyId` ON `practitionerRole` (`researchStudy_id`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20240607091645_01-Initial') THEN

    CREATE INDEX `idx_researchStudyTeamMember_roleTypeId` ON `practitionerRole` (`roleType_id`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20240607091645_01-Initial') THEN

    CREATE INDEX `IX_practitionerRole_organization_id` ON `practitionerRole` (`organization_id`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20240607091645_01-Initial') THEN

    CREATE INDEX `idx_researchStudy_sourceSystem` ON `researchStudy` (`sourceSystem_id`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20240607091645_01-Initial') THEN

    CREATE INDEX `idx_researchStudyIdentifier_researchStudyId` ON `researchStudyIdentifier` (`researchStudy_id`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20240607091645_01-Initial') THEN

    CREATE INDEX `idx_researchStudyIdentifier_sourceSystemId` ON `researchStudyIdentifier` (`sourceSystem_id`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20240607091645_01-Initial') THEN

    CREATE INDEX `IX_researchStudyIdentifier_IdentifierTypeId` ON `researchStudyIdentifier` (`IdentifierTypeId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20240607091645_01-Initial') THEN

    CREATE INDEX `IX_researchStudyIdentifierStatus_ResearchStudyIdentifierId` ON `researchStudyIdentifierStatus` (`ResearchStudyIdentifierId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20240607091645_01-Initial') THEN

    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20240607091645_01-Initial', '6.0.25');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

COMMIT;

