﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebVetMobile.Models
{
    class Token
    {
        public string? AccessToken { get; set; }
        public string? TokenType { get; set; }
        public string? UserId { get; set; }
        public string? UserName { get; set; }
    }
}
