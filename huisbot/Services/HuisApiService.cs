﻿using huisbot.Models.Huis;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace huisbot.Services;

/// <summary>
/// The Huis API service is responsible for communicating with the Huis API @ https://pp-api.huismetbenen.nl/.
/// </summary>
public class HuisApiService
{
  private readonly HttpClient _http;
  private readonly ILogger<HuisApiService> _logger;

  /// <summary>
  /// The cached reworks from the API.
  /// </summary>
  private readonly Cached<Rework[]> _reworks = new Cached<Rework[]>(TimeSpan.FromMinutes(5));

  public HuisApiService(IHttpClientFactory httpClientFactory, ILogger<HuisApiService> logger)
  {
    _http = httpClientFactory.CreateClient("huisapi");
    _logger = logger;
  }

  /// <summary>
  /// Returns a bool whether a connection to the Huis API can be established.
  /// </summary>
  /// <returns>Bool whether a connection can be established.</returns>
  public async Task<bool> IsAvailableAsync()
  {
    try
    {
      // Try to send a request to the base URL of the Huis API.
      string result = await _http.GetStringAsync("/");

      // Check whether it returns the expected result.
      if (result != "Cannot GET /")
        throw new Exception("Result does not match \"Cannot GET /\".");

      return true;
    }
    catch (Exception ex)
    {
      _logger.LogError("IsAvailable() returned false: {Message}", ex.Message);
      return false;
    }
  }

  /// <summary>
  /// Returns an array of all reworks from the API.
  /// </summary>
  /// <returns></returns>
  public async Task<Rework[]?> GetReworksAsync()
  {
    // If the cached reworks are not expired, return them.
    if (!_reworks.IsExpired)
      return _reworks.Value;

    try
    {
      // Get the reworks from the API.
      string json = await _http.GetStringAsync("/reworks/list");
      Rework[]? reworks = JsonConvert.DeserializeObject<Rework[]>(json);

      // Check whether the deserialized json is valid.
      if (reworks is null || reworks.Length == 0)
      {
        _logger.LogError("Failed to deserialize the reworks from the Huis API.\nhttps://pp-api.huismetbenen.nl/reworks/list");
        return null;
      }

      // Update the cached reworks and return it.
      _reworks.Value = reworks;
      return reworks;
    }
    catch (Exception ex)
    {
      _logger.LogError("Failed to get the list of reworks from the Huis API: {Message}", ex.Message);
      return null;
    }
  }

  /// <summary>
  /// Returns the player with the specified id in the specified rework from the API.
  /// </summary>
  /// <param name="playerId">The osu! id of the player.</param>
  /// <param name="playerId">The id of the rework.</param>
  /// <returns></returns>
  public async Task<Player?> GetPlayerAsync(int playerId, int reworkId)
  {
    // TODO: Implement caching

    try
    {
      // Get the player from the API.
      string json = await _http.GetStringAsync($"/player/userdata/{playerId}/{reworkId}");
      Player? player = JsonConvert.DeserializeObject<Player>(json);

      // Check whether the deserialized json is valid.
      if (player is null)
      {
        _logger.LogError("Failed to deserialize the player from the Huis API.\nhttps://pp-api.huismetbenen.nl/player/userdata/{playerId}/{reworkId}",
          playerId, reworkId);
        return null;
      }

      return player;
    }
    catch (Exception ex)
    {
      _logger.LogError("Failed to get the player from the Huis API: {Message}\nhttps://pp-api.huismetbenen.nl/player/userdata/{playerId}/{reworkId}",
        ex.Message, playerId, reworkId);
      return null;
    }
  }
}
