using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApplication1.DAO;
using WindowsFormsApplication1.POJO;

namespace WindowsFormsApplication1
{
    
    public partial class home : Form
    {
        List<OnibusObj> lista = new List<OnibusObj>();
        static Double latitude = 0;
        static Double longitude = 0;
        GMarkerGoogle marker = new GMarkerGoogle(new PointLatLng(latitude, longitude), GMarkerGoogleType.green);
        bool selecionar = false;
        bool selecionarExcluir = false;
        bool selecionarArea = false;
        bool selecionarExcluirArea = false;
        bool selecionarTerminal= false;
        bool selecionarExcluirTerminal = false;
        List<PontoObj> listaPontos = new List<PontoObj>();
        List<areaObj> listaAreas = new List<areaObj>();
        PontoDAO pontoDAO = new PontoDAO();
        areaDAO areaDAO = new areaDAO();
        List<GMarkerGoogle> listMarks;
        public home()
        {
            InitializeComponent();
        }
        public void atualizaOnibus()
        {
            lista.Clear();
            OnibusDAO oniDAO = new OnibusDAO();
            lista = oniDAO.listaPontoDAO();
            dataGridView1.DataSource = lista;
        }
        public bool testaInternet()
        {
            Conexao c = new Conexao();
            if (!c.OpenConnection())
            {
                MessageBox.Show("Parece que você esta sem internet!");
                return false;
            }
            return true;
        }
        private void Form2_Load(object sender, EventArgs e)
        {
            this.Size = Screen.PrimaryScreen.WorkingArea.Size;
            if (!testaInternet())
                this.Close();
             listaPontos = pontoDAO.listaPontoDAO();
            listaAreas = areaDAO.listaPontoDAO();
            listMarks = new List<GMarkerGoogle>();
            gMapControl1.DragButton = MouseButtons.Left;
            gMapControl1.CanDragMap = true;
            gMapControl1.Position = new PointLatLng(-22.725, -47.6476);
            gMapControl1.MinZoom = 0;
            gMapControl1.MaxZoom = 24;
            gMapControl1.Zoom = 12;
            gMapControl1.AutoScroll = true;
            gMapControl1.MapProvider = GMapProviders.OviHybridMap;
            atualizaOnibus();
          
            atualizaMarkers();

        }
        public void atualizaMarkers()
        {
            GMapOverlay markersOverlay = new GMapOverlay("markers");
            gMapControl1.Overlays.Clear();
            listMarks.Clear();
            if (gMapControl1.Zoom > 15)
            {
                foreach (PontoObj ponto in listaPontos)
                {
                    try
                    {
                        if (ponto.Terminal == 0)
                        {
                            marker = new GMarkerGoogle(new PointLatLng(Convert.ToDouble(ponto.Latitude), Convert.ToDouble(ponto.Longitude)), GMarkerGoogleType.blue_small);
                            marker.ToolTipText = "Latitude:"+ponto.Latitude+"\n"+"Longitude:"+ponto.Longitude;
                        }
                        else
                        {
                            marker = new GMarkerGoogle(new PointLatLng(Convert.ToDouble(ponto.Latitude), Convert.ToDouble(ponto.Longitude)), GMarkerGoogleType.red_big_stop);
                            marker.ToolTipText = "Terminal";
                        }
                        }
                    catch {
                        MessageBox.Show(ponto.Latitude+"+"+ponto.Longitude);
                    }
                    
                    marker.Tag = ponto.Id;

                  
                    
                    listMarks.Add(marker);
                    markersOverlay.Markers.Add(marker);
                    gMapControl1.Overlays.Add(markersOverlay);
                }
            }
            else
            {
                foreach (areaObj area in listaAreas)
                {
                    marker = new GMarkerGoogle(new PointLatLng(Convert.ToDouble(area.Latitude), Convert.ToDouble(area.Longitude)), GMarkerGoogleType.blue);
                    marker.Tag = area.Id;
                    listMarks.Add(marker);
                    markersOverlay.Markers.Add(marker);
                    gMapControl1.Overlays.Add(markersOverlay);
                }
            }
        }
      
    
      

        private void gMapControl1_OnMapZoomChanged()
        {
            if (gMapControl1.Zoom > 15)
            {
                button6.Enabled = true;
                button2.Enabled = true;
                button7.Enabled = false;
                button5.Enabled = false;
                button11.Enabled = true;
                button12.Enabled = true;

                atualizaMarkers();
            }
            if (gMapControl1.Zoom < 15)
            {
                button6.Enabled = false;
                button2.Enabled = false;
                button7.Enabled = true;
                button5.Enabled = true;
                button11.Enabled = false;
                button12.Enabled = false;
                atualizaMarkers();
            }
        }

    
        private void gMapControl1_MouseClick(object sender, MouseEventArgs e)
        {
            if (testaInternet())
            {
                if (selecionar)
                {
                    selecionar = false;
                    latitude = gMapControl1.FromLocalToLatLng(e.X, e.Y).Lat;
                    longitude = gMapControl1.FromLocalToLatLng(e.X, e.Y).Lng;
                    PontoDAO ponto = new PontoDAO();
                    ponto.adicionaPonto(latitude, longitude);
                    atualizaMarkers();
                    MessageBox.Show("Adicionado!");
                    listaPontos.Clear();
                    listaPontos = pontoDAO.listaPontoDAO();
                    button2.Enabled = true;
                }
                if (selecionarExcluir)
                {
                    foreach (GMarkerGoogle g in listMarks)
                    {
                        if (g.IsMouseOver)
                        {
                            selecionarExcluir = false;
                            latitude = gMapControl1.FromLocalToLatLng(e.X, e.Y).Lat;
                            longitude = gMapControl1.FromLocalToLatLng(e.X, e.Y).Lng;
                            PontoDAO ponto = new PontoDAO();
                            ponto.ExcluirPonto(Convert.ToDouble(g.Tag));
                            atualizaMarkers();
                            MessageBox.Show("Excluido!");
                            break;
                        }
                    }
                    button6.Enabled = true;
                    listaPontos.Clear();
                    listaPontos = pontoDAO.listaPontoDAO();
                }
                if (selecionarArea)
                {
                    selecionarArea = false;
                    latitude = gMapControl1.FromLocalToLatLng(e.X, e.Y).Lat;
                    longitude = gMapControl1.FromLocalToLatLng(e.X, e.Y).Lng;
                    areaDAO ponto = new areaDAO();
                    ponto.adicionaPonto(latitude, longitude);
                    atualizaMarkers();
                    MessageBox.Show("Adicionado!");
                    listaAreas.Clear();
                    listaAreas = areaDAO.listaPontoDAO();
                    button7.Enabled = true;
                }
                if (selecionarExcluirArea)
                {
                    foreach (GMarkerGoogle g in listMarks)
                    {
                        if (g.IsMouseOver)
                        {
                            selecionarExcluirArea = false;
                            latitude = gMapControl1.FromLocalToLatLng(e.X, e.Y).Lat;
                            longitude = gMapControl1.FromLocalToLatLng(e.X, e.Y).Lng;
                            areaDAO ponto = new areaDAO();
                            ponto.ExcluirPonto(Convert.ToDouble(g.Tag));
                            atualizaMarkers();

                            MessageBox.Show("Excluido!");
                            break;
                        }
                    }
                    button5.Enabled = true;
                    listaAreas.Clear();
                    listaAreas = areaDAO.listaPontoDAO();
                }

                if (selecionarTerminal)
                {
                    foreach (GMarkerGoogle g in listMarks)
                    {
                        if (g.IsMouseOver)
                        {
                            selecionarExcluirTerminal = false;
                            PontoDAO ponto = new PontoDAO();
                            ponto.updatePontoTerminal(Convert.ToDouble(g.Tag),1);
                            atualizaMarkers();
                            MessageBox.Show("Alterado!");
                            break;
                        }
                    }
                    button11.Enabled = true;
                }
                if (selecionarExcluirTerminal)
                {
                    foreach (GMarkerGoogle g in listMarks)
                    {
                        if (g.IsMouseOver)
                        {
                            selecionarExcluirTerminal = false;
                            PontoDAO ponto = new PontoDAO();
                            ponto.updatePontoTerminal(Convert.ToDouble(g.Tag), 0);
                            atualizaMarkers();
                            MessageBox.Show("Alterado!");
                            break;
                        }
                    }
                    button12.Enabled = true;
                
                }
            }
        }

        private void gMapControl1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            testaInternet();
            gMapControl1.Position = new PointLatLng(gMapControl1.FromLocalToLatLng(e.X, e.Y).Lat, gMapControl1.FromLocalToLatLng(e.X, e.Y).Lng);
            gMapControl1.Zoom++;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (testaInternet())
            {
                MessageBox.Show("Clique no ponto que deseja adicionar!");
                selecionar = true;
                selecionarExcluir = false;
                selecionarArea = false;
                selecionarExcluirArea = false;
                selecionarTerminal = false;
                selecionarExcluirTerminal = false;
                if (button2.Enabled)
                {
                    button2.Enabled = false;
                    button6.Enabled = true;
                }
                else
                    button2.Enabled = true;
            }
        }
        private void button6_Click(object sender, EventArgs e)
        {
            if (button6.Enabled)
            {
                button6.Enabled = false;
                button2.Enabled = true;
            }
            else
                button6.Enabled = true;
            MessageBox.Show("Clique no marcador que deseja excluir!");
            selecionar = false;
            selecionarExcluir = true;
            selecionarArea = false;
            selecionarExcluirArea = false;
            selecionarTerminal = false;
            selecionarExcluirTerminal = false;

        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (button7.Enabled)
            {
                button7.Enabled = false;
                button5.Enabled = true;
            }
            else
                button7.Enabled = true;
            MessageBox.Show("Clique na area que deseja adicionar!");
            selecionar = false;
            selecionarExcluir = false;
            selecionarArea = true;
            selecionarExcluirArea = false;
        
          
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (button5.Enabled)
            {
                button5.Enabled = false;
                button7.Enabled = true;
            }
            else
                button5.Enabled = true;

            MessageBox.Show("Clique na area que deseja excluir!");
                selecionar = false;
                selecionarExcluir = false;
                selecionarArea = false;
                selecionarExcluirArea = true;
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            criarOnibus frm = new criarOnibus();

            frm.Show();        }

        private void button8_Click(object sender, EventArgs e)
        {
            gMapControl1.Zoom++;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            gMapControl1.Zoom--;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                dataGridView1.CurrentRow.Cells[0].Value.ToString();
                DialogResult dialogResult = MessageBox.Show("O onibus será excluido e todas suas informações serão perdidas.\n Voce quer continuar?", "Excluir onibus", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    OnibusDAO oni = new OnibusDAO();
                    oni.ExcluirOnibus(Convert.ToInt16(dataGridView1.CurrentRow.Cells[0].Value));
                    dataGridView1.CurrentCell.Dispose();
                    atualizaOnibus();
                    MessageBox.Show("Excluido!");
                }
                else if (dialogResult == DialogResult.No)
                {
                    //do something else
                }
               

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        private void button10_Click(object sender, EventArgs e)
        {
            atualizaOnibus();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            
            gerenciaOnibus form = new gerenciaOnibus(Convert.ToInt16(dataGridView1.CurrentRow.Cells[0].Value), dataGridView1.CurrentRow.Cells[1].Value.ToString(), dataGridView1.CurrentRow.Cells[2].Value.ToString(), dataGridView1.CurrentRow.Cells[3].Value.ToString());
            form.Show();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            if (testaInternet())
            {
                MessageBox.Show("Clique no ponto que deseja adicionar!");
                selecionar = false;
                selecionarExcluir = false;
                selecionarArea = false;
                selecionarExcluirArea = false;
                selecionarTerminal = true;
                selecionarExcluirTerminal = false;
                if (button2.Enabled)
                {
                    button2.Enabled = false;
                    button6.Enabled = true;
                }
                else
                    button2.Enabled = true;
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (button6.Enabled)
            {
                button6.Enabled = false;
                button2.Enabled = true;
            }
            else
                button6.Enabled = true;
            MessageBox.Show("Clique no marcador que deseja excluir!");
            selecionar = false;
            selecionarExcluir = false;
            selecionarArea = false;
            selecionarExcluirArea = false;
            selecionarTerminal = false;
            selecionarExcluirTerminal = true;
        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            var height = 30;
            foreach (DataGridViewRow dr in dataGridView1.Rows)
            {
                height += dr.Height;
            }

            dataGridView1.Height = height;
        }
    }
}
