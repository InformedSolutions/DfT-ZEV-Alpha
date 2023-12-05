using DfT.ZEV.Common.Enumerations;

namespace DfT.ZEV.Common.Models;

/// <summary>
/// Common model definition for notifications (e.g., email, SMS etc.)
/// </summary>
public class Notification
{
    /// <summary>
    /// Gets or sets list of notification recipients.
    /// </summary>
    public List<string> Recipients { get; set; } = new List<string>();

    /// <summary>
    /// Gets or sets list of notification cc recipients.
    /// </summary>
    public List<string> Ccs { get; set; } = new List<string>();

    /// <summary>
    /// Gets or sets list of notification bcc recipients.
    /// </summary>
    public List<string> Bccs { get; set; } = new List<string>();

    /// <summary>
    /// Gets or sets notification template id used for rendering message body.
    /// </summary>
    public Guid TemplateId { get; set; }

    /// <summary>
    /// Gets or sets list of parameters used to replace template placeholders.
    /// </summary>
    public Dictionary<string, string> TemplateParameters { get; set; } = new Dictionary<string, string>();

    /// <summary>
    /// Gets or sets notification type eg. ApplicationResponse, PasswordReset etc.
    /// </summary>
    public NotificationType NotificationType { get; set; }
}