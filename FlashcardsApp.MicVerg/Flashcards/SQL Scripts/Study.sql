USE [Flashcards]
GO

/****** Object:  Table [dbo].[Study]    Script Date: 1/12/2023 10:28:19 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Study](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Date] [date] NULL,
	[Score] [varchar](max) NULL,
	[StackId] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[Study] ADD  DEFAULT (getdate()) FOR [Date]
GO

ALTER TABLE [dbo].[Study]  WITH CHECK ADD FOREIGN KEY([StackId])
REFERENCES [dbo].[Stacks] ([Id])
GO

ALTER TABLE [dbo].[Study]  WITH CHECK ADD  CONSTRAINT [FK_Study_Stack] FOREIGN KEY([StackId])
REFERENCES [dbo].[Stacks] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[Study] CHECK CONSTRAINT [FK_Study_Stack]
GO
