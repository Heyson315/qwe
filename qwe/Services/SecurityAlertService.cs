using System;
using System.Collections.Generic;
using System.Linq;
using qwe.Models;

namespace qwe.Services
{
    public class SecurityAlertService
    {
        private static readonly object _lock = new object();
        private static readonly List<SecurityAlert> _alerts = new List<SecurityAlert>();
        private static int _nextId = 1;

        // Step 1: Collect all current security alerts
        public List<SecurityAlert> GetAllAlerts()
        {
            lock (_lock)
            {
                return _alerts.ToList();
            }
        }

        public List<SecurityAlert> GetActiveAlerts()
        {
            lock (_lock)
            {
                return _alerts.Where(a => a.Status != AlertStatus.Closed && 
                                          a.Status != AlertStatus.FalsePositive).ToList();
            }
        }

        public SecurityAlert GetAlertById(int id)
        {
            lock (_lock)
            {
                return _alerts.FirstOrDefault(a => a.Id == id);
            }
        }

        // Simulate collecting alerts from SIEM/security dashboard
        public SecurityAlert CreateAlert(string title, string description, AlertSeverity severity, 
                                        string source, string affectedEndpoint = null, string affectedUser = null)
        {
            lock (_lock)
            {
                var id = _nextId++;
                var alert = new SecurityAlert
                {
                    Id = id,
                    AlertId = $"SEC-{DateTime.UtcNow:yyyyMMdd}-{id:D6}",
                    Title = title,
                    Description = description,
                    Severity = severity,
                    Source = source,
                    AffectedEndpoint = affectedEndpoint,
                    AffectedUser = affectedUser,
                    DetectedAt = DateTime.UtcNow
                };

                _alerts.Add(alert);
                return alert;
            }
        }

        // Step 2: Validate severity and source
        public bool ValidateAlert(int alertId, out string validationMessage)
        {
            lock (_lock)
            {
                var alert = _alerts.FirstOrDefault(a => a.Id == alertId);
                if (alert == null)
                {
                    validationMessage = "Alert not found";
                    return false;
                }

                // Validate severity
                if (!Enum.IsDefined(typeof(AlertSeverity), alert.Severity))
                {
                    validationMessage = "Invalid severity level";
                    return false;
                }

                // Validate source
                if (string.IsNullOrWhiteSpace(alert.Source))
                {
                    validationMessage = "Alert source is missing";
                    return false;
                }

                validationMessage = "Alert validated successfully";
                return true;
            }
        }

        // Step 2: Gather related logs, endpoints, and user activity
        public SecurityAlert GatherAlertContext(int alertId, List<string> logs)
        {
            if (logs == null || logs.Count == 0)
            {
                return GetAlertById(alertId);
            }

            lock (_lock)
            {
                var alert = _alerts.FirstOrDefault(a => a.Id == alertId);
                if (alert != null)
                {
                    // Validate and limit log entries to prevent abuse
                    var validLogs = logs.Where(log => !string.IsNullOrWhiteSpace(log))
                                        .Take(100)
                                        .ToList();
                    alert.RelatedLogs.AddRange(validLogs);
                }
                return alert;
            }
        }

        // Step 2: Check for false positives
        public bool CheckFalsePositive(int alertId, out string reason)
        {
            lock (_lock)
            {
                var alert = _alerts.FirstOrDefault(a => a.Id == alertId);
                if (alert == null)
                {
                    reason = "Alert not found";
                    return false;
                }

                // Implement false positive detection logic
                // This is a simplified version - in production, use ML or rule-based system
                var falsePositiveIndicators = new[]
                {
                    "test environment",
                    "scheduled maintenance",
                    "authorized security scan",
                    "known benign activity"
                };

                var description = alert.Description?.ToLower() ?? "";
                var title = alert.Title?.ToLower() ?? "";
                var isFalsePositive = falsePositiveIndicators.Any(indicator => 
                    description.Contains(indicator) || title.Contains(indicator));

                if (isFalsePositive)
                {
                    alert.IsFalsePositive = true;
                    alert.Status = AlertStatus.FalsePositive;
                    reason = "Alert marked as false positive based on context analysis";
                    return true;
                }

                reason = "Alert appears to be legitimate";
                return false;
            }
        }

        // Start investigation
        public SecurityAlert InvestigateAlert(int alertId, string investigatedBy)
        {
            lock (_lock)
            {
                var alert = _alerts.FirstOrDefault(a => a.Id == alertId);
                if (alert != null && (alert.Status == AlertStatus.New || alert.Status == AlertStatus.Escalated))
                {
                    alert.Status = AlertStatus.InProgress;
                    alert.InvestigatedAt = DateTime.UtcNow;
                    alert.InvestigatedBy = investigatedBy;
                }
                return alert;
            }
        }

        // Step 3: Apply remediation actions
        public bool RemediateAlert(int alertId, string remediationType, string notes, string remediatedBy, out string message)
        {
            lock (_lock)
            {
                var alert = _alerts.FirstOrDefault(a => a.Id == alertId);
                if (alert == null)
                {
                    message = "Alert not found";
                    return false;
                }

                if (alert.IsFalsePositive)
                {
                    message = "Cannot remediate a false positive alert";
                    return false;
                }

                // Apply remediation based on type
                var remediationResult = ApplyRemediation(alert, remediationType, out string remediationError);
                
                if (remediationResult)
                {
                    alert.RemediationAction = $"{remediationType}: {notes}";
                    alert.Status = AlertStatus.Remediated;
                    alert.ResolvedAt = DateTime.UtcNow;
                    message = $"Alert remediated successfully using {remediationType}";
                    return true;
                }

                message = remediationError ?? "Remediation failed";
                return false;
            }
        }

        private bool ApplyRemediation(SecurityAlert alert, string remediationType, out string errorMessage)
        {
            errorMessage = null;
            
            switch (remediationType.ToLower())
            {
                case "isolate_endpoint":
                    // Simulate endpoint isolation
                    if (!string.IsNullOrEmpty(alert.AffectedEndpoint))
                    {
                        // In production: Call endpoint management API
                        return true;
                    }
                    errorMessage = "Cannot isolate endpoint: AffectedEndpoint not specified";
                    return false;

                case "revoke_credentials":
                    // Simulate credential revocation
                    if (!string.IsNullOrEmpty(alert.AffectedUser))
                    {
                        // In production: Call identity management API
                        return true;
                    }
                    errorMessage = "Cannot revoke credentials: AffectedUser not specified";
                    return false;

                case "patch_vulnerability":
                    // Simulate vulnerability patching
                    // In production: Call patch management system
                    return true;

                case "block_ip":
                    // Simulate IP blocking
                    // In production: Call firewall/WAF API
                    return true;

                case "disable_account":
                    // Simulate account disabling
                    if (!string.IsNullOrEmpty(alert.AffectedUser))
                    {
                        // In production: Call user management API
                        return true;
                    }
                    errorMessage = "Cannot disable account: AffectedUser not specified";
                    return false;

                default:
                    errorMessage = $"Unknown remediation type: {remediationType}";
                    return false;
            }
        }

        // Step 4: Escalate to security team
        public SecurityAlert EscalateAlert(int alertId, string escalationNotes, string recommendedNextSteps, string escalatedBy)
        {
            lock (_lock)
            {
                var alert = _alerts.FirstOrDefault(a => a.Id == alertId);
                if (alert != null)
                {
                    alert.Status = AlertStatus.Escalated;
                    alert.EscalationNotes = $"[{escalatedBy}] {escalationNotes}\nRecommended: {recommendedNextSteps}";
                    // In production: Send notification to security team
                }
                return alert;
            }
        }

        // Step 5: Generate summary report
        public SecurityAlertSummary GenerateSummaryReport()
        {
            lock (_lock)
            {
                var summary = new SecurityAlertSummary
                {
                    TotalAlerts = _alerts.Count,
                    AlertsInvestigated = _alerts.Count(a => a.InvestigatedAt.HasValue),
                    AlertsRemediated = _alerts.Count(a => a.Status == AlertStatus.Remediated),
                    AlertsEscalated = _alerts.Count(a => a.Status == AlertStatus.Escalated),
                    AlertsClosed = _alerts.Count(a => a.Status == AlertStatus.Closed),
                    FalsePositives = _alerts.Count(a => a.IsFalsePositive)
                };

                // Group alerts by severity
                foreach (AlertSeverity severity in Enum.GetValues(typeof(AlertSeverity)))
                {
                    var count = _alerts.Count(a => a.Severity == severity);
                    summary.AlertsBySeverity[severity] = count;
                }

                // Group alerts by status
                foreach (AlertStatus status in Enum.GetValues(typeof(AlertStatus)))
                {
                    var count = _alerts.Count(a => a.Status == status);
                    summary.AlertsByStatus[status] = count;
                }

                // Collect actions taken
                summary.ActionsTaken = _alerts
                    .Where(a => !string.IsNullOrEmpty(a.RemediationAction))
                    .Select(a => $"Alert {a.AlertId}: {a.RemediationAction}")
                    .ToList();

                // Collect pending escalations
                summary.PendingEscalations = _alerts
                    .Where(a => a.Status == AlertStatus.Escalated)
                    .Select(a => $"Alert {a.AlertId} - {a.Title}: {a.EscalationNotes}")
                    .ToList();

                return summary;
            }
        }

        // Step 6: Close resolved alerts
        public SecurityAlert CloseAlert(int alertId, string closedBy)
        {
            lock (_lock)
            {
                var alert = _alerts.FirstOrDefault(a => a.Id == alertId);
                if (alert != null && (alert.Status == AlertStatus.Remediated || alert.Status == AlertStatus.FalsePositive))
                {
                    alert.Status = AlertStatus.Closed;
                    alert.ResolvedAt = DateTime.UtcNow;
                }
                return alert;
            }
        }

        // Bulk operations
        public int CloseResolvedAlerts(string closedBy)
        {
            lock (_lock)
            {
                var resolvedAlerts = _alerts.Where(a => 
                    a.Status == AlertStatus.Remediated || 
                    a.Status == AlertStatus.FalsePositive).ToList();

                foreach (var alert in resolvedAlerts)
                {
                    alert.Status = AlertStatus.Closed;
                    alert.ResolvedAt = DateTime.UtcNow;
                }

                return resolvedAlerts.Count;
            }
        }

        // Clear all alerts (for testing purposes)
        public void ClearAllAlerts()
        {
            lock (_lock)
            {
                _alerts.Clear();
                _nextId = 1;
            }
        }
    }
}
