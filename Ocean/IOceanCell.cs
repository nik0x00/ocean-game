namespace OceanGame
{
    public interface IOceanCell
    {
        public Cell GetCell(int x, int y);
        public void SetCellOrNothing(int x, int y, Cell cell);
        public void OnPreyConsumed();
        public void OnPreyReproduced();
        public void OnPredatorDied();
        public void OnPredatorReproduced();
    }
}
