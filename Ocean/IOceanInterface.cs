namespace OceanGame
{
    public interface IOceanInterface
    {
        public void Display(in Cell[,] field, in GameStats stats);
        public int oceanWidth { get; }
        public int oceanHeight { get; }
    }
}
