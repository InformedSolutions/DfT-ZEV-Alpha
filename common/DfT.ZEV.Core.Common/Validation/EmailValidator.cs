using System.Net.Mail;
using System.Text.RegularExpressions;

namespace DfT.ZEV.Common.Validation;

/// <summary>
/// Class used for validating and parsing email addresses.
/// </summary>
public static class EmailValidator
{
    /// <summary>
    /// Validates email address.
    /// </summary>
    /// <param name="email">Email to be validated.</param>
    /// <returns>Returns true if email is valid.</returns>
    public static bool IsValid(string email)
    {
        if (email == null)
        {
            return true;
        }

        var rgx = new Regex(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$");

        return MailAddress.TryCreate(email, out var _) && rgx.IsMatch(email);
    }

    /// <summary>
    /// Parse comma separated emails string to list of emails.
    /// </summary>
    /// <param name="commaSeparatedEmails">Comma separated emails string.</param>
    /// <returns>List of emails.</returns>
    public static List<string> ParseCommaSeparatedEmails(string commaSeparatedEmails)
    {
        return string.IsNullOrEmpty(commaSeparatedEmails) ? null : commaSeparatedEmails
            .Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .ToList();
    }
}
