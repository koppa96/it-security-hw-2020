using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CAFFShop.Application.Models
{
    public class UserListModel
    {
        public Guid Id { get; set; }

        [Display(Name = "E-mail cím")]
        public string Email { get; set; }

        [Display(Name = "2FA engedélyezve")]
        public bool TwoFactorEnabled { get; set; }

        [Display(Name = "Adminisztrátor?")]
        public bool IsAdmin { get; set; }

        [Display(Name = "Engedélyezett?")]
        public bool IsActive { get; set; }
    }
}
