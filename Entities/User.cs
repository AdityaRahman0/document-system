﻿using System;


namespace Entities
{
    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string Password { get; set; }
        public DateTime dtmUpd { get; set; }
        public string usrUpd { get; set; }
        public int DepartmentId { get; set; }
    }
}
