USE [master]
GO
/****** Object:  Database [ventas]    Script Date: 18/6/2019 9:50:22 ******/
CREATE DATABASE [ventas]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'ventas', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.SQLEXPRESS\MSSQL\DATA\ventas.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'ventas_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.SQLEXPRESS\MSSQL\DATA\ventas_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [ventas] SET COMPATIBILITY_LEVEL = 140
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [ventas].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [ventas] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [ventas] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [ventas] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [ventas] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [ventas] SET ARITHABORT OFF 
GO
ALTER DATABASE [ventas] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [ventas] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [ventas] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [ventas] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [ventas] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [ventas] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [ventas] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [ventas] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [ventas] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [ventas] SET  ENABLE_BROKER 
GO
ALTER DATABASE [ventas] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [ventas] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [ventas] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [ventas] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [ventas] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [ventas] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [ventas] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [ventas] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [ventas] SET  MULTI_USER 
GO
ALTER DATABASE [ventas] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [ventas] SET DB_CHAINING OFF 
GO
ALTER DATABASE [ventas] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [ventas] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [ventas] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [ventas] SET QUERY_STORE = OFF
GO
USE [ventas]
GO
/****** Object:  Table [dbo].[cliente]    Script Date: 18/6/2019 9:50:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cliente](
	[idCliente] [int] NOT NULL,
	[nombre] [varchar](50) NULL,
	[direccion] [varchar](100) NULL,
	[contacto] [varchar](50) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[venta]    Script Date: 18/6/2019 9:50:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[venta](
	[idventa] [int] IDENTITY(1,1) NOT NULL,
	[descripcion] [varchar](100) NULL,
	[momento] [datetime] NULL,
	[idCliente] [int] NULL,
	[monto] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[idventa] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[ingresarCliente]    Script Date: 18/6/2019 9:50:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ingresarCliente] (@idCliente INT, @nombre VARCHAR(50), @direccion VARCHAR(100), @contacto VARCHAR(100)) AS
BEGIN
INSERT INTO cliente VALUES(@idCliente, @nombre, @direccion, @contacto)
END
GO
/****** Object:  StoredProcedure [dbo].[ingresarClienteSinID]    Script Date: 18/6/2019 9:50:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ingresarClienteSinID] (@nombre VARCHAR(50), @direccion VARCHAR(100), @contacto VARCHAR(100)) AS
BEGIN
INSERT INTO cliente (nombre, direccion, contacto) VALUES( @nombre, @direccion, @contacto)
END
GO
/****** Object:  StoredProcedure [dbo].[verDetalleVentas]    Script Date: 18/6/2019 9:50:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[verDetalleVentas]
AS
SELECT venta.descripcion, venta.momento, venta.monto, cliente.nombre FROM venta INNER JOIN cliente ON (venta.idCliente = cliente.idCliente);
GO
/****** Object:  StoredProcedure [dbo].[verListaClientes]    Script Date: 18/6/2019 9:50:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[verListaClientes] AS
BEGIN
SELECT DISTINCT * FROM cliente ORDER BY idCliente ASC
END
GO
/****** Object:  StoredProcedure [dbo].[verListaNombreClientes]    Script Date: 18/6/2019 9:50:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[verListaNombreClientes] AS
SELECT DISTINCT nombre FROM cliente ORDER BY nombre ASC;
GO
/****** Object:  StoredProcedure [dbo].[verUltimoCliente]    Script Date: 18/6/2019 9:50:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[verUltimoCliente] AS
BEGIN
	--Obtener el id del Cliente con mayor ID hasta el momento--
	SET ROWCOUNT 1
	SELECT idCliente FROM Cliente ORDER BY idCliente DESC;
	return
END
GO
USE [master]
GO
ALTER DATABASE [ventas] SET  READ_WRITE 
GO
