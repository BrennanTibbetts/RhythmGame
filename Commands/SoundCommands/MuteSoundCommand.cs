namespace Sprint5BeanTeam
{
    public class MuteSoundCommand : ICommand
    {
        private SoundEffectManager soundReceiver;

        public MuteSoundCommand(SoundEffectManager soundReceiver)
        {
            this.soundReceiver = soundReceiver;
        }
        public void Execute()
        {
            this.soundReceiver.MuteOrReverse();
        }
    }
}