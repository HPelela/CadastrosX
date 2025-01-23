
Guia de Instalação e Execução do Projeto API de Usuários com Redis e Docker

Este guia tem como objetivo ensinar como baixar o projeto e colocá-lo para rodar em sua máquina, tanto usando o Docker para rodar os containers do Redis e da API quanto sem o Docker.

Pré-Requisitos

Antes de começar, você precisa ter as seguintes ferramentas instaladas em sua máquina:

- Docker: Para rodar o Redis e a API em containers.
- .NET Core SDK: Para rodar o projeto localmente sem Docker (opcional).
- Git: Para clonar o repositório.

Certifique-se de ter o Docker e o .NET Core SDK corretamente instalados antes de prosseguir.

Passo 1: Clonando o Repositório

Primeiro, você precisará clonar o repositório do projeto para o seu computador.

1. Abra o terminal ou o Git Bash.
2. Execute o seguinte comando para clonar o repositório:

```bash
git clone https://github.com/seu-usuario/seu-repositorio.git
```

Substitua https://github.com/seu-usuario/seu-repositorio.git pelo link correto do repositório.

Passo 2: Configurando o Redis com Docker

Este projeto utiliza o Redis como cache e você pode rodá-lo facilmente usando Docker.

2.1 Criando o arquivo docker-compose.yml

Dentro do diretório raiz do projeto, crie um arquivo chamado docker-compose.yml com o seguinte conteúdo:

```yaml
version: '3.7'

services:
  redis:
    image: redis:latest
    container_name: redis
    ports:
      - "6379:6379"
    networks:
      - redis-net

  api:
    build: .
    container_name: usuario-api
    ports:
      - "5000:80"
    environment:
      - REDIS_CONNECTION=redis:6379
    depends_on:
      - redis
    networks:
      - redis-net

networks:
  redis-net:
    driver: bridge
```

Este arquivo define dois serviços:

- redis: O Redis será executado em um container.
- api: A API da aplicação será executada em um container e se comunicará com o Redis.

2.2 Rodando o Docker

Agora, você pode rodar os containers do Docker para a API e o Redis.

1. No diretório onde o arquivo docker-compose.yml está localizado, execute o seguinte comando:

```bash
docker-compose up --build
```

O Docker irá baixar as imagens necessárias, construir o container da API, iniciar o Redis e a API, e expor a API na porta 5000.

2. Após a execução do comando, o Redis estará rodando na porta 6379, e a API estará acessível em http://localhost:5000.

Passo 3: Rodando o Projeto Sem Docker

Se preferir rodar o projeto sem o Docker, você pode fazer isso diretamente no seu ambiente local usando o .NET Core.

3.1 Instalando Dependências

1. Navegue até o diretório onde o projeto foi clonado.
2. Se você não tiver as dependências do projeto, execute o comando abaixo para restaurá-las:

```bash
dotnet restore
```

3.2 Executando o Projeto Localmente

1. Após restaurar as dependências, você pode executar a API com o seguinte comando:

```bash
dotnet run
```

Isso irá iniciar a API localmente na porta 5000.

2. Para rodar o Redis, você precisará ter o Redis instalado localmente ou usar o Redis em outro container Docker. Se tiver o Redis instalado, basta configurá-lo para rodar na porta 6379.

Passo 4: Testando a API

A API estará acessível em http://localhost:5000. Você pode utilizar o Postman ou qualquer outra ferramenta para testar os endpoints da API.

Endpoints Disponíveis

1. Adicionar um novo usuário (Método: POST):
   - Rota: /api/usuario
   - Corpo da requisição (JSON):

```json
{
  "usuario": {
    "primeiroNome": "João",
    "ultimoNome": "Silva",
    "email": "joao.silva@example.com",
    "permissao": 1,
    "telefones": ["123456789"]
  },
  "usuarioAtualId": 1
}
```

2. Login de usuário (Método: POST):
   - Rota: /api/usuario/login
   - Corpo da requisição (JSON):

```json
{
  "email": "joao.silva@example.com",
  "senha": "senha_do_usuario"
}
```

3. Obter usuário por ID (Método: GET):
   - Rota: /api/usuario/{id}

4. Obter todos os usuários (Método: GET):
   - Rota: /api/usuario

5. Atualizar um usuário (Método: PUT):
   - Rota: /api/usuario/{id}
   - Corpo da requisição (JSON):

```json
{
  "usuario": {
    "id": 1,
    "primeiroNome": "João Atualizado",
    "ultimoNome": "Silva"
  }
}
```

6. Remover um usuário (Método: DELETE):
   - Rota: /api/usuario/{id}

Passo 5: Executando os Testes

O projeto utiliza o XUnit para realizar testes automatizados. Para rodar os testes, execute o seguinte comando:

```bash
dotnet test
```

Isso irá rodar todos os testes do projeto.

Passo 6: Parando os Containers Docker

Quando terminar de usar o Docker, você pode parar os containers com o comando:

```bash
docker-compose down
```

Isso irá parar e remover os containers do Redis e da API.

Conclusão

Agora você tem uma instância da API de Usuários rodando com Redis, tanto usando Docker quanto localmente. Certifique-se de testar os endpoints utilizando ferramentas como Postman e verificar se o Redis está configurado corretamente.

Se você encontrar algum problema ou precisar de ajuda, sinta-se à vontade para abrir uma issue no repositório.

Se precisar de mais ajuda ou tiver dúvidas, consulte a documentação oficial do Docker ou do Redis.

