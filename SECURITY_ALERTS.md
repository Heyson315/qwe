# Security Alert Investigation and Remediation System

## Overview

This system provides comprehensive security alert management capabilities for investigating, validating, remediating, and escalating security alerts across the environment.

## Features

### 1. Alert Collection
- Collect security alerts from SIEM or security dashboard
- Support for multiple alert sources (SIEM, Firewall, Antivirus, IDS/IPS, etc.)
- Automatic alert ID generation with timestamp
- Severity classification (Critical, High, Medium, Low, Informational)

### 2. Alert Investigation
- Validate alert severity and source
- Gather related logs, endpoints, and user activity
- Check for false positives using pattern matching
- Track investigation status and investigator

### 3. Alert Remediation
- Multiple remediation types supported:
  - **Isolate Endpoint** - Quarantine compromised endpoints
  - **Revoke Credentials** - Disable compromised user accounts
  - **Patch Vulnerability** - Apply security patches
  - **Block IP** - Block malicious IP addresses
  - **Disable Account** - Disable user accounts
- Document remediation actions taken
- Track remediation timestamps

### 4. Alert Escalation
- Escalate unresolvable alerts to security team
- Include full context (alert details, logs, recommended steps)
- Track escalation notes and recommendations

### 5. Summary Reporting
- Generate comprehensive security reports
- Metrics included:
  - Total alerts investigated
  - Alerts remediated vs escalated
  - False positive count
  - Alerts by severity and status
  - Actions taken
  - Pending escalations

### 6. Alert Management
- Close resolved alerts
- Bulk close all resolved alerts
- Update ticketing system status

## API Endpoints

### Get All Alerts
```
GET /api/securityalert
```
Returns all security alerts.

**Response:**
```json
{
  "success": true,
  "count": 5,
  "data": [...]
}
```

### Get Active Alerts
```
GET /api/securityalert/active
```
Returns only active (non-closed, non-false positive) alerts.

### Get Alert by ID
```
GET /api/securityalert/{id}
```
Returns a specific alert by ID.

### Create Alert
```
POST /api/securityalert/create
```

**Request Body:**
```json
{
  "title": "Suspicious Login Attempt",
  "description": "Multiple failed login attempts from unknown IP",
  "severity": "High",
  "source": "SIEM",
  "affectedEndpoint": "DESKTOP-123",
  "affectedUser": "user@company.com"
}
```

**Severity Levels:** Critical, High, Medium, Low, Informational

### Validate Alert
```
POST /api/securityalert/validate/{id}
```
Validates alert severity and source.

### Investigate Alert
```
POST /api/securityalert/investigate
```

**Request Body:**
```json
{
  "alertId": 1,
  "investigatedBy": "SecurityAnalyst1"
}
```

### Gather Logs
```
POST /api/securityalert/gatherlogs/{id}
```

**Request Body:**
```json
[
  "2024-01-15 10:30:00 - Failed login attempt from 192.168.1.100",
  "2024-01-15 10:30:15 - Failed login attempt from 192.168.1.100",
  "2024-01-15 10:30:30 - Account locked"
]
```

### Check False Positive
```
POST /api/securityalert/checkfalsepositive/{id}
```
Analyzes alert to determine if it's a false positive.

### Remediate Alert
```
POST /api/securityalert/remediate
```

**Request Body:**
```json
{
  "alertId": 1,
  "remediationType": "isolate_endpoint",
  "notes": "Endpoint isolated from network pending forensic analysis",
  "remediatedBy": "Admin"
}
```

**Remediation Types:**
- `isolate_endpoint` - Quarantine the affected endpoint
- `revoke_credentials` - Revoke user credentials
- `patch_vulnerability` - Apply security patch
- `block_ip` - Block IP address
- `disable_account` - Disable user account

### Escalate Alert
```
POST /api/securityalert/escalate
```

**Request Body:**
```json
{
  "alertId": 1,
  "escalationNotes": "Advanced persistent threat detected - requires incident response team",
  "recommendedNextSteps": "Engage IR team for forensic analysis and threat hunting",
  "escalatedBy": "SecurityAnalyst1"
}
```

### Generate Summary Report
```
GET /api/securityalert/summary
```

**Response:**
```json
{
  "success": true,
  "data": {
    "totalAlerts": 25,
    "alertsInvestigated": 23,
    "alertsRemediated": 18,
    "alertsEscalated": 3,
    "alertsClosed": 20,
    "falsePositives": 2,
    "alertsBySeverity": {
      "Critical": 5,
      "High": 8,
      "Medium": 7,
      "Low": 3,
      "Informational": 2
    },
    "alertsByStatus": {
      "New": 2,
      "InProgress": 3,
      "Remediated": 15,
      "Escalated": 3,
      "Closed": 20,
      "FalsePositive": 2
    },
    "actionsTaken": [...],
    "pendingEscalations": [...],
    "generatedAt": "2024-01-15T10:30:00Z"
  }
}
```

### Close Alert
```
POST /api/securityalert/close/{id}
```

**Request Body:**
```json
{
  "closedBy": "Admin"
}
```

### Close Resolved Alerts (Bulk)
```
POST /api/securityalert/closeresolved
```

**Request Body:**
```json
{
  "closedBy": "Admin"
}
```

Closes all alerts with status "Remediated" or "FalsePositive".

## Usage Workflow

### Standard Investigation Flow

1. **Create Alert** - Alert detected by security tools
   ```
   POST /api/securityalert/create
   ```

2. **Validate Alert** - Verify severity and source
   ```
   POST /api/securityalert/validate/{id}
   ```

3. **Investigate Alert** - Start investigation
   ```
   POST /api/securityalert/investigate
   ```

4. **Gather Context** - Collect logs and evidence
   ```
   POST /api/securityalert/gatherlogs/{id}
   ```

5. **Check False Positive** - Analyze for false positives
   ```
   POST /api/securityalert/checkfalsepositive/{id}
   ```

6. **Decision Point:**
   - If false positive → Alert automatically marked and can be closed
   - If remediable → Apply remediation
   - If not remediable → Escalate

7a. **Remediate** (if possible)
   ```
   POST /api/securityalert/remediate
   ```

7b. **Escalate** (if remediation not possible)
   ```
   POST /api/securityalert/escalate
   ```

8. **Close Alert** - Close resolved alerts
   ```
   POST /api/securityalert/close/{id}
   ```

9. **Generate Report** - Create summary report
   ```
   GET /api/securityalert/summary
   ```

## Alert Status Flow

```
New → InProgress → [Remediated|Escalated|FalsePositive] → Closed
```

## Security Considerations

⚠️ **Important Security Notes:**

1. **Authentication Required** - In production, all endpoints should require authentication
2. **Authorization** - Implement role-based access control (RBAC)
3. **Audit Logging** - Log all alert modifications and actions
4. **Rate Limiting** - Prevent API abuse
5. **Input Validation** - Validate all user inputs
6. **Secure Communication** - Use HTTPS only
7. **Data Encryption** - Encrypt sensitive alert data at rest

## Integration Examples

### Example 1: Automated Alert Processing

```csharp
// Pseudo-code for automated alert handling
var alert = CreateAlert("Malware Detected", "Ransomware on endpoint", 
                       AlertSeverity.Critical, "Antivirus", "DESKTOP-123");

ValidateAlert(alert.Id);
InvestigateAlert(alert.Id, "AutomatedSystem");

if (!IsFalsePositive(alert.Id))
{
    RemediateAlert(alert.Id, "isolate_endpoint", 
                  "Endpoint automatically isolated", "System");
}

CloseAlert(alert.Id);
```

### Example 2: Security Dashboard Integration

```javascript
// JavaScript example for dashboard
async function loadSecurityDashboard() {
    // Get active alerts
    const response = await fetch('/api/securityalert/active');
    const data = await response.json();
    
    // Display alerts
    displayAlerts(data.data);
    
    // Get summary
    const summary = await fetch('/api/securityalert/summary');
    const summaryData = await summary.json();
    
    // Display metrics
    displayMetrics(summaryData.data);
}
```

## Testing

Unit tests are provided in `qwe.Tests/Services/SecurityAlertServiceTests.cs`

Run tests using:
```bash
dotnet test
```

Or in Visual Studio:
- Test → Run All Tests

## Future Enhancements

- [ ] Database persistence (currently in-memory)
- [ ] Real-time notifications (email, Slack, Teams)
- [ ] Integration with ticketing systems (Jira, ServiceNow)
- [ ] Machine learning for false positive detection
- [ ] Automated remediation workflows
- [ ] SOAR platform integration
- [ ] Threat intelligence feed integration
- [ ] Custom remediation playbooks
- [ ] Alert correlation and grouping
- [ ] Compliance reporting (SOC 2, ISO 27001, etc.)

## Support

For questions or issues, contact the security team or open an issue in the repository.

## License

© 2025 HHR CPA. All rights reserved.
