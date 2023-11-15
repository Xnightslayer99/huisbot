﻿using Newtonsoft.Json;

namespace huisbot.Models.Huis;

/// <summary>
/// Represents a player from the Huis API.
/// </summary>
public class Player
{
  [JsonProperty("user_id")]
  /// <summary>
  /// The osu! user id of the player.
  /// </summary>
  public int Id { get; set; }

  [JsonProperty("name")]
  /// <summary>
  /// The name of the player.
  /// </summary>
  public string? Name { get; private set; }

  [JsonProperty("old_pp")]
  /// <summary>
  /// The live pp of the player.
  /// </summary>
  public double? OldPP { get; private set; }

  [JsonProperty("new_pp_incl_bonus")]
  /// <summary>
  /// The pp of the player in the rework the player object is from.
  /// </summary>
  public double? NewPP { get; private set; }

  [JsonProperty("bonus_pp")]
  /// <summary>
  /// The bonus pp of the player.
  /// </summary>
  public double BonusPP { get; private set; }

  [JsonProperty("weighted_acc_pp")]
  /// <summary>
  /// The weighted accuracy pp of the player in the rework the player object is from.
  /// </summary>
  public double WeightedAccPP { get; private set; }

  [JsonProperty("weighted_aim_pp")]
  /// <summary>
  /// The weighted aim pp of the player in the rework the player object is from.
  /// </summary>
  public double WeightedAimPP { get; private set; }

  [JsonProperty("weighted_tap_pp")]
  /// <summary>
  /// The weighted tapping pp of the player in the rework the player object is from.
  /// </summary>
  public double WeightedTapPP { get; private set; }

  [JsonProperty("weighted_fl_pp")]
  /// <summary>
  /// The weighted flashlight pp of the player in the rework the player object is from.
  /// </summary>
  public double WeightedFLPP { get; private set; }

  [JsonProperty("last_updated")]
  /// <summary>
  /// The last time the player got updated on Huismetbenen.
  /// </summary>
  public DateTime LastUpdated { get; private set; }
}
