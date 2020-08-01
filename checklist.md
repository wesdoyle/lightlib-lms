# Library Mgmt System Update Checklist

- Make service layer, web async
- Caching

- Generic Repository
- Write new tests

- ERROR HANDLING
  - First() etc. everywhere
  - Handle the unhappy paths
  - Flesh out the idea of a Service-Level Error in ServiceResult objects

- Establish types for magic strings (enums)
- More expressive return types from CheckoutService on success
  - Replace all ServiceResult<bool> type ServiceResults with add+ info

- Logging (NLog)
- Monitoring (Prometheus + Grafana)


