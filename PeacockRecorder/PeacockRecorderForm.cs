using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;
using QTCSharpTool;
using Un4seen.Bass;
using Un4seen.Bass.Misc;
using Un4seen.Bass.AddOn.Enc;
using Un4seen.Bass.AddOn.Tags;
using ZDCloudAPISharp;
using PeacockRecorder.Properties;

namespace PeacockRecorder
{
    public partial class PeacockRecorderForm : Form
    {
        private SYNCPROC _playSync;
        private RECORDPROC _myRecProc;
        private int _notifyHandle = 0;
        private int _playHandle = 0;
        private int _recHandle = 0;
        private int _encHandle = 0;
        private bool prepareRecord = false;
        private EncoderLAME lame;
        private EncoderWAV wav;
        private EncoderOGG ogg;
        private EncoderWMA wma;
        private BaseEncoder enc;
        private string SavePath;
        private string RecordingFileName;
        private string RecordingFormat = "mp3";
        private int InputDevice = -1;
        private int CurrentInputDevice = 0;
        private int OutputDevice = -1;
        private int CurrentOutputDevice = 0;
        private bool canSpeak = false;

        public PeacockRecorderForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string appkey = "81c69df243";
            string seckey = "3d98555aac";
            if (File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "zdsr\\common\\bin\\ZDCloudAPI.dll")))
            {
                
            int rs = ZDCloudAPI.Initial(appkey, seckey, new ZDCloudAPI.ZDCloudAPICallBack((int t) =>
            {
                if (t == 1) Application.Exit();
            }));
            if (rs == 0) this.canSpeak = true;
            if (rs > 0)
            {
                this.canSpeak = false;
                //MessageBox.Show("错误代码：" + rs.ToString(), "错误");
                //Application.Exit();
            }
            }
            string mwtitle = "";
            if (File.Exists(Application.StartupPath + "\\rn.txt"))
            {
                mwtitle = File.ReadAllText(Application.StartupPath + "\\rn.txt", Encoding.Default);
            }
            if (!string.IsNullOrEmpty(mwtitle))
            {
                this.Text = mwtitle;
                this.notifyIconPeacockRecorder.Text = mwtitle;
            }
            BassNet.Registration("yk000123@sina.com", "2X34201017282922");
            this.initSavePath();
            this.initREcordingFormat();
            this.initCombBoxInputDevice(this.comboBoxInputDevice);
            this.initCombBoxOutputDevice(this.comboBoxOutputDevice);
            this.BassInit(true);
            this.PlayNotifySound("welcome.wav");
            this.BassRecordInit(true);
            this.supportCommandLine();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            bool reghotkeysuccess = this.RegHotKey();
            if (!reghotkeysuccess)
            {
                MessageBox.Show("热键注册失败可能是被其他程序占用，这会导致全局热键失效。", "热键注册失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            bool starthide = Settings.Default.StartHide;
            if (starthide)
            {
                this.checkBoxStartHide.Checked = true;
            }

            if (starthide && reghotkeysuccess)
            {
                this.Hide();
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.UnregHotKey();
            this.Hide();
            if (this.canSpeak) ZDCloudAPI.UnInitial();
            if (Settings.Default.NotificationSound)
            {
                _playSync = new SYNCPROC(EndSync);
                this.PlayNotifySound("exit.wav", _playSync);
                if (Bass.BASS_ChannelIsActive(_notifyHandle) == BASSActive.BASS_ACTIVE_PLAYING) e.Cancel = true;
            }
            else
            {
                this.BassFree();
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                this.PlayNotifySound("hide.wav");
                this.Hide();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }


        protected override void WndProc(ref Message m)
        {
            const int WM_HOTKEY = 0x0312;
            switch (m.Msg)
            {
                case WM_HOTKEY:
                    switch (m.WParam.ToInt32())
                    {
                        case 6768001:
                            this.buttonStartRecording_Click(null, null);
                            break;
                        case 6768002:
                            this.buttonStopRecording_Click(null, null);
                            break;
                        case 6768003:
                            this.buttonPlay_Click(null, null);
                            break;
                        case 6768004:
                            this.changeDevice(this.comboBoxRecordingFormat);
                            break;
                        case 6768005:
                            this.changeDevice(this.comboBoxInputDevice);
                            break;
                        case 6768006:
                            this.DeleteRecFile();
                            break;
                        case 6768007:
                            this.BassPosition("-");
                            break;
                        case 6768008:
                            this.BassPosition("+");
                            break;
                        case 6768009:
                            this.buttonOpenRecordingFolder_Click(null, null);
                            break;
                        case 6768010:
                            /**
                            if (this.Visible)
                            {
                                this.PlayNotifySound("hide.wav");
                                this.Hide();
                            }
                            else
                            {
                             * */
                            this.Show();
                            this.Focus();
                            //}
                            break;
                        case 6768100:
                            this.Close();
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
            base.WndProc(ref m);
        }

        private void buttonStartRecording_Click(object sender, EventArgs e)
        {
            this.StartRecording();
        }

                                    private void buttonStopRecording_Click(object sender, EventArgs e)
        {               
            this.StopRecording();
            this.StopPlaying();
        }

        private void buttonPlay_Click(object sender, EventArgs e)
        {
            this.StartPlaying();
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            this.StopPlaying();
        }

        private void buttonSavePath_Click_1(object sender, EventArgs e)
        {
            FolderBrowserDialog fb = new FolderBrowserDialog();
            fb.Description = "请选择录音文件的保存位置";
            if (fb.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string path = fb.SelectedPath;
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                textBoxSavePath.Text = path;
                Settings.Default.Savepath = path;
                Settings.Default.Save();
            }
        }

        private void buttonOpenRecordingFolder_Click(object sender, EventArgs e)
        {
            Process.Start("explorer.exe", this.SavePath);
        }

        private void checkBoxStartHide_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            Settings.Default.StartHide = cb.Checked;
            Settings.Default.Save();
        }

        private void checkBoxEnableNotificationSound_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            Settings.Default.NotificationSound = cb.Checked;
            Settings.Default.Save();
        }

        private void comboBoxInputDevice_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cb = sender as ComboBox;
            if (cb.SelectedIndex < 0)
            {
                return;
            }
            int id = cb.SelectedIndex;
            Settings.Default.InputDevice = id;
            Settings.Default.Save();
            if (id > 0)
            {
                id = id - 1;
            }
            this.InputDevice = id;
        }

        private void comboBoxOutputDevice_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cb = sender as ComboBox;
            if (cb.SelectedIndex < 0)
            {
                return;
            }
            int id = cb.SelectedIndex;
            Settings.Default.OutputDevice = id;
            Settings.Default.Save();
            if (id == 0)
            {
                id = -1;
            }
            this.OutputDevice = id;
        }

        private void comboBoxRecordingFormat_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cb = sender as ComboBox;
            if (cb.SelectedIndex < 0)
            {
                return;
            }
            Settings.Default.RecordingFormat = cb.Text;
            Settings.Default.Save();
            this.RecordingFormat = cb.Text;
        }

        private void notifyIconPeacockRecorder_Click(object sender, EventArgs e)
        {
            this.Show();
            this.Focus();
        }

        public void StartRecording()
        {
            if (this.prepareRecord)
            {
                return;
            }
            if (Bass.BASS_ChannelIsActive(_recHandle) == BASSActive.BASS_ACTIVE_PLAYING)
            {
                enc.Pause(true);
                Bass.BASS_ChannelPause(_recHandle);
                this.PlayNotifySound("pause_record.wav");
                this.buttonStartRecord.Text = "继续录音(&R)";
                return;
            }
            if (Bass.BASS_ChannelIsActive(_recHandle) == BASSActive.BASS_ACTIVE_PAUSED)
            {
                if (Settings.Default.NotificationSound)
                {
                    this.prepareRecord = true;
                                        _playSync = new SYNCPROC(ContinueRecordSync);
                    this.PlayNotifySound("continue_record.wav", _playSync);
                }
                else
                {
                    enc.Pause(false);
                    Bass.BASS_ChannelPlay(_recHandle, false);
                }
                this.buttonStartRecord.Text = "暂停录音(&R)";
                return;
            }
            this.StopPlaying();
            if (Settings.Default.NotificationSound)
            {
                this.prepareRecord = true;
                _playSync = new SYNCPROC(BeginRecordSync);
                this.PlayNotifySound("begin_record.wav", _playSync);
            }
            else
            {
                this._StartRecording();
            }
            this.buttonStopRecord.Enabled = true;
            this.buttonPlay.Enabled = false;
            this.buttonStop.Enabled = false;
        }

        public void _StartRecording()
        {
            this.BassRecordInit(false);
            // start recording paused           
            _myRecProc = new RECORDPROC(MyRecoring);
            _recHandle = Bass.BASS_RecordStart(44100, 2, BASSFlag.BASS_RECORD_PAUSE, _myRecProc, new IntPtr(_encHandle));
            TAG_INFO ti = new TAG_INFO();
            ti.album = "孔雀录音";
            ti.artist = "孔雀录音机";
            ti.year = DateTime.Now.Year.ToString();
            ti.publisher = "孔雀录音机";
            ti.producer = "孔雀录音机";
            switch (this.RecordingFormat)
            {
                case "mp3":
                    RecordingFileName = this.GetRecName() + ".mp3";
                    // needs 'lame.exe' !
                    // the recorded data will be written to a file called rectest.mp3
                    // create the encoder...320kbps, stereo
                    // MP3 encoder setup
                    lame = new EncoderLAME(_recHandle);
                    lame.InputFile = null;	//STDIN
                    lame.OutputFile = RecordingFileName;
                    lame.LAME_Bitrate = (int)EncoderLAME.BITRATE.kbps_320;
                    lame.LAME_Mode = EncoderLAME.LAMEMode.Default;
                    lame.LAME_TargetSampleRate = (int)EncoderLAME.SAMPLERATE.Hz_44100;
                    lame.LAME_Quality = EncoderLAME.LAMEQuality.Quality;
                    lame.TAGs = ti;
                    // really start recording
                    lame.Start(null, IntPtr.Zero, false);
                    enc = lame;
                    break;
                case "wav":
                    RecordingFileName = this.GetRecName() + ".wav";
                    // writing 16-bit wave file here (even if we use a float asio channel)
                    wav = new EncoderWAV(_recHandle);
                    wav.InputFile = null;  // STDIN
                    wav.OutputFile = RecordingFileName;
                    //wav.TAGs = ti;
                    wav.Start(null, IntPtr.Zero, false);
                    enc = wav;
                    break;
                case "ogg":
                    RecordingFileName = this.GetRecName() + ".ogg";
                    ogg = new EncoderOGG(_recHandle);
                    ogg.InputFile = null;	//STDIN
                    ogg.OutputFile = RecordingFileName;
                    ogg.OGG_UseQualityMode = true;
                    ogg.OGG_Quality = 4.0f;
                    //ogg.TAGs = ti;
                    ogg.Start(null, IntPtr.Zero, false);
                    enc = ogg;
                    break;
                case "wma":
                    RecordingFileName = this.GetRecName() + ".wma";
                    wma = new EncoderWMA(_recHandle);
                    wma.InputFile = null;
                    wma.OutputFile = RecordingFileName;
                    wma.WMA_Bitrate = (int)EncoderWMA.BITRATE.kbps_320;
                    wma.TAGs = ti;
                    wma.Start(null, IntPtr.Zero, false);
                    enc = wma;
                    break;
                default:
                    break;
            }
            Bass.BASS_ChannelPlay(_recHandle, false);
            this.buttonStartRecord.Text = "暂停录音(&R)";
        }

        public void StopRecording()
        {
            BASSActive status = Bass.BASS_ChannelIsActive(_recHandle);
            if (_recHandle != 0 && (status == BASSActive.BASS_ACTIVE_PLAYING || status == BASSActive.BASS_ACTIVE_PAUSED))
            {
                enc.Stop();
                Bass.BASS_ChannelStop(_recHandle);
                _recHandle = 0;
                enc = null;
                this.PlayNotifySound("end_record.wav");
                this.buttonStartRecord.Text = "开始录音(&R)";
                this.buttonStopRecord.Enabled = false;
                this.buttonPlay.Enabled = true;
                //this.buttonStop.Enabled = true;
            }
        }

        public void StartPlaying()
        {
            if (this.prepareRecord || Bass.BASS_ChannelIsActive(_recHandle) == BASSActive.BASS_ACTIVE_PLAYING || Bass.BASS_ChannelIsActive(_recHandle) == BASSActive.BASS_ACTIVE_PAUSED)
            {
                return;
            }
            if (Bass.BASS_ChannelIsActive(_playHandle) == BASSActive.BASS_ACTIVE_PLAYING)
            {
                Bass.BASS_ChannelPause(_playHandle);
                this.buttonPlay.Text = "继续播放(&P)";
                return;
            }
            if (Bass.BASS_ChannelIsActive(_playHandle) == BASSActive.BASS_ACTIVE_PAUSED)
            {
                Bass.BASS_ChannelPlay(_playHandle, false);
                this.buttonPlay.Text = "暂停播放(&P)";
                return;
            }
            Bass.BASS_StreamFree(_playHandle);
            this.BassInit(false);
            _playSync = new SYNCPROC(BassPlaySync);
            _playHandle = Bass.BASS_StreamCreateFile(RecordingFileName, 0L, 0L, BASSFlag.BASS_DEFAULT);
            Bass.BASS_ChannelSetSync(_playHandle, BASSSync.BASS_SYNC_ONETIME | BASSSync.BASS_SYNC_END, 0, _playSync, this.Handle);
            Bass.BASS_ChannelPlay(_playHandle, false);
            if (_playHandle != 0) this.buttonPlay.Text = "暂停播放(&P)";
            this.buttonStop.Enabled = true;
        }

        public void StopPlaying()
        {
            BASSActive status = Bass.BASS_ChannelIsActive(_playHandle);
            if (status == BASSActive.BASS_ACTIVE_PLAYING)
            {
                Bass.BASS_ChannelStop(_playHandle);
            }
            this.buttonPlay.Text = "开始播放(&P)";
            this.buttonStop.Enabled = false;
        }

        // the recording callback
        private bool MyRecoring(int handle, IntPtr buffer, int length, IntPtr user)
        {
            return true; // always continue recording
        }

        private void BeginRecordSync(int handle, int channel, int data, IntPtr user)
        {
            this._StartRecording();
            this.prepareRecord = false;
        }

        private void ContinueRecordSync(int handle, int channel, int data, IntPtr user)
        {
            enc.Pause(false);
            Bass.BASS_ChannelPlay(_recHandle, false);
            this.prepareRecord = false;
        }

        private void BassPlaySync(int handle, int channel, int data, IntPtr user)
        {
            this.buttonPlay.Text = "开始播放(&P)";
        }

        private void EndSync(int handle, int channel, int data, IntPtr user)
        {
            // close bass
            Bass.BASS_Stop();
            Bass.BASS_Free();
            Application.Exit();
        }

        public void BassPosition(string type)
        {
            if (_playHandle == 0) return;
            BASSActive status = Bass.BASS_ChannelIsActive(_playHandle);
            if (status != BASSActive.BASS_ACTIVE_PLAYING) return;
            if (type == "+")
            {
                Bass.BASS_ChannelSetPosition(_playHandle, Bass.BASS_ChannelBytes2Seconds(_playHandle, Bass.BASS_ChannelGetPosition(_playHandle)) + 3.000);
            }
            else if (type == "-")
            {
                Bass.BASS_ChannelSetPosition(_playHandle, Bass.BASS_ChannelBytes2Seconds(_playHandle, Bass.BASS_ChannelGetPosition(_playHandle)) - 3.000);
            }
        }

        public void BassInit(bool force)
        {
            if (force || this.OutputDevice != this.CurrentOutputDevice)
            {
                this.CurrentOutputDevice = this.OutputDevice;
                Bass.BASS_Free();
                if (!Bass.BASS_Init(this.CurrentOutputDevice, 44100, BASSInit.BASS_DEVICE_DEFAULT, this.Handle)) MessageBox.Show("Bass_Init error!");
            }
        }

        public void BassRecordInit(bool force)
        {
            if (force || this.InputDevice != this.CurrentInputDevice)
            {
                this.CurrentInputDevice = this.InputDevice;
                Bass.BASS_RecordFree();
                if (!Bass.BASS_RecordInit(this.InputDevice)) MessageBox.Show("Bass_RecordInit error!");
            }
        }

        public void BassFree()
        {
            // close bass
            Bass.BASS_Stop();
            Bass.BASS_Free();
        }

        public string GetRecName()
        {
            string path = this.SavePath; //Application.StartupPath + "\\录音文件";
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            string[] sa = Directory.GetFiles(path);
            int num = sa.Length + 1;
            string fm = null;
            if (num < 10)
            {
                fm = "00";
            }
            else if (num >= 10 && num < 100)
            {
                fm = "0";
            }
            string numstr = String.Format("{0}\\{1}{2}{3}", path, fm, num, DateTime.Now.ToString("-y年M月d日H点m分s秒"));
            return numstr;
        }

        public void initCombBoxInputDevice(ComboBox cb)
        {
            cb.Items.Add("Windows默认设备");
            BASS_DEVICEINFO info = new BASS_DEVICEINFO();
            for (int i = 0; Bass.BASS_RecordGetDeviceInfo(i, info); i++)
            {
                cb.Items.Add(info.name);
            }
            if (cb.Items.Count > 0)
            {
                int defaultId = Settings.Default.InputDevice;
                if (defaultId < 0)
                {
                    defaultId = 0;
                }
                cb.SelectedIndex = defaultId < cb.Items.Count ? defaultId : 0;
            }
        }

        public void initCombBoxOutputDevice(ComboBox cb)
        {
            cb.Items.Add("Windows默认设备");
            BASS_DEVICEINFO info = new BASS_DEVICEINFO();
            for (int i = 1; Bass.BASS_GetDeviceInfo(i, info); i++)
            {
                cb.Items.Add(info.name);
            }
            if (cb.Items.Count > 0)
            {
                int defaultId = Settings.Default.OutputDevice;
                if (defaultId < 0)
                {
                    defaultId = 0;
                }
                cb.SelectedIndex = defaultId < cb.Items.Count ? defaultId : 0;
            }
        }

        public void initSavePath()
        {
            string path = Settings.Default.Savepath;
            if (string.IsNullOrEmpty(path)) path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\我的" + this.Text;
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            Settings.Default.Save();
            this.SavePath = path;
            this.textBoxSavePath.Text = this.SavePath;
        }

        public void initREcordingFormat()
        {
            string _rf = Settings.Default.RecordingFormat;
            this.RecordingFormat = _rf;
            comboBoxRecordingFormat.SelectedItem = _rf;
        }

        public bool RegHotKey()
        {
            bool r = HotKey.RegisterHotKey(this.Handle, 6768001, HotKey.KeyModifiers.Control | HotKey.KeyModifiers.Shift, Keys.R);
            bool s = HotKey.RegisterHotKey(this.Handle, 6768002, HotKey.KeyModifiers.Control | HotKey.KeyModifiers.Shift, Keys.S);
            bool p = HotKey.RegisterHotKey(this.Handle, 6768003, HotKey.KeyModifiers.Control | HotKey.KeyModifiers.Shift, Keys.P);
            bool f = HotKey.RegisterHotKey(this.Handle, 6768004, HotKey.KeyModifiers.Control | HotKey.KeyModifiers.Shift, Keys.F);
            bool i = HotKey.RegisterHotKey(this.Handle, 6768005, HotKey.KeyModifiers.Control | HotKey.KeyModifiers.Shift, Keys.I);
            bool d = HotKey.RegisterHotKey(this.Handle, 6768006, HotKey.KeyModifiers.Control | HotKey.KeyModifiers.Shift, Keys.D);
            bool j = HotKey.RegisterHotKey(this.Handle, 6768007, HotKey.KeyModifiers.Control | HotKey.KeyModifiers.Shift, Keys.OemOpenBrackets);
            bool t = HotKey.RegisterHotKey(this.Handle, 6768008, HotKey.KeyModifiers.Control | HotKey.KeyModifiers.Shift, Keys.OemCloseBrackets);
            bool o = HotKey.RegisterHotKey(this.Handle, 6768009, HotKey.KeyModifiers.Control | HotKey.KeyModifiers.Shift, Keys.O);
            bool h = HotKey.RegisterHotKey(this.Handle, 6768010, HotKey.KeyModifiers.Control | HotKey.KeyModifiers.Shift, Keys.H);
            bool q = HotKey.RegisterHotKey(this.Handle, 6768100, HotKey.KeyModifiers.Control | HotKey.KeyModifiers.Shift, Keys.Q);
            return (r && s && p && f && i && d && t && j && o && h && q) ? true : false;
        }

        public void UnregHotKey()
        {
            HotKey.UnregisterHotKey(this.Handle, 6768100);
            HotKey.UnregisterHotKey(this.Handle, 6768001);
            HotKey.UnregisterHotKey(this.Handle, 6768002);
            HotKey.UnregisterHotKey(this.Handle, 6768003);
            HotKey.UnregisterHotKey(this.Handle, 6768004);
            HotKey.UnregisterHotKey(this.Handle, 6768005);
            HotKey.UnregisterHotKey(this.Handle, 6768006);
            HotKey.UnregisterHotKey(this.Handle, 6768007);
            HotKey.UnregisterHotKey(this.Handle, 6768008);
            HotKey.UnregisterHotKey(this.Handle, 6768009);
            HotKey.UnregisterHotKey(this.Handle, 6768010);
        }

        public void PlayNotifySound(string filename)
        {
            this.PlayNotifySound(filename, null);
        }

        public void PlayNotifySound(string filename, SYNCPROC proc)
        {
            if (Settings.Default.NotificationSound)
            {
                Bass.BASS_ChannelStop(_playHandle);
                this.BassInit(false);
                _notifyHandle = Bass.BASS_StreamCreateFile(Application.StartupPath + "\\sound\\" + filename, 0L, 0L, BASSFlag.BASS_DEFAULT);
                if (proc != null)
                {

                    Bass.BASS_ChannelSetSync(_notifyHandle, BASSSync.BASS_SYNC_ONETIME | BASSSync.BASS_SYNC_END, 0, _playSync, this.Handle);
                }
                Bass.BASS_ChannelPlay(_notifyHandle, false);
            }
        }

        public void DeleteRecFile()
        {
            BASSActive status = Bass.BASS_ChannelIsActive(_recHandle);
            if (status == BASSActive.BASS_ACTIVE_PLAYING)
            {
                return;
            }
            if (!File.Exists(RecordingFileName))
            {
                return;
            }
            if (MessageBox.Show("您确定要删除“" + RecordingFileName + "”吗？", "询问                                    ", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                Bass.BASS_StreamFree(_playHandle);
                enc = null;
                File.Delete(RecordingFileName);
            }
        }


        public void changeDevice(ComboBox cb)
        {
            int index = cb.SelectedIndex + 1;
            int cnt = cb.Items.Count;
            if (index >= cnt)
            {
                index = 0;
            }
            cb.SelectedIndex = index;
            this.PlayNotifySound("notify.wav");
            if (this.canSpeak)
            {
                ZDCloudAPI.Speak(cb.Text);
            }
        }
        public void supportCommandLine()
        {
           List<string> args = Environment.GetCommandLineArgs().ToList<string>();
            if (args.Count <= 1)
            {
                return;
            }
            bool beginRecord = false;
            if (args.Contains("sr") || args.Contains("startrecording") || args.Contains("开始录音"))
            {
                beginRecord = true;
            }
            string fmt = "";
            foreach (var item in args)
            {
                if (item == "mp3" || item == "wav" || item == "wma" || item == "ogg")
                {
                    fmt = item;
                    args.Remove(item);
                    break;
                }
            }
            if (!string.IsNullOrEmpty(fmt))
            {
                this.RecordingFormat = fmt;
                comboBoxRecordingFormat.SelectedItem = fmt;
            }
            if (beginRecord)
            {
                this.buttonStartRecording_Click(null, null);
            }
        }


    }
}
