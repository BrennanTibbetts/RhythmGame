using System;

namespace Sprint5BeanTeam
{
    public class AutoCommand : ICommand
    {

        private NoteManager noteManager;

        public AutoCommand(NoteManager manager)
        {
            this.noteManager = manager;
        }
        public void Execute()
        {
            this.noteManager.DemonstrateAuto = !this.noteManager.DemonstrateAuto;
        }
    }
}