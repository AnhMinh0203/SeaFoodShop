using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaFoodShop.DataContext.Data
{
    public class User
    {
        public Guid Id { get; set; }    
        public DateTime Dob {  get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public int Role {  get; set; }

/*        public User() { }
        public User(Guid id, DateTime dob, string fullName, string phoneNumber, string password, int role)
        {
            Id = id;
            Dob = dob;
            FullName = fullName;
            PhoneNumber = phoneNumber;
            Password = password;
            Role = role;
        }*/
    }
}
