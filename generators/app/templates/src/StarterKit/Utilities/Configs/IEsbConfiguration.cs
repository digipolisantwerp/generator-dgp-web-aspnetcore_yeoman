using System.Net;

namespace StarterKit.Utilities.Configs
{
    public interface IEsbConfiguration
    {
		string AuthScheme { get; }
        string Domain { get; }
        string User { get; }
        string Password { get; }

		string Url { get; }

        NetworkCredential Credential { get; }
        string CredentialBase64 { get; }
    }
}