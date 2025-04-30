using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure;
using Azure.Communication.Email;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Praetor.Notifications;

/// <summary>
/// Sends HTML e-mail through <b>Azure Communication Services – E-mail</b>.
/// Expects two configuration keys:
/// <list type="bullet">
///   <item><c>Acs:Conn</c> – connection string for the Communication resource.</item>
///   <item><c>Acs:From</c> – a verified “from” address (e.g. <i>noreply@myfirm.com</i>).</item>
/// </list>
/// </summary>
public sealed class AcsEmailSender(IConfiguration cfg, ILogger<AcsEmailSender> log)
  : IEmailSender
{
  readonly EmailClient client = new(cfg["Acs:Conn"] ?? throw
    new InvalidOperationException("Missing ACS connection string (Acs:Conn)"));

  readonly string from = cfg["Acs:From"] ?? throw
    new InvalidOperationException("Missing ACS from address (Acs:From)");

  public async Task SendAsync(EmailRequest request, CancellationToken ct = default)
  {
    var msg = new EmailMessage(
      senderAddress: from,
      content: new EmailContent(request.Subject)
      {
        Html = request.HtmlBody
      },
      recipients: new EmailRecipients(
        new[] { new EmailAddressInfo(request.To) }));

    try
    {
      Response<SendEmailResult> res =
        await client.SendAsync(WaitUntil.Started, msg, ct);

      log.LogInformation("E-mail {MessageId} queued for {Recipient}",
        res.Value.MessageId, request.To);
    }
    catch(Exception ex)
    {
      log.LogError(ex, "Failed to send e-mail to {Recipient}", request.To);
      throw; // Surface to caller so grain can decide on retry
    }
  }
}
