using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using qwe.Models;
using qwe.Services;

namespace qwe.Controllers
{
    public class SecurityAlertController : ApiController
    {
        private readonly SecurityAlertService _alertService;

        public SecurityAlertController(SecurityAlertService alertService)
        {
            _alertService = alertService;
        }

        // GET: api/securityalert
        [HttpGet]
        [Route("api/securityalert")]
        public IHttpActionResult GetAllAlerts()
        {
            try
            {
                var alerts = _alertService.GetAllAlerts();
                return Ok(new
                {
                    success = true,
                    count = alerts.Count,
                    data = alerts
                });
            }
            catch (Exception)
            {
                return InternalServerError(new Exception("An error occurred while retrieving all alerts"));
            }
        }

        // GET: api/securityalert/active
        [HttpGet]
        [Route("api/securityalert/active")]
        public IHttpActionResult GetActiveAlerts()
        {
            try
            {
                var alerts = _alertService.GetActiveAlerts();
                return Ok(new
                {
                    success = true,
                    count = alerts.Count,
                    data = alerts
                });
            }
            catch (Exception)
            {
                return InternalServerError(new Exception("An error occurred while retrieving active alerts"));
            }
        }

        // GET: api/securityalert/{id}
        [HttpGet]
        [Route("api/securityalert/{id}")]
        public IHttpActionResult GetAlert(int id)
        {
            try
            {
                var alert = _alertService.GetAlertById(id);
                if (alert == null)
                {
                    return NotFound();
                }

                return Ok(new
                {
                    success = true,
                    data = alert
                });
            }
            catch (Exception)
            {
                return InternalServerError(new Exception("An error occurred while retrieving the alert"));
            }
        }

        // POST: api/securityalert/create
        [HttpPost]
        [Route("api/securityalert/create")]
        public IHttpActionResult CreateAlert([FromBody] CreateAlertRequest request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest("Alert data is required");
                }

                if (string.IsNullOrWhiteSpace(request.Title) || string.IsNullOrWhiteSpace(request.Description) ||
                    string.IsNullOrWhiteSpace(request.Severity) || string.IsNullOrWhiteSpace(request.Source))
                {
                    return BadRequest("Title, description, severity, and source are required");
                }

                AlertSeverity severity;
                if (!Enum.TryParse(request.Severity, true, out severity))
                {
                    return BadRequest("Invalid severity level. Must be: Critical, High, Medium, Low, or Informational");
                }

                var alert = _alertService.CreateAlert(request.Title, request.Description, severity, request.Source, 
                                                      request.AffectedEndpoint, request.AffectedUser);

                return Ok(new
                {
                    success = true,
                    message = "Alert created successfully",
                    data = alert
                });
            }
            catch (Exception)
            {
                return InternalServerError(new Exception("An error occurred while creating the alert"));
            }
        }

        // POST: api/securityalert/validate/{id}
        [HttpPost]
        [Route("api/securityalert/validate/{id}")]
        public IHttpActionResult ValidateAlert(int id)
        {
            try
            {
                string validationMessage;
                var isValid = _alertService.ValidateAlert(id, out validationMessage);

                return Ok(new
                {
                    success = true,
                    isValid = isValid,
                    message = validationMessage
                });
            }
            catch (Exception)
            {
                return InternalServerError(new Exception("An error occurred while validating the alert"));
            }
        }

        // POST: api/securityalert/investigate
        [HttpPost]
        [Route("api/securityalert/investigate")]
        public IHttpActionResult InvestigateAlert([FromBody] AlertInvestigationRequest request)
        {
            try
            {
                if (request == null || request.AlertId <= 0)
                {
                    return BadRequest("Valid alert ID is required");
                }

                var alert = _alertService.InvestigateAlert(request.AlertId, request.InvestigatedBy ?? "System");
                
                if (alert == null)
                {
                    return NotFound();
                }

                return Ok(new
                {
                    success = true,
                    message = "Alert investigation started",
                    data = alert
                });
            }
            catch (Exception)
            {
                return InternalServerError(new Exception("An error occurred while investigating the alert"));
            }
        }

        // POST: api/securityalert/gatherlogs/{id}
        [HttpPost]
        [Route("api/securityalert/gatherlogs/{id}")]
        public IHttpActionResult GatherLogs(int id, [FromBody] List<string> logs)
        {
            try
            {
                var alert = _alertService.GetAlertById(id);
                if (alert == null)
                {
                    return NotFound();
                }

                if (logs != null && logs.Any())
                {
                    _alertService.GatherAlertContext(id, logs);
                    // Refresh alert after gathering logs
                    alert = _alertService.GetAlertById(id);
                }

                return Ok(new
                {
                    success = true,
                    message = "Logs gathered successfully",
                    data = alert
                });
            }
            catch (Exception)
            {
                return InternalServerError(new Exception("An error occurred while gathering logs"));
            }
        }

        // POST: api/securityalert/checkfalsepositive/{id}
        [HttpPost]
        [Route("api/securityalert/checkfalsepositive/{id}")]
        public IHttpActionResult CheckFalsePositive(int id)
        {
            try
            {
                var alert = _alertService.GetAlertById(id);
                if (alert == null)
                {
                    return NotFound();
                }

                string reason;
                var isFalsePositive = _alertService.CheckFalsePositive(id, out reason);
                
                // Refresh alert after checking false positive (status may have changed)
                alert = _alertService.GetAlertById(id);

                return Ok(new
                {
                    success = true,
                    isFalsePositive = isFalsePositive,
                    reason = reason,
                    data = alert
                });
            }
            catch (Exception)
            {
                return InternalServerError(new Exception("An error occurred while checking for false positive"));
            }
        }

        // POST: api/securityalert/remediate
        [HttpPost]
        [Route("api/securityalert/remediate")]
        public IHttpActionResult RemediateAlert([FromBody] AlertRemediationRequest request)
        {
            try
            {
                if (request == null || request.AlertId <= 0)
                {
                    return BadRequest("Valid alert ID is required");
                }

                if (string.IsNullOrWhiteSpace(request.RemediationType))
                {
                    return BadRequest("Remediation type is required");
                }

                string message;
                var success = _alertService.RemediateAlert(
                    request.AlertId, 
                    request.RemediationType, 
                    request.Notes ?? "", 
                    request.RemediatedBy ?? "System",
                    out message);

                if (!success)
                {
                    return BadRequest(message);
                }

                return Ok(new
                {
                    success = true,
                    message = message,
                    data = _alertService.GetAlertById(request.AlertId)
                });
            }
            catch (Exception)
            {
                return InternalServerError(new Exception("An error occurred while remediating the alert"));
            }
        }

        // POST: api/securityalert/escalate
        [HttpPost]
        [Route("api/securityalert/escalate")]
        public IHttpActionResult EscalateAlert([FromBody] AlertEscalationRequest request)
        {
            try
            {
                if (request == null || request.AlertId <= 0)
                {
                    return BadRequest("Valid alert ID is required");
                }

                var alert = _alertService.EscalateAlert(
                    request.AlertId,
                    request.EscalationNotes ?? "Requires security team review",
                    request.RecommendedNextSteps ?? "Further investigation needed",
                    request.EscalatedBy ?? "System");

                if (alert == null)
                {
                    return NotFound();
                }

                return Ok(new
                {
                    success = true,
                    message = "Alert escalated to security team",
                    data = alert
                });
            }
            catch (Exception)
            {
                return InternalServerError(new Exception("An error occurred while escalating the alert"));
            }
        }

        // GET: api/securityalert/summary
        [HttpGet]
        [Route("api/securityalert/summary")]
        public IHttpActionResult GetSummaryReport()
        {
            try
            {
                var summary = _alertService.GenerateSummaryReport();

                return Ok(new
                {
                    success = true,
                    data = summary
                });
            }
            catch (Exception)
            {
                return InternalServerError(new Exception("An error occurred while generating the summary report"));
            }
        }

        // POST: api/securityalert/close/{id}
        [HttpPost]
        [Route("api/securityalert/close/{id}")]
        public IHttpActionResult CloseAlert(int id, [FromBody] CloseAlertRequest request)
        {
            try
            {
                var alert = _alertService.GetAlertById(id);
                if (alert == null)
                {
                    return NotFound();
                }

                string closedBy = request?.ClosedBy ?? "System";
                _alertService.CloseAlert(id, closedBy);
                
                // Refresh alert after closing
                alert = _alertService.GetAlertById(id);

                return Ok(new
                {
                    success = true,
                    message = "Alert closed successfully",
                    data = alert
                });
            }
            catch (Exception)
            {
                return InternalServerError(new Exception("An error occurred while closing the alert"));
            }
        }

        // POST: api/securityalert/closeresolved
        [HttpPost]
        [Route("api/securityalert/closeresolved")]
        public IHttpActionResult CloseResolvedAlerts([FromBody] CloseAlertRequest request)
        {
            try
            {
                string closedBy = request?.ClosedBy ?? "System";
                var count = _alertService.CloseResolvedAlerts(closedBy);

                return Ok(new
                {
                    success = true,
                    message = $"{count} resolved alerts closed",
                    count = count
                });
            }
            catch (Exception)
            {
                return InternalServerError(new Exception("An error occurred while closing resolved alerts"));
            }
        }
    }
}
