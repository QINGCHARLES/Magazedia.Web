using System.Globalization;

namespace Magazedia;

public static class Helpers
{
	public static string GetCultureFromHostname(string Hostname, string DefaultCulture)
	{
		int DotIndex = Hostname.IndexOf('.');

		// If there is no subdomain use default culture, otherwise get the substring containing the subdomain
		return DotIndex >= 0 ? Hostname[..DotIndex] : DefaultCulture;
	}
	
	// List of all languages that read from right-to-left that we support
	//static readonly string[] RightToLeftTextDirectionCultureCodes = { "ar" };

	public static string GetTextDirectionFromHostname(string Hostname, string DefaultCulture)
	{
		string Culture = GetCultureFromHostname(Hostname, DefaultCulture);
		CultureInfo CultureInfo = new(Culture);

		return CultureInfo.TextInfo.IsRightToLeft ? "rtl" : "ltr";
		//return RightToLeftTextDirectionCultureCodes.Contains(Culture) ? "rtl" : "ltr";
	}

	public static string GetDomainAndPortFromHostname(string Hostname)
	{
		return Hostname.IndexOf('.') >= 0 ? Hostname.Substring(Hostname.IndexOf('.') + 1) : Hostname;
	}
}