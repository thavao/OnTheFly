create database DBSales;
GO
use DbSales;
go

create table Sale(
Id int Identity(1,1),
FlightId int,
CpfBuyer char (14),
Reserved bit not null,
Sold bit not null,
constraint PK_Sale primary key (Id)
);
go
create table PassengerSale(
SaleId int not null,
CpfPassenger char(14),
constraint PK_PassengerSale primary key (SaleId, CpfPassenger),
constraint FK_Sale_PassengerSale foreign key (SaleId) references Sale (Id)
);
go

create table CanceledSale(
Id int,
FlightId int,
CpfBuyer char (14),
Reserved bit not null,
Sold bit not null,
constraint PK_CanceledSale primary key (Id)
);
go
