using Microsoft.AspNetCore.Razor.TagHelpers;


namespace DfT.ZEV.Administration.Web.Extensions.TagHelpers;

public class DisplayInlineValueWithLabelTagHelper : TagHelper
{
    public string Label { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";
        output.Attributes.Add("class", "govuk-!-font-size-19");

        var preContent = $"<span class=\"govuk-!-font-weight-bold\">{Label}</span> <span>";
        var postContent = "</span>";

        output.PreContent.SetHtmlContent(preContent);
        output.PostContent.SetHtmlContent(postContent);
    }
}
