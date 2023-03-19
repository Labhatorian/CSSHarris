using CSSHarris.Models.DeveloperModels;
using Mailjet.Client.TransactionalEmails;
using Mailjet.Client;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using System.Net.Mail;

namespace CSSHarris.Services;

public class EmailSender : IEmailSender
{
    private readonly ILogger _logger;
    private readonly IConfiguration Configuration;

    public EmailSender(IOptions<AuthMessageSenderOptions> optionsAccessor,
                       ILogger<EmailSender> logger,
                       IConfiguration configuration)
    {
        Options = optionsAccessor.Value;
        _logger = logger;
        Configuration = configuration;
    }

    public AuthMessageSenderOptions Options { get; } //Set with Secret Manager.

    public async Task SendEmailAsync(string toEmail, string subject, string message)
    {
        //We dont use this
        //if (string.IsNullOrEmpty(Options.MailJetSecret) || string.IsNullOrEmpty(Options.MailJetPublicKey))
        //{
        //    throw new Exception("Null Key");
        //}
        await Execute(Options.MailJetSecret, Options.MailJetPublicKey, subject, message, toEmail);
    }

    public async Task Execute(string apiKey, string publickey, string subject, string message, string toEmail)
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
        var response = await client.SendTransactionalEmailAsync(emailtosend);
    }
}