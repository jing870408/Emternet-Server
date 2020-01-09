using System;
using System.Collections.Generic;

namespace SDServer.Models
{
    public partial class EmoVarData
    {
        public int? 憤怒 { get; set; }
        public int? 羨慕 { get; set; }
        public int? 恐懼 { get; set; }
        public int? 悲傷 { get; set; }
        public int? 希望 { get; set; }
        public int? 快樂 { get; set; }
        public int? 愛 { get; set; }
        public int? 厭惡 { get; set; }
        public int? 不滿 { get; set; }
        public string 評分 { get; set; }
        public int? 入睡 { get; set; }
        public int? 時常 { get; set; }
        public int? 平均 { get; set; }
        public double? 社交軟體 { get; set; }
        public double? 遊戲軟體 { get; set; }
        public double? 娛樂軟體 { get; set; }
        public double? 其他軟體 { get; set; }
        public int? 溫度 { get; set; }
        public int? 濕度 { get; set; }
        public int? 日照 { get; set; }
        public int? 空氣品質 { get; set; }
        public string Account { get; set; }
        public DateTime Date { get; set; }
        public int Idx { get; set; }
        public string MoodFinal { get; set; }
    }
}
