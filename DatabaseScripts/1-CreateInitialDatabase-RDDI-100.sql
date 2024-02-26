/*
	SQL File 1-CreateInitialDatabase-RDDI-100.sql
	Description: This file is an initial creation of schema objects for the Study Management DATABASE
				It includes the following tables personRole, personType, researchInitiativeIdentifierType, researchInitiativeType, sourceSystem, person, researchInitiative, researchInitiativeIdentifier,  
                researchInitiativeIdentifier, personName etc.
				as well as reference data.
*/

CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
    `MigrationId` varchar(150) NOT NULL,
    `ProductVersion` varchar(32) NOT NULL,
    PRIMARY KEY (`MigrationId`)
);

START TRANSACTION;

CREATE TABLE `personRole` (
    `id` int NOT NULL AUTO_INCREMENT,
    `type` varchar(45) NOT NULL,
    `description` varchar(150) NULL,
    `created` datetime(6) NOT NULL,
    PRIMARY KEY (`id`)
);

CREATE TABLE `personType` (
    `id` int NOT NULL AUTO_INCREMENT,
    `description` varchar(45) NOT NULL,
    `created` datetime(6) NOT NULL,
    PRIMARY KEY (`id`)
);

CREATE TABLE `researchInitiativeIdentifierType` (
    `id` int NOT NULL AUTO_INCREMENT,
    `description` varchar(45) NOT NULL,
    `created` datetime(6) NOT NULL,
    PRIMARY KEY (`id`)
);

CREATE TABLE `researchInitiativeType` (
    `id` int NOT NULL AUTO_INCREMENT,
    `description` varchar(45) NULL,
    `created` datetime(6) NOT NULL,
    PRIMARY KEY (`id`)
);

CREATE TABLE `sourceSystem` (
    `id` int NOT NULL AUTO_INCREMENT,
    `code` varchar(45) NOT NULL,
    `description` varchar(45) NOT NULL,
    `created` datetime(6) NOT NULL,
    PRIMARY KEY (`id`)
);

CREATE TABLE `person` (
    `id` int NOT NULL AUTO_INCREMENT,
    `personType_id` int NOT NULL,
    `created` datetime(6) NOT NULL,
    PRIMARY KEY (`id`),
    CONSTRAINT `fk_person_type` FOREIGN KEY (`personType_id`) REFERENCES `personType` (`id`)
);

CREATE TABLE `researchInitiative` (
    `id` int NOT NULL AUTO_INCREMENT,
    `researchInitiativeType_id` int NULL,
    `created` datetime(6) NOT NULL,
    PRIMARY KEY (`id`),
    CONSTRAINT `fk_researchInitiative_type` FOREIGN KEY (`researchInitiativeType_id`) REFERENCES `researchInitiativeType` (`id`)
);

CREATE TABLE `researchInitiativeIdentifier` (
    `int` int NOT NULL AUTO_INCREMENT,
    `sourceSystem_id` int NOT NULL,
    `value` varchar(150) NOT NULL,
    `researchInitiativeIdentifierType_id` int NOT NULL,
    `created` datetime(6) NOT NULL,
    PRIMARY KEY (`int`),
    CONSTRAINT `fk_researchInitiativeIdentifier_sourceSystem` FOREIGN KEY (`sourceSystem_id`) REFERENCES `sourceSystem` (`id`),
    CONSTRAINT `fk_researchInitiativeIdentifier_type` FOREIGN KEY (`researchInitiativeIdentifierType_id`) REFERENCES `researchInitiativeIdentifierType` (`id`)
);

CREATE TABLE `personName` (
    `id` int NOT NULL AUTO_INCREMENT,
    `person_id` int NOT NULL,
    `family` varchar(100) NOT NULL,
    `given` varchar(10) NOT NULL,
    `email` varchar(150) NOT NULL,
    `created` datetime(6) NOT NULL,
    PRIMARY KEY (`id`),
    CONSTRAINT `fk_personName_person` FOREIGN KEY (`person_id`) REFERENCES `person` (`id`)
);

CREATE TABLE `researcher` (
    `id` int NOT NULL AUTO_INCREMENT,
    `person_id` int NOT NULL,
    `created` datetime(6) NOT NULL,
    PRIMARY KEY (`id`),
    CONSTRAINT `fk_researcher_person` FOREIGN KEY (`person_id`) REFERENCES `person` (`id`)
);

CREATE TABLE `griResearchStudy` (
    `id` int NOT NULL AUTO_INCREMENT,
    `researchInitiative_id` int NOT NULL,
    `gri` varchar(100) NOT NULL,
    `shortTitle` varchar(150) NOT NULL,
    `requestSourceSystem_id` int NOT NULL,
    `created` datetime(6) NOT NULL,
    PRIMARY KEY (`id`),
    CONSTRAINT `fk_griResearchStudy_researchInitiative` FOREIGN KEY (`researchInitiative_id`) REFERENCES `researchInitiative` (`id`),
    CONSTRAINT `fk_griResearchStudy_sourceSystem` FOREIGN KEY (`requestSourceSystem_id`) REFERENCES `sourceSystem` (`id`)
);

CREATE TABLE `griMapping` (
    `id` int NOT NULL AUTO_INCREMENT,
    `griResearchStudy_id` int NOT NULL,
    `sourceSystem_id` int NOT NULL,
    `researchInitiativeIdentifier_id` int NOT NULL,
    `created` datetime(6) NOT NULL,
    PRIMARY KEY (`id`),
    CONSTRAINT `fk_griMapping_griResearchStudy` FOREIGN KEY (`griResearchStudy_id`) REFERENCES `griResearchStudy` (`id`),
    CONSTRAINT `fk_griMapping_researchInitiativeIdentifier` FOREIGN KEY (`researchInitiativeIdentifier_id`) REFERENCES `researchInitiativeIdentifier` (`int`),
    CONSTRAINT `fk_griMapping_sourceSystem` FOREIGN KEY (`sourceSystem_id`) REFERENCES `sourceSystem` (`id`)
);

CREATE TABLE `researchStudyTeamMember` (
    `id` int NOT NULL AUTO_INCREMENT,
    `griMapping_id` int NOT NULL,
    `researcher_id` int NOT NULL,
    `personRole_id` int NOT NULL,
    `effective_from` datetime(6) NOT NULL,
    `effective_to` datetime(6) NULL,
    `created` datetime(6) NOT NULL,
    PRIMARY KEY (`id`),
    CONSTRAINT `fk_researchStudyTeamMember_griResearch` FOREIGN KEY (`griMapping_id`) REFERENCES `griResearchStudy` (`id`),
    CONSTRAINT `fk_researchStudyTeamMember_personRol` FOREIGN KEY (`personRole_id`) REFERENCES `personRole` (`id`),
    CONSTRAINT `researchStudyTeamMember_researcher` FOREIGN KEY (`researcher_id`) REFERENCES `researcher` (`id`)
);

INSERT INTO `personRole` (`id`, `created`, `description`, `type`)
VALUES (1, '2024-02-20 10:33:23.980503', 'A Chief investigator role', 'CHIEF_INVESTIGATOR');

INSERT INTO `personType` (`id`, `created`, `description`)
VALUES (1, '2024-02-20 10:33:23.980569', 'RESEARCHER');

INSERT INTO `researchInitiativeIdentifierType` (`id`, `created`, `description`)
VALUES (1, '2024-02-20 10:33:23.981589', 'PROJECT');
INSERT INTO `researchInitiativeIdentifierType` (`id`, `created`, `description`)
VALUES (2, '2024-02-20 10:33:23.981592', 'PROTOCOL');

INSERT INTO `researchInitiativeType` (`id`, `created`, `description`)
VALUES (1, '2024-02-20 10:33:23.981657', 'STUDY');

INSERT INTO `sourceSystem` (`id`, `code`, `created`, `description`)
VALUES (1, 'EDGE', '2024-02-20 10:33:23.982819', 'Edge system');
INSERT INTO `sourceSystem` (`id`, `code`, `created`, `description`)
VALUES (2, 'IRAS', '2024-02-20 10:33:23.982822', 'IRAS system');

CREATE INDEX `fk_griMapping_griResearchStudy_idx` ON `griMapping` (`griResearchStudy_id`);

CREATE INDEX `fk_griMapping_researchInitiativeIdentifier_idx` ON `griMapping` (`researchInitiativeIdentifier_id`);

CREATE INDEX `fk_griMapping_sourceSystem_idx` ON `griMapping` (`sourceSystem_id`);

CREATE INDEX `fk_griResearchStudy_researchInitiative_idx` ON `griResearchStudy` (`researchInitiative_id`);

CREATE INDEX `fk_griResearchStudy_sourceSystem_idx` ON `griResearchStudy` (`requestSourceSystem_id`);

CREATE INDEX `fk_person_type_idx` ON `person` (`personType_id`);

CREATE INDEX `fk_personName_person_idx` ON `personName` (`person_id`);

CREATE INDEX `fk_researcher_person_idx` ON `researcher` (`person_id`);

CREATE INDEX `fk_researchInitiative_type_idx` ON `researchInitiative` (`researchInitiativeType_id`);

CREATE INDEX `fk_researchInitiativeIdentifier_sourceSystem_idx` ON `researchInitiativeIdentifier` (`sourceSystem_id`);

CREATE INDEX `fk_researchInitiativeIdentifier_type_idx` ON `researchInitiativeIdentifier` (`researchInitiativeIdentifierType_id`);

CREATE INDEX `fk_researchStudyTeamMember_griResearch_idx` ON `researchStudyTeamMember` (`griMapping_id`);

CREATE INDEX `fk_researchStudyTeamMember_personRol_idx` ON `researchStudyTeamMember` (`personRole_id`);

CREATE INDEX `researchStudyTeamMember_researcher_idx` ON `researchStudyTeamMember` (`researcher_id`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20240220103324_01-Initial', '6.0.25');

COMMIT;

