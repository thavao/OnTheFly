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

-- Inserindo dados na tabela Sale
INSERT INTO Sale (FlightId, CpfBuyer, Reserved, Sold) VALUES
(101, '123.456.789-00', 1, 0),
(102, '987.654.321-00', 0, 1),
(103, '123.123.123-12', 1, 0),
(104, '321.321.321-32', 0, 1),
(105, '456.456.456-45', 1, 0),
(106, '654.654.654-65', 0, 1),
(107, '789.789.789-78', 1, 0),
(108, '987.987.987-98', 0, 1),
(109, '147.147.147-14', 1, 0),
(110, '258.258.258-25', 0, 1),
(111, '369.369.369-36', 1, 0),
(112, '741.741.741-74', 0, 1),
(113, '852.852.852-85', 1, 0),
(114, '963.963.963-96', 0, 1),
(115, '111.111.111-11', 1, 0),
(116, '222.222.222-22', 0, 1),
(117, '333.333.333-33', 1, 0),
(118, '444.444.444-44', 0, 1),
(119, '555.555.555-55', 1, 0),
(120, '666.666.666-66', 0, 1);

-- Inserindo dados na tabela PassengerSale
INSERT INTO PassengerSale (SaleId, CpfPassenger) VALUES
(1, '123.456.789-01'),
(2, '987.654.321-01'),
(3, '123.123.123-13'),
(4, '321.321.321-33'),
(5, '456.456.456-46'),
(6, '654.654.654-66'),
(7, '789.789.789-79'),
(8, '987.987.987-99'),
(9, '147.147.147-15'),
(10, '258.258.258-26'),
(11, '369.369.369-37'),
(12, '741.741.741-75'),
(13, '852.852.852-86'),
(14, '963.963.963-97'),
(15, '111.111.111-12'),
(16, '222.222.222-23'),
(17, '333.333.333-34'),
(18, '444.444.444-45'),
(19, '555.555.555-56'),
(20, '666.666.666-67');

-- Inserindo dados na tabela CanceledSale
INSERT INTO CanceledSale (Id, FlightId, CpfBuyer, Reserved, Sold) VALUES
(21, 121, '777.777.777-77', 1, 0),
(22, 122, '888.888.888-88', 0, 1),
(23, 123, '999.999.999-99', 1, 0),
(24, 124, '000.000.000-00', 0, 1),
(25, 125, '123.123.123-00', 1, 0);
