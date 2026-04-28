using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PoliRiwi.Data;
using PoliRiwi.Models;
using PoliRiwi.Response;
using PoliRiwi.Services.Interfaces;

namespace PoliRiwi.Services;

public class PlaceService : IPlaceServices
{
    // --------- BOOKS SERVICES -----------
    // We call the DbContext for introducing it into the layer Services
    private readonly MySqlDbContext _context;
    private readonly IValidator<Places> _placeValidator;

    public PlaceService(MySqlDbContext context, IValidator<Places> validator)
    {
        _context = context;
        _placeValidator = validator;
    }
    
    // ----------- GET ALL PLACES -------------
    public async Task<ServiceResponse<IEnumerable<Places>>> GetAllPlaces()
    {
        /*
         * We call the response to previously using it to return data to the User and avoiding futures crashes in the system
         */
        var places = await _context.Places.ToListAsync();
        return new ServiceResponse<IEnumerable<Places>>()
        {
            Data = places,
            IsSuccess = true
        };
    }

    public async  Task<ServiceResponse<Places?>> GetPlaceById(int id)
    {
        var response = new ServiceResponse<Places>();
        
        /*
         * Using await for waiting for a response of the database avoiding freezings insede our system
         * We use FirstOrDefault to bring him the parameter he wanted to search inside our system
         */
        
        var places = await _context.Places.FirstOrDefaultAsync(p => p.Id == id);
        if (places != null)
        {
            // ------------- GET BY ID ----------
            response.Data = places;
            response.IsSuccess = true;
            response.Message = "Place found";
            return response;
        }
        else
        {
            response.IsSuccess = false;
            response.Message = "Place not found";
            return response;
        }
    }
    
    
    public async Task<ServiceResponse<Places>> CreatePlace(Places place)
    {
        // ------------- CREATE ----------
        var response = new ServiceResponse<Places>();
        var validation = await _placeValidator.ValidateAsync(place);
        if (!validation.IsValid)
        {
            response.IsSuccess = false;
            response.Message = string.Join(" | ", validation.Errors.Select(x => x.ErrorMessage));
            return response;
        }
        var nameExist = await _context.Places.FirstOrDefaultAsync(p => p.Name == place.Name);

        if (nameExist != null)
        {
            response.IsSuccess = false;
            response.Message = "There is a place already in the system!!";
            return response;
        }
        else
        {
            // We use await methods for waiting the response from the datebase like a promise 
            // when it brings the response he is going to add the place and then savechanges
            await _context.Places.AddAsync(place);
            await _context.SaveChangesAsync();
            
            response.IsSuccess = true;
            response.Data = place;
            response.Message = "Place created";
            return response;
            // We return the response to the user, we send a Message letting him know the place was created
        }
    }

    public async Task<ServiceResponse<Places>> UpdatePlace(int id, Places place)
    {
        // ------------- Update ----------
        var response = new ServiceResponse<Places>();
        var validation = await _placeValidator.ValidateAsync(place);
        if (!validation.IsValid)
        {
            response.IsSuccess = false;
            response.Message = string.Join(" | ", validation.Errors.Select(x => x.ErrorMessage));
            return response;
        }
        var idExist = await _context.Places.FindAsync(id);

        if (idExist == null)
        {
            response.IsSuccess = false;
            response.Message = "Place not found in the System";
            return response;
        }
        else
        {
            // if the ID is found we are going to call the paramethers, and then we are going to modify the old data 
            // and replacing it for the new data sending it to the system
            idExist.Name = place.Name;
            idExist.Capacity = place.Capacity;
            idExist.SpaceType = place.SpaceType;
            
            _context.Places.Update(idExist);
            await _context.SaveChangesAsync();
            
            response.IsSuccess = true;
            response.Message = "Place updated";
            response.Data = place;
            return response;
        }
    }

    public async Task<ServiceResponse<Places>> DeletePlace(int id)
    {
        // ------------- Delete ----------
        var response = new ServiceResponse<Places>();
        // We verify if in the system there are the id that the users is requesting for 
        var idExist = await _context.Places.FindAsync(id);
        // If not exists it is going th throw a new response that the service could not find that ID
        if (idExist == null)
        {
            response.IsSuccess = false;
            response.Message = "Place not found";
            return response;
        }
        //If exist it will remove it and wait for the promise to savethechanges
        else
        {
            _context.Remove(idExist);
            await _context.SaveChangesAsync();
            
            // We modify Data into database and then we send response
            response.IsSuccess = true;
            response.Data = idExist;
            response.Message = "Place deleted";
            return response;
        }
    }
}

