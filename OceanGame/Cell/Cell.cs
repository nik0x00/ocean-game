namespace OceanGame
{
    public class Cell
    {
        public int UID { get; } = Globals.gameUIDGenerator.Generate();
        public virtual char Image { get; } = GameSettings.VoidImage;
        public virtual void Process(int x, int y, IOceanCell ocean)
        {

        }
    }
}
