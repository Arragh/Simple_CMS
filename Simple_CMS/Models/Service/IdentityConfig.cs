using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Simple_CMS.Models.Service
{
    public class IdentityConfig
    {
        public static string AdminName { get; set; }
        public static string NormalizedAdminName { get; set; }
        public static string AdminEmail { get; set; }
        public static string NormalizedAdminEmail { get; set; }
        public static string AdminPassword { get; set; }
        public static string AllowedUserNameCharacters { get; set; }
    }
}
