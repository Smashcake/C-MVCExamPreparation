using BattleCards.Data;
using BattleCards.ViewModels.Cards;
using System.Collections.Generic;
using System.Linq;

namespace BattleCards.Services
{
    public class CardsService : ICardsService
    {
        private readonly ApplicationDbContext dbContext;

        public CardsService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IEnumerable<AddCardInputModel> All()
        {
            var allCards = this.dbContext.Cards.Select(x => new AddCardInputModel
            {
                Id = x.Id,
                Name = x.Name,
                Image = x.ImageUrl,
                Keyword = x.Keyword,
                Attack = x.Attack,
                Health = x.Health,
                Description = x.Description
            }).ToList();

            return allCards;
        }

        public void Create(CreateCardInputModel model)
        {
            var card = new Card
            {
                Name = model.Name,
                ImageUrl = model.Image,
                Keyword = model.Keyword,
                Attack = model.Attack,
                Health = model.Health,
                Description = model.Description
            };

            this.dbContext.Cards.Add(card);
            this.dbContext.SaveChanges();
        }

        public void AddCardToCollection(string userId, int cardId)
        {
            var card = this.dbContext.UserCards.Where(uc => uc.CardId == cardId && uc.UserId == userId).FirstOrDefault();
            if (card != null)
            {
                return;
            }

            var userCard = new UserCard()
            {
                UserId = userId,
                CardId = cardId
            };

            userCard.UserId = userId;
            userCard.CardId = cardId;
            this.dbContext.UserCards.Add(userCard);
            this.dbContext.SaveChanges();
        }

        public void RemoveFromCollection(string userId, int cardId)
        {
            var card = this.dbContext.UserCards.Where(uc => uc.UserId == userId && uc.CardId == cardId).FirstOrDefault();
            this.dbContext.UserCards.Remove(card);
            this.dbContext.SaveChanges();
        }

        public IEnumerable<AddCardInputModel> MyCollection(string userId)
        {
            return this.dbContext
                .UserCards
                .Where(uc => uc.UserId == userId)
                .Select(c => new AddCardInputModel
                {
                    Id = c.CardId,
                    Name = c.Card.Name,
                    Image = c.Card.ImageUrl,
                    Keyword = c.Card.Keyword,
                    Attack = c.Card.Attack,
                    Health = c.Card.Health,
                    Description = c.Card.Description
                }).ToList();
        }
    }
}