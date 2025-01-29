using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XGym.Application.UserOperations.Commands.CreateToken
{
    public record TokenDto
    {
        public String AccessToken { get; init; }
        public String RefreshToken { get; init; }
    }
}
