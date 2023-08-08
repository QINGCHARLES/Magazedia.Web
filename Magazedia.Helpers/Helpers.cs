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

	// Note: this function only works for localhost and domains with two levels, e.g.
	// magazedia.wiki, or mysite.com. It will not work for sites like site.co.uk
	public static string GetDomainAndPortFromHostname(string Hostname)
	{
		// Check if port is present
		string Port = string.Empty;
		if (Hostname.Contains(":"))
		{
			string[] Split = Hostname.Split(":");
			Hostname = Split[0];
			Port = ":" + Split[1];
		}

		// Remove subdomains
		string[] SplitHostname = Hostname.Split('.');
		if (SplitHostname.Length > 1)
		{
			Hostname = string.Join(".", SplitHostname[^2], SplitHostname[^1]);
		}
		//else if (SplitHostname.Length == 2)
		//{
		//	Hostname = string.Join(".", SplitHostname[^2], SplitHostname[^1]);
		//}

		return Hostname + Port;
	}

	public static string Slugify(string Text)
	{
		string output = Regex.Replace(Text, @"[^A-Za-z0-9\s-]", "");
		output = Regex.Replace(output, @"\s+", " ").Trim();
		output = Regex.Replace(output, @"\s", "-");
		return output.ToLower();
	}

	public static string ConvertNumberToLetters(int Number)
	{
		string Result = "";

		while(Number > 0)
		{
			Number--; // Adjusting the number to start from 0
			int Remainder = Number % 26; // Get the remainder after division by 26
			char Letter = (char)('a' + Remainder); // Convert the remainder to the corresponding letter
			Result = Letter + Result; // Prepend the letter to the result string
			Number /= 26; // Divide the number by 26 for the next iteration
		}

		return Result;
	}
}