namespace Interactor
{
    public enum InteractionType
    {
        Press,
        MoveUp,
        MoveDown,
        MoveRight,
        MoveLeft,
    }
    
    public interface IInteractorController
    {
        InteractionType[] InteractionTypes();

        void Handle(InteractionType type);
    }
}