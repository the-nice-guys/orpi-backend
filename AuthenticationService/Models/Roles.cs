using System;

namespace AuthenticationService.Models {
    [Flags]
    public enum Roles {
        User = 1,
        Admin = 2
    }
}