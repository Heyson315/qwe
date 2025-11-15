
---

### ✅ SECURITY.md Template
```markdown
# Security Policy

## Supported Versions
We support the latest main branch.

## Reporting a Vulnerability
Please open a private security advisory or email hassan@hhr-cpa.us. Do not open a public issue for sensitive reports.

## Best Practices
- Never commit secrets; use environment variables and secret managers.
- Encrypt sensitive data and enforce role-based access.
- Enable CI checks for:
  - Secret scanning (`gitleaks`)
  - Dependency audit (`safety`)
  - Linting and tests
