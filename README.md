# API de Usuários com Redis e Docker

Esta é uma API RESTful para gerenciamento de usuários, construída com ASP.NET Core. A API utiliza o Redis como cache para melhorar a performance nas operações de leitura e armazenamento temporário de dados.

## Funcionalidades

- **Adicionar usuários**: Criação de novos usuários.
- **Login de usuários**: Autenticação dos usuários com email e senha.
- **Buscar usuários por ID**: Recuperação dos dados de um usuário específico.
- **Buscar todos os usuários**: Recuperação de todos os usuários cadastrados.
- **Atualizar usuários**: Atualização dos dados de um usuário.
- **Remover usuários**: Exclusão de um usuário.

## Requisitos

- **Docker**: Para rodar a aplicação e o Redis.
- **.NET Core SDK**: Para rodar o projeto localmente sem Docker.
- **Redis**: Usado como cache.

## Tecnologias

- ASP.NET Core
- Redis
- Docker
- XUnit (Testes)
- Moq (Mocking)

## Passo a Passo para Rodar o Projeto com Docker e Redis

### 1. Clonar o Repositório

Primeiro, clone o repositório para sua máquina local:

```bash
git clone https://github.com/seu-usuario/seu-repositorio.git
