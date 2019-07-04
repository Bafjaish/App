using System;
using System.Threading.Tasks;
using DatingApp.api.Data;
using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{

    public class AuthRepository : IAuthRepository
    {  // the repostiry is responebale for databse query
       // so we have to inject the dataconxt here conctrcotr 
        private readonly DataContext _context; // we do _ cz its private
        public AuthRepository(DataContext context)
        {
            _context = context;

        }

 
        public async Task<User> Login(string usrname, string password)
        {
            var user  = await _context.Users.FirstOrDefaultAsync (x=> x.Username==usrname);// we use this frist or default cz it will if its there then ok if not then 404 fault
            if (user==null)
                  return null;
            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                  return null;
       

             return user;
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
             using(var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt)) // there are sevveral method for hashing pass, this is one type
                    {
                     var  computeHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password)); 

                     for (int i =0; i < computeHash.Length; i++)  // we use the for loop to compare comptued pass witht the passhas  
                     {
                         if (computeHash[i] != passwordHash [i]) return false;
                     }
                    }

                    return true;
        }

        public async Task<User> Register(User user, string password)
        {
            // this method take usename and passord, so we have to declare var as 3 types 

            byte[] passwordHash, passwordSalt;
            CreatePasswordHash (password, out passwordHash, out passwordSalt); // this method takes three pass and it passes them as referece to Create...
              user.PasswordHash= passwordHash;
              user.PasswordSalt= passwordSalt;
              
              await _context.Users.AddAsync(user);
              await _context.SaveChangesAsync();
              return user;

        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            // here the method for creating password
            // to make sure the dispose method is called we make then in between () using
           using(var hmac = new System.Security.Cryptography.HMACSHA512()) // there are sevveral method for hashing pass, this is one type
                    {
                       passwordSalt = hmac.Key;
                       passwordHash= hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password)); 
            
                        
                    }
        
        }

        public  async Task<bool> UserExists(string usrname)
        {
            if (await _context.Users.AnyAsync (x =>x.Username ==usrname))
                return true; 
            return false;

        }
    }
}