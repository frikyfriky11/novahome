namespace Novahome.Infrastructure.Services;

/// <summary>
///   An implementation of the client that connects to the Keycloak authentication provider
/// </summary>
/// <param name="httpClient">An HTTP client instance</param>
/// <param name="configuration">The configuration object of the app</param>
public class KeycloakAuthProviderApi(HttpClient httpClient, IConfiguration configuration) : IAuthProviderApi
{
  public async Task<AuthProviderUser> GetUser(Guid id, CancellationToken ct)
  {
    var accessToken = await GetAccessToken(ct);
    HttpRequestMessage request = new(HttpMethod.Get,
      $"{configuration["Auth:InternalRootUrl"]}/admin/realms/{configuration["Auth:Realm"]}/users/{id}");
    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

    HttpResponseMessage response = await httpClient.SendAsync(request, ct);
    response.EnsureSuccessStatusCode();

    var responseContent = await response.Content.ReadAsStringAsync(ct);
    return JsonSerializer.Deserialize<AuthProviderUser>(responseContent,
      new JsonSerializerOptions(JsonSerializerDefaults.Web))!;
  }

  private async Task<string> GetAccessToken(CancellationToken ct)
  {
    // TODO check for nulls from config and warn accordingly
    var tokenEndpoint =
      $"{configuration["Auth:InternalRootUrl"]}/realms/{configuration["Auth:Realm"]}/protocol/openid-connect/token";
    var clientId = configuration["Auth:ClientId"]!;
    var clientSecret = configuration["Auth:ClientSecret"]!;

    FormUrlEncodedContent content = new([
      new KeyValuePair<string, string>("client_id", clientId),
      new KeyValuePair<string, string>("client_secret", clientSecret),
      new KeyValuePair<string, string>("grant_type", "client_credentials")
    ]);

    HttpResponseMessage response = await httpClient.PostAsync(tokenEndpoint, content, ct);
    response.EnsureSuccessStatusCode();

    var responseContent = await response.Content.ReadAsStringAsync(ct);
    var tokenResponse =
      JsonSerializer.Deserialize<Dictionary<string, object>>(responseContent,
        new JsonSerializerOptions(JsonSerializerDefaults.Web))!;

    // TODO check if access token is really there
    return tokenResponse["access_token"].ToString()!;
  }
}
