# Introdução
Projeto exemplo de gerenciador de tarefas simples utilizando a seguinte stack : C# .NET (com EF), SQL Server, RabbitMQ e Angular.

# Preparando
Para dar início ao projeto é necessário ter instalado o Docker (RabbitMQ & SQL Server em imagens) para rodar os seguintes comandos de criação das imagens Ou Pode se também, alterar a conexão do 
SQL Server para o servidor desejado (e, no caso, usuário + senha) nos arquivos de configurações: 

```
> TaskManagement\TaskManagement.API\appsettings.json
> TaskManagement\TaskManagement.API\appsettings.Development.json
```
e :
```
> TaskManagement\TaskManagement.Worker\appsettings.json
> TaskManagement\TaskManagement.Worker\appsettings.Development.json  
```

Caso opte por utilizar as imagens do Docker : 

Imagem do banco (SQL Server) 
```
$> docker pull mcr.microsoft.com/mssql/server:2022-latest
```
```
$> docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=RFJN8Cr7kt" -p 1433:1433 -d mcr.microsoft.com/mssql/server:2022-latest
```

Imagem do RabbitMQ
```
$> docker run -d --hostname rabbitserver --name rabbitmq-server -p 15672:15672 -p 5672:5672 rabbitmq:3-management
```


Criadas as imagens (ou optado por utilizar um outro servidor SQL Server)  o próximo será rodar o script SQL Server que está no seguinte caminho
Obs.: No caso da imagem do Sql Server as credenciais são "-user=sa -password=RFJN8Cr7kt" 

```
> TaskManagement/TaskManagement.Repository/Script_001.sql
```
Este Script faz a criação do banco de dados e da tabela necessária para que a aplicação inicie. Por fim, basta instalar a aplicação Angular (TaskManagement\TaskManagement.App)

```
npm install
```

# Executando
Garantir que a API e o Worker estejam como "Multiple Startup Projects" para rodar e "npm  run start" para o frontend

