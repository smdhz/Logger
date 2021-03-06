USE [master]
GO
/****** Object:  Database [Note]    Script Date: 2015/3/29 下午 4:36:02 ******/
CREATE DATABASE [Note]
GO
ALTER DATABASE [Note] SET COMPATIBILITY_LEVEL = 120
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Note].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Note] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Note] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Note] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Note] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Note] SET ARITHABORT OFF 
GO
ALTER DATABASE [Note] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [Note] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Note] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Note] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Note] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Note] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Note] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Note] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Note] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Note] SET  DISABLE_BROKER 
GO
ALTER DATABASE [Note] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Note] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Note] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Note] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Note] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Note] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Note] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Note] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [Note] SET  MULTI_USER 
GO
ALTER DATABASE [Note] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Note] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Note] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Note] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [Note] SET DELAYED_DURABILITY = DISABLED 
GO
USE [Note]
GO
/****** Object:  Table [dbo].[Boss]    Script Date: 2015/3/29 下午 4:36:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Boss](
	[BossName] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Boss] PRIMARY KEY CLUSTERED 
(
	[BossName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ShipLog]    Script Date: 2015/3/29 下午 4:36:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ShipLog](
	[Time] [datetime] NOT NULL,
	[Area] [nvarchar](50) NOT NULL,
	[Enemy] [nvarchar](50) NOT NULL,
	[Rank] [nchar](1) NOT NULL,
	[Drop] [nvarchar](50) NULL,
	[Fight] [uniqueidentifier] NULL,
 CONSTRAINT [PK_Ship] PRIMARY KEY CLUSTERED 
(
	[Time] DESC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  View [dbo].[Last3Hours]    Script Date: 2015/3/29 下午 4:36:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE VIEW [dbo].[Last3Hours]
AS
SELECT   Time, Area, Enemy, Rank, [Drop]
FROM      dbo.ShipLog
WHERE   (Area <> N'鎮守府正面海域') AND (Time > DATEADD(HOUR, -3,GETDATE()))



GO
/****** Object:  StoredProcedure [dbo].[BattleResult]    Script Date: 2015/3/29 下午 4:36:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Fabre
-- Create date: 2015-2-21
-- Description:	某地图的战斗记录（BOSS或劝退）
-- =============================================
CREATE PROCEDURE [dbo].[BattleResult]
	@Area nvarchar(50),
	@Time datetime
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	IF(@Time IS NULL)
		SET @Time = CONVERT(date, GETDATE())

    SELECT *
	FROM [Note].[dbo].[ShipLog] AS a
	WHERE [Time] =
	(	SELECT TOP 1 [Time]
		FROM [Note].[dbo].[ShipLog] AS b
		WHERE Area = @Area AND a.[Fight] = b.[Fight])
	AND [Time] >= @Time
END

GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "Ship"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 146
               Right = 180
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'Last3Hours'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'Last3Hours'
GO
USE [master]
GO
ALTER DATABASE [Note] SET  READ_WRITE 
GO
