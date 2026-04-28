using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PoliRiwi.Data;
using PoliRiwi.Enums;
using PoliRiwi.Models;
using PoliRiwi.Response;
using PoliRiwi.Services.Interfaces;

namespace PoliRiwi.Services;

public class ReservationService : IReservationService
{
    private readonly IValidator<Reservations> _reservationsValidator;
    private readonly MySqlDbContext _context;

    public ReservationService(IValidator<Reservations> reservationsValidator, MySqlDbContext context)
    {
        _reservationsValidator = reservationsValidator;
        _context = context;
    }

    public async Task<ServiceResponse<IEnumerable<Reservations>>> GetAllReservations()
    {
        // we use Include to validate and do the reservations that are in the model before going to the database
        var reservations = await _context.Reservations.Include(r => r.User).Include(r => r.Place).ToListAsync();

        // we return a service response to verify that the data has been brought to the view of the user
        return new ServiceResponse<IEnumerable<Reservations>>()
        {
            Data = reservations,
            IsSuccess = true
        };
    }

    public async Task<ServiceResponse<Reservations?>> GetReservationById(int id)
    {
        var response = new ServiceResponse<Reservations>();
        var reservations = await _context.Reservations.FirstOrDefaultAsync(r => r.Id == id);

        if (reservations != null)
        {
            response.Data = reservations;
            response.IsSuccess = true;
            response.Message = "Reservation Found";
            return response;
        }
        else
        {
            response.IsSuccess = false;
            response.Message = "Reservation not found";
            return response;
        }
    }

    public async Task<ServiceResponse<Reservations>> UpdateReservation(int id, Reservations reservation)
    {
        var response = new ServiceResponse<Reservations>();
        var validation = await _reservationsValidator.ValidateAsync(reservation);
        if (!validation.IsValid)
        {
            response.IsSuccess = false;
            response.Message = string.Join(" | ", validation.Errors.Select(x => x.ErrorMessage));
            return response;
        }
        var idExist = await _context.Reservations.FindAsync(id);

        if (idExist != null)
        {
            idExist.Date = reservation.Date;
            idExist.EndTime = reservation.EndTime;
            idExist.Status = reservation.Status;
            
            _context.Reservations.Update(idExist);
            await _context.SaveChangesAsync();
            
            response.IsSuccess = true;
            response.Data = idExist;
            response.Message = "User updated";
            return response;
        }
        else
        {
            response.IsSuccess = false;
            response.Message = "This user does not exist";
            return response;
        }
    }

    public async Task<ServiceResponse<Reservations>> CreateReservation(Reservations reservation)
    {
        var response = new ServiceResponse<Reservations>();
        var validation = await _reservationsValidator.ValidateAsync(reservation);
        if (!validation.IsValid)
        {
            response.IsSuccess = false;
            response.Message = string.Join(" | ", validation.Errors.Select(x => x.ErrorMessage));
            return response;
        }
        var userExists = await _context.Users.FindAsync(reservation.UserId);
        var placeExists = await _context.Places.FindAsync(reservation.PlaceId);

        if (placeExists != null && userExists != null)
        {
            response.IsSuccess = true;
            response.Message = "User and Place founded";
            var status = placeExists.Status;
            if (status == Status.Available)
            {
                response.Data = reservation;
                response.IsSuccess = true;
                response.Message = "Reservation has been created";
                reservation.Status = Status.Unavailable;
                
                _context.Reservations.Add(reservation);
                await _context.SaveChangesAsync();
                return response;
            }else if (status == Status.Unavailable)
            {
                response.IsSuccess = false;
                response.Message = "Reservation has been reserved before";
            }
            else
            {
                response.IsSuccess = false;
                response.Message = "Reservation already exists";
                return response;
            }
        }
        return response;
    }
    
    public async Task<ServiceResponse<Reservations>> DeleteReservation(int id)
    {
        var response = new ServiceResponse<Reservations>();
        var idExists = await _context.Reservations.FindAsync(id);
        if (idExists != null)
        {
            response.IsSuccess = true;
            response.Message = "Reservation found";
            response.Data = idExists;
            _context.Reservations.Remove(idExists);
            await _context.SaveChangesAsync();
            return response;
        }
        else
        {
            response.IsSuccess = false;
            response.Message = "Reservation not found";
            return response;
        }
    }
    
}