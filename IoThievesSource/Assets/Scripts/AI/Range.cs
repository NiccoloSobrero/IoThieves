namespace AI
{
    public class Range
    {
        private float min;
        private float max;

        public Range(float min, float max)
        {
            this.min = min;
            this.max = max;
        }

        public float Min
        {
            get => min;
            set => min = value;
        }

        public float Max
        {
            get => max;
            set => max = value;
        }

        public bool InRange(float f)
        {
            return min <= f && f <= max;
        }

        public void Edit(float newMin, float newMax)
        {
            this.min = newMin;
            this.max = newMax;
        }
        
    }
}