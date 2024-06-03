# Desafio de Projeto proposto pela [DIO](www.dio.me)

RESTful API do Desafio do Bootcamp Avanade 2024 construída em .Net 8.0.

Esta API é um projeto elaborado em C#, que serve como base para elaboração dos testes unitários e testes de integração.

Somente os endpoints `Get`, `GetById` e `Post` foram implementados para a controller Category. No entanto por se tratar de um projeto bem estruturado, ele é facilmente escalável para implementação dos demais endpoints.

## Principais Tecnologias

- **.Net 8.0**: Utilizaremos a versão LTS do .Net Core para tirar vantagem das inovações que essa linguagem robusta e amplamente utilizada oferece;
- **ASP.NET Core Identity**: Um mecanismo que permite adicionar funcionalidades de autenticação e autorização dentro de uma aplicação. Com ele podemos adicionar mecanismo de gerenciamento de conta como criação, edição e exclusão de usuários além de fornecer mecanismo de autenticação externa como Facebook, Google e outros utilizando o OpenId ou OAuth.
- **Entity Framework Core**: Exploraremos como essa ferramenta pode simplificar nossa camada de acesso aos dados, facilitando a integração com bancos de dados SQL utilizando todo potencial que a feature 'Migration' é capaz de oferecer. As migrations já estão criadas no projeto, para executar o projeto localmente, basta executar o comando `Update-Database` ou `dotnet ef database update`;
- **xUnit**: Framework de teste unitário gratuito, de código aberto e focada na comunidade para o .NET Framework. Faz parte da [.NET Foundation](https://dotnetfoundation.org/);
- **OpenAPI (Swagger)**: Vamos criar uma documentação de API eficaz e fácil de entender usando a OpenAPI (Swagger).
