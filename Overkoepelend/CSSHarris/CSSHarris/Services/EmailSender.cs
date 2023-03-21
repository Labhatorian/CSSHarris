using Mailjet.Client;
using Mailjet.Client.TransactionalEmails;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;

namespace CSSHarris.Services;

public class EmailSender : IEmailSender
{
    private readonly IConfiguration Configuration;

    public EmailSender(IOptions<AuthMessageSenderOptions> optionsAccessor,
                       IConfiguration configuration)
    {
        Options = optionsAccessor.Value;
        Configuration = configuration;
    }

    public AuthMessageSenderOptions Options { get; } //Set with Secret Manager but unused

    /// <summary>
    /// Former code unused and removed, go straight to execute
    /// </summary>
    /// <param name="toEmail"></param>
    /// <param name="subject"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    public async Task SendEmailAsync(string toEmail, string subject, string message)
    {
        await Execute(subject, message, toEmail);
    }

    /// <summary>
    /// Send email
    /// </summary>
    /// <param name="apiKey"></param>
    /// <param name="publickey"></param>
    /// <param name="subject"></param>
    /// <param name="message"></param>
    /// <param name="toEmail"></param>
    /// <returns></returns>
    public async Task Execute(string subject, string message, string toEmail)
    {
        MailjetClient client = new(Configuration["PublicKeys:MailJetPublicKey"], Configuration["SecretKeys:MailJetSecret"]);

        // construct your email with builder
        var emailtosend = new TransactionalEmailBuilder()
        .WithFrom(new SendContact("s1168716@student.windesheim.nl"))
        .WithSubject(subject)
        .WithHtmlPart(message)
        .WithTextPart(message)
               .WithTo(new SendContact(toEmail))
               .Build();

        // invoke API to send email
        await client.SendTransactionalEmailAsync(emailtosend);
    }
}