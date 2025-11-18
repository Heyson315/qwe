using qwe.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json;

namespace qwe.Controllers
{
    public class ChatController : ApiController
    {
        private static readonly List<ChatMessage> chatHistory = new List<ChatMessage>();

        // System prompt to guide the AI's behavior
        private const string SystemPrompt = @"You are a helpful assistant for HHR CPA, a certified public accounting firm. 
You help clients with questions about:
- Tax preparation and filing
- Bookkeeping services
- Payroll processing
- Financial consulting
- Document upload and management
- Scheduling appointments

Be professional, friendly, and helpful. If you don't know something specific about HHR CPA, 
suggest the client contact them directly at contact@hhrcpa.us or call 123-456-7890.";

        // POST: api/Chat
        [HttpPost]
        public async Task<IHttpActionResult> Post([FromBody] ChatRequest request)
        {
            if (string.IsNullOrWhiteSpace(request?.Message))
            {
                return BadRequest("Message cannot be empty");
            }

            try
            {
                // Add user message to history
                var userMessage = new ChatMessage
                {
                    Role = "user",
                    Content = request.Message,
                    Timestamp = DateTime.Now
                };
                chatHistory.Add(userMessage);

                // Get AI response
                string aiResponse = await GetAIResponse(request.Message);

                // Add assistant message to history
                var assistantMessage = new ChatMessage
                {
                    Role = "assistant",
                    Content = aiResponse,
                    Timestamp = DateTime.Now
                };
                chatHistory.Add(assistantMessage);

                // Return response
                var response = new ChatResponse
                {
                    Response = aiResponse,
                    Timestamp = assistantMessage.Timestamp
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // GET: api/Chat/History
        [HttpGet]
        [Route("api/Chat/History")]
        public IHttpActionResult GetHistory()
        {
            return Ok(chatHistory.OrderBy(m => m.Timestamp));
        }

        // DELETE: api/Chat/History
        [HttpDelete]
        [Route("api/Chat/History")]
        public IHttpActionResult ClearHistory()
        {
            chatHistory.Clear();
            return Ok();
        }

        private async Task<string> GetAIResponse(string userMessage)
        {
            // For now, we'll use a simple rule-based response system
            // You can replace this with Azure OpenAI API calls later
            
            string response = GetSmartResponse(userMessage.ToLower());
            
            // Simulate a small delay for realistic feel
            await Task.Delay(500);
            
            return response;
        }

        private string GetSmartResponse(string message)
        {
            // Simple keyword-based responses
            if (message.Contains("tax") || message.Contains("taxes"))
            {
                return "We offer comprehensive tax preparation services for both individuals and businesses. " +
                       "Our team can help with tax planning, filing, and IRS representation. " +
                       "Would you like to schedule a consultation? You can contact us at contact@hhrcpa.us or call 123-456-7890.";
            }
            else if (message.Contains("bookkeeping") || message.Contains("books"))
            {
                return "Our bookkeeping services include transaction recording, account reconciliation, " +
                       "financial statement preparation, and monthly reporting. We use the latest accounting software " +
                       "to keep your financial records accurate and up-to-date.";
            }
            else if (message.Contains("payroll"))
            {
                return "We provide full-service payroll processing including payroll calculation, tax withholding, " +
                       "direct deposits, and compliance reporting. We ensure your employees are paid accurately and on time.";
            }
            else if (message.Contains("document") || message.Contains("upload") || message.Contains("file"))
            {
                return "You can securely upload your documents using our Documents page. " +
                       "We accept PDF, Word, Excel, and image files. Navigate to the Documents section in the menu above " +
                       "to upload your tax forms, receipts, or other financial documents.";
            }
            else if (message.Contains("price") || message.Contains("cost") || message.Contains("fee"))
            {
                return "Our pricing varies based on the services you need and the complexity of your situation. " +
                       "Please contact us at contact@hhrcpa.us or call 123-456-7890 for a personalized quote. " +
                       "We offer competitive rates and transparent pricing with no hidden fees.";
            }
            else if (message.Contains("appointment") || message.Contains("schedule") || message.Contains("meeting"))
            {
                return "To schedule an appointment, please contact us at contact@hhrcpa.us or call 123-456-7890. " +
                       "Our office hours are Monday-Friday, 9 AM - 5 PM. We offer both in-person and virtual consultations.";
            }
            else if (message.Contains("hello") || message.Contains("hi") || message.Contains("hey"))
            {
                return "Hello! Welcome to HHR CPA. I'm here to help answer your questions about our accounting services. " +
                       "How can I assist you today?";
            }
            else if (message.Contains("thank"))
            {
                return "You're welcome! If you have any other questions, feel free to ask. " +
                       "We're here to help with all your accounting needs!";
            }
            else if (message.Contains("service") || message.Contains("what do you"))
            {
                return "HHR CPA offers a comprehensive range of services including:\n" +
                       "• Tax Preparation and Planning\n" +
                       "• Bookkeeping Services\n" +
                       "• Payroll Processing\n" +
                       "• Financial Consulting\n" +
                       "• Business Advisory Services\n\n" +
                       "Which service are you interested in learning more about?";
            }
            else
            {
                return "Thank you for your question. I'd be happy to help! " +
                       "For specific information about your situation, I recommend contacting our team directly at " +
                       "contact@hhrcpa.us or calling 123-456-7890. You can also visit our Services page to learn more " +
                       "about what we offer.";
            }
        }
    }
}
