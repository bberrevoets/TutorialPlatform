﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Berrevoets.TutorialPlatform.Data;

namespace Berrevoets.TutorialPlatform.Models.Certificates;

public class IssuedCertificate
{
    public int Id { get; set; }

    [Required]
    [MaxLength(450)]
    public string UserId { get; set; } = default!;

    [ForeignKey(nameof(UserId))]
    public ApplicationUser User { get; set; } = default!;

    [Required]
    public int TutorialId { get; set; }

    public Tutorial Tutorial { get; set; } = default!;

    [Required]
    public DateTime IssuedAt { get; set; }

    [Required]
    [MaxLength(50)]
    public string SerialNumber { get; set; } = default!;
}