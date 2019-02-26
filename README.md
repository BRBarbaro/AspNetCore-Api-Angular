# App Controle de Salas de Reuniões 

# Tecnologias Utilizadas

- Back-end REST em **[ASP.NET CORE 2.2](https://docs.microsoft.com/pt-br/aspnet/core/?view=aspnetcore-2.2)** utilizando **WebApi**
- Autenticação com **[JWT](https://jwt.io/)**
- Acesso aos dados utilizando  **[ORM Entity Framework CORE](https://docs.microsoft.com/pt-br/ef/core/)** gerando a estrutura do BD com **Code-First**
- Banco de dados **[SQLite](https://www.sqlite.org/index.html)**
- Front-end em **[Angular 7](https://angular.io/)** e **[Bootstrap4](https://getbootstrap.com/)**
- Documentação da API com **[Swagger](https://swagger.io/)**

## Requisitos para executar o projeto
	>[ASP.NET Core versão 2.2 SDK](https://dotnet.microsoft.com/download) ou Superior instalado
	>[NPM](https://nodejs.org/en/) instalado (vem junto ao NodeJS)
	>[@angular/cli](https://angular.io/cli) instalado (com o NPM instalado, execute `npm install -g @angular/cli` no prompt de comando)


Abra a pasta Projeto no prompt de comando e execute  `dotnet run`, esse comando é responsável por colocar o back-end em execução, a API estará disponível em http://localhost:5000. Esse comando também irá criar o banco de dados na pasta raiz de nome **Projeto.db**. Caso queria verificar a estrutura do banco gerado, sugiro o uso da ferramenta [DB Browser for SQLite](https://sqlitebrowser.org/).

Em uma outra aba do prompt de comando va até a pasta *wwwroot* do projeto execute o comando `npm install`, assim todos os módulos dependentes da aplicação web serão instalados, após isso execute o comando `ng serve --o`, esse comando colocará o front-end em execução disponível e abrirá o browser padrão no caminho http://localhost:4200. 

## Funcionalidades API
Para acessar a documentação da API, basta acessar o caminho http://localhost:5000/swagger/index.html.

PS.: Para testar as funcionalidades das rotas **/api/sala**, **/api/reserva** é necessário informar o token de autorização, para obtê-lo basta realizar o login pelo próprio **Swagger** na opção **/api/Usuario/Login**, informando o objeto no mesmo modelo da amostra, porém com as informações de usuário que foi criada anteriormente pela aplicação ou pelo **Swagger** na opção **/api/Usuario/Register**. Ao efetuar o login é retornado um JSON com a propriedade "token" e seu valor, após isso basta ir ao topo da documentação e clicar no botão Authorize e informar no campo o prefixo `Bearer` juntamente com o token.
e.g.: Com o retorno do login 
>`{  "token":  "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJVc2VySUQiOiI4NDVlNTcyMi1iNjRmLTRmNGItOWQzMi0wYjA4OTIxMTE0NDkiLCJuYmYiOjE1NTEyMTY1NTcsImV4cCI6MTU1MTMwMjk1NywiaWF0IjoxNTUxMjE2NTU3fQ.v_nn6rHOb3Yt-CvPt8hyr_9wqpmDAqMbFYpOZdGd3uU"  }` 

informaremos no campo do botão Authorize o valor 
>`Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJVc2VySUQiOiI4NDVlNTcyMi1iNjRmLTRmNGItOWQzMi0wYjA4OTIxMTE0NDkiLCJuYmYiOjE1NTEyMTY1NTcsImV4cCI6MTU1MTMwMjk1NywiaWF0IjoxNTUxMjE2NTU3fQ.v_nn6rHOb3Yt-CvPt8hyr_9wqpmDAqMbFYpOZdGd3uU`

## Funcionalidades WebApp
- Login 
	- Após informar usuário e senha, o servidor retorna o token para ser utilizado no header das requisições para o servidor.
- Registrar 
	 - Cadastra um novo usuário para acessar a  aplicação.
- Salas
 Após a obtenção do token será permitido:
	 - Listar as salas;
	 - Cadastrar nova sala;
	 - Alterar sala existente (apenas o usuário que inseriu pode alterar, para acessar essas funcionalidade basta clicar no registro da sala desejada);
	 - Excluir sala existente (apenas o usuário que inseriu pode excluir);
	 
- Reservas (Para acessar as funcionalidades basta clicar no registro da sala desejada)
Após a obtenção do token será permitido:
	- Listar as reservas da sala selecionada;
	- Cadastrar nova reserva para a sala selecionada;
	- Alterar reserva existente para a sala selecionada (apenas o usuário que inseriu pode alterar);
	- Excluir reserva existente para a sala selecionada (apenas o usuário que inseriu pode excluir);
