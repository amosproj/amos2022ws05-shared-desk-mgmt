﻿using System.ComponentModel.DataAnnotations;

namespace Deskstar.Models;

public class CurrentDesk
{
  [Required] public string DeskId { get; set; } = null!;

  [Required] public string DeskName { get; set; } = null!;

  [Required] public string DeskTyp { get; set; } = null!;

  [Required] public string BuildingId { get; set; } = null!;

  [Required] public string BuildingName { get; set; } = null!;

  [Required] public string Location { get; set; } = null!;

  [Required] public string RoomId { get; set; } = null!;

  [Required] public string RoomName { get; set; } = null!;

  [Required] public string FloorId { get; set; } = null!;

  [Required] public string FloorName { get; set; } = null!;

  [Required] public bool IsMarkedForDeletion { get; set; } = false;

  [Required] public List<BookingDesks> Bookings { get; set; } = new();
}

public class BookingDesks
{
  [Required] public string BookingId { get; set; } = null!;

  [Required] public string UserId { get; set; } = null!;

  [Required] public string UserName { get; set; } = null!;

  [Required] public DateTime? StartTime { get; set; } = null!;

  [Required] public DateTime? EndTime { get; set; } = null!;
}
