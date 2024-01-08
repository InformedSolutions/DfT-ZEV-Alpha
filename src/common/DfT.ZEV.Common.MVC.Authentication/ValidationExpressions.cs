namespace DfT.ZEV.Common.MVC.Authentication;

public class ValidationExpressions
{
    public const string PasswordRequirements = @"^(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*()_+])[A-Za-z\d!@#$%^&*()_+]{12,}$";
}