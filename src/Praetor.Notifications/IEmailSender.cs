namespace Praetor.Notifications;

public interface IEmailSender
{
  Task SendAsync(EmailRequest request, CancellationToken ct = default);
}