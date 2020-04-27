USE WoWClassicGear

CREATE TABLE ItemSlots (
	SlotID INT IDENTITY(1,1) PRIMARY KEY,
	SlotName VARCHAR(255) NOT NULL
);

INSERT INTO ItemSlots (SlotName) VALUES ('Head')
INSERT INTO ItemSlots (SlotName) VALUES ('Neck')
INSERT INTO ItemSlots (SlotName) VALUES ('Shoulders')
INSERT INTO ItemSlots (SlotName) VALUES ('Back')
INSERT INTO ItemSlots (SlotName) VALUES ('Chest')
INSERT INTO ItemSlots (SlotName) VALUES ('Wrist')
INSERT INTO ItemSlots (SlotName) VALUES ('Hands')
INSERT INTO ItemSlots (SlotName) VALUES ('Waist')
INSERT INTO ItemSlots (SlotName) VALUES ('Legs')
INSERT INTO ItemSlots (SlotName) VALUES ('Feet')
INSERT INTO ItemSlots (SlotName) VALUES ('Finger')
INSERT INTO ItemSlots (SlotName) VALUES ('Trinket')
INSERT INTO ItemSlots (SlotName) VALUES ('Ranged')

CREATE TABLE ArmorClasses (
	ClassID INT IDENTITY(1,1) PRIMARY KEY,
	Name VARCHAR(255) NOT NULL
);

INSERT INTO ArmorClasses (Name) VALUES ('None')
INSERT INTO ArmorClasses (Name) VALUES ('Cloth')
INSERT INTO ArmorClasses (Name) VALUES ('Leather')
INSERT INTO ArmorClasses (Name) VALUES ('Mail')
INSERT INTO ArmorClasses (Name) VALUES ('Plate')

CREATE TABLE SetBonuses (
	ID INT IDENTITY(1,1) PRIMARY KEY,
	Name VARCHAR(255) NOT NULL,
	Bonus VARCHAR(MAX) NOT NULL
);

INSERT INTO SetBonuses (Name, Bonus) VALUES
	('Devilsaur Armor', '[ { "itemsRequired": 2, "stats": { "hit": 2 } } ]'),
	('Black Dragon Mail', '[ { "itemsRequired": 2, "stats": { "hit": 1 } }, { "itemsRequired": 3, "stats": { "crit": 2 } } ]')

CREATE TABLE Items (
	ID INT IDENTITY(1,1) PRIMARY KEY,
	Name VARCHAR(255) NOT NULL,
	ItemSlot INT FOREIGN KEY REFERENCES ItemSlots(SlotID),
	ArmorClass INT FOREIGN KEY REFERENCES ArmorClasses(ClassID),
	Phase INT NOT NULL,
	SetBonus INT FOREIGN KEY REFERENCES SetBonuses(ID),
	WowHeadID INT
);

INSERT INTO Items (Name, ItemSlot, ArmorClass, Phase, SetBonus) VALUES 
	('Onyxia Tooth Pendant', 2, 1, 1, NULL),
	('Vambraces of the Sadist', 6, 5, 1, NULL),
	('Devilsaur Gauntlets', 7, 3, 1, 1),
	('Devilsaur Leggings', 9, 3, 1, 1),
	('Bloodmail Boots', 10, 4, 1, NULL),
	('Blackhand''s Breadth', 12, 1, 1, NULL),
	('Hand of Justice', 12, 1, 1, NULL),
	('Onslaught Girdle', 8, 5, 1, NULL),
	('Spaulders of Valor', 3, 5, 1, NULL),
	('Eldritch Reinforced Legplates', 9, 5, 1, NULL),
	('Mark of Fordring', 2, 1, 1, NULL),
	('Lionheart Helm', 1, 5, 1, NULL),
	('Truestrike Shoulders', 3, 3, 1, NULL),
	('Battleborn Armbraces', 6, 5, 1, NULL),
	('Sapphiron Scale Boots', 10, 5, 1, NULL),
	('Striker''s Mark', 13, 1, 1, NULL),
	('Don Julio''s Band', 11, 1, 2, NULL),
	('Flameguard Gauntlets', 7, 5, 1, NULL),
	('Savage Gladiator Chain', 5, 4, 1, NULL),
	('Cape of the Black Baron', 4, 1, 1, NULL),
	('Wristguards of Stability', 6, 3, 1, NULL),
	('Master Dragonslayer''s Ring', 11, 1, 3, NULL),
	('Quickstrike Ring', 11, 1, 1, NULL),
	('Cadaverous Armor', 5, 3, 1, NULL),
	('Omokk''s Girth Restrainer', 8, 5, 1, NULL),
	('Blackstone Ring', 11, 1, 1, NULL),
	('Brigam Girdle', 8, 5, 1, NULL),
	('Tarnished Elven Ring', 11, 1, 1, NULL),
	('Riphook', 13, 1, 1, NULL),
	('Satyr''s Bow', 13, 1, 1, NULL),
	('Zandalar Vindicator''s Armguards', 6, 5, 4, NULL),
	('Sacrificial Gauntlets', 7, 5, 4, NULL),
	('Bloodsoaked Pauldrons', 3, 5, 4, NULL),
	('Abyssal Leather Leggings of Striking', 9, 3, 4, NULL),
	('Black Dragonscale Boots', 10, 4, 1, 2),
	('Black Dragonscale Leggings', 9, 4, 1, 2),
	('Black Dragonscale Shoulders', 3, 4, 1, 2)

CREATE TABLE ItemStats (
	ID INT IDENTITY(1,1) PRIMARY KEY,
	ItemID INT FOREIGN KEY REFERENCES Items(ID),
	Strength INT NOT NULL,
	Agility INT NOT NULL,
	Stamina INT NOT NULL,
	Intellect INT NOT NULL,
	Spirit INT NOT NULL,
	AttackPower INT NOT NULL,
	Crit INT NOT NULL,
	Hit INT NOT NULL,
	Armor INT NOT NULL,
	FireResistance INT NOT NULL,
	FrostResistance INT NOT NULL,
	NatureResistance INT NOT NULL,
	ShadowResistance INT NOT NULL,
	ArcaneResistance INT NOT NULL
);

INSERT INTO ItemStats (ItemID, Strength, Agility, Stamina, Intellect, Spirit, AttackPower, Crit, Hit, Armor, FireResistance, FrostResistance, NatureResistance, ShadowResistance, ArcaneResistance) VALUES
		(1, 0, 12, 9, 0, 0, 0, 1, 1, 0, 10, 0, 0, 0, 0),
		(2, 6, 0, 7, 0, 0, 0, 1, 0, 270, 0, 0, 0, 0, 0),
		(3, 0, 0, 9, 0, 0, 28, 1, 0, 103, 0, 0, 0, 0, 0),
		(4, 0, 0, 12, 0, 0, 46, 1, 0, 148, 0, 0, 0, 0, 0),
		(5, 9, 9, 10, 10, 0, 0, 0, 1, 247, 0, 0, 0, 0, 0),
		(6, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0),
		(7, 0, 0, 0, 0, 0, 20, 0, 0, 0, 0, 0, 0, 0, 0),
		(8, 31, 0, 11, 0, 0, 0, 1, 1, 494, 0, 0, 0, 0, 0),
		(9, 11, 9, 17, 0, 0, 0, 0, 0, 470, 0, 0, 0, 0, 0),
		(10, 15, 9, 20, 0, 0, 0, 1, 0, 566, 0, 0, 0, 0, 0),
		(11, 0, 0, 0, 0, 0, 26, 1, 0, 0, 0, 0, 0, 0, 0),
		(12, 18, 0, 0, 0, 0, 0, 2, 2, 565, 0, 0, 0, 0, 0),
		(13, 0, 0, 0, 0, 0, 24, 0, 2, 129, 0, 0, 0, 0, 0),
		(14, 0, 0, 0, 0, 0, 0, 1, 1, 287, 0, 0, 0, 0, 0),
		(15, 14, 9, 14, 0, 0, 0, 0, 0, 417, 0, 0, 0, 0, 0),
		(16, 0, 0, 0, 0, 0, 22, 0, 1, 0, 0, 0, 0, 0, 0),
		(17, 0, 0, 11, 0, 0, 16, 1, 1, 0, 0, 0, 0, 0, 0),
		(18, 0, 0, 13, 0, 0, 54, 1, 0, 488, 0, 0, 0, 0, 0),
		(19, 13, 14, 13, 0, 0, 0, 2, 0, 369, 0, 0, 0, 0, 0),
		(20, 0, 15, 0, 0, 0, 20, 0, 0, 45, 0, 0, 0, 0, 0),
		(21, 24, 0, 8, 0, 0, 0, 0, 0, 86, 0, 0, 0, 0, 0),
		(22, 0, 0, 14, 0, 0, 48, 0, 1, 0, 0, 0, 0, 0, 0),
		(23, 5, 0, 8, 0, 0, 30, 1, 0, 0, 0, 0, 0, 0, 0),
		(24, 8, 8, 0, 0, 0, 60, 0, 0, 172, 0, 0, 0, 0, 0),
		(25, 15, 0, 9, 0, 0, 0, 1, 0, 353, 0, 0, 0, 0, 0),
		(26, 0, 0, 6, 0, 0, 20, 0, 1, 0, 0, 0, 0, 0, 0),
		(27, 15, 0, 16, 0, 0, 0, 0, 1, 369, 0, 0, 0, 0, 0),
		(28, 0, 15, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0),
		(29, 0, 0, 0, 0, 0, 22, 0, 0, 0, 0, 0, 0, 0, 0),
		(30, 0, 3, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0),
		(31, 13, 13, 13, 0, 0, 0, 0, 0, 304, 0, 0, 0, 0, 0),
		(32, 19, 0, 0, 0, 0, 0, 1, 1, 441, 0, 0, 0, 0, 0),
		(33, 16, 11, 16, 0, 0, 0, 0, 0, 552, 0, 0, 0, 0, 0),
		(34, 15, 15, 15, 0, 0, 0, 1, 0, 152, 0, 0, 0, 0, 0),
		(35, 0, 0, 10, 0, 0, 28, 0, 0, 270, 24, 0, 0, 0, 0),
		(36, 0, 0, 8, 0, 0, 54, 0, 0, 320, 13, 0, 0, 0, 0),
		(37, 0, 0, 9, 0, 0, 40, 0, 0, 266, 6, 0, 0, 0, 0)
		--(1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0),

CREATE TABLE Users (
	ID INT IDENTITY(1,1) PRIMARY KEY,
	Username VARCHAR(255) NOT NULL
);

INSERT INTO Users (Username) VALUES
	('Chris')

CREATE TABLE UserSets (
	ID INT IDENTITY(1,1) PRIMARY KEY,
	UserID INT FOREIGN KEY REFERENCES Users(ID),
	ItemID INT FOREIGN KEY REFERENCES Items(ID)
);

INSERT INTO UserSets (UserID, ItemID) VALUES
	(1, 1),
	(1, 2),
	(1, 3),
	(1, 4),
	(1, 5),
	(1, 6),
	(1, 7),
	(1, 8),
	(1, 9),
	(1, 10),
	(1, 11),
	(1, 12),
	(1, 13),
	(1, 14),
	(1, 15),
	(1, 16),
	(1, 17),
	(1, 18),
	(1, 19),
	(1, 20),
	(1, 21),
	(1, 22),
	(1, 23)