using System.Security.Cryptography;
using System.Text;

namespace EjerciciosProgramacion.Util
{
  public class Func
  {
    public static string Sha256(string input)
    {
      var crypt = new SHA256Managed();
      string hash = string.Empty;
      byte[] crypto = crypt.ComputeHash(Encoding.ASCII.GetBytes(input));
      foreach(byte b in crypto)
      {
        hash += b.ToString("x2");
      }
      return hash;
    }
  }
}
