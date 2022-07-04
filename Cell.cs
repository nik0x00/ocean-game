namespace OceanGame
{
    public class Cell
    {
        public int uid { get; } = Globals.gameUIDGenerator.Generate();
        public virtual char image { get; } = GameSettings.VoidImage;
        public virtual void Process(int x, int y, Ocean ocean)
        {

        }
    }
}
