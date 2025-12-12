using System;
using System.Collections.Generic;
using qwe.Models;
using qwe.Services;

namespace qwe.Examples
{
    /// <summary>
    /// Example usage of the Security Alert Investigation and Remediation System
    /// This demonstrates the complete workflow from alert creation to resolution
    /// </summary>
    public class SecurityAlertExample
    {
        public static void DemonstrateWorkflow()
        {
            var alertService = new SecurityAlertService();

            Console.WriteLine("=== Security Alert Investigation and Remediation System Demo ===\n");

            // Step 1: Collect security alerts from SIEM
            Console.WriteLine("Step 1: Collecting alerts from SIEM...");
            var alert1 = alertService.CreateAlert(
                "Suspicious Login Attempt",
                "Multiple failed login attempts from unknown IP 192.168.1.100",
                AlertSeverity.High,
                "SIEM",
                "DESKTOP-123",
                "john.doe@company.com"
            );
            Console.WriteLine($"Created Alert: {alert1.AlertId} - {alert1.Title}");

            var alert2 = alertService.CreateAlert(
                "Malware Detected",
                "Ransomware detected on endpoint",
                AlertSeverity.Critical,
                "Antivirus",
                "LAPTOP-456",
                "jane.smith@company.com"
            );
            Console.WriteLine($"Created Alert: {alert2.AlertId} - {alert2.Title}");

            var alert3 = alertService.CreateAlert(
                "Test Environment Alert",
                "Authorized security scan in test environment",
                AlertSeverity.Low,
                "SecurityScanner",
                "TEST-SERVER-01",
                null
            );
            Console.WriteLine($"Created Alert: {alert3.AlertId} - {alert3.Title}\n");

            // Step 2a: Validate severity and source
            Console.WriteLine("Step 2: Validating alerts...");
            string validationMessage;
            alertService.ValidateAlert(alert1.Id, out validationMessage);
            Console.WriteLine($"Alert {alert1.AlertId}: {validationMessage}");

            // Step 2b: Gather related logs
            Console.WriteLine($"\nGathering logs for Alert {alert1.AlertId}...");
            var logs = new List<string>
            {
                "2024-01-15 10:30:00 - Failed login from 192.168.1.100",
                "2024-01-15 10:30:15 - Failed login from 192.168.1.100",
                "2024-01-15 10:30:30 - Account locked for john.doe@company.com",
                "2024-01-15 10:31:00 - Suspicious activity detected"
            };
            alertService.GatherAlertContext(alert1.Id, logs);
            Console.WriteLine($"Gathered {logs.Count} log entries");

            // Step 2c: Check for false positives
            Console.WriteLine("\nChecking for false positives...");
            string fpReason;
            alertService.CheckFalsePositive(alert3.Id, out fpReason);
            Console.WriteLine($"Alert {alert3.AlertId}: {fpReason}");

            // Step 3: Investigate alerts
            Console.WriteLine("\nStep 3: Starting investigations...");
            alertService.InvestigateAlert(alert1.Id, "SecurityAnalyst1");
            Console.WriteLine($"Alert {alert1.AlertId} assigned to SecurityAnalyst1");

            alertService.InvestigateAlert(alert2.Id, "SecurityAnalyst2");
            Console.WriteLine($"Alert {alert2.AlertId} assigned to SecurityAnalyst2");

            // Step 4a: Remediate alert 1
            Console.WriteLine($"\nStep 4a: Remediating Alert {alert1.AlertId}...");
            string remediationMessage;
            alertService.RemediateAlert(
                alert1.Id,
                "revoke_credentials",
                "User credentials revoked and password reset email sent",
                "Admin",
                out remediationMessage
            );
            Console.WriteLine(remediationMessage);

            // Step 4b: Remediate alert 2
            Console.WriteLine($"\nRemediating Alert {alert2.AlertId}...");
            alertService.RemediateAlert(
                alert2.Id,
                "isolate_endpoint",
                "Endpoint isolated from network, forensic analysis initiated",
                "Admin",
                out remediationMessage
            );
            Console.WriteLine(remediationMessage);

            // Step 4c: Demonstrate escalation for complex issues
            Console.WriteLine("\nDemonstrating escalation workflow...");
            var complexAlert = alertService.CreateAlert(
                "Advanced Persistent Threat Detected",
                "Sophisticated APT activity detected across multiple endpoints",
                AlertSeverity.Critical,
                "ThreatIntelligence",
                null,
                null
            );

            alertService.EscalateAlert(
                complexAlert.Id,
                "Complex APT requires incident response team engagement",
                "1. Engage IR team\n2. Perform threat hunting\n3. Analyze attack vectors",
                "SecurityAnalyst1"
            );
            Console.WriteLine($"Alert {complexAlert.AlertId} escalated to security team");

            // Step 5: Generate summary report
            Console.WriteLine("\nStep 5: Generating summary report...");
            var summary = alertService.GenerateSummaryReport();
            Console.WriteLine("\n=== Security Alert Summary Report ===");
            Console.WriteLine($"Generated: {summary.GeneratedAt:yyyy-MM-dd HH:mm:ss}");
            Console.WriteLine($"\nTotal Alerts: {summary.TotalAlerts}");
            Console.WriteLine($"Investigated: {summary.AlertsInvestigated}");
            Console.WriteLine($"Remediated: {summary.AlertsRemediated}");
            Console.WriteLine($"Escalated: {summary.AlertsEscalated}");
            Console.WriteLine($"False Positives: {summary.FalsePositives}");

            Console.WriteLine("\nAlerts by Severity:");
            foreach (var kvp in summary.AlertsBySeverity.Where(x => x.Value > 0))
            {
                Console.WriteLine($"  {kvp.Key}: {kvp.Value}");
            }

            Console.WriteLine("\nAlerts by Status:");
            foreach (var kvp in summary.AlertsByStatus.Where(x => x.Value > 0))
            {
                Console.WriteLine($"  {kvp.Key}: {kvp.Value}");
            }

            Console.WriteLine("\nActions Taken:");
            foreach (var action in summary.ActionsTaken)
            {
                Console.WriteLine($"  - {action}");
            }

            Console.WriteLine("\nPending Escalations:");
            foreach (var escalation in summary.PendingEscalations)
            {
                Console.WriteLine($"  - {escalation}");
            }

            // Step 6: Close resolved alerts
            Console.WriteLine("\nStep 6: Closing resolved alerts...");
            int closedCount = alertService.CloseResolvedAlerts("Admin");
            Console.WriteLine($"Closed {closedCount} resolved alerts");

            // Display final status
            Console.WriteLine("\n=== Final Alert Status ===");
            var allAlerts = alertService.GetAllAlerts();
            foreach (var alert in allAlerts)
            {
                Console.WriteLine($"{alert.AlertId}: {alert.Title} - Status: {alert.Status}");
            }

            Console.WriteLine("\n=== Demo Complete ===");
        }

        /// <summary>
        /// Example: Automated remediation workflow
        /// </summary>
        public static void AutomatedRemediationExample()
        {
            var alertService = new SecurityAlertService();

            Console.WriteLine("=== Automated Remediation Workflow ===\n");

            // Create an alert
            var alert = alertService.CreateAlert(
                "Brute Force Attack",
                "Brute force attack detected from IP 10.0.0.50",
                AlertSeverity.High,
                "IDS",
                "WEB-SERVER-01",
                null
            );

            // Automated workflow
            string message;
            
            // 1. Validate
            alertService.ValidateAlert(alert.Id, out message);
            
            // 2. Investigate
            alertService.InvestigateAlert(alert.Id, "AutomatedSystem");
            
            // 3. Check false positive
            string reason;
            if (!alertService.CheckFalsePositive(alert.Id, out reason))
            {
                // 4. Apply remediation
                alertService.RemediateAlert(
                    alert.Id,
                    "block_ip",
                    "IP blocked at firewall level",
                    "AutomatedSystem",
                    out message
                );
                
                // 5. Close
                alertService.CloseAlert(alert.Id, "AutomatedSystem");
                
                Console.WriteLine($"Alert {alert.AlertId} automatically remediated and closed");
            }
            else
            {
                Console.WriteLine($"Alert {alert.AlertId} marked as false positive: {reason}");
            }
        }
    }
}
