using System;

namespace OsaVR.CockpitFramework.Interactor
{
    public class SwitchController: AInteractorController
    {
        [Flags] public enum Position
        {
            Neutral = 1,
            Up = 1 << 1,
            Right = 1 << 2,
            Down = 1 << 3,
            Left = 1 << 4,
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
                case InteractionType.MoveLeft:
                    switch (currentPosition)
                    {
                        case Position.Left:
                            break;
                        case Position.Right:
                            if (MoveToPositionIfSupported(Position.Neutral) || (MoveToPositionIfSupported(Position.Left))) { }
                            break;
                        case Position.Neutral:
                        case Position.Down:
                        case Position.Up:
                            MoveToPositionIfSupported(Position.Left);
                            MoveToPositionIfSupported(Position.Left);
                            break;
                    }
                    break;
                case InteractionType.MoveRight:
                    switch (currentPosition)
                    {
                        case Position.Right:
                            break;
                        case Position.Left:
                            if (MoveToPositionIfSupported(Position.Neutral) || (MoveToPositionIfSupported(Position.Right))) { }
                            break;
                        case Position.Neutral:
                        case Position.Down:
                        case Position.Up:
                            MoveToPositionIfSupported(Position.Right);
                            MoveToPositionIfSupported(Position.Right);
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