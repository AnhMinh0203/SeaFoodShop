using SeaFoodShop.DataContext.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaFoodShop.Repository.Interface
{
    public interface ITokenRespon
    {
        public string GenerateJwtToken(SignInModel account, Guid idUser);
        public Guid? ValidateJwtToken(string? token);
    }
}
