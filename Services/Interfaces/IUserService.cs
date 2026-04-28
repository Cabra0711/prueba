using PoliRiwi.Models;
using PoliRiwi.Response;

namespace PoliRiwi.Services.Interfaces;

public interface IUserService
{
    // At this scenario we just use IEnumerable beacause it is a list only for read and its more useful for this kind of things
    public Task<ServiceResponse<IEnumerable<Users>>> GetAllUsers();
    public Task<ServiceResponse<Users?>> GetUserById(int id);
    public Task<ServiceResponse<Users>> UpdateUser(int id, Users user);
    public Task<ServiceResponse<Users>> CreateUser(Users user);
    public Task<ServiceResponse<Users>> DeleteUser(int id);
}