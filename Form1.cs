using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WMPLib;

namespace MusicPlayerListBoxTask
{
    public partial class Form1 : Form
    {
        List<MediaFile> mediaFiles = new List<MediaFile>();
        public Form1()
        {
            InitializeComponent();
            var myPlayList = axWindowsMediaPlayer1.playlistCollection.newPlaylist("MyPlayList");
            axWindowsMediaPlayer1.currentPlaylist = myPlayList;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            listBox1.ValueMember = "Path";
            listBox1.DisplayMember = "FileName";
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = 0;
            for (int i = 0; i < axWindowsMediaPlayer1.currentPlaylist.count; i++)
            {

                if (axWindowsMediaPlayer1.currentPlaylist.Item[i].name.Equals(listBox1.SelectedItem.ToString()))
                {
                    index = i;
                    break;
                    
                }
            }
            axWindowsMediaPlayer1.Ctlcontrols.playItem(axWindowsMediaPlayer1.currentPlaylist.Item[index]);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            using(OpenFileDialog openFileDialog = new OpenFileDialog() { Multiselect = true,ValidateNames= true,Filter = "MP3|*.mp3|MP4|*mp4|WAV|*wav"})
            {
                if(openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    if(!mediaFiles.Any((x) => x.FileName.Equals(Path.GetFileNameWithoutExtension(openFileDialog.FileName))))
                    {
                        listBox1.Items.Add(Path.GetFileNameWithoutExtension(openFileDialog.FileName));
                        mediaFiles.Add(new MediaFile() { FileName = Path.GetFileNameWithoutExtension(openFileDialog.FileName), Path = openFileDialog.FileName });
                        var mediaItem = axWindowsMediaPlayer1.newMedia(openFileDialog.FileName);
                        axWindowsMediaPlayer1.currentPlaylist.appendItem(mediaItem);
                        axWindowsMediaPlayer1.Ctlcontrols.play();
                    }
                } 
            }
        }

        private void axWindowsMediaPlayer1_Enter(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.listBox1.Items.Clear();
            this.mediaFiles.Clear();
            axWindowsMediaPlayer1.Ctlcontrols.stop();
        }

        private void axWindowsMediaPlayer1_PlayStateChange(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e)
        {
            if (axWindowsMediaPlayer1.playState == WMPLib.WMPPlayState.wmppsMediaEnded)
            {
                axWindowsMediaPlayer1.Ctlcontrols.next();
                //axWindowsMediaPlayer1.Ctlcontrols.;
                axWindowsMediaPlayer1.Ctlcontrols.play();

                //try
                //{

                //    MediaFile media = listBox1.SelectedItem as MediaFile;
                //    var task = Task.Factory.StartNew(() =>
                //    {
                //        if (media != null)
                //        {
                //            axWindowsMediaPlayer1.URL = media.Path;
                //            axWindowsMediaPlayer1.Ctlcontrols.stop();
                //            axWindowsMediaPlayer1.Ctlcontrols.play();
                //        }
                //    });
                //    task.Wait();

                //}
                //catch (Exception ex)
                //{

                //}

            }
        }

        private void axWindowsMediaPlayer1_CurrentItemChange(object sender, AxWMPLib._WMPOCXEvents_CurrentItemChangeEvent e)
        {

            //if(listBox1.Items.Count != 0)
            //{

            //    listBox1.SelectedIndex = axWindowsMediaPlayer1.Ctlcontrols.currentItem.attributeCount-1;
            //        //(e.pdispMedia as IWMPMedia).attributeCount - 1;
            //}

            if (listBox1 == null || listBox1.Items.Count == 0)
            {
                return;
            }

            for (int i = 0; i < axWindowsMediaPlayer1.currentPlaylist.count; i++)
            {

                if (axWindowsMediaPlayer1.currentPlaylist.Item[i].name.Equals(axWindowsMediaPlayer1.Ctlcontrols.currentItem.name))
                {
                    listBox1.SelectedIndex = i;

                    break;

                }
            }
        }
    }
}
