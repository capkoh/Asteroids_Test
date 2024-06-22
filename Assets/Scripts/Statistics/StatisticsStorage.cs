namespace Game.Statistics
{
    public class StatisticsStorage
    {
        private int _score;
        private bool _everStarted;

        public int Score => _score;
        public bool EverStarted => _everStarted;

        public void Reset()
        {
            _score = 0;
        }

        public void SetEverStarted()
        {
            _everStarted = true;
        }

        public void ScoreAsteroid()
        {
            _score += 5;
        }

        public void ScoreSaucer()
        {
            _score += 50;
        }
    }
}