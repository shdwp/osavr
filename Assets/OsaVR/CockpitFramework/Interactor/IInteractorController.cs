namespace OsaVR.CockpitFramework.Interactor
{
    public enum InteractionType
    {
        Press,
        Release,
        MoveUp,
        MoveDown,
        MoveRight,
        MoveLeft,
        ClickRight,
        ClickLeft,
    }
    
    public interface IInteractorController
    {
        InteractionType[] InteractionTypes();

        void Handle(InteractionType type);
    }
}