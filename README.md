# BlogNoticias

BlogNoticias é um projeto de estudo para a fase 2 e 3 da pós graduação da FIAP (Arquitetura de Sistemas .NET com Azure). 
Desenvolvido em C# com .NET 7, a aplicação utiliza o Identity como autenticação e possui alguns endpoints para o gerenciamento de noticias (put, get, post e delete - todos necessitando de autenticação).
Utiliza o sql como base de dados, em um container do docker adicionado juntamente aos scripts no projeto.

## Autenticação
Está sendo utilizada a autenticação do tipo JWT (JSON Web Token). Sendo necessário realizar a autenticação para obtenção do token, e então enviá-lo em todas as rotas protegidas.

## Implementação dos testes automatizados
Foram implementados os testes automatizados na aplicação.

- **Application.UnitTests**
  <p>Foram realizados os testes unitários de todos os endpoints da camada de aplicação. Tanto a parte de cadastro e login de usuário, quanto os endpoints de manipulação das noticias (put, post, get e delete).
  Utilizada as bibliotecas NUnit, MoQ para criar os mocks necessários durante os testes e também o FluentAssertions para as asserções dos testes realizados de forma legível.
  
- **Infrastructure.UnitTests**
  <p>Aqui estão os testes da camanda de infra, onde validamos os repositórios que se conectam com o banco de dados e todas as manipulações que ocorrem.
  Tambem foram utilizadas as bibliotecas NUnit, Mock e FluentAssertions. Nesta camada, também foi utilizado o AutoFixture para facilitar a geração dos mocks.

- **BlogNoticias.IntegrationTests**
  <p>Nos Testes Integrados, foi utilizada a biblioteca Xunit (diferente das demais camadas, apenas com intuíto de estudo). Também utilizado Moq, AutoFixture. 
      Para simular o ambiente da aplicação, foi usada a biblioteca Docker.DotNet para criar contêineres Docker com bancos de dados fakes e outros serviços de infraestrutura. 
      Isso garante que os testes sejam executados em um ambiente próximo ao de produção, permitindo a validação de casos de uso reais.

## Azure Container Registry (ACR)
Foi utilizado o Azure Container Registry (ACR) para hospedar as imagens Docker da aplicação. A publicação da imagem é feita com o comando ```(az acr build --image blognewsimage:v1 --registry acrtechchallengejfm --file resources/Dockerfile .)```, permitindo que essa imagem seja armazenada com segurança e disponibilizada para implantação. Você pode obter a imagem mais recente usando ```docker pull acrtechchallengejfm.azurecr.io/blognewsimage:v1```.

Além disso, foi criada a instâncias no ACI para implantar a imagem. No entanto, devido ao custo associado ao Azure, não é viável manter essas instâncias sempre disponíveis.

## GitHub Actions
O Github Actions está sendo utilizado para controle da pipeline. Onde, toda vez que ocorre uma alteração no código, são executados os passos de validação do código, build, etc, além da execução dos testes unitários e integrados.


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

### 3. Obtem uma noticia cadastrada atráves de um Id

- **URL:** `http://localhost:8181/api/Noticias/{id}`
- **Método HTTP:** GET
- **Descrição:** Busca noticia específica com o ID.

### 4. Cria um novo usuário

- **URL:** `http://localhost:8181/api/auth/createuser`
- **Método HTTP:** POST
- **Descrição:** Cria um novo usuário.

### 5. Realiza o login de usuário para obter Token

- **URL:** `http://localhost:8181/api/auth/login`
- **Método HTTP:** POST
- **Descrição:** Autentica usuário e fornece token.


(Necessário realizar primeiro a criação do usuário e após isso o login. Com o login, utilizar o token retornado, para configurar a autenticação e acessar os endpoints de manipulação de notícias.)


## References

https://learn.microsoft.com/pt-br/azure/devops/pipelines/ecosystems/dotnet-core?view=azure-devops&tabs=dotnetfive#create-your-first-pipeline

https://learn.microsoft.com/pt-br/aspnet/core/security/authentication/identity?view=aspnetcore-7.0&tabs=visual-studio

https://www.endpointdev.com/blog/2022/01/database-integration-testing-with-dotnet/

https://www.alura.com.br/conteudo/integracao-continua-testes-automatizados-pipeline-github-actions
