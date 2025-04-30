using Microsoft.Extensions.Logging;
using Praetor.Contracts;
using Praetor.Contracts.Grains;
using Praetor.Enums;
using Praetor.Grains.State;
using Praetor.Notifications;

namespace Praetor.Grains;

public sealed class WaiverGrain(IEmailSender email, ILogger<WaiverGrain> log)
  : Grain<WaiverStateRecord>, IWaiverGrain
{
  public async Task RequestAsync(Guid conflictId, string toEmail)
  {
    State = new WaiverStateRecord(conflictId, WaiverState.Pending, DateTimeOffset.UtcNow, null, toEmail);
    await WriteStateAsync();

    var magicLink = $"{RequestContext.Get("BaseUrl")}/respondents/{this.GetPrimaryKey()}";
    await email.SendAsync(new Notifications.EmailRequest(To: toEmail, Subject: "Conflict-waiver request", HtmlBody: $"Click <a href=\"{magicLink}\">here</a> to grant or refuse."));
  }

  public Task GrantAsync() => Transition(WaiverState.Granted);
  public Task RefuseAsync() => Transition(WaiverState.Refused);

  async Task Transition(WaiverState next)
  {
    State = State with { Status = next, RespondedUtc = DateTimeOffset.UtcNow };
    await WriteStateAsync();
  }

  public Task<WaiverSnapshot> GetAsync() =>
    Task.FromResult(new WaiverSnapshot(
      this.GetPrimaryKey(), State.ConflictId,
      State.Status, State.RequestedUtc, State.RespondedUtc));
}