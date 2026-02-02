# Keycloak Setup Guide

## Quick Setup

### 1. Access Admin Console

```
URL: http://localhost:8080
User: admin
Password: admin123
```

### 2. Create Realm: `monitoring-system`

### 3. Create Roles
- `admin` - Full access
- `operator` - Monitor and alert
- `viewer` - Read only

### 4. Create Clients

**monitor-service**
- Client Protocol: openid-connect
- Access Type: confidential
- Valid Redirect URIs: `http://localhost:5001/*`

**alert-service, metric-service, report-service, api-gateway** (same pattern)

### 5. Create Users

**admin** / Admin@123 → role: admin
**operator** / Operator@123 → role: operator
**viewer** / Viewer@123 → role: viewer

### 6. Test Token

```bash
curl -X POST http://localhost:8080/realms/monitoring-system/protocol/openid-connect/token \
  -d "client_id=monitor-service" \
  -d "client_secret=YOUR_SECRET" \
  -d "grant_type=password" \
  -d "username=admin" \
  -d "password=Admin@123"
```

See [KEYCLOAK.md](../../docs/KEYCLOAK.md) for detailed instructions.
