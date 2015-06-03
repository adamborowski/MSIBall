using UnityEngine;


namespace AssemblyCSharp
{
    public class ObstacleGenerator
    {

        /*
         * Klasa odpowiada za generowanie przeszkód
         * Przeszkoda opisana jest za pomocą punktów w skali [0,1] które potem podczas wizualizacji można przekalować
         * Każda przeszkoda składa się z dziury w którą wjeżdza kula gracza. Zatem każda przeszkoda to dwa klocki. Lewy i prawy.
         */ 


        public class ObstacleParams
        {
            //poniższe parametry są znormalizowane do 1
            public float start;//położenie lewej krawędzi lewego klocka (zawsze będzie zero)
            public float gapStart;//położenie prawej krawędzi lewego klocka
            public float gapEnd; //położenie lewej krawędzi prawego klocka
            public float end; //położenie prawej krawędzi prawego klocka
            public float depth; //grubość klocka
            public float height; //wysokość klocka
            public float distance; //odległość od poprzedniego klocka
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
            if (left > right)//zamień aby left < right
            {
                var a = left;
                left = right;
                right = a;
            }
            if (right - left < minGap)//wymuszenie aby dziura zmieściła kulę
            {
                var mid = (right + left) / 2;
                left = mid - minGap / 2;
                right = mid + minGap / 2;
            }
            if (right - left > avaliableWidth - minGap)//zakres i tak sie nie zmieści, zmniejsz go
            {
                right = left + avaliableWidth - minGap;
            }
            if (left < minMargin)//zapewnij lewy margines dziury
            {
                var size = right - left;
                left = minMargin;
                right = left + size;

            }
            if (right > avaliableWidth - minMargin)//zapewnij prawy margines dziury
            {
                var size = right - left;
                right = avaliableWidth - minMargin;
                left = right - size;
            }
            //zapisz wygenerowane parametry
            var op = new ObstacleParams();
            op.depth = 0.5f;
            op.height = Random.Range(0.5f, 2);
            op.gapStart = left;
            op.gapEnd = right;
            op.distance = lastDistance;
            op.start = 0;
            op.end = avaliableWidth;
            //zapamiętaj gdzie ostatnio była dziura aby wylosować w miarę podobną dziurę
            lastGapLeft = left;
            lastGapRight = right;
            return op;
        }

    }
}

