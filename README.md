# OnTheFly

OnTheFly é um projeto onde o foco é apresentar informações sobre um projeto que controlará as vendas de passagens áreas de um aeroporto denominado ON THE FLY. Os módulos são referentes aos controles de:

- Passageiros
- Companhias Aéreas
- Voos Agendados
- Venda de passagens

Dentro desse repositório contém o módulo de vendas de passagens.

### Regras de Negócio

**API Sales**<br>

Após o cadastro de voos, as passagens ficam disponíveis para serem vendidas de acordo com essas especificações:

- O máximo de passagens é definido através do máximo de assentos que o avião comporta.
- Na venda ou reserva não poderá constar o mesmo CPF na lista de passageiros do voo disponível.
- Nenhum passageiro na lista de vendas de passagem pode constar na lista de restritos. Caso aconteça, a venda não poderá ser registrada.
- O primeiro passageiro informado deve ser sempre maior de 18 anos.


# Rotas do módulo

### 1. Listar todas as vendas

- **Endpoint**: `GET /api/Sales`
- **Descrição**: Retorna todas as vendas cadastradas.
- **Parâmetros**: Nenhum.
- **Exemplo de Resposta**:
  ```json
  [
    {
        "id": 1,
        "flight": {
            "flightNumber": 1277,
            "sales": 25,
            "status": true
        },
        "passengers": [
            {
                "cpf": "150.106.910-19",
                "name": "Ana",
                "dtBirth": "2000-04-30T00:00:00",
                "status": false
            },
            {
                "cpf": "237.651.750-80",
                "name": "Mariana",
                "dtBirth": "1985-09-23T00:00:00",
                "status": false
            },
            {
                "cpf": "263.963.420-81",
                "name": "Fernando",
                "dtBirth": "1978-12-05T00:00:00",
                "status": false
            }
        ],
        "reserved": true,
        "sold": false
    },
    {
        "id": 2,
        "flight": {
            "flightNumber": 1300,
            "sales": 12,
            "status": true
        },
        "passengers": [
            {
                "cpf": "150.106.910-19",
                "name": "Ana",
                "dtBirth": "2000-04-30T00:00:00",
                "status": false
            },
            {
                "cpf": "237.651.750-80",
                "name": "Mariana",
                "dtBirth": "1985-09-23T00:00:00",
                "status": false
            },
            {
                "cpf": "263.963.420-81",
                "name": "Fernando",
                "dtBirth": "1978-12-05T00:00:00",
                "status": false
            }
        ],
        "reserved": true,
        "sold": false
    },
    {
        "id": 3,
        "flight": {
            "flightNumber": 1401,
            "sales": 45,
            "status": true
        },
        "passengers": [
            {
                "cpf": "150.106.910-19",
                "name": "Ana",
                "dtBirth": "2000-04-30T00:00:00",
                "status": false
            }
        ],
        "reserved": true,
        "sold": false
    },
    {
        "id": 4,
        "flight": {
            "flightNumber": 1277,
            "sales": 25,
            "status": true
        },
        "passengers": [
            {
                "cpf": "312.905.760-92",
                "name": "Carlos",
                "dtBirth": "1990-06-15T00:00:00",
                "status": false
            }
        ],
        "reserved": false,
        "sold": false
    }
  ]
  ```

### 2. Encontrar uma venda

- **Endpoint**: `GET /api/Sales/id`
- **Descrição**: Retorna uma venda com base no id informado.
- **Parâmetros**:
    - **id**: int
- **Exemplo de Resposta**:
    ```json
    {
      "id": 2,
      "flight": {
          "flightNumber": 1300,
          "sales": 12,
          "status": true
      },
      "passengers": [
          {
              "cpf": "150.106.910-19",
              "name": "Ana",
              "dtBirth": "2000-04-30T00:00:00",
              "status": false
          },
          {
              "cpf": "237.651.750-80",
              "name": "Mariana",
              "dtBirth": "1985-09-23T00:00:00",
              "status": false
          },
          {
              "cpf": "263.963.420-81",
              "name": "Fernando",
              "dtBirth": "1978-12-05T00:00:00",
              "status": false
          }
      ],
    "reserved": true,
    "sold": false
   }
    ```

### 3. Cadastrar uma venda

- **Endpoint**: `POST /api/Sales`
- **Descrição**: Cadastra uma nova venda.
- **Parâmetros**: Nenhum.
- **Corpo da requisição**:
    ```json
    {
      "Flight": 1277,
      "Passengers":[
          "312.905.760-92",
          "150.106.910-19"
      ]
    }
    ```
- **Exemplo de Resposta**:
    ```json
    sem resposta
    ```

### 4. Completar uma venda

- **Endpoint**: `PATCH /api/Sales/id`
- **Descrição**: Muda o status de uma venda para vendido.
- **Parâmetros**:
    - **id**: int
      
- **Exemplo de Resposta**:
    ```json
    sem resposta
    ```
    
### 5. Remover uma venda

- **Endpoint**: `DELETE /api/Sales/id`
- **Descrição**: Cancela uma venda transferindo ela para uma tabela de vendas canceladas
- **Parâmetros**:
    - **id**: int
- **Exemplo de Resposta**:
    ```json
    sem respoosta
    ```
