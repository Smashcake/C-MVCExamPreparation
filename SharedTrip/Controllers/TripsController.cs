using SharedTrip.Services;
using SharedTrip.ViewModels.Trips;
using SUS.HTTP;
using SUS.MvcFramework;
using System;
using System.Globalization;

namespace SharedTrip.Controllers
{
    public class TripsController : Controller
    {
        private readonly ITripsService tripsService;

        public TripsController(ITripsService tripsService)
        {
            this.tripsService = tripsService;
        }

        public HttpResponse All()
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/Users/Login");
            }

            var trips = this.tripsService.AllTrips();
            return this.View(trips);
        }

        public HttpResponse Add()
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/Users/Login");
            }

            return this.View();
        }

        [HttpPost]
        public HttpResponse Add(AddTripInputModel model)
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/Users/Login");
            }
            if (string.IsNullOrWhiteSpace(model.StartPoint))
            {
                return this.Error("Invalid starting point input.");
            }
            if (string.IsNullOrWhiteSpace(model.EndPoint))
            {
                return this.Error("Invalid end point input.");
            }
            DateTime validDepartureTime;
            DateTime.TryParseExact(model.DepartureTime, string.Format("dd.MM.yyyy HH:mm")
                , CultureInfo.InvariantCulture, DateTimeStyles.None, out validDepartureTime);
            if (validDepartureTime == null)
            {
                return this.Error("Invalid departure time input.Please input a valid departure time.");
            }
            if (model.Seats < 2 || model.Seats > 6)
            {
                return this.Error("Invalid seats input.Seats must be between 2 and 6.");
            }

            if (string.IsNullOrWhiteSpace(model.Description) || model.Description.Length > 80)
            {
                return this.Error("Invalid description input.Description must be below 80 characters.");
            }

            this.tripsService.AddTrip(model);
            return this.Redirect("/Trips/All");
        }

        public HttpResponse Details(string tripId)
        {
            if (!this.IsUserSignedIn())
            {
                return this.Error("You must be logged in to view trip details.");
            }
            var trip = this.tripsService.Details(tripId);
            return this.View(trip);
        }

        public HttpResponse AddUserToTrip(string tripId)
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/Users/Login");
            }
            var userId = this.GetUserId();
            this.tripsService.AddUserToTrip(userId, tripId);
            return this.Redirect("/Trips/All");
        }
    }
}
