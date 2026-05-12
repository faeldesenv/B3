# Calculadora CDB

Solução completa para cálculo de rendimento de investimentos em CDB, composta por uma Web API em **.NET 8** e uma aplicação frontend em **Angular 17**.
Apesar de ser Angular 17 foi usado NgModel

## Pré-requisitos

| Ferramenta | Versão Mínima |
|---|---|
| .NET SDK | 8.0 |
| Node.js | 18.x |
| npm | 9.x |
| Angular CLI | 17.x |

### Como Verificar suas versões instaladas

Abra um terminal e digite
dotnet --version
node --version
npm --version
ng version


# Executando o projeto

## Execute a Web API

### 1. Navegar ate a pasta da API
'''Terminal
cd CalculadoraCdb.Api
'''

### 2. Restaurar dependências e executar

```Terminal
dotnet restore
dotnet run
```
A API estará disponível em:
- **HTTP:** `http://localhost:7001`
- **HTTPS:** `https://localhost:7000`
- **Swagger UI:** `https://localhost:7000/swagger`


## Executar o Frontend Angular

### 1. Navegar para a pasta do frontend

```terminal
cd frontend
```

### 2. Instalar dependências

```terminal
npm install
```

### 3. Executar

```terminal
ng serve
```


## Arquitetura e Decisões Técnicas

### Web API (.NET 8)

- **Clean Architecture** com separação clara entre Controllers, Services e Interfaces
- **Princípios SOLID:**
  - *SRP*: cada serviço tem responsabilidade única (`CdbCalculatorService` calcula, `TaxCalculatorService` determina as taxas)
  - *OCP*: novas faixas de IR podem ser adicionadas sem modificar o cálculo principal
  - *DIP*: controller e service dependem de abstrações (interfaces), não implementações concretas
- **Injeção de Dependência** via `IServiceCollection` nativa do ASP.NET Core
- **Nullable Reference Types** habilitado para segurança de nulos
- **XML Comments** para documentação do Swagger

### Frontend Angular 17

- **NgModule Components**
- **Reactive Forms** com validação declarativa
- **HttpClient** com Observables para comunicação com a API
- **CORS** configurado na API para aceitar requisições de `localhost:4200`

---

## Análise de Código

O projeto foi desenvolvido seguindo as regras padrão do **SonarLint**, sem supressões de alertas:

- Sem membros `public` desnecessários
- Sem magic numbers (constantes nomeadas)
- Sem variáveis não utilizadas
- Nullable habilitado e tratado corretamente
- Sem dependências circulares

