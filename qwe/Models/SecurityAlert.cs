using System;
using System.Collections.Generic;

namespace qwe.Models
{
    public class SecurityAlert
    {
        public int Id { get; set; }
        public string AlertId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public AlertSeverity Severity { get; set; }
        public string Source { get; set; }
        public AlertStatus Status { get; set; }
        public DateTime DetectedAt { get; set; }
        public DateTime? InvestigatedAt { get; set; }
        public DateTime? ResolvedAt { get; set; }
        public string AffectedEndpoint { get; set; }
        public string AffectedUser { get; set; }
        public List<string> RelatedLogs { get; set; }
        public bool IsFalsePositive { get; set; }
        public string RemediationAction { get; set; }
        public string EscalationNotes { get; set; }
        public string InvestigatedBy { get; set; }

        public SecurityAlert()
        {
            RelatedLogs = new List<string>();
            Status = AlertStatus.New;
            DetectedAt = DateTime.UtcNow;
        }
    }

    public enum AlertSeverity
    {
        Critical = 1,
        High = 2,
        Medium = 3,
        Low = 4,
        Informational = 5
    }

    public enum AlertStatus
    {
        New = 1,
        InProgress = 2,
        Remediated = 3,
        Escalated = 4,
        Closed = 5,
        FalsePositive = 6
    }

    public class AlertInvestigationRequest
    {
        public int AlertId { get; set; }
        public string InvestigatedBy { get; set; }
    }

    public class AlertRemediationRequest
    {
        public int AlertId { get; set; }
        public string RemediationType { get; set; }
        public string Notes { get; set; }
        public string RemediatedBy { get; set; }
    }

    public class AlertEscalationRequest
    {
        public int AlertId { get; set; }
        public string EscalationNotes { get; set; }
        public string RecommendedNextSteps { get; set; }
        public string EscalatedBy { get; set; }
    }

    public class SecurityAlertSummary
    {
        public int TotalAlerts { get; set; }
        public int AlertsInvestigated { get; set; }
        public int AlertsRemediated { get; set; }
        public int AlertsEscalated { get; set; }
        public int AlertsClosed { get; set; }
        public int FalsePositives { get; set; }
        public Dictionary<AlertSeverity, int> AlertsBySeverity { get; set; }
        public Dictionary<AlertStatus, int> AlertsByStatus { get; set; }
        public List<string> ActionsTaken { get; set; }
        public List<string> PendingEscalations { get; set; }
        public DateTime GeneratedAt { get; set; }

        public SecurityAlertSummary()
        {
            AlertsBySeverity = new Dictionary<AlertSeverity, int>();
            AlertsByStatus = new Dictionary<AlertStatus, int>();
            ActionsTaken = new List<string>();
            PendingEscalations = new List<string>();
            GeneratedAt = DateTime.UtcNow;
        }
    }
}
