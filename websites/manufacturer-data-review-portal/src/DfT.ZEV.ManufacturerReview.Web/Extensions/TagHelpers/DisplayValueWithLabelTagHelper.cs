using Microsoft.AspNetCore.Razor.TagHelpers;

namespace DfT.ZEV.ManufacturerReview.Web.Extensions.TagHelpers;

public class DisplayValueWithLabelTagHelper : TagHelper
{
    public string Label { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";
        output.Attributes.Add("class", "govuk-!-font-size-19");

        var preContent = $"<span class=\"govuk-!-font-weight-bold\">{Label}</span><p class=\"govuk-body\">";
        var postContent = "</p>";

        output.PreContent.SetHtmlContent(preContent);
        output.PostContent.SetHtmlContent(postContent);
    }
}
