using BattleCards.ViewModels.Cards;

namespace BattleCards.Services
{
    public interface ICardsService 
    {
        void Create(AddCardInputModel model);
    }
}
