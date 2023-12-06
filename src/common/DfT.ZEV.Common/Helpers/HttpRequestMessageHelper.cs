namespace DfT.ZEV.Common.Helpers;

/// <summary>
/// Helper methods for the lists.
/// </summary>
public static class HttpRequestMessageHelper
{
    /// <summary>
    /// Clones an HttpRequest message so that can be resent in retry loops.
    /// </summary>
    /// <param name="req">Original request.</param>
    /// <returns>Cloned request message.</returns>
    public static async Task<HttpRequestMessage> CloneHttpRequestMessage(HttpRequestMessage req)
    {
        HttpRequestMessage clone = new HttpRequestMessage(req.Method, req.RequestUri);

        // Copy the request's content (via a MemoryStream) into the cloned object
        using (var ms = new MemoryStream())
        {
            if (req.Content != null)
            {
                await req.Content.CopyToAsync(ms).ConfigureAwait(false);
                ms.Position = 0;
                clone.Content = new StreamContent(ms);

                // Copy the content headers
                if (req.Content.Headers != null)
                {
                    foreach (var h in req.Content.Headers)
                    {
                        clone.Content.Headers.Add(h.Key, h.Value);
                    }
                }
            }

            clone.Version = req.Version;

            foreach (KeyValuePair<string, object> option in req.Options)
            {
                clone.Options.Set(new HttpRequestOptionsKey<object>(option.Key), option.Value);
            }

            foreach (KeyValuePair<string, IEnumerable<string>> header in req.Headers)
            {
                clone.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            return clone;
        }
    }
}