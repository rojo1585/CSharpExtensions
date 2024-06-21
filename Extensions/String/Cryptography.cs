using System.Security.Cryptography;
using System.Text;

namespace Red.ToolKit.Extensions.String;
public static class Cryptography
{
    public static string ToSHA256(this string str)
    {
        byte[] hashBytes = SHA256.HashData(Encoding.UTF8.GetBytes(str));
        return Convert.ToHexString(hashBytes);
    }
    public static (byte[] encryptedData, byte[] key, byte[] iv) EncryptAES(this string str, int keySize = 256)
    {
        if (string.IsNullOrEmpty(str))
            throw new ArgumentException("The string to encrypt cannot be null or empty.");

        if (!(keySize is 128 or 192 or 256))
            throw new ArgumentException("Invalid key size. Supported sizes: 128, 192, 256 bits.");

        using var aes = Aes.Create();
        aes.KeySize = keySize;
        aes.Padding = PaddingMode.PKCS7;
        aes.GenerateKey();
        aes.GenerateIV();

        using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
        byte[] plainTextBytes = Encoding.UTF8.GetBytes(str);
        byte[] encryptedBytes = encryptor.TransformFinalBlock(plainTextBytes, 0, plainTextBytes.Length);

        return (encryptedBytes, aes.Key, aes.IV);
    }

    public static string DecryptAES(this byte[] encryptedBytes, byte[] key, byte[] iv)
    {
        if (encryptedBytes == null || encryptedBytes.Length == 0)
            throw new ArgumentException("Encrypted bytes cannot be null or empty.");
        if (key == null || key.Length == 0)
            throw new ArgumentException("Key cannot be null or empty.");
        if (iv == null || iv.Length == 0)
            throw new ArgumentException("IV cannot be null or empty.");

        using var aes = Aes.Create();
        aes.KeySize = key.Length * 8;
        aes.Padding = PaddingMode.PKCS7;

        using var descriptor = aes.CreateDecryptor(key, iv);
        byte[] decryptedBytes = descriptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);
        return Encoding.UTF8.GetString(decryptedBytes);
    }
}
