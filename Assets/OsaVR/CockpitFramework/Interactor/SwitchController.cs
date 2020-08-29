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

        protected Position supportedPositions = Position.Up | Position.Down;
        protected Position neutralPosition = Position.Down;
        protected Position currentPosition = Position.Down;
        protected bool springloaded = false;
        
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

            if (supportedPositions.HasFlag(Position.Up))
            {
                types[++i] = InteractionType.MoveUp;
            }
            
            if (supportedPositions.HasFlag(Position.Down))
            {
                types[++i] = InteractionType.MoveDown;
            }
            
            if (supportedPositions.HasFlag(Position.Left))
            {
                types[++i] = InteractionType.MoveLeft;
            }
            
            if (supportedPositions.HasFlag(Position.Right))
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
                    switch (currentPosition)
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
                    switch (currentPosition)
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
                    if (springloaded)
                    {
                        MoveToPositionIfSupported(neutralPosition);
                    }
                    break;
            }
            
            _animator.SetInteger("Position", (int)currentPosition);
        }

        private void Start()
        {
            _animator.SetInteger("Position", (int)currentPosition);
        }

        private bool MoveToPositionIfSupported(Position p)
        {
            if (supportedPositions.HasFlag(p))
            {
                currentPosition = p;
                return true;
            }

            return false;
        }
    }
}