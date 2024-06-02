# market-back-cs

##Este Back va acompañado de algunas consultas en la Base de Datos(Yo estoy usando SQLServer)

----> CONSULTAS SQL:
          ||
          ||
          \/

CREATE TABLE Usuarios(
  Id INT PRIMARY KEY,
  Email VARCHAR(255) NOT NULL,
  Password VARCHAR(255) NOT NULL,
  NameUser VARCHAR(255),
  Apellido VARCHAR(50)
);

CREATE TABLE Cliente(
  IdCliente INT PRIMARY KEY,
  EmailCliente VARCHAR(255) NOT NULL,
  ClaveCliente VARCHAR(255) NOT NULL,
  NombreCliente VARCHAR(255),
  ApellidoCliente VARCHAR(50)
);

CREATE TABLE Articulo(
  Id INT PRIMARY KEY,
  Talle VARCHAR(10),
  Precio DECIMAL(10,2),
  Stock INT,
  Imagen VARCHAR(255),
  NombreArticulo VARCHAR(255)
);

CREATE TABLE Carrito(
  Id INT PRIMARY KEY,
  ClienteCarrito INT NOT NULL,
  ArticuloCarrito INT NOT NULL,
  Cantidad INT NOT NULL,
  FechaAgregado DATETIME NOT NULL,
  FOREIGN KEY (ClienteCarrito) REFERENCES Cliente(IdCliente),
  FOREIGN KEY (ArticuloCarrito) REFERENCES Articulo(Id)
);

CREATE TABLE Billeteras(
  Id INT PRIMARY KEY,
  Saldo DECIMAL(20,2),
  ClienteId INT
);

CREATE TABLE Transacciones(
  Id INT PRIMARY KEY,
  BilleteraId INT NOT NULL,
  Monto DECIMAL(10,2),
  TipoTransaccion VARCHAR(20),
  FechaTransaccion DATETIME,
  FOREIGN KEY (BilleteraId) REFERENCES Billeteras(Id)
);


