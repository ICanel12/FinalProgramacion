using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikesApplicationModel
{
    public class User
    {
        public int IdUser { get; set; }
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
