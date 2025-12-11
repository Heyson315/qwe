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

        public SecurityAlertController()
        {
            _alertService = new SecurityAlertService();
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
            catch (Exception ex)
            {
                return InternalServerError(ex);
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
            catch (Exception ex)
            {
                return InternalServerError(ex);
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
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // POST: api/securityalert/create
        [HttpPost]
        [Route("api/securityalert/create")]
        public IHttpActionResult CreateAlert([FromBody] dynamic alertData)
        {
            try
            {
                if (alertData == null)
                {
                    return BadRequest("Alert data is required");
                }

                string title = alertData.title;
                string description = alertData.description;
                string severityStr = alertData.severity;
                string source = alertData.source;
                string affectedEndpoint = alertData.affectedEndpoint;
                string affectedUser = alertData.affectedUser;

                if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(description) ||
                    string.IsNullOrWhiteSpace(severityStr) || string.IsNullOrWhiteSpace(source))
                {
                    return BadRequest("Title, description, severity, and source are required");
                }

                AlertSeverity severity;
                if (!Enum.TryParse(severityStr, true, out severity))
                {
                    return BadRequest("Invalid severity level");
                }

                var alert = _alertService.CreateAlert(title, description, severity, source, 
                                                      affectedEndpoint, affectedUser);

                return Ok(new
                {
                    success = true,
                    message = "Alert created successfully",
                    data = alert
                });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
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
            catch (Exception ex)
            {
                return InternalServerError(ex);
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

                var alert = _alertService.GetAlertById(request.AlertId);
                if (alert == null)
                {
                    return NotFound();
                }

                _alertService.InvestigateAlert(request.AlertId, request.InvestigatedBy ?? "System");

                return Ok(new
                {
                    success = true,
                    message = "Alert investigation started",
                    data = _alertService.GetAlertById(request.AlertId)
                });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
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
                }

                return Ok(new
                {
                    success = true,
                    message = "Logs gathered successfully",
                    data = _alertService.GetAlertById(id)
                });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
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

                return Ok(new
                {
                    success = true,
                    isFalsePositive = isFalsePositive,
                    reason = reason,
                    data = _alertService.GetAlertById(id)
                });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
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
            catch (Exception ex)
            {
                return InternalServerError(ex);
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

                var alert = _alertService.GetAlertById(request.AlertId);
                if (alert == null)
                {
                    return NotFound();
                }

                _alertService.EscalateAlert(
                    request.AlertId,
                    request.EscalationNotes ?? "Requires security team review",
                    request.RecommendedNextSteps ?? "Further investigation needed",
                    request.EscalatedBy ?? "System");

                return Ok(new
                {
                    success = true,
                    message = "Alert escalated to security team",
                    data = _alertService.GetAlertById(request.AlertId)
                });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
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
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // POST: api/securityalert/close/{id}
        [HttpPost]
        [Route("api/securityalert/close/{id}")]
        public IHttpActionResult CloseAlert(int id, [FromBody] dynamic request)
        {
            try
            {
                var alert = _alertService.GetAlertById(id);
                if (alert == null)
                {
                    return NotFound();
                }

                string closedBy = request?.closedBy ?? "System";
                _alertService.CloseAlert(id, closedBy);

                return Ok(new
                {
                    success = true,
                    message = "Alert closed successfully",
                    data = _alertService.GetAlertById(id)
                });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // POST: api/securityalert/closeresolved
        [HttpPost]
        [Route("api/securityalert/closeresolved")]
        public IHttpActionResult CloseResolvedAlerts([FromBody] dynamic request)
        {
            try
            {
                string closedBy = request?.closedBy ?? "System";
                var count = _alertService.CloseResolvedAlerts(closedBy);

                return Ok(new
                {
                    success = true,
                    message = $"{count} resolved alerts closed",
                    count = count
                });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
