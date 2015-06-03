using UnityEngine;


namespace AssemblyCSharp
{
    public class ObstacleGenerator
    {

        /*
         * Klasa odpowiada za generowanie przeszkod
         * 
         */ 


        public class ObstacleParams
        {
            public float start;
            public float gapStart;
            public float gapEnd;
            public float end;
            public float depth;
            public float height;
            public float distance;
        }
        public ObstacleGenerator(PlayerController player)
        {
            this.player = player;
            lastGapRight = avaliableWidth / 2 + minGap;
            lastGapLeft = avaliableWidth / 2 + minGap;
        }

        private PlayerController player;
        float avaliableWidth = 9;//ile jednostek szerokości ma droga
        float minGap = 1.5f;//trzeba jakoś pozwolić kuli gdziekolwiek się przedostać


        /**
         * Generowanie  musi być takie, że (lastPos-lastWidth/2)/lastDistance
         * 
         */
        float lastGapLeft = 0;
        float lastGapRight;
        float lastDistance = 10f;//jak dawno była generowana przeszkoda
        float maxDistance = 10f;
        float minMargin = 0.5f;

        public ObstacleParams generateParams()
        {
            //lewa i prawa mogą się różnić od lastWidth i lastPos o pewien mnożnik zależny od lastDistance
            lastDistance = Random.Range(6, 10);
            var marginSpread = (lastDistance / maxDistance) * avaliableWidth / 2;

            var left = Random.Range(lastGapLeft - marginSpread, lastGapLeft + marginSpread);
            var right = Random.Range(lastGapRight - marginSpread, lastGapRight + marginSpread);
            if (left > right)
            {
                var a = left;
                left = right;
                right = a;
            }
            if (right - left < minGap)
            {
                var mid = (right + left) / 2;
                left = mid - minGap / 2;
                right = mid + minGap / 2;
            }
            if (right - left > avaliableWidth - minGap)
            {
                //zakres i tak sie nie zmieści, zmniejsz go
                right = left + avaliableWidth - minGap;
            }
            if (left < minMargin)
            {
                var size = right - left;
                left = minMargin;
                right = left + size;

            }
            if (right > avaliableWidth - minMargin)
            {
                var size = right - left;
                right = avaliableWidth - minMargin;
                left = right - size;
            }
            Debug.LogWarning(string.Format("left: {0} right:{1}", left, right));
            var op = new ObstacleParams();
            op.depth = 0.5f;
            op.height = Random.Range(0.5f, 2);
            op.gapStart = left;
            op.gapEnd = right;
            op.distance = lastDistance;
            op.start = 0;
            op.end = avaliableWidth;
            //
            lastGapLeft = left;
            lastGapRight = right;
            return op;
        }

    }
}

