using AngryBirds.Properties;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AngryBirds
{
    public class Pig : Sprite
    {
        public Pig(Form form, int size, int left, int ground, List<Sprite> gameSprites) : base(form, Resources.pig, size, left, ground, gameSprites)
        {

        }
    }
}
