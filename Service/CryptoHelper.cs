using System.Security.Cryptography;
using System.Text;

namespace SistemasdeTarefas.Service
{
    public static class CryptoHelper
    {
        private static readonly string ChavePrivada = "SysGest@2025_Key!"; // 16 caracteres para AES-128

        public static string Encriptar(string textoClaro)
        {
            var bytes = Encoding.UTF8.GetBytes(textoClaro);
            return Convert.ToBase64String(bytes);
        }

        public static string Desencriptar(string textoEncriptado)
        {
            var bytes = Convert.FromBase64String(textoEncriptado);
            return Encoding.UTF8.GetString(bytes);
        }
    }

}
