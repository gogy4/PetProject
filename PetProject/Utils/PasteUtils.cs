using System.IO.Compression;
using System.Text;
using PetProject.Models;

namespace PetProject.Utils;

public class PasteUtils
{
    public string DecompressString(byte[] byteArr)
    {
        var resultString = string.Empty;
        if (byteArr == null || byteArr.Length <= 0) return resultString;
        using var stream = new MemoryStream(byteArr);
        using var zip = new GZipStream(stream, CompressionMode.Decompress);
        using var reader = new StreamReader(zip);
        resultString = reader.ReadToEnd();

        return resultString;
    }

    public byte[] CompressString(string content)
    {
        var byteArr = new byte[0];
        if (string.IsNullOrEmpty(content)) return byteArr;
        byteArr = Encoding.UTF8.GetBytes(content);
        using var stream = new MemoryStream();
        using (var zip = new GZipStream(stream, CompressionMode.Compress))
        {
            zip.Write(byteArr, 0, byteArr.Length);
        }

        byteArr = stream.ToArray();

        return byteArr;
    }

    public async Task<string> CheckRights(HttpContext httpContext, Paste paste)
    {
        if (paste == null) return "Прошлая паста была удалена или не найдена";

        return httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) != paste.UserId
            ? "Вы не можете удалить чужую пасту"
            : "";
    }
}