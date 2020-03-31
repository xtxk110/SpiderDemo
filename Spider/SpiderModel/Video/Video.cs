using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
/// <summary>
/// 视频相关
/// </summary>
namespace SpiderModel.Video
{
    [Description("影视")]
    public class VideoInfo
    {
        public VideoInfo()
        {
            Details = new List<VideoDetail>();
        }
        /// <summary>
        /// 1:电影；2:电视剧；3:动漫；4:综艺
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 封面
        /// </summary>
        public string Cover { get; set; }
        /// <summary>
        /// 年份
        /// </summary>
        public string Year { get; set; }
        /// <summary>
        /// 描述或参演人员
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 是否需要付费(电视剧请以详情为准)
        /// </summary>
        public bool IsPay { get; set; }
        /// <summary>
        /// 详情页面URL
        /// </summary>
        public string ParentUrl { get; set; }
        /// <summary>
        /// 视频详情
        /// </summary>
        public List<VideoDetail> Details { get; set; }
    }
    public class VideoDetail
    {
        /// <summary>
        /// 播放地址
        /// </summary>
        public string PlayUrl { get; set; }
        /// <summary>
        /// 对应集数
        /// </summary>
        public string Number { get; set; }
        /// <summary>
        /// 是否需要付费
        /// </summary>
        public bool IsPay { get; set; }
        /// <summary>
        /// 视频网站名
        /// </summary>
        public string PlayName { get; set; }
    }
}
