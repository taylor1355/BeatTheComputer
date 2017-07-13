using BeatTheComputer.Core;

namespace BeatTheComputer.GUI
{
    public interface GameView
    {
        void initGraphics(GameForm form);
        void updateGraphics(GameForm form);
    }
}