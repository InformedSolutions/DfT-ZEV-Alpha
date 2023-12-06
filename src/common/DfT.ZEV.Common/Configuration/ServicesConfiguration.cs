namespace DfT.ZEV.Common.Configuration;

public class ServicesConfiguration
{
    public const string SectionName = "Services";

    public string OrganisationApiBaseUrl { get; set; } = null!;
    public string ManufacturerPortalBaseUrl { get; set; } = null!;
}