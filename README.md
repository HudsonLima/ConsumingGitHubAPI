## Nome do Projeto

Hudson Lima Github

## Descrição

Aplicação  utilizando o framework Asp.Net MVC que conecta na API do GitHub, busca e armazena no SQL Server os repositórios destaques de 5 linguagens, que são: **C#, JavaScript, Arduino, Groovy e Ruby**

## Deploy
Projeto está online na plataforma **Azure** no endereço: 
```
https://hudsonlimagithub.azurewebsites.net/
```
## Estrutura do Banco de dados
Script para a criação da tabela Reposit

CREATE TABLE [dbo].[Reposit](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nchar](50) NULL,
	[Description] [varchar](200) NULL,
	[hasDownloads] [tinyint] NULL,
	[hasIssues] [tinyint] NULL,
	[htmlURL] [varchar](100) NULL,
	[Language] [varchar](50) NULL,
	[Stars] [int] NULL,
	[CreatedAt] [datetime] NULL,
	[UpdatedAt] [datetime] NULL,
	[InseridoNoBD] [datetime] NULL,
 CONSTRAINT [PK_Reposit] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]


