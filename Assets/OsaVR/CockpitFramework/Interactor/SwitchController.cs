using System;

namespace OsaVR.CockpitFramework.Interactor
{
    public class SwitchController: AInteractorController
    {
        [Flags] public enum Position
        {
            Neutral = 1,
            Up = 2,
            Right = 3,
            Down = 4,
            Left = 5,
        }

        protected Position SupportedPositions = Position.Up | Position.Down;
        protected Position NeutralPosition = Position.Down;
        protected Position CurrentPosition = Position.Down;
        protected bool Springloaded = false;
        
        public override InteractionType[] InteractionTypes()
        {
            var types = new InteractionType[]
            {
                InteractionType.Release,
                InteractionType.Release,
                InteractionType.Release,
                InteractionType.Release,
                InteractionType.Release,
            };
            var i = 1;

            if (SupportedPositions.HasFlag(Position.Up))
            {
                types[++i] = InteractionType.MoveUp;
            }
            
            if (SupportedPositions.HasFlag(Position.Down))
            {
                types[++i] = InteractionType.MoveDown;
            }
            
            if (SupportedPositions.HasFlag(Position.Left))
            {
                types[++i] = InteractionType.MoveLeft;
            }
            
            if (SupportedPositions.HasFlag(Position.Right))
            {
                types[++i] = InteractionType.MoveRight;
            }

            return types;
        }

        public override void Handle(InteractionType type)
        {
            switch (type)
            {
                case InteractionType.MoveDown:
                    switch (CurrentPosition)
                    {
                        case Position.Up:
                            if (MoveToPositionIfSupported(Position.Neutral) || MoveToPositionIfSupported(Position.Down)) { }
                            break;
                        case Position.Neutral:
                            MoveToPositionIfSupported(Position.Down);
                            break;
                        case Position.Left:
                        case Position.Right:
                            MoveToPositionIfSupported(Position.Down);
                            break;
                    }
                    break;
                case InteractionType.MoveUp:
                    switch (CurrentPosition)
                    {
                        case Position.Up:
                            break;
                        case Position.Neutral:
                            MoveToPositionIfSupported(Position.Up);
                            break;
                        case Position.Down:
                            if (MoveToPositionIfSupported(Position.Neutral) || MoveToPositionIfSupported(Position.Up)) {}
                            break;
                        case Position.Left:
                            MoveToPositionIfSupported(Position.Up);
                            break;
                        case Position.Right:
                            MoveToPositionIfSupported(Position.Up);
                            break;
                    }
                    break;
                case InteractionType.Release:
                    if (Springloaded)
                    {
                        MoveToPositionIfSupported(NeutralPosition);
                    }
                    break;
            }
            
            _animator.SetInteger("Position", (int)CurrentPosition);
        }

        private void Start()
        {
            _animator.SetInteger("Position", (int)CurrentPosition);
        }

        private bool MoveToPositionIfSupported(Position p)
        {
            if (SupportedPositions.HasFlag(p))
            {
                CurrentPosition = p;
                return true;
            }

            return false;
        }
    }
}