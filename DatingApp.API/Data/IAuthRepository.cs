using System.Threading.Tasks;
using DatingApp.API.Models;

namespace DatingApp.API.Data
{

    // interface for the repostiry, 
    // dosnt contain implenation code but only the methods 
    // contract to tell reposryt hey this are the methods to use thats it.
    // interface with I
    public interface IAuthRepository
    {  // here are the tree methods for the user
        Task<User> Register (User user, string password); 
        Task<User> Login(string usrname, string password);
        Task<bool> UserExists (string usrname); 

    }
}