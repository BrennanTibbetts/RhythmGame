using System;

namespace Sprint5BeanTeam
{
    public class MinusOffsetCommand : ICommand
    {

        private NoteManager noteManager;

        public MinusOffsetCommand(NoteManager manager)
        {
            this.noteManager = manager;
        }
        public void Execute()
        {
            this.noteManager.MinusOffset(0.005);
        }
    }
}
