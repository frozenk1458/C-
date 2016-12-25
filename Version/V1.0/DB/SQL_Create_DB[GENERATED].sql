USE [master]
GO

/****** Object:  Database [SoftCSharp]    Script Date: 25/12/2016 20:08:19 ******/
CREATE DATABASE [SoftCSharp]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'SoftCSharp', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.SQLEXPRESS\MSSQL\DATA\SoftCSharp.mdf' , SIZE = 5120KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'SoftCSharp_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.SQLEXPRESS\MSSQL\DATA\SoftCSharp_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO

ALTER DATABASE [SoftCSharp] SET COMPATIBILITY_LEVEL = 120
GO

IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [SoftCSharp].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO

ALTER DATABASE [SoftCSharp] SET ANSI_NULL_DEFAULT OFF 
GO

ALTER DATABASE [SoftCSharp] SET ANSI_NULLS OFF 
GO

ALTER DATABASE [SoftCSharp] SET ANSI_PADDING OFF 
GO

ALTER DATABASE [SoftCSharp] SET ANSI_WARNINGS OFF 
GO

ALTER DATABASE [SoftCSharp] SET ARITHABORT OFF 
GO

ALTER DATABASE [SoftCSharp] SET AUTO_CLOSE OFF 
GO

ALTER DATABASE [SoftCSharp] SET AUTO_SHRINK OFF 
GO

ALTER DATABASE [SoftCSharp] SET AUTO_UPDATE_STATISTICS ON 
GO

ALTER DATABASE [SoftCSharp] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO

ALTER DATABASE [SoftCSharp] SET CURSOR_DEFAULT  GLOBAL 
GO

ALTER DATABASE [SoftCSharp] SET CONCAT_NULL_YIELDS_NULL OFF 
GO

ALTER DATABASE [SoftCSharp] SET NUMERIC_ROUNDABORT OFF 
GO

ALTER DATABASE [SoftCSharp] SET QUOTED_IDENTIFIER OFF 
GO

ALTER DATABASE [SoftCSharp] SET RECURSIVE_TRIGGERS OFF 
GO

ALTER DATABASE [SoftCSharp] SET  DISABLE_BROKER 
GO

ALTER DATABASE [SoftCSharp] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO

ALTER DATABASE [SoftCSharp] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO

ALTER DATABASE [SoftCSharp] SET TRUSTWORTHY OFF 
GO

ALTER DATABASE [SoftCSharp] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO

ALTER DATABASE [SoftCSharp] SET PARAMETERIZATION SIMPLE 
GO

ALTER DATABASE [SoftCSharp] SET READ_COMMITTED_SNAPSHOT OFF 
GO

ALTER DATABASE [SoftCSharp] SET HONOR_BROKER_PRIORITY OFF 
GO

ALTER DATABASE [SoftCSharp] SET RECOVERY SIMPLE 
GO

ALTER DATABASE [SoftCSharp] SET  MULTI_USER 
GO

ALTER DATABASE [SoftCSharp] SET PAGE_VERIFY CHECKSUM  
GO

ALTER DATABASE [SoftCSharp] SET DB_CHAINING OFF 
GO

ALTER DATABASE [SoftCSharp] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO

ALTER DATABASE [SoftCSharp] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO

ALTER DATABASE [SoftCSharp] SET DELAYED_DURABILITY = DISABLED 
GO

ALTER DATABASE [SoftCSharp] SET  READ_WRITE 
GO


