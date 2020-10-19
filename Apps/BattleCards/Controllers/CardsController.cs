﻿using BattleCards.Services;
using BattleCards.ViewModels.Cards;
using SUS.HTTP;
using SUS.MvcFramework;

namespace BattleCards.Controllers
{
    public class CardsController : Controller
    {
        private readonly ICardsService cardsService;

        public CardsController(ICardsService cardsService)
        {
            this.cardsService = cardsService;
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
        public HttpResponse Add(AddCardInputModel model)
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/Users/Login");
            }

            if(string.IsNullOrWhiteSpace(model.Name) || model.Name.Length < 5 || model.Name.Length > 15)
            {
                return this.Error("Invalid name input.Name must be between 5 and 15 characters.");
            }
            if (string.IsNullOrWhiteSpace(model.Image))
            {
                return this.Error("Invalid image input.");
            }            
            if(model.Attack < 0)
            {
                return this.Error("Attack cannot be below zero.");
            }
            if(model.Health <= 0)
            {
                return this.Error("Health cannot be at or below zero.");
            }
            if(string.IsNullOrWhiteSpace(model.Description) || model.Description.Length > 200)
            {
                return this.Error("Invalid description input.Description cann't be above 200 characters.");
            }

            this.cardsService.Create(model);
            return this.Redirect("/");
        }
    }
}
