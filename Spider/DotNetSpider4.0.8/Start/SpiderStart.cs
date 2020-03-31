using System;
using System.Collections.Generic;
using System.Text;
using DotnetSpider;
using Microsoft.Extensions.Configuration;
using Serilog;
using DotnetSpider.Downloader;
using DotnetSpider.DataFlow;
using DotnetSpider.DataFlow.Parser;
using DotnetSpider.DataFlow.Storage;
namespace Spider4_0_8
{
	/// <summary>
	/// 爬虫启动
	/// </summary>
	public class SpiderStart
	{
		SpiderHostBuilder _hostBuilder;
		SpiderProvider _provider;
		
		public SpiderStart()
		{
			_hostBuilder = new SpiderHostBuilder()
				.ConfigureLogging(x => x.AddSerilog())
				.ConfigureAppConfiguration(x => {
					x.AddJsonFile("appsettings.json");
				})
				.ConfigureServices(services =>
				{
					services.AddLocalMessageQueue();
					services.AddDownloadCenter(x => x.UseLocalDownloaderAgentStore()) ;
					services.AddDownloaderAgent(x =>
					{
						x.UseFileLocker();
						x.UseDefaultAdslRedialer();
						x.UseDefaultInternetDetector();
					});
					services.AddStatisticsCenter(x => x.UseMemory());
				});
			_provider = _hostBuilder.Build();
			
		}
		/// <summary>
		/// 启动
		/// </summary>
		/// <param name="url">抓取地址</param>
		/// <param name="parser">页面分析器</param>
		/// <param name="storage">数据存储器,,默认显示在控制台</param>
		public void Start(string url, List<IDataFlow> parser, IDataFlow storage = null)
		{
			Spider _spider = _provider.Create<Spider>();
			if (null == storage)
				storage = new ConsoleStorage();
			_spider.NewGuidId(); // 设置任务标识
			_spider.Name = "测试采集"; // 设置任务名称
			_spider.Speed = 10; // 设置采集速度, 表示每秒下载多少个请求, 大于 1 时越大速度越快, 小于 1 时越小越慢, 不能为0.
			_spider.Depth = 3; // 设置采集深度
			
			if(parser != null)
			{
				foreach (IDataFlow item in parser)
					_spider.AddDataFlow(item);
			}
			_spider.AddDataFlow(storage);
			_spider.AddRequests(new Request(url)); // 设置起始链接
			_spider.RunAsync(); // 启动

		}
		/// <summary>
		/// 启动
		/// </summary>
		/// <param name="request">抓取请求</param>
		/// <param name="parser">页面分析器</param>
		/// <param name="storage">数据存储器,,默认显示在控制台</param>
		public void Start(List<Request> request, List<IDataFlow> parser, IDataFlow storage = null)
		{
			Spider _spider = _provider.Create<Spider>();
			if (null == storage)
				storage = new ConsoleStorage();
			_spider.NewGuidId(); // 设置任务标识
			_spider.Name = "测试采集"; // 设置任务名称
			_spider.Speed = 10; // 设置采集速度, 表示每秒下载多少个请求, 大于 1 时越大速度越快, 小于 1 时越小越慢, 不能为0.
			_spider.Depth = 3; // 设置采集深度
			if (parser != null)
			{
				foreach (IDataFlow item in parser)
					_spider.AddDataFlow(item);
			}
			_spider.AddDataFlow(storage);
			_spider.AddRequests(request.ToArray()); // 设置链接
			_spider.RunAsync(); // 启动

		}
	}
}
