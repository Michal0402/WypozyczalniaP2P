using WypozyczalniaP2P.Data;
using Microsoft.EntityFrameworkCore;

namespace WypozyczalniaP2P.Services
{
    public class CarAvailabilityService
    {
        private readonly ApplicationDbContext _context;

        public CarAvailabilityService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> IsCarAvailable(int carId, DateTime start, DateTime? end)
        {
            // Sprawdzenie, czy daty są w przyszłości
            if (start < DateTime.Today || (end.HasValue && end < start))
            {
                return false;
            }

            // Pobranie wszystkich wypożyczeń dla danego samochodu
            var rentals = await _context.Wypozyczenia
                .Where(r => r.SamochodId == carId)
                .ToListAsync();

            // Sprawdzenie nakładania się dat
            foreach (var rental in rentals)
            {
                // Jeśli istniejące wypożyczenie nakłada się na proponowany okres
                if (rental.DataRozpoczecia <= end && (rental.DataZakonczenia == null || rental.DataZakonczenia >= start))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
