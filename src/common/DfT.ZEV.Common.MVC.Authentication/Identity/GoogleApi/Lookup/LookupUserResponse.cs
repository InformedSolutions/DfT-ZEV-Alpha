namespace DfT.ZEV.Common.MVC.Authentication.Identity.GoogleApi.Lookup;

public class MfaInfo
{
    public string MfaEnrollmentId { get; set; }
    public string DisplayName { get; set; }
    public string EnrolledAt { get; set; }
}

public class UserData
{
    public string LocalId { get; set; }
    public string Email { get; set; }
    public string DisplayName { get; set; }
    public string Language { get; set; }
    public string PhotoUrl { get; set; }
    public string TimeZone { get; set; }
    public string DateOfBirth { get; set; }
    public bool EmailVerified { get; set; }
    public bool Disabled { get; set; }
    public string LastLoginAt { get; set; }
    public List<MfaInfo> MfaInfo { get; set; }
}

public class LookupUserResponse
{
    public string Kind { get; set; }
    public List<UserData> Users { get; set; }
}