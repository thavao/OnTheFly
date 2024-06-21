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
    {"Inserir resposta aqui"}
  ```

### 2. Encontrar uma venda

- **Endpoint**: `GET /api/Sales/id`
- **Descrição**: Retorna uma venda com base no id informado.
- **Parâmetros**:
    - **id**: int
- **Exemplo de Resposta**:
    ```json
    {"Inserir resposta aqui"}
    ```

### 3. Cadastrar uma venda

- **Endpoint**: `POST /api/Sales`
- **Descrição**: Cadastra uma nova venda.
- **Parâmetros**: Nenhum.
- **Corpo da requisição**:
    ```json
    {"Inserir requisição aqui"}
    ```
- **Exemplo de Resposta**:
    ```json
    {"Inserir resposta aqui"}
    ```

### 4. Completar uma venda

- **Endpoint**: `PATCH /api/Sales/id`
- **Descrição**: Muda o status de uma venda para vendido.
- **Parâmetros**:
    - **id**: int
- **Corpo da requisição**:
    ```json
    {"Inserir requisição aqui"}
    ```
- **Exemplo de Resposta**:
    ```json
    {"Inserir resposta aqui"}
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