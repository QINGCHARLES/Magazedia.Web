using System.Globalization;
using System.Text.RegularExpressions;

namespace Magazedia;

public static class Helpers
{
	public static string GetCultureFromHostname(string Hostname, string DefaultCulture)
	{
		int DotIndex = Hostname.IndexOf('.');

		// If there is no subdomain use default culture, otherwise get the substring containing the subdomain
		return DotIndex >= 0 ? Hostname[..DotIndex] : DefaultCulture;
	}

	public static string GetTextDirectionFromHostname(string Hostname, string DefaultCulture)
	{
		string Culture = GetCultureFromHostname(Hostname, DefaultCulture);
		CultureInfo CultureInfo = new(Culture);

		return CultureInfo.TextInfo.IsRightToLeft ? "rtl" : "ltr";
	}

	public static string GetDomainAndPortFromHostname(string Hostname)
	{
		return Hostname.IndexOf('.') >= 0 ? Hostname.Substring(Hostname.IndexOf('.') + 1) : Hostname;
	}

	public static string Slugify(string Text)
	{
		string output = Regex.Replace(Text, @"[^A-Za-z0-9\s-]", "");
		output = Regex.Replace(output, @"\s+", " ").Trim();
		output = Regex.Replace(output, @"\s", "-");
		return output.ToLower();
	}
}