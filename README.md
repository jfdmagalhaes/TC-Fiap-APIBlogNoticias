# BlogNoticias

BlogNoticias é um projeto de estudo para a fase 2 da pós graduação da FIAP (Arquitetura de Sistemas .NET com Azure). 
A aplicação utiliza o Identity como autenticação e possui alguns endpoints para o gerenciamento de noticias (getAll, getById e AddNoticia - todos necessitando de autenticação).
Utiliza o sql como base de dados, em um container do docker adicionado juntamente aos scripts no projeto.

## Usage

Para testar localmente, executar o docker-compose disponível no repositório. Cadastrar um usuário e utilizar o token de login para acessar os demais endpoints.


```dotnet
docker-compose up --build -d
```
Realizar o acesso através do Postman ou outra ferramenta semelhante.
Para usar a API, configure as seguintes variáveis no Postman:

- Authorization Type: Bearer Token
- Adicionar o token de autenticação obtido através do endpoint de login.


## Rotas
(Todas utilizam JSON)

### 1. Criar uma nova noticia

- **URL:** `http://localhost:8181/api/Noticias/addnoticia`
- **Método HTTP:** POST
- **Descrição:** Cria uma nova noticia.

### 2. Obtem todas as noticias cadastradas

- **URL:** `http://localhost:8181/api/Noticias/getall`
- **Método HTTP:** GET
- **Descrição:** Retorna informações sobre um recurso específico.

### 3. Obtem uma noticias cadastrada atráves de um Id

- **URL:** `http://localhost:8181/api/Noticias/{id}`
- **Método HTTP:** GET
- **Descrição:** Busca noticia específica com o ID.

### 4. Cria um novo usuário

- **URL:** `http://localhost:8181/api/auth/createuser`
- **Método HTTP:** POST
- **Descrição:** Cria um novo usuário.

### 4. Realiza o login de usuário para obter Token

- **URL:** `http://localhost:3000/api/auth/login`
- **Método HTTP:** POST
- **Descrição:** Autentica usuário e fornece token.


(Necessário realizar primeiro a criação do usuário e após isso o login. Com o login, utilizar o token retornado, para configurar a autenticação e acessar os endpoints de manipulação de notícias.)


## Contributing






## License
