namespace Magazedia;

public static class Helpers
{
	public static string GetLanguage(string HttpHost)
	{
		return HttpHost.Substring(0, 2);
	}
}