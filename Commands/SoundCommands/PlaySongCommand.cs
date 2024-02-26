namespace Sprint5BeanTeam
{
    public class PlaySongCommand : ICommand
    {
        private SoundEffectManager soundReceiver;

        public PlaySongCommand(SoundEffectManager soundReceiver)
        {
            this.soundReceiver = soundReceiver;
        }
        public void Execute()
        {
            this.soundReceiver.PlayMusic();
        }
    }
}