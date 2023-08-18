# BlogNoticias

BlogNoticias é um projeto de estudo para a fase 2 da pós graduação da FIAP (Arquitetura de Sistemas .NET com Azure). 
A aplicação utiliza o Identity como autenticação e possui alguns endpoints para o gerenciamento de noticias (getAll, getById e AddNoticia - todos necessitando de autenticação).
Utiliza o sql como base de dados, em um container do docker adicionado juntamente aos scripts no projeto.

## Usage

Para testar localmente, executar o docker-compose disponível no repositório. Cadastrar um usuário e utilizar o token de login para acessar os demais endpoints.


```dotnet
docker-compose up -d
```

## Contributing






## License
