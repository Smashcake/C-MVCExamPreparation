using BattleCards.Data;
using BattleCards.ViewModels.Cards;
using System.Collections.Generic;

namespace BattleCards.Services
{
    public interface ICardsService
    {
        void Create(CreateCardInputModel model);

        IEnumerable<AddCardInputModel> All();

        IEnumerable<AddCardInputModel> MyCollection(string userId);

        void AddCardToCollection(string userId, int cardId);

        void RemoveFromCollection(string userId, int cardId);
    }
}