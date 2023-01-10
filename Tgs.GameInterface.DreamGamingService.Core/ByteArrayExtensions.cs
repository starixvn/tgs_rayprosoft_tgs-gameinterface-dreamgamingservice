using System.Text;

namespace Tgs.GameInterface.DreamGamingService.Core;

public static class ByteArrayExtensions
{
    public static string ToHexString(this byte[] byteArray)
    {
        var sb = new StringBuilder();
        foreach (var t in byteArray)
        {
            sb.Append(t.ToString("x2"));
        }
        return sb.ToString();
    }
}