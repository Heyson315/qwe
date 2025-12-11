using Microsoft.VisualStudio.TestTools.UnitTesting;
using qwe.Models;
using qwe.Services;
using System;
using System.Linq;

namespace qwe.Tests.Services
{
    [TestClass]
    public class SecurityAlertServiceTests
    {
        private SecurityAlertService _service;

        [TestInitialize]
        public void Setup()
        {
            _service = new SecurityAlertService();
            _service.ClearAllAlerts();
        }

        [TestCleanup]
        public void Cleanup()
        {
            _service.ClearAllAlerts();
        }

        [TestMethod]
        public void CreateAlert_ShouldCreateAlertSuccessfully()
        {
            // Arrange
            var title = "Suspicious Login Attempt";
            var description = "Multiple failed login attempts from unknown IP";
            var severity = AlertSeverity.High;
            var source = "SIEM";

            // Act
            var alert = _service.CreateAlert(title, description, severity, source);

            // Assert
            Assert.IsNotNull(alert);
            Assert.AreEqual(title, alert.Title);
            Assert.AreEqual(description, alert.Description);
            Assert.AreEqual(severity, alert.Severity);
            Assert.AreEqual(source, alert.Source);
            Assert.AreEqual(AlertStatus.New, alert.Status);
            Assert.IsFalse(alert.IsFalsePositive);
        }

        [TestMethod]
        public void GetAllAlerts_ShouldReturnAllAlerts()
        {
            // Arrange
            _service.CreateAlert("Alert 1", "Description 1", AlertSeverity.High, "SIEM");
            _service.CreateAlert("Alert 2", "Description 2", AlertSeverity.Medium, "Firewall");

            // Act
            var alerts = _service.GetAllAlerts();

            // Assert
            Assert.AreEqual(2, alerts.Count);
        }

        [TestMethod]
        public void GetActiveAlerts_ShouldReturnOnlyActiveAlerts()
        {
            // Arrange
            var alert1 = _service.CreateAlert("Alert 1", "Description 1", AlertSeverity.High, "SIEM");
            var alert2 = _service.CreateAlert("Alert 2", "Description 2", AlertSeverity.Medium, "Firewall");
            _service.CloseAlert(alert1.Id, "Admin");

            // Act
            var activeAlerts = _service.GetActiveAlerts();

            // Assert
            Assert.AreEqual(1, activeAlerts.Count);
            Assert.AreEqual(alert2.Id, activeAlerts[0].Id);
        }

        [TestMethod]
        public void ValidateAlert_ShouldValidateCorrectly()
        {
            // Arrange
            var alert = _service.CreateAlert("Test Alert", "Test Description", AlertSeverity.Critical, "SIEM");

            // Act
            string message;
            var isValid = _service.ValidateAlert(alert.Id, out message);

            // Assert
            Assert.IsTrue(isValid);
            Assert.AreEqual("Alert validated successfully", message);
        }

        [TestMethod]
        public void ValidateAlert_WithInvalidId_ShouldReturnFalse()
        {
            // Act
            string message;
            var isValid = _service.ValidateAlert(999, out message);

            // Assert
            Assert.IsFalse(isValid);
            Assert.AreEqual("Alert not found", message);
        }

        [TestMethod]
        public void GatherAlertContext_ShouldAddLogs()
        {
            // Arrange
            var alert = _service.CreateAlert("Test Alert", "Test Description", AlertSeverity.High, "SIEM");
            var logs = new System.Collections.Generic.List<string> { "Log 1", "Log 2", "Log 3" };

            // Act
            _service.GatherAlertContext(alert.Id, logs);

            // Assert
            var updatedAlert = _service.GetAlertById(alert.Id);
            Assert.AreEqual(3, updatedAlert.RelatedLogs.Count);
            Assert.IsTrue(updatedAlert.RelatedLogs.Contains("Log 1"));
        }

        [TestMethod]
        public void CheckFalsePositive_WithTestEnvironment_ShouldReturnTrue()
        {
            // Arrange
            var alert = _service.CreateAlert("Test Alert", "This is a test environment alert", 
                                            AlertSeverity.Low, "SIEM");

            // Act
            string reason;
            var isFalsePositive = _service.CheckFalsePositive(alert.Id, out reason);

            // Assert
            Assert.IsTrue(isFalsePositive);
            var updatedAlert = _service.GetAlertById(alert.Id);
            Assert.IsTrue(updatedAlert.IsFalsePositive);
            Assert.AreEqual(AlertStatus.FalsePositive, updatedAlert.Status);
        }

        [TestMethod]
        public void CheckFalsePositive_WithLegitimateAlert_ShouldReturnFalse()
        {
            // Arrange
            var alert = _service.CreateAlert("Malware Detected", 
                                            "Ransomware detected on endpoint", 
                                            AlertSeverity.Critical, "Antivirus");

            // Act
            string reason;
            var isFalsePositive = _service.CheckFalsePositive(alert.Id, out reason);

            // Assert
            Assert.IsFalse(isFalsePositive);
            Assert.AreEqual("Alert appears to be legitimate", reason);
        }

        [TestMethod]
        public void InvestigateAlert_ShouldUpdateStatus()
        {
            // Arrange
            var alert = _service.CreateAlert("Test Alert", "Description", AlertSeverity.High, "SIEM");

            // Act
            _service.InvestigateAlert(alert.Id, "SecurityAnalyst1");

            // Assert
            var updatedAlert = _service.GetAlertById(alert.Id);
            Assert.AreEqual(AlertStatus.InProgress, updatedAlert.Status);
            Assert.IsNotNull(updatedAlert.InvestigatedAt);
            Assert.AreEqual("SecurityAnalyst1", updatedAlert.InvestigatedBy);
        }

        [TestMethod]
        public void RemediateAlert_WithIsolateEndpoint_ShouldSucceed()
        {
            // Arrange
            var alert = _service.CreateAlert("Compromised Endpoint", "Malware detected", 
                                            AlertSeverity.Critical, "Antivirus", 
                                            "DESKTOP-123", "user@company.com");

            // Act
            string message;
            var success = _service.RemediateAlert(alert.Id, "isolate_endpoint", 
                                                 "Endpoint isolated from network", 
                                                 "Admin", out message);

            // Assert
            Assert.IsTrue(success);
            var updatedAlert = _service.GetAlertById(alert.Id);
            Assert.AreEqual(AlertStatus.Remediated, updatedAlert.Status);
            Assert.IsNotNull(updatedAlert.ResolvedAt);
            Assert.IsNotNull(updatedAlert.RemediationAction);
        }

        [TestMethod]
        public void RemediateAlert_WithRevokeCredentials_ShouldSucceed()
        {
            // Arrange
            var alert = _service.CreateAlert("Compromised Account", "Suspicious activity", 
                                            AlertSeverity.High, "SIEM", 
                                            null, "user@company.com");

            // Act
            string message;
            var success = _service.RemediateAlert(alert.Id, "revoke_credentials", 
                                                 "User credentials revoked", 
                                                 "Admin", out message);

            // Assert
            Assert.IsTrue(success);
            var updatedAlert = _service.GetAlertById(alert.Id);
            Assert.AreEqual(AlertStatus.Remediated, updatedAlert.Status);
        }

        [TestMethod]
        public void RemediateAlert_OnFalsePositive_ShouldFail()
        {
            // Arrange
            var alert = _service.CreateAlert("Test Alert", "test environment alert", 
                                            AlertSeverity.Low, "SIEM");
            string falsePositiveReason;
            _service.CheckFalsePositive(alert.Id, out falsePositiveReason);

            // Act
            string message;
            var success = _service.RemediateAlert(alert.Id, "isolate_endpoint", 
                                                 "Test", "Admin", out message);

            // Assert
            Assert.IsFalse(success);
            Assert.AreEqual("Cannot remediate a false positive alert", message);
        }

        [TestMethod]
        public void EscalateAlert_ShouldUpdateStatus()
        {
            // Arrange
            var alert = _service.CreateAlert("Complex Attack", "Advanced persistent threat", 
                                            AlertSeverity.Critical, "SIEM");

            // Act
            _service.EscalateAlert(alert.Id, "Requires advanced analysis", 
                                  "Engage incident response team", "Analyst1");

            // Assert
            var updatedAlert = _service.GetAlertById(alert.Id);
            Assert.AreEqual(AlertStatus.Escalated, updatedAlert.Status);
            Assert.IsNotNull(updatedAlert.EscalationNotes);
            Assert.IsTrue(updatedAlert.EscalationNotes.Contains("Analyst1"));
            Assert.IsTrue(updatedAlert.EscalationNotes.Contains("incident response team"));
        }

        [TestMethod]
        public void GenerateSummaryReport_ShouldIncludeAllMetrics()
        {
            // Arrange
            var alert1 = _service.CreateAlert("Alert 1", "Description", AlertSeverity.Critical, "SIEM");
            var alert2 = _service.CreateAlert("Alert 2", "Description", AlertSeverity.High, "Firewall");
            var alert3 = _service.CreateAlert("Alert 3", "test environment", AlertSeverity.Low, "SIEM");

            _service.InvestigateAlert(alert1.Id, "Analyst1");
            string msg;
            _service.RemediateAlert(alert1.Id, "patch_vulnerability", "Patched", "Admin", out msg);
            
            string reason;
            _service.CheckFalsePositive(alert3.Id, out reason);

            // Act
            var summary = _service.GenerateSummaryReport();

            // Assert
            Assert.AreEqual(3, summary.TotalAlerts);
            Assert.AreEqual(1, summary.AlertsInvestigated);
            Assert.AreEqual(1, summary.AlertsRemediated);
            Assert.AreEqual(1, summary.FalsePositives);
            Assert.IsTrue(summary.AlertsBySeverity.ContainsKey(AlertSeverity.Critical));
            Assert.IsTrue(summary.AlertsByStatus.ContainsKey(AlertStatus.Remediated));
            Assert.IsTrue(summary.ActionsTaken.Count > 0);
        }

        [TestMethod]
        public void CloseAlert_ShouldCloseRemediatedAlert()
        {
            // Arrange
            var alert = _service.CreateAlert("Alert", "Description", AlertSeverity.High, "SIEM", "endpoint1");
            string msg;
            _service.RemediateAlert(alert.Id, "isolate_endpoint", "Isolated", "Admin", out msg);

            // Act
            _service.CloseAlert(alert.Id, "Admin");

            // Assert
            var updatedAlert = _service.GetAlertById(alert.Id);
            Assert.AreEqual(AlertStatus.Closed, updatedAlert.Status);
            Assert.IsNotNull(updatedAlert.ResolvedAt);
        }

        [TestMethod]
        public void CloseResolvedAlerts_ShouldCloseMultipleAlerts()
        {
            // Arrange
            var alert1 = _service.CreateAlert("Alert 1", "Desc", AlertSeverity.High, "SIEM", "endpoint1");
            var alert2 = _service.CreateAlert("Alert 2", "test environment", AlertSeverity.Low, "SIEM");
            
            string msg, reason;
            _service.RemediateAlert(alert1.Id, "isolate_endpoint", "Isolated", "Admin", out msg);
            _service.CheckFalsePositive(alert2.Id, out reason);

            // Act
            var count = _service.CloseResolvedAlerts("Admin");

            // Assert
            Assert.AreEqual(2, count);
            var closedAlerts = _service.GetAllAlerts().Where(a => a.Status == AlertStatus.Closed).ToList();
            Assert.AreEqual(2, closedAlerts.Count);
        }
    }
}
