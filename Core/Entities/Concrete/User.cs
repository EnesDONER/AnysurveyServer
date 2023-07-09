using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities.Concrete
{
    public class User :IEntity
    {
        public int Id { get; set; }
        public int GenderId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Country { get; set; }
        public DateTime BirthDay { get; set; }
        public string ASTWalletAddress { get; set; }
        public string Images { get; set; }
        public string ResetToken { get; set; }
        public DateTime ResetTokenExpiration { get; set; }
        public byte[] PasswordSalt { get; set; }
        public byte[] PasswordHash { get; set; }
        public bool Status { get; set; }
    }
}