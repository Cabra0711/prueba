using PoliRiwi.Models;
using PoliRiwi.Response;

namespace PoliRiwi.Services.Interfaces;

// We define a Contract called Interface it means that the thigs we assign here the service has to sign it 
public interface IPlaceServices
{
    // We use Asycronous methods for avoiding freezings at the database and making the UX UI experience more 
    // friendly
    public Task<ServiceResponse<IEnumerable<Places>>> GetAllPlaces();
    public Task<ServiceResponse<Places?>> GetPlaceById(int id);
    public Task<ServiceResponse<Places>> CreatePlace(Places place);
    // we use id for bring the object and making to the promagram do it quickly
    public Task<ServiceResponse<Places>> UpdatePlace(int id, Places place);
    // its more efficient to delete place for id to make it faster and easy to detect in the database
    public Task<ServiceResponse<Places>> DeletePlace(int id);
}