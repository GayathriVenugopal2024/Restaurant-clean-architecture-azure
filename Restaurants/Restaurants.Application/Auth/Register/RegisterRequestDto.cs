using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.Application.Auth.Register
{
    public class RegisterRequestDto
    {
        [DataType(DataType.EmailAddress)]
        public required string  UserName { get; set; }
        [DataType(DataType.Password)]
        public required string  Password { get; set; }
        public string[] Roles { get; set; }
    }
}
