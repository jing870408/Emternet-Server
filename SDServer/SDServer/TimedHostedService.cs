using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SDServer.Models;
using System.IO;
using System.Diagnostics;
using Microsoft.Extensions.ML;
using SDServer.DataModels;
using System.Collections.Generic;
using SDServer.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace SDServer
{
    internal class TimedHostedService : IHostedService
    {
        private Timer timer;
        private readonly IServiceScopeFactory scopeFactory;
        private PredictionEnginePool<EmoData, EmoPredict> _prediction;
        private SDContext _context;
        private static string _appPath => Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]);
        private static string _DataPath => Path.Combine(_appPath, "..", "..", "..", "dist", "getX.exe");

        public TimedHostedService(IServiceScopeFactory scopeFactory)
        {
            this.scopeFactory = scopeFactory;
        }


        public Task StartAsync(CancellationToken cancellationToken)
        {
            timer = new Timer(DoWork, null, TimeSpan.Zero,
            TimeSpan.FromSeconds(30));

            return Task.CompletedTask;

        }

        private void DoWork(object state)
        {  
            if (DateTime.Now.Hour == 01 && DateTime.Now.Minute == 00)
            {
                using (var scope = scopeFactory.CreateScope())
                {
                    _context = scope.ServiceProvider.GetRequiredService<SDContext>();

                    Dopython();
                    //用爬蟲填入天氣資料
                    DoSort();
                    //將資料整理至emovardata
                    DoAvgHeartRate();
                    //整理心律資料
                    DoMechineLearning();
                    //呼叫機器學習
                    DoEmo();
                }


            }
        }

        private void DoEmo()
        {
            var emo = from p in _context.EmoVarData
                      where p.Date.ToString("d") == DateTime.Now.AddDays(-1).ToString("d")
                      select p;

            foreach (var i in emo)
            {
                
                var member = from m in _context.AccountEmo
                             where m.Account == i.Account
                             select m;
                
                member.FirstOrDefault().Mood = i.評分;
            }
            _context.SaveChanges();
        }

        private void DoMechineLearning()
        {
            
            var label = from w in _context.EmoVarData
                        where w.評分 == null
                        select w;

            foreach(var labellist in label)
            {
                EmoData emo = new EmoData
                {
                    Sleep = Convert.ToInt32(labellist.入睡),
                    Sleeptime = Convert.ToInt32(labellist.時常),
                    AvgHeartRate = Convert.ToInt32(labellist.平均),
                    SocialApp = Convert.ToSingle(labellist.社交軟體),
                    GameApp = Convert.ToSingle(labellist.遊戲軟體),
                    FunApp = Convert.ToSingle(labellist.娛樂軟體),
                    OtherApp = Convert.ToSingle(labellist.其他軟體),
                    Temperture = Convert.ToInt32(labellist.溫度),
                    Humidity = Convert.ToInt32(labellist.濕度),
                    //sunshine == Rain,日照 == 雨量
                    Sunshine = Convert.ToInt32(labellist.日照),
                    Airquality = Convert.ToInt32(labellist.空氣品質)
                };

                using (var scope = scopeFactory.CreateScope())
                {
                    _prediction = scope.ServiceProvider.GetRequiredService<PredictionEnginePool<EmoData, EmoPredict>>();
                    var res = _prediction.Predict(emo);
                    labellist.評分 = res.Score;
                }
                

            }
            _context.SaveChanges();
        }

        private void DoAvgHeartRate()
        {

            int tv3;

            
            var g = from o in _context.EmoVarData
                    where o.評分 == null
                    select o;

        
            foreach(var i in g)
            {

                var f = from p in _context.OriVarData
                        where p.Account == i.Account && p.Date.ToString("d") != i.Date.ToString("d")
                        select p.AvgHeartRate;


                if (!f.Any())
                {
                    tv3 = 75;
                }
                else
                {
                    tv3 = Convert.ToInt32(f.Average());
                }


                 var h  = from q in _context.OriVarData
                                            where q.Account == i.Account && q.Date.ToString("d") == i.Date.ToString("d")
                                            select q;

                
                if(h.FirstOrDefault().AvgHeartRate > (tv3 + 3))
                {
                        i.平均 = 3;
                }
                else if(h.FirstOrDefault().AvgHeartRate < (tv3 - 3))
                {
                        i.平均 = 1;                 
                }
                else
                {
                        i.平均 = 2;
                }
            }
            _context.SaveChanges();

        }

        private void DoSort()
        {
            string s0 = "21:00";
            string s1 = "23:00";
            string s2 = "00:00";
            string s3 = "1:00";
            DateTime d0 = Convert.ToDateTime(s0);
            DateTime d1 = Convert.ToDateTime(s1);
            DateTime d2 = Convert.ToDateTime(s2);
            DateTime d3 = Convert.ToDateTime(s3);

            int v1, v2,v8,v9,v10,v11;
            double v4,v5,v6,v7;

            var processlist = from s in _context.OriVarData
                              where s.Pros == 0
                              select s;
            foreach(var pl in processlist)
            {
                var temptime = Convert.ToDateTime(pl.Sleep.ToString());
                //sleep變數
                //9-11
                if (DateTime.Compare(temptime,d0)>=0 && DateTime.Compare(temptime,d1)<=0)
                {
                    v1 = 1;
                }
                //11x-12
                else if (DateTime.Compare(temptime, d1) > 0 && DateTime.Compare(temptime, d2) <= 0)
                {
                    v1 = 2;
                }
                //12x-1
                else if (DateTime.Compare(temptime, d2) > 0 && DateTime.Compare(temptime, d3) <= 0)
                {
                    v1 = 3;
                }
                //其他
                else
                {
                    v1 = 4;
                }
                //sleeptime變數
                if (pl.Sleeptime<=7.0)
                {
                    v2 = 1;
                }else if (pl.Sleeptime > 7.0 && pl.Sleeptime < 9.0)
                {
                    v2 = 2;
                }else if (pl.Sleeptime >= 9)
                {
                    v2 = 3;
                }
                else
                {
                    v2 = 0;
                }
                //avgheartrate另外處理

                //v4,v5,v6,v7 --> app
                double tv4 = pl.SocialApp.Value.Hours * 60 * 60 + pl.SocialApp.Value.Minutes * 60 + pl.SocialApp.Value.Seconds;
                double tv5 = pl.GameApp.Value.Hours * 60 * 60 + pl.GameApp.Value.Minutes * 60 + pl.GameApp.Value.Seconds;
                double tv6 = pl.FunApp.Value.Hours * 60 * 60 + pl.FunApp.Value.Minutes * 60 + pl.FunApp.Value.Seconds;
                double tv7 = pl.OtherApp.Value.Hours * 60 * 60 + pl.OtherApp.Value.Minutes * 60 + pl.OtherApp.Value.Seconds;

                v4 = Math.Round((tv4 / (tv4 + tv5 + tv6 + tv7)),2) ;
                v5 = Math.Round((tv5 / (tv4 + tv5 + tv6 + tv7)),2) ;
                v6 = Math.Round((tv6 / (tv4 + tv5 + tv6 + tv7)),2) ;
                v7 = Math.Round((tv7 / (tv4 + tv5 + tv6 + tv7)),2) ;

                if (pl.Temperture < 18)
                {
                    v8 = 1;
                }else if(pl.Temperture >= 18 && pl.Temperture <= 25)
                {
                    v8 = 2;
                }else if (pl.Temperture >= 26 && pl.Temperture <= 30)
                {
                    v8 = 3;
                }else if (pl.Temperture > 30)
                {
                    v8 = 4;
                }else
                {
                    v8 = 0;
                }

                if (pl.Humidity < 40)
                {
                    v9 = 1;
                }
                else if (pl.Humidity >= 40 && pl.Humidity <= 70)
                {
                    v9 = 2;
                } 
                else if (pl.Humidity > 70)
                {
                    v9 = 3;
                }
                else
                {
                    v9 = 0;
                }

                if (pl.Rain == 0)
                {
                    v10 = 1;
                }
                else if (pl.Rain >= 1 && pl.Rain <= 10)
                {
                    v10 = 2;
                }
                else if (pl.Rain >= 11 && pl.Rain <= 25)
                {
                    v10 = 3;
                }
                else if (pl.Rain >= 26 && pl.Rain <= 50)
                {
                    v10 = 4;
                }
                else if (pl.Rain >= 51 && pl.Rain <= 100)
                {
                    v10 = 5;
                }
                else if (pl.Rain > 100)
                {
                    v10 = 6;
                }
                else
                {
                    v10 = 0;
                }

                if (pl.AirQ >= 0 && pl.AirQ <= 50)
                {
                    v11= 1;
                }
                else if (pl.AirQ >= 51 && pl.AirQ <= 100)
                {
                    v11 = 2;
                }
                else if (pl.AirQ >= 101 && pl.AirQ <= 150)
                {
                    v11 = 3;
                }
                else if (pl.AirQ >= 151 && pl.AirQ <= 200)
                {
                    v11 = 4;
                }
                else if (pl.AirQ > 200)
                {
                    v11 = 5;
                }
                else
                {
                    v11 = 0;
                }
                var tempEmoVar = new EmoVarData
                {
                    Account = pl.Account,
                    Date = pl.Date,
                    入睡 = v1,
                    時常 = v2,
                    社交軟體 = v4,
                    遊戲軟體 = v5,
                    娛樂軟體 = v6,
                    其他軟體 = v7,
                    溫度 = v8,
                    濕度 = v9,
                    //日照==雨量(打錯)
                    日照 = v10,
                    空氣品質 = v11
                };
                _context.EmoVarData.Add(tempEmoVar);
                pl.Pros = 1;
            }
            _context.SaveChanges();
            
        }

        //執行python 
        private void Dopython()
        {
            var list = from o in _context.OriVarData
                       where o.Temperture == null && o.Humidity == null && o.Rain == null && o.AirQ == null
                       select o;

            foreach(var i in list) {
                String dat = i.Date.ToString("yyyy#MM#dd");
                Process P = new Process();
                P.StartInfo.FileName = _DataPath;   //路徑
                P.StartInfo.UseShellExecute = false; //必需
                P.StartInfo.RedirectStandardOutput = true;//输出参数设定
                P.StartInfo.RedirectStandardInput = true;//传入参数设定
                P.StartInfo.Arguments = (dat);
                P.StartInfo.CreateNoWindow = true;
                P.Start();
                var res = P.StandardOutput.ReadToEnd();
                P.WaitForExit();//关键，等待外部程序退出后才能往下执行
                P.Close();

                string[] result = res.Split(",");
                i.Temperture = Convert.ToDouble(result[0]);
                i.Humidity = Convert.ToDouble(result[1]);
                i.Rain = Convert.ToDouble(result[2]);
                i.AirQ = Convert.ToDouble(result[3]);

            }
            

            _context.SaveChanges();
            
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {

            timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }


    }
}
