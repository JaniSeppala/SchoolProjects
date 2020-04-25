/*Poistetaan ensin vanhat taulut mikäli niitä on*/
DROP TABLE IF EXISTS WorkspaceUsers;
DROP TABLE IF EXISTS Pages;
DROP TABLE IF EXISTS Workspaces;
DROP TABLE IF EXISTS Users;

/*Luodaan uudet taulut tilalle*/
CREATE TABLE Users(
	userID INT AUTO_INCREMENT PRIMARY KEY,
	username VARCHAR(30) UNIQUE NOT NULL,
    pswd VARCHAR(30) NOT NULL,
    firstname VARCHAR(50) NOT NULL,
    lastname VARCHAR(50) NOT NULL,
    teacher BOOLEAN NOT NULL DEFAULT FALSE,
    administrator BOOLEAN NOT NULL DEFAULT FALSE
)ENGINE=InnoDB;

CREATE TABLE Workspaces(
	workspaceID INT AUTO_INCREMENT PRIMARY KEY,
	workspacename VARCHAR(50) UNIQUE NOT NULL,
    creatorname VARCHAR(30) NOT NULL,
    CONSTRAINT workspaces_users_fk FOREIGN KEY (creatorname) REFERENCES Users(username) ON UPDATE CASCADE ON DELETE CASCADE
)ENGINE=InnoDB;

CREATE TABLE WorkspaceUsers(
	ID INT AUTO_INCREMENT PRIMARY KEY,
    workspaceID INT NOT NULL,
    userID INT NOT NULL,
    teacher BOOLEAN NOT NULL DEFAULT FALSE,
    CONSTRAINT workspacesusers_users_fk FOREIGN KEY (userID) REFERENCES Users(userID) ON UPDATE CASCADE ON DELETE CASCADE,
    CONSTRAINT workspacesusers_workspace_fk FOREIGN KEY (workspaceID) REFERENCES Workspaces(workspaceID) ON UPDATE CASCADE ON DELETE CASCADE
)ENGINE=InnoDB;

CREATE TABLE Pages(
	pageID INT AUTO_INCREMENT PRIMARY KEY,
    pageName VARCHAR(50) NOT NULL,
    pageHTML TEXT(65000),
    workspaceID INT NOT NULL,
    visible BOOLEAN NOT NULL DEFAULT TRUE,
    CONSTRAINT pages_workspace_fk FOREIGN KEY (workspaceID) REFERENCES Workspaces(workspaceID) ON UPDATE CASCADE ON DELETE CASCADE
)ENGINE=InnoDB;

/* Luodaan pääkäyttäjä */
INSERT INTO users(username, pswd, firstname, lastname, administrator) VALUES ('admin', 'qwerty', 'Jani', 'Seppälä', true);

/* Luodaan malliksi pari opettajaa */
INSERT INTO users(username, pswd, firstname, lastname, teacher) VALUES ('janse', 'qwerty', 'Jani', 'Seppälä', true);
INSERT INTO users(username, pswd, firstname, lastname, teacher) VALUES ('taute', 'qwerty', 'Tauno', 'Testiopettaja', true);

/* Luodaan malliksi muutana oppilas */
INSERT INTO users(username, pswd, firstname, lastname) VALUES ('M3063', 'qwerty', 'Jani', 'Seppälä');
INSERT INTO users(username, pswd, firstname, lastname) VALUES ('M3064', 'qwerty', 'Olli', 'Opiskelija');
INSERT INTO users(username, pswd, firstname, lastname) VALUES ('M3065', 'qwerty', 'Kalevi', 'Sorsa');
INSERT INTO users(username, pswd, firstname, lastname) VALUES ('M3066', 'qwerty', 'Bob', 'Bobrikov');
INSERT INTO users(username, pswd, firstname, lastname) VALUES ('M3067', 'qwerty', 'Anneli', 'Auvinen');
INSERT INTO users(username, pswd, firstname, lastname) VALUES ('M3068', 'qwerty', 'Jorma', 'Jauhiainen');

/* Luodaan malliksi yksi työtila */
INSERT INTO workspaces(workspacename, creatorname) VALUES ('Websovelluskehitys', 'M3063');

/* Lisätään yksi opiskelija ja yksi opettaja esimerkkityötilaan */
INSERT INTO workspaceusers(workspaceid, userid) VALUES (1,4);
INSERT INTO workspaceusers(workspaceid, userid, teacher) VALUES (1,2,true);