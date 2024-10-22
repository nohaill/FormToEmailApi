using FormToEmailApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;

namespace FormToEmailApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactFormController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> SubmitForm(
                [FromQuery] string recipientEmail,
                [FromBody] ContactForm contactForm)
        {
            if (contactForm == null || string.IsNullOrEmpty(recipientEmail) || !ModelState.IsValid)
            {
                return BadRequest("Invalid data.");
            }

            try
            {
                using (var mailMessage = new MailMessage())
                {
                    mailMessage.From = new MailAddress("muhammadnohail1@gmail.com"); 
                    mailMessage.To.Add(recipientEmail); 
                    mailMessage.Subject = "New Contact Form Submission";
                    mailMessage.Body = $"Name: {contactForm.Name}\n" +
                                       $"Email: {contactForm.Email}\n" +
                                       $"Phone: {contactForm.PhoneNumber}\n" +
                                       $"Message: {contactForm.Message}";

                    using (var smtpClient = new SmtpClient("smtp.gmail.com"))
                    {
                        smtpClient.Port = 587; 
                        smtpClient.Credentials = new System.Net.NetworkCredential("muhammadnohail1@gmail.com", "wjfz ozge hlrg jpcp");
                        smtpClient.EnableSsl = true;
                        await smtpClient.SendMailAsync(mailMessage);
                    }
                }

                return Ok("Email sent successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
