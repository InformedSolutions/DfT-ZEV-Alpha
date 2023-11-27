namespace DfT.ZEV.Common.MVC.Authentication;

public static class AuthErrorMessages
{
    public const string SignInErrorMessage = "The email or password you entered is incorrect";

    public const string PasswordEmpty = "Enter your password";

    public const string TemporaryPasswordEmpty = "Enter your password";

    public const string OldPasswordEmpty = "Enter your password";

    public const string NewPasswordEmpty = "Enter your new password";

    public const string PasswordsMustMatch = "Password and password confirmation must be the same";

    public const string PasswordUsed = "You have already used that password, choose a new one";

    // Can be used only for already authenticated user
    public const string CurrentPasswordInvalid = "Current password is not valid";

    public const string EmailEmpty = "Enter your email address";

    public const string EmailInvalid = "Enter your email address in a valid format";

    public const string EmailsMustMatch = "Email address and email confirmation must be the same";

    public const string EmailTaken = "User with given email already exists";
}
