using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PrikazoveVykreslovani
{

    //AS
    public partial class GroupManager : Form
    {
        List<Shape> shapes = new List<Shape>();
        List<Command> commands = new List<Command>();
        Command currentlyDraggedCommand;

        public string GroupName => textBox1.Text;
        public List<Shape> Shapes => shapes;
        public GroupManager()
        {
            InitializeComponent();
            UpdateCommands();
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            CommandForm cmd = new CommandForm();
            cmd.SetOtherShapes(shapes);
            cmd.FormClosing += OnCommandClosing;
            cmd.ShowDialog();
        }

        private void OnCommandClosing(object sender, FormClosingEventArgs e)
        {
            var shp = ((CommandForm)sender).Shape;
            if (shp != null)
            {
                shapes.Add(shp);
                panel1.Refresh();
            }

            UpdateCommands();
        }

        private void UpdateCommands()
        {
            panel2.Controls.Clear();
            commands.Clear();
            for (int i = 0; i < shapes.Count; i++)
            {
                Shape shp = (Shape)shapes[i];
                Command c = new Command();
                c.SetShape(shp);

                c.DragStart += OnDragStart;
                c.DragEnded += OnDragEnded;
                c.MovedDragged += OnMovedDragged;

                c.Smazat += OnSmazat;
                c.Location = new Point(0, i * c.Height + 5 * i);
                panel2.Controls.Add(c);
                commands.Add(c);
            }
            panel2.Refresh();
        }

        private void OnMovedDragged(Command obj)
        {
            RepositionCommands();
        }


        private void RepositionCommands()
        {
            commands = commands.OrderBy(x => x.Location.Y).ToList();
            for (int i = 0; i < commands.Count; i++)
            {
                if (commands[i] != currentlyDraggedCommand)
                    commands[i].Location = new Point(0, i * commands[i].Height);
            }
        }
        private void OnDragEnded(Command obj)
        {
            shapes = commands.Select(x => x.Shape).ToList();
            panel1.Refresh();
            currentlyDraggedCommand = null;
            RepositionCommands();
        }

        private void OnDragStart(Command obj)
        {
            currentlyDraggedCommand = obj;
        }

        private void OnPosunNiz(Command c)
        {
            int index = shapes.IndexOf(c.Shape);
            if (index - 1 >= 0)
            {
                shapes.RemoveAt(index);
                index--;
                shapes.Insert(index, c.Shape);
            }
            UpdateCommands();
            panel1.Refresh();
        }

        private void OnPosunVys(Command c)
        {
            // TODO: Opravit!
            int index = shapes.IndexOf(c.Shape);

            if (index + 1 < shapes.Count)
            {
                shapes.RemoveAt(index);
                shapes.Insert(index, c.Shape);
            }
            UpdateCommands();
            panel1.Refresh();
        }

        private void OnSmazat(Command c)
        {
            shapes.Remove(c.Shape);
            UpdateCommands();
            panel1.Refresh();
        }

        private void Panel1_Paint(object sender, PaintEventArgs e)
        {
            shapes.ForEach(shp => shp.Draw(e.Graphics));
        }

        private void okButton_Click_1(object sender, EventArgs e)
        {
            string name = textBox1.Text;
            if (string.IsNullOrEmpty(name))
            {
                var res = MessageBox.Show("Chybí vám název skupiny! Chcete pokračovat? Skupina se neulož!", "Upozornění!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (res == DialogResult.Yes)
                {
                    Close();
                }
            }
            else if (shapes.Count == 0)
            {
                var res = MessageBox.Show("Chybí vám název skupiny! Chcete pokračovat? Skupina se neulož!", "Upozornění!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (res == DialogResult.Yes)
                {
                    Close();
                }
            }
            else
                Close();

        }
    }
}


