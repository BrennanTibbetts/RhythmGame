namespace Sprint5BeanTeam
{
    public interface IState
    {
        public enum GameState
        {
            StartMenu,
            SongSelectionMenu,
            SettingsMenu,
            PauseMenu,
            GameOver,
            Play,
            Countdown,
            Transition
        }
        
        enum MusicSelectionState {bbc,lrs,cld,ukt}
        

        string StateCode { get; }
    }
}