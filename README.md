# compose-docker
  
  - Compose docker example
  - Aplicação que acessa a API pública (https://documenter.getpostman.com/view/10808728/SzS8rjbc) e realiza integrações com Rabbit MQ e banco Postgres SQL.  

## Techs:

 - ASP.NET Core 3.1
 - ASP.NET WebApi Core
 - Swagger UI
 - Docker
 - Rabbit MQ
 - POSTGRES DB
 - PG Admin

## Para iniciar a aplicação via docker-compose
  
  - 1) No terminal, ir até o local do arquivo docker-compose.yml;
  - 2) Para subir os containers: docker-compose up -d 
  - 3) Parar containers, remover volumes e imagens : docker-compose down -v --rmi all


## Web API para enviar ao RABBIT MQ   
  
  - Acessar: http://localhost:5000/swagger/index.html
  - Alguns paises de exemplo: Jamaica, Brazil, Norway, Guatemala, Netherlands, Venezuela, Colombia ,Cuba, Germany.
  
## Rabbit MQ
   
   - Acessar : http://localhost:8080
   - User : guest
   - Password : guest
   
## POSTGRESS Admin
   
   - Acessar : http://localhost:80
   
## Devs 
   
   - Lucas Scheid (https://github.com/LucasScheid)  
   
   
