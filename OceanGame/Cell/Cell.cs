namespace OceanGame
{
    public class Cell
    {
        public int Heat { get; protected set; } = 0;
        public int UID { get; } = Globals.gameUIDGenerator.Generate();
        public virtual char Image { get; } = GameSettings.VoidImage;
        public virtual void Process(int x, int y, IOceanCell ocean)
        {
            if (Heat > 0)
            {
                Heat--;
            }
        }

        public Cell(int heat)
        {
            Heat = heat;
        }

        public Cell()
        {

        }

        public void HeatUp()
        {
            Heat = GameSettings.HeatLength;
        }
    }
}
