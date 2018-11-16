## Desáfio Super heróis

# Instalação
Os primeiros passos que devem ser verificados são:

 - Existência da IDE Visual Studio 2017
 - Existência da SDK do .NET Core na máquina e dos demais componentes necessários para desenvolvimento em .NET Core ( é possível instalá-los pelo Visual Studio Installer)

Tendo estes 2 requisitos ok, basta clonar o repositório e abrir a solução que existe na pasta no visual Studio.
Assim ja estará em funcionamento.

Chamadas para utilização da API

- Retornar todos super-heróis: GET: <URL>/api
- Retornar super-herói pelo Id: GET: <URL>/api/getbyid/{id}
- Deletar super-herói pelo Id: DELETE: <URL>/api/deletehero/{id}
- Inverter booleano referente ao super-herói fazer parte da lista dos favoritos PUT: <URL>/api/SetFavorite/{id}
- Inserir novo herói POST: <URL>/api/newhero através de json/xml no corpo
  com seguinte estrutura: 
         {
          "Name": string,
          "Description": string
          "Picture": string base64 da imagem
         }
