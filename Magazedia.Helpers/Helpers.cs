namespace Magazedia;

public static class Helpers
{
	public static string GetLanguage(string HttpHost)
	{
		return HttpHost.Substring(0, 2);
	}
	
	// List of all languages that read from right-to-left that we support
	static readonly string[] RightToLeftTextDirectionLanguageCodes = { "ar" };

	public static string GetTextDirection(string HttpHost)
	{
		string Language = GetLanguage(HttpHost);
		return RightToLeftTextDirectionLanguageCodes.Contains(Language) ? "rtl" : "ltr";
	}
}