namespace OsaVR.Osa.Interactor.SUA
{
    public class SUAAutoacquireButtonController: OsaButtonController
    {
        protected override void OnPressed()
        {
            _state.Sua.autoAcquisition = true;
        }
    }
}