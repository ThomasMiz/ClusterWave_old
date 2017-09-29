using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ClusterWave.Scenario
{
    struct PolyTexture
    {
        public Texture2D texture;
        public Vector2 min, max;
        private Vector2 size;

        public PolyTexture(Texture2D texture, Vector2 min, Vector2 max)
        {
            this.texture = texture;
            this.min = min;
            this.max = max;
            size = max - min;
        }

        public Vector2 GetTexCoord(Vector2 scenarioPos)
        {
            return (scenarioPos - min) / size;
        }
    }
}
