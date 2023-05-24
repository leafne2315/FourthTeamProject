using FourthTeamProject.Models;
using System;
using System.Collections.Generic;

namespace FourthTeamProject.Models;

public partial class Income
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int Amount { get; set; }

    public string Year { get; set; } = null!;

    public string Month { get; set; } = null!;

    //public virtual TMember Member { get; set; } = null!;
}
