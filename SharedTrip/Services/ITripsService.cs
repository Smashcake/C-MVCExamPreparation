using SharedTrip.ViewModels.Trips;
using System.Collections;
using System.Collections.Generic;

namespace SharedTrip.Services
{
    public interface ITripsService
    {
        void AddTrip(AddTripInputModel model);

        IEnumerable<AllTripsViewModel> AllTrips();

        TripDetailsViewModel Details(string tripId);

        void AddUserToTrip(string userId, string tripId);
    }
}
