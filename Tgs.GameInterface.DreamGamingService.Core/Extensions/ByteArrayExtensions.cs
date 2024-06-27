using System.Text;

namespace Tgs.GameInterface.DreamGamingService.Core.Extensions;

public static class ByteArrayExtensions
{
	public static string ToHexString(this byte[] byteArray)
	{
		var sb = new StringBuilder();
		foreach (var t in byteArray)
		{
			_ = sb.Append(t.ToString("x2"));
		}

		return sb.ToString();
	}
}
