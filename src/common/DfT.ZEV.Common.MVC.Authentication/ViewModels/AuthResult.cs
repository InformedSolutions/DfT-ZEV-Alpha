namespace DfT.ZEV.Common.MVC.Authentication.ViewModels;
public class AuthResult
{
    /// <summary>
    /// Gets an <see cref="AuthResult"/> indicating a successful identity operation.
    /// </summary>
    /// <returns>An <see cref="AuthResult"/> indicating a successful operation.</returns>
    public static AuthResult Success => new () { Succeeded = true };

    public static AuthResult InitialPasswordSetRequired => new () { ForceInitialPasswordSet = true };

    public static AuthResult PasswordChangeRequired => new AuthResult { ForcePasswordChange = true };

    public static AuthResult LogoutRequired => new AuthResult { ForceLogout = true };

    /// <summary>
    /// Creates an <see cref="AuthResult"/> indicating a failed identity operation, with a list of errors if applicable.
    /// </summary>
    /// <param name="errors">An optional Dict of key-message pairs which caused the operation to fail.</param>
    /// <returns>An <see cref="AuthResult"/> indicating a failed identity operation, with a list of <paramref name="errors"/> if applicable.</returns>
    public static AuthResult Failed(Dictionary<string, string> errors)
    {
        var result = new AuthResult { Succeeded = false };
        if (errors != null)
        {
            result.Errors = errors;
        }

        return result;
    }

    /// <summary>
    /// Creates an <see cref="AuthResult"/> indicating a failed identity operation, with a list of errors if applicable.
    /// </summary>
    /// <param name="field">String containing input field that caused the error.</param>
    /// <param name="error">String containing error message associated with provided field.</param>
    /// <returns>An <see cref="AuthResult"/> indicating a failed identity operation, with a list of errors if applicable.</returns>
    public static AuthResult Failed(string field, string error)
    {
        var result = new AuthResult { Succeeded = false };
        if (error != null)
        {
            result.Errors.Add(field, error);
        }

        return result;
    }

    /// <summary>
    /// Creates an <see cref="AuthResult"/> indicating a failed identity operation, with a list of errors if applicable.
    /// </summary>
    /// <param name="error">String containing error message. Error is not associated with any field.</param>
    /// <returns>An <see cref="AuthResult"/> indicating a failed identity operation, with a list of errors if applicable.</returns>
    public static AuthResult Failed(string error)
    {
        var result = new AuthResult { Succeeded = false };
        if (error != null)
        {
            result.Errors.Add(string.Empty, error);
        }

        return result;
    }

    /// <summary>
    /// Creates an <see cref="AuthResult"/> indicating a failed identity operation, which should redirect user to new page.
    /// </summary>
    /// <returns>An <see cref="AuthResult"/> indicating a failed identity operation, with instruction to redirect.</returns>
    public static AuthResult FailWithRedirect()
    {
        var result = new AuthResult
        {
            Succeeded = false,
            FailedWithRedirect = true,
        };

        return result;
    }

    /// <summary>
    /// Gets a value indicating whether the operation succeeded or not.
    /// </summary>
    /// <value>True if the operation succeeded, otherwise false.</value>
    public bool Succeeded { get; private set; }

    public bool FailedWithRedirect { get; private set; }

    public bool ForceInitialPasswordSet { get; private set; }

    public bool ForcePasswordChange { get; private set; }

    public bool ForceLogout { get; private set; }

    /// <summary>
    /// Gets a dictionary of key-message pairs containing errors.
    /// </summary>
    public Dictionary<string, string> Errors { get; private set; } = new Dictionary<string, string>();
}
