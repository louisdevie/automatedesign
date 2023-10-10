drop table if exists `Session`;
drop table if exists `Registration`;
drop table if exists `User`;

create table `User` (
   `IdUser` INT auto_increment,
   `Email` VARCHAR(50) character set ascii collate ascii_bin not null,
   `PasswordHash` BINARY(32) not null,
   `PasswordSalt` CHAR(32) character set ascii collate ascii_bin not null,
   `Verified` BOOL not null,
   primary key (`IdUser`),
   unique (`Email`)
);

create table `Session`(
   `Token` CHAR(20) character set ascii collate ascii_bin not null,
   `LastUse` DATETIME not null,
   `Expiration` DATETIME not null,
   `IdUser` INT not null,
   primary key (`Token`)
);

CREATE TABLE `Registration` (
   `IdUser` INT,
   `VerificationCode` INT unsigned not null,
   `Expiration` DATETIME not null,
   primary key (`IdUser`)
);

alter table `Session` add foreign key (`IdUser`) references `User`(`IdUser`) on update cascade on delete cascade;
alter table `Registration` add foreign key (`IdUser`) references `User`(`IdUser`) on update cascade on delete cascade;