using System;

namespace Sprint5BeanTeam
{
    public class AddOffsetCommand : ICommand
    {

        private NoteManager noteManager;

        public AddOffsetCommand(NoteManager manager)
        {
            this.noteManager = manager;
        }
        public void Execute()
        {
            this.noteManager.AddOffset(0.005);
        }
    }
}
