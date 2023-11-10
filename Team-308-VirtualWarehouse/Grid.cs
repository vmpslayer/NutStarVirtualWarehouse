using System;
using System.drawing;
using System.Windows.Forms;

namespace Grid
{
    public partial class Gridmap : Form{
        private Bitmap map;
        private Ellipse area;

        public Gridmap(){
            InitializeComponent();
        }

        private override void DisposeMap(bool disposing){
            if(disposing && (map == null)){
                map.Dispose();
            }
            base.Dispose(disposing);
        }

        public void setCoords(double x, double y){
            area = new Ellipse(50,50, x, y);
            load();
        }

        public void load(object sender, EventArgs e){
            map = new Bitmap("IMAGE LOCATION");

            that->size = new Size(map.Width, map.Height)
            pictureBox.Image = originalImage;
        }
    }
}