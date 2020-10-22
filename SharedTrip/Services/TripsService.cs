using SharedTrip.Data;
using SharedTrip.ViewModels.Trips;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace SharedTrip.Services
{
    public class TripsService : ITripsService
    {
        private readonly ApplicationDbContext db;

        public TripsService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public void AddTrip(AddTripInputModel model)
        {
            var trip = new Trip()
            {
                StartPoint = model.StartPoint,
                EndPoint = model.EndPoint,
                DepartureTime = DateTime.ParseExact(model.DepartureTime, string.Format("dd.MM.yyyy HH:mm"), CultureInfo.InvariantCulture),
                Seats = (byte)model.Seats,
                Description = model.Description,
                ImagePath = model.CarImage
            };

            this.db.Trips.Add(trip);
            this.db.SaveChanges();
        }

        public void AddUserToTrip(string userId, string tripId)
        {
            var isUserAlreadyAssignedToTrip = this.db.UserTrips
                .Where(ut => ut.UserId == userId && ut.TripId == tripId)
                .FirstOrDefault();

            if(isUserAlreadyAssignedToTrip != null)
            {
                return;
            }

            var userTrip = new UserTrip()
            {
                UserId = userId,
                TripId = tripId
            };

            var trip = this.db.Trips.Where(t => t.Id == tripId).FirstOrDefault();
            trip.Seats -= 1;

            this.db.UserTrips.Add(userTrip);
            this.db.SaveChanges();
        }

        public IEnumerable<AllTripsViewModel> AllTrips()
        {
            return this.db.Trips.Select(t => new AllTripsViewModel
            {
                Id = t.Id,
                StartPoint = t.StartPoint,
                EndPoint = t.EndPoint,
                DepartureTime = t.DepartureTime.ToString("dd.MM.yyyy HH:mm"),
                Seats = t.Seats.ToString()
            }).ToList();
        }

        public TripDetailsViewModel Details(string tripId)
        {
            var trip = this.db.Trips.Where(t => t.Id == tripId).Select(t => new TripDetailsViewModel
            {
                Id = t.Id,
                StartPoint = t.StartPoint,
                EndPoint = t.EndPoint,
                DepartureTime = t.DepartureTime.ToString("dd.MM.yyyy HH.mm"),
                Seats = t.Seats,
                Description = t.Description
            }).FirstOrDefault();

            return trip;
        }
    }
}
