using System;
using System.Collections.Generic;

namespace BikeAPI.Models;

public partial class User
{
    public int IdUser { get; set; }

    public string UserName { get; set; } = null!;

    public string Password { get; set; } = null!;
}
