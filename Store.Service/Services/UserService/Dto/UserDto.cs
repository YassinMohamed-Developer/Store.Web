using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Service.Services.UserService.Dto
{
    public class UserDto
    {
        public Guid Id { get; set; }

        public string DisplayName { get; set; }

        public string Email { get; set; }

        public string Token { get; set; }
    }
}
