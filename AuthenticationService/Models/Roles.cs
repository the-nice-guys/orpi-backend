using System;

namespace AuthenticationService.Models {
    [Flags]
    public enum Roles {
        User,
        Admin
    }
}