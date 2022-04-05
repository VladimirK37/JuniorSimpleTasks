CREATE TABLE Users(
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Name TEXT NOT NULL
);
CREATE TABLE Tasks(
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Subject TEXT NOT NULL,
    Description TEXT NULL
);
CREATE TABLE TasksUsers(
	UserId INTEGER NOT NULL,
    TaskId INTEGER NOT NULL,
	primary key (UserId,TaskId),
	constraint fk_UserId foreign key (UserId) references Users(Id),
	constraint fk_TaskId foreign key (TaskId) references Tasks(Id)
);
INSERT INTO Users(Name) VALUES('User0');
INSERT INTO Users(Name) VALUES('User1');