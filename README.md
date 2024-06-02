## Market Back-End (C#)
Este repositorio contiene la API de back-end para la aplicación de mercado.


## Descripcion
- **API**: para la gestion de un mercado virtual con funcionalidades para usuarios y clientes.
- **_**:Tener en cuenta la configuracion y conexión a la base de datos SQL Server(u otro SQL).


## La Base de datos contiene lo siguiente:

- **Usuarios(Administradores)**: Almacena informacion de los clientes y articulos(ABM).
- **Cliente**: Puede ingresar dinero en su billetera y hacer compras de los articulos.
- **Articulo**: Almacena informaciOn de los artículos disponibles.
- **Carrito**: Almacena los articulos/productos agregados al carrito de un cliente.
- **Billeteras**: Almacena el saldo de las billeteras virtuales de los clientes.
- **Transacciones**: Registra las transacciones realizadas en las billeteras virtuales.

## ----> CREAR TABLAS <----
          
```sql
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


