drop table if exists `Session`;
drop table if exists `Registration`;
drop table if exists `User`;

create table `User` (
   `UserId` INT auto_increment,
   `Email` VARCHAR(50) character set ascii collate ascii_bin not null,
   `PasswordHash` BINARY(32) not null,
   `PasswordSalt` CHAR(32) character set ascii collate ascii_bin not null,
   `IsVerified` BOOL not null,
   primary key (`UserId`),
   unique (`Email`)
);

create table `Session`(
   `Token` CHAR(20) character set ascii collate ascii_bin not null,
   `LastUse` DATETIME not null,
   `Expiration` DATETIME not null,
   `UserId` INT not null,
   primary key (`Token`)
);

CREATE TABLE `Registration` (
   `UserId` INT,
   `VerificationCode` INT unsigned not null,
   `Expiration` DATETIME not null,
   primary key (`UserId`)
);

create table `Document` (
   `DocumentId` INT AUTO_INCREMENT,
   `UserId` INT not null,
   `HeaderSize` INT unsigned not null,
   `HeaderData` BLOB,
   `BodySize` INT unsigned not null,
   `BodyData` BLOB,
   primary key (`DocumentId`)
);


alter table `Session` add foreign key (`UserId`) references `User`(`UserId`) on update cascade on delete cascade;
alter table `Registration` add foreign key (`UserId`) references `User`(`UserId`) on update cascade on delete cascade;
alter table `Document` add foreign key (`UserId`) references `User`(`UserId`) on update cascade on delete cascade;