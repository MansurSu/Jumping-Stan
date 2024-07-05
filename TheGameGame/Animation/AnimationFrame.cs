using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheGameGame.Animation
{
    public class AnimationFrame
    {

        public Rectangle SourceRectangle { get; set; }

        public AnimationFrame (Rectangle rectangle)
        {
            SourceRectangle = rectangle;
        }
    }
}
