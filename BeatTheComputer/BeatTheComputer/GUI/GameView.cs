using BeatTheComputer.Core;

namespace BeatTheComputer.GUI
{
    public interface GameView
    {
        void initGraphics(IGameContext context, GameForm form);
        void updateGraphics(GameController controller, GameForm form);
    }
}