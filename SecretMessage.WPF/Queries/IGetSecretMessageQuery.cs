using Refit;
using SecretMessage.Core;

namespace SecretMessage.WPF.Queries
{
    public interface IGetSecretMessageQuery
    {
        [Get("/")]
        Task<SecretMessageResponse> ExecuteAsync();
    }
}
