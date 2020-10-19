using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrikazoveVykreslovani
{
    public struct Group
    {
        public string name;
        public List<Shape> shapes;
        public Point position;
        public Size size;

        public Group(Group origin)
        {
            this.name = origin.name;
            this.size = origin.size;
            this.position = origin.position;
            this.shapes = origin.shapes;
        }


        public void DrawGroup(Graphics g)
        {
            foreach (var shape in shapes)
            {
                shape.DrawInSize(g, position, size);
            }
        }
    }
}
