using System;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Net;
using System.ComponentModel;
using System.Drawing;

namespace WeatherApplication
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.KeyUp += new KeyEventHandler(keys);
            this.BackColor = Color.FromArgb(42, 18, 36);
        }
        string APIKey = "e8bb2307ab09037674a50226a0f9ad79";

        private void btn_search_Click(object sender, EventArgs e)
        {
            getWeather();
        }

        private void getWeather()
        {
            using (WebClient web = new WebClient())
            {
                string url = string.Format("https://api.openweathermap.org/data/2.5/weather?q={0}&appid={1}&units=metric", TbCity.Text, APIKey);
                var json = web.DownloadString(url);
                WeatherInfo.root Info = JsonConvert.DeserializeObject<WeatherInfo.root>(json);
                pic_icon.ImageLocation = "http://openweathermap.org/img/wn/" + Info.weather[0].icon + "@2x.png";
                lab_condtion.Text = Info.weather[0].main;
                lab_detail.Text = Info.weather[0].description;
                lab_sunset.Text = convertDateTime(Info.sys.sunset).ToShortTimeString();
                lab_sunrise.Text = convertDateTime(Info.sys.sunrise).ToShortTimeString();
                lab_windspeed.Text = Info.wind.speed.ToString() + " m/s";
                lab_pressure.Text = Info.main.pressure.ToString() + " mm";
                lab_temp.Text = (Math.Ceiling(Info.main.temp).ToString() + " °C");
            }

        }
        DateTime convertDateTime(long sec)

        {
            DateTime day = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            day = day.AddSeconds(sec).ToLocalTime();
            return day;
        }

        private void keys(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                getWeather(); 
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lab_time.Text = DateTime.Now.ToString("HH:mm:ss");
        }
    }
}
