# Library Mgmt System Update Checklist

- ERROR HANDLING
  - First() etc. everywhere
  - Handle the unhappy paths
  - Flesh out the idea of a Service-Level Error in ServiceResult objects

- More expressive return types from service layer on success / error
  - Replace all ServiceResult<bool> type ServiceResults with add+ info

- Establish types for magic strings (enums)
- Create AutoMapper mappings
- Query String validation middleware
- a11y
- Monitoring (Prometheus + Grafana)
  - Load Testing with Locust
- Generic Repository
- Write new tests
- Caching
- ELK
