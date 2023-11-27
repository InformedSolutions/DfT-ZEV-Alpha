namespace DfT.ZEV.Common.ContentSecurityPolicy;

public class CspOptions
{
    public CspOption Default { get; set; } = new ("default-src");

    public CspOption Script { get; set; } = new ("script-src");

    public CspOption Style { get; set; } = new ("style-src");

    public CspOption StyleElement { get; set; } = new ("style-src-elem");

    public CspOption Image { get; set; } = new ("img-src");

    public CspOption Connect { get; set; } = new ("connect-src");

    public CspOption Frame { get; set; } = new ("frame-ancestors");

    public CspOption Form { get; set; } = new ("form-action");

    public CspOption Fonts { get; set; } = new ("font-src");

    public CspOption Media { get; set; } = new ("media-src");

    public override string ToString()
    {
        var value = string.Empty;
        value += Default.ToString();
        value += Script.ToString();
        value += Style.ToString();
        value += StyleElement.ToString();
        value += Image.ToString();
        value += Connect.ToString();
        value += Frame.ToString();
        value += Form.ToString();
        value += Fonts.ToString();
        value += Media.ToString();
        return value;
    }
}
