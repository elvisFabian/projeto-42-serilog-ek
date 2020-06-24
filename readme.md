# [ElasticSearch](https://www.elastic.co/pt/)
- O Elasticsearch é um mecanismo de busca e análise de dados distribuído e open source para todos os tipos de dados, incluindo textuais, numéricos, geoespaciais, estruturados e não estruturados.
- Também é uma solução NoSql
- Tem como base a biblioteca [Lucene](https://github.com/apache/lucene-solr)

# [Kibana](http://localhost:5601/app/kibana)
- O Kibana é um plugin de visualização de dados de fonte aberta para o Elasticsearch.
- Ele fornece recursos de visualização em cima do conteúdo indexado em um cluster Elasticsearch.

# [Serilog](https://serilog.net/)
- Serilog é uma biblioteca para NetCore que facilita o registro de log.
- Fornece provedor para log em arquivo, console e outro locais (Sinks).

# [keycloak](https://www.keycloak.org/)
- Gerenciamento de acesso e identidade de código aberto

# Código

## Bibliotecas usadas:

```csharp
dotnet add package Serilog.AspNetCore //https://github.com/serilog/serilog-aspnetcore

dotnet add package Serilog.Enrichers.Environment //https://github.com/serilog/serilog-enrichers-environment
dotnet add package Serilog.Enrichers.Thread //https://github.com/serilog/serilog-enrichers-thread

dotnet add package Serilog.Sinks.Elasticsearch //https://github.com/serilog/serilog-sinks-elasticsearch

dotnet add package Serilog.Exceptions //https://github.com/RehanSaeed/Serilog.Exceptions
dotnet add package Serilog.Exceptions.EntityFrameworkCore
dotnet add package Serilog.Exceptions.SqlServer

dotnet add package Serilog.Extras.Attributed
dotnet add package Destructurama.Attributed //https://github.com/destructurama/attributed
```

# Lembrete
- appsettings
- Program
- Startup
- Enrich
- MinimumLevel.Override
- ElasticsearchSinkOptions
- Destructure
- WriteTo
- Log scope
- Middleware
  - Exception
  - UserProperties
- Estatística sql
  - Dapper
  - EF
- [Elasticsearch](http://localhost:9200/)
- [Kibana](http://localhost:5601/app/kibana)