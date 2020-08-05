# Library Mgmt System Update Checklist

- ERROR HANDLING
  - First() etc. everywhere
  - Handle the unhappy paths
  - Flesh out the idea of a Service-Level Error in ServiceResult objects

- Establish types for magic strings (enums)
- a11y
- Create AutoMapper mappings
- Run the latest migrations

- More expressive return types from service layer on success / error
  - Replace all ServiceResult<bool> type ServiceResults with add+ info

- Query String validation middleware
- Logging (NLog)
- Monitoring (Prometheus + Grafana)
- Generic Repository
- Write new tests
- Caching
