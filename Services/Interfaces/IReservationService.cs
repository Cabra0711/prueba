using PoliRiwi.Models;
using PoliRiwi.Response;

namespace PoliRiwi.Services.Interfaces;

public interface IReservationService
{
    public Task<ServiceResponse<IEnumerable<Reservations>>> GetAllReservations();
    public Task<ServiceResponse<Reservations?>> GetReservationById(int id);
    public Task<ServiceResponse<Reservations>> UpdateReservation(int id, Reservations reservation );
    public Task<ServiceResponse<Reservations>> CreateReservation(Reservations reservation);
    public Task<ServiceResponse<Reservations>> DeleteReservation(int id);
    
    
}