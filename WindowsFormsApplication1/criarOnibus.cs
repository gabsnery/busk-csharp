using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GMap.NET.MapProviders;
using GMap.NET;
using GMap.NET.ObjectModel;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using WindowsFormsApplication1.DAO;
using WindowsFormsApplication1.POJO;
using System.Runtime.Serialization;

namespace WindowsFormsApplication1
{
    public partial class criarOnibus : Form
    {
        int linhaSegunda=0;
        int linhaDomingo=0;
        int linhaFeriado=0;
        HorarioObj horaObj;
        List<HorarioObj> listaHorarios = new List<HorarioObj>();
        List<onibusPontoObj>   listaPontosOnibus= new List<onibusPontoObj>();
        List<PontoObj> listaPontos = new List<PontoObj>();
        List<areaObj> listaAreas = new List<areaObj>();
        PontoDAO pontoDAO = new PontoDAO();
        areaDAO areaDAO = new areaDAO();
        onibusPontoObj onibuspontoObj = new onibusPontoObj();
        GMarkerGoogle marker;
        List<GMarkerGoogle> listMarks;

        public void atualizaHorarios()
        {
            
            linhaSegunda = 0;
            linhaDomingo = 0;
            linhaFeriado = 0;
            dataGridView1.Rows.Clear();
            listaHorarios = listaHorarios.OrderBy(o => o.Horario).ToList();
            dataGridView1.Rows.Add();
            dataGridView1.Rows.Add();


            foreach (HorarioObj h in listaHorarios)
            {
                if (h.Dia == 0)
                {

                    dataGridView1.Rows[linhaSegunda].Cells[0].Value = h.Horario;
                    linhaSegunda++;


                }
                if (h.Dia == 1)
                {

                    dataGridView1.Rows[linhaDomingo].Cells[1].Value = h.Horario;
                    linhaDomingo++;

                }
                if (h.Dia == 2)
                {
                    dataGridView1.Rows[linhaFeriado].Cells[2].Value = h.Horario;
                    linhaFeriado++;
                }
                if (linhaSegunda - dataGridView1.Rows.Count == -1 || linhaDomingo - dataGridView1.Rows.Count == -1 || linhaFeriado - dataGridView1.Rows.Count == -1)
                {
                    dataGridView1.Rows.Add();
                }
            }
        }

        public criarOnibus()
        {
            InitializeComponent();
        }
        public void atualizaMarkers()
        {
            GMapOverlay markersOverlay = new GMapOverlay("markers");
            gMapControl1.Overlays.Clear();
            listMarks.Clear();
            if (gMapControl1.Zoom > 14)
            {
                foreach (PontoObj ponto in listaPontos)
                {

                    if (ponto.Terminal == 1)
                    {
                        marker = new GMarkerGoogle(new PointLatLng(Convert.ToDouble(ponto.Latitude), Convert.ToDouble(ponto.Longitude)), GMarkerGoogleType.red_big_stop);
                        marker.ToolTipText = "Terminal";
                    }

                    else if (ponto.Selecionado)
                    {
                        marker = new GMarkerGoogle(new PointLatLng(Convert.ToDouble(ponto.Latitude), Convert.ToDouble(ponto.Longitude)), GMarkerGoogleType.green_small);
                        marker.ToolTipText = ponto.Ordem.ToString();
                    }
                    else
                    {
                        marker = new GMarkerGoogle(new PointLatLng(Convert.ToDouble(ponto.Latitude), Convert.ToDouble(ponto.Longitude)), GMarkerGoogleType.blue_small);
                        marker.ToolTipText = "Latitude:" + ponto.Latitude + "\n" + "Longitude:" + ponto.Longitude;
                    }
                    markersOverlay.Markers.Add(marker);
                    marker.Tag = ponto.Id;

                    gMapControl1.Overlays.Add(markersOverlay);
                    listMarks.Add(marker);
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
  
        private void maskedTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {

                string text = "";
                int hour = 0;
                int minute = 0;
                try
                {
                     text = maskedTextBox1.Text.ToString();
                    string[] parts = text.Split(':');
                    hour = int.Parse(parts[0]);
                    minute = int.Parse(parts[1]);
                }
                catch (Exception)
                {
                    MessageBox.Show("Complete o campo!");
                }

                if (maskedTextBox1.Text.Count() == 5 && (hour <= 24 && hour >= 0) && (minute >= 00 && minute <= 59))
                {
                    
                    if (checkBoxSeg.Checked || (checkBoxDom.Checked) || checkBoxFer.Checked)
                    {
                        if (linhaSegunda-dataGridView1.Rows.Count==-1 || linhaDomingo-dataGridView1.Rows.Count == -1 || linhaFeriado- dataGridView1.Rows.Count == -1)
                        {
                            dataGridView1.Rows.Add();
                        }

                    }
                        
                    else
                        MessageBox.Show("Selecione o(os) dia!");
                    if (checkBoxSeg.Checked)
                    {
                        horaObj = new HorarioObj();
                        horaObj.Horario = text;
                        horaObj.Dia = 0;
                        dataGridView1.Rows[linhaSegunda].Cells[0].Value=text;
                        listaHorarios.Add(horaObj);
                        linhaSegunda++;
       

                    }
                    if (checkBoxDom.Checked)
                    {
                        horaObj = new HorarioObj();
                        horaObj.Horario = text;
                        horaObj.Dia = 1;
                        dataGridView1.Rows[linhaDomingo].Cells[1].Value = text;
                        listaHorarios.Add(horaObj);
                        linhaDomingo++;

                    }
                    if (checkBoxFer.Checked)
                    {
                        horaObj = new HorarioObj();
                        horaObj.Horario = text;
                        horaObj.Dia = 2;
                        dataGridView1.Rows[linhaFeriado].Cells[2].Value = text;
                        listaHorarios.Add(horaObj);
                        linhaFeriado++;
                    }
                    
                }
                
                else { MessageBox.Show("Hora invalida!"); }
                maskedTextBox1.Clear();
                listaHorarios.ToString();
            }
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
        private void gerenciaOnibus_Load(object sender, EventArgs e)
        {
            this.Size = Screen.PrimaryScreen.WorkingArea.Size;
            pictureBox1.ImageLocation = @"download.gif";
            listaAreas = areaDAO.listaPontoDAO();
            PontoDAO pontoDAO = new PontoDAO();
            listaPontos = pontoDAO.listaPontoDAO();
            listMarks = new List<GMarkerGoogle>();
            gMapControl1.DragButton = MouseButtons.Left;
            gMapControl1.CanDragMap = true;
            // gMapControl1.MapProvider = GMapProviders.GoogleMap;
            gMapControl1.Position = new PointLatLng(-22.725, -47.6476);
            gMapControl1.MinZoom = 0;
            gMapControl1.MaxZoom = 24;
            gMapControl1.Zoom = 12;
            gMapControl1.AutoScroll = true;
            gMapControl1.MapProvider = GMapProviders.OviHybridMap;
            atualizaMarkers();
        }   
        private void gMapControl1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            gMapControl1.Position = new PointLatLng(gMapControl1.FromLocalToLatLng(e.X, e.Y).Lat, gMapControl1.FromLocalToLatLng(e.X, e.Y).Lng);
            gMapControl1.Zoom++;
            atualizaMarkers();      
        }
        
        //cria tudo
        private void button2_Click(object sender, EventArgs e)
        {
            if (testaInternet())
            {
                pictureBox1.Visible = true;
                OnibusDAO oni = new OnibusDAO();
                PontoDAO pontoDAO = new PontoDAO();
                int idOnibus = oni.adicionaOnibus(textBoxNome.Text.ToString(), maskedTextBox2.Text.ToString(), textBoxLetra.Text.ToString());

                foreach (HorarioObj l in listaHorarios)
                {
                    HorarioDAO horDAO = new HorarioDAO();
                    horDAO.adicionaHorarioDAO(l.Horario.ToString(), idOnibus, l.Dia);
                }
                onibusPontoDAO onibusPonto = new onibusPontoDAO();
                foreach (onibusPontoObj onHor in listaPontosOnibus)
                {
                    onibusPonto.adicionaPontoOnibus(onHor.Id_ponto, idOnibus, onHor.Ida,onHor.Ordem);
                }
                MessageBox.Show("Onibus cadastrado com sucesso!");
                linhaSegunda = 0;
                linhaDomingo = 0;
                linhaFeriado = 0;
                maskedTextBox2.Clear();
                maskedTextBox1.Clear();
                dataGridView1.Rows.Clear();
                textBoxLetra.Clear();
                textBoxNome.Clear();
                pictureBox1.Visible = false;
            }

        }
        private void button1_Click(object sender, EventArgs e)
        {
            // criarPonto lala = new criarPonto();
            // lala.Show();
        }
        private void gMapControl1_OnMapZoomChanged_1()
        {
            atualizaMarkers();
        }
        private void selecaoPonto_Click(object sender, EventArgs e)
        {
            if (selecaoPonto.Enabled)
            {
                MessageBox.Show("Selecione o(os) pontos!");
                selecaoPonto.Enabled = false;
                selecaoPonto.BackColor = Color.Red;
                deselecaoPonto.Enabled = true;
                deselecaoPonto.BackColor = Color.Transparent;
            }
        }
        private void deselecaoPonto_Click(object sender, EventArgs e)
        {
            if (deselecaoPonto.Enabled)
            {
                MessageBox.Show("Selecione o(os) pontos para tirar!");
                deselecaoPonto.Enabled = false;
                deselecaoPonto.BackColor = Color.Red;
                selecaoPonto.BackColor = Color.Transparent;
                selecaoPonto.Enabled = true;
            }
            else
                deselecaoPonto.Enabled = true;
        }
        private void button6_Click(object sender, EventArgs e)
        {
            atualizaMarkers();

        }
        private void gMapControl1_MouseClick(object sender, MouseEventArgs e)
        {
            if (gMapControl1.Zoom > 15)
            {
                foreach (GMarkerGoogle g in listMarks)
                {
                    if (g.IsMouseOver)
                    {


                        if (radioButton1.Checked || radioButton2.Checked)
                        {
                            if (!maskedTextBox3.Text.Equals(""))
                            {
                                // Console.WriteLine("Marker hit? " + gmark.Position.ToString());
                                if (!selecaoPonto.Enabled)
                                {
                                    onibuspontoObj = new onibusPontoObj();
                                    onibuspontoObj.Latitude = g.Position.Lat.ToString();
                                    if (radioButton1.Checked)
                                        onibuspontoObj.Ida = 0;
                                    else
                                        onibuspontoObj.Ida = 1;
                                    onibuspontoObj.Longitude = g.Position.Lng.ToString();
                                    onibuspontoObj.Id_ponto = Convert.ToInt16(g.Tag);
                                    onibuspontoObj.Ordem = Convert.ToInt16(maskedTextBox3.Text);
                                    listaPontosOnibus.Add(onibuspontoObj);
                                    foreach (PontoObj ponto in listaPontos)
                                    {
                                        if ((Convert.ToDouble(ponto.Latitude) == g.Position.Lat) && (Convert.ToDouble(ponto.Longitude) == g.Position.Lng))
                                        {
                                            ponto.Selecionado = true;
                                            ponto.Ordem = Convert.ToInt16(maskedTextBox3.Text);
                                        }
                                    }
                                    atualizaMarkers();
                                    break;
                                }

                            }
                            else
                            { MessageBox.Show("Escolha a ordem do ponto do trajeto do onibus"); }
                        }
                        else
                        { MessageBox.Show("Defina se é um ponto de ida ou volta"); }

                        if (!deselecaoPonto.Enabled)
                        {
                            foreach (onibusPontoObj pontoni in listaPontosOnibus)
                            {
                                Console.WriteLine(pontoni.Latitude.ToString() + pontoni.Longitude.ToString());
                                if ((Convert.ToDouble(pontoni.Latitude) == g.Position.Lat) && Convert.ToDouble(pontoni.Longitude) == g.Position.Lng)
                                {
                                    listaPontosOnibus.Remove(pontoni);
                                    foreach (PontoObj ponto in listaPontos)
                                    {
                                        if ((Convert.ToDouble(ponto.Latitude) == g.Position.Lat) && (Convert.ToDouble(ponto.Longitude) == g.Position.Lng))
                                        {
                                            ponto.Selecionado = false;
                                        }
                                    }
                                    atualizaMarkers();
                                    break;
                                }
                            }
                        }
                        break;

                    }

                }

            }
           
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_KeyPress(object sender, KeyPressEventArgs e)
        {
           
            
        }

        private void dataGridView1_Leave(object sender, EventArgs e)
        {
           
        }

        private void buttonApagaHora_Click(object sender, EventArgs e)
        {
            try
            {
                dataGridView1.CurrentCell.Value.ToString();
            }
            catch {
                MessageBox.Show("Seleciona uma celula para Excluir!");
            }
            foreach (HorarioObj h in listaHorarios)
            {
                if (dataGridView1.CurrentCell.Value.ToString().Equals(h.Horario))
                {
                    if (dataGridView1.CurrentCell.ColumnIndex ==h.Dia)
                    {
                        listaHorarios.Remove(h);
                        if (dataGridView1.CurrentCell.ColumnIndex == 0)
                        {
                            dataGridView1.CurrentCell.Value = "";
                            linhaSegunda--;
                        }
                        if (dataGridView1.CurrentCell.ColumnIndex == 1)
                        {
                            dataGridView1.CurrentCell.Value = "";
                            linhaDomingo--;
                        }
                        if (dataGridView1.CurrentCell.ColumnIndex == 2)
                        {
                            dataGridView1.CurrentCell.Value = "";
                            linhaFeriado--;
                        }
                        MessageBox.Show("Excluido!");
                        break;
                    }
                }
               
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            atualizaHorarios();
        }

        private void maskedTextBox2_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void gMapControl1_Load(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            gMapControl1.Zoom++;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            gMapControl1.Zoom--;
        }

        private void maskedTextBox1_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
           
        }

        private void dataGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
        
        }

        private void dataGridView1_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
       
        }
    }
}

	
	
	
	
	
	
	
	
	
	     