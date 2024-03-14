# BlogNoticias

BlogNoticias é um projeto de estudo para as fases 2 e 3 da Pós Graduação da FIAP (Arquitetura de Sistemas .NET com Azure). 
Desenvolvido em C# com .NET 7, seguindo um modelo de <strong>Clean Architecture</strong> (com as camadas de domínio, infra e aplicação bem separadas), a aplicação utiliza o Identity como autenticação e possui alguns endpoints para o gerenciamento de noticias (PUT, GET, POST e DELETE - todos necessitando de autenticação).
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
  <p>Nos Testes de Integração, foi utilizada a biblioteca Xunit (diferente das demais camadas, apenas com intuíto de estudo). Também utilizado Moq, AutoFixture. 
      Para simular o ambiente da aplicação, foi usada a biblioteca <strong>Docker.DotNet</strong> para criar contêineres Docker com bancos de dados fakes e outros serviços de infraestrutura. 
      Isso garante que os testes sejam executados em um ambiente próximo ao de produção, permitindo a validação de casos de uso reais.
      Já nos testes para a Aplicação (endpoints da API Rest), foi utilizado o banco de dados <strong>em memória</strong> para exploração de outras possilidades, com apoio de uma WebApplicationFactory customizada.
      
## Azure Container Registry (ACR)
Foi utilizado o Azure Container Registry (ACR) para hospedar as imagens Docker da aplicação. A publicação da imagem é feita com o comando ```(az acr build --image blognewsimage:v1 --registry acrtechchallengejfm --file resources/Dockerfile .)```, permitindo que essa imagem seja armazenada com segurança e disponibilizada para implantação. Você pode obter a imagem mais recente usando ```docker pull acrtechchallengejfm.azurecr.io/blognewsimage:v1```.

Além disso, foi criada a instâncias no ACI para implantar a imagem. No entanto, devido ao custo associado ao Azure, não é viável manter essas instâncias sempre disponíveis.

## GitHub Actions
O Github Actions está sendo utilizado para controle da pipeline. Onde, toda vez que ocorre uma alteração no código, são executados os passos de validação do código, build, etc, além da execução dos testes unitários e integrados.

## Implementação do Application Insights
Foi implementado o application insights da Azure, como o serviço de monitoramento em tempo real da aplicação. Esta é uma ótima ferramenta, pois contibui com a observabilidade do nosso serviço, fornecendo uma ótima visão de recursos utilizados, falhas na aplicação, utilização de alertas, insights de usuários, além de integração na azure com outros recursos, e muito mais.

## Usage - Testes na aplicação

Para testar localmente, executar o docker-compose disponível no repositório. Cadastrar um usuário e utilizar o token de login para acessar os demais endpoints.

```dotnet
docker-compose up --build -d
```
Realizar o acesso através do Postman ou outra ferramenta semelhante.
Para usar a API, configure as seguintes variáveis no Postman:

- Authorization Type: Bearer Token
- Adicionar o token de autenticação obtido através do endpoint de login.

Utilize a coleção do postman disponibilizada na pasta ```Resources``` do projeto. 
 - No postman, selecione File > Import > selecionar o arquivo Json ```BlogNoticias.postman_collection.json```;
 - Realize a criação de usuário no endpoint correspondente;
 - Em seguida, realize o login do mesmo usuário para obter o token JWT;
 - Adicione o token obtido, na aba "Authorization" dentro de cada um dos endpoints que for testar;
 - A guia "Variables" possui a 'base_url' que pode ser alterada conforme necessário.

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

### 6. Remove uma noticia cadastrada atráves de um Id

- **URL:** `http://localhost:8181/api/Noticias/DeleteById/{id}`
- **Método HTTP:** DELETE
- **Descrição:** Remove uma noticia específica, a partir de um ID.

(Necessário realizar primeiro a criação do usuário e após isso o login. Com o login, utilizar o token retornado, para configurar a autenticação e acessar os endpoints de manipulação de notícias.)

## Kubernetes
Para estudo, foi feita a configuração da API junto com um banco de dados SQL Server em um cluster local do Kubernetes. Isso permite que a aplicação seja escalável e gerenciável em um ambiente de contêineres.

**Componentes:**
- **API Deployment YAML**: Este arquivo descreve como implantar sua API no cluster Kubernetes. Ele especifica o contêiner da sua aplicação, suas configurações e recursos necessários.

- **SQL Server Deployment YAML:** Similar ao anterior, este arquivo descreve como implantar o SQL Server no cluster Kubernetes. Ele inclui informações sobre o contêiner do SQL Server e suas configurações.

- **API Service YAML:** Este arquivo descreve como expor sua API dentro do cluster Kubernetes, permitindo que outros serviços acessem.

- **SQL Server Service YAML:** Semelhante ao serviço da API, este arquivo descreve como expor o SQL Server dentro do cluster para que outros serviços possam se conectar a ele.

- **ConfigMap:** Um ConfigMap contendo o script inicial da base de dados. Ele é usado para configurar o banco de dados SQL Server com dados iniciais ou configurações necessárias durante a inicialização.

Foi utilizado o **Minikube** como ferramenta local.
Comandos utilizados:

```dotnet

minikube start
kubectl apply -f deployment.yml (api)
kubectl apply -f deployment-sql.yml (sql)
kubectl apply -f service.yml (service api)
kubectl apply -f service-sql.yml (service sql)
kubectl apply -f configmap.yml (script inicial)
kubectl port-forward NOME_POD 2501:8080
```
## References

https://learn.microsoft.com/pt-br/azure/devops/pipelines/ecosystems/dotnet-core?view=azure-devops&tabs=dotnetfive#create-your-first-pipeline

https://learn.microsoft.com/pt-br/aspnet/core/security/authentication/identity?view=aspnetcore-7.0&tabs=visual-studio

https://www.endpointdev.com/blog/2022/01/database-integration-testing-with-dotnet/

https://www.alura.com.br/conteudo/integracao-continua-testes-automatizados-pipeline-github-actions

https://kubernetes.io/docs/home/
