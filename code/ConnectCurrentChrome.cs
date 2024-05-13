using Microsoft.Playwright;
using Newtonsoft.Json;

namespace PlayWrightLibrary;

public class ConnectCurrentChrome
{
    public delegate Task DoSth();

    /// <summary>
    /// 连接当前浏览器，对指定网页进行操作
    /// </summary>
    /// <param name="title">网页标题的部分内容</param>
    public static async Task ConnectCurrentPage(string title, DoSth sth)
    {
        var playwright = await Playwright.CreateAsync();
        var browser =
            await playwright.Chromium.ConnectOverCDPAsync(await GetChromeUrl());
        var page = browser.Contexts.First().Pages;
        IPage target;
        target = null!;
        foreach (var i in page)
        {
            await i.WaitForLoadStateAsync(LoadState.NetworkIdle);
            string pageTitle = await i.TitleAsync();
            if (pageTitle.Contains(title))
            {
                target = i;
                break;
            }
        }

        // NOTE:进行操作的地方
        await sth();
    }

    /// <summary>
    /// 获取目标IPage，并返回
    /// </summary>
    /// <param name="title">网页标题的部分内容</param>
    /// <returns>返回目标IPage</returns>
    public static async Task<IPage> GetCurrentPage(string title)
    {
        var playwright = await Playwright.CreateAsync();
        var browser =
            await playwright.Chromium.ConnectOverCDPAsync(await GetChromeUrl());
        var page = browser.Contexts.First().Pages;
        IPage target;
        target = null!;
        foreach (var i in page)
        {
            await i.WaitForLoadStateAsync(LoadState.NetworkIdle);
            string pageTitle = await i.TitleAsync();
            if (pageTitle.Contains(title))
            {
                target = i;
                break;
            }
        }

        return target;
    }

    /// <summary>
    /// 连接浏览器，并创建新的页面
    /// </summary>
    /// <param name="url">访问的url地址</param>
    /// <returns></returns>
    public static async Task<IPage> CreateNewPage(string url)
    {
        var playwright = await Playwright.CreateAsync();
        var browser =
            await playwright.Chromium.ConnectOverCDPAsync(await GetChromeUrl());
        IPage target = await browser.Contexts.First().NewPageAsync();
        await target.GotoAsync(url);
        await target.WaitForLoadStateAsync(LoadState.NetworkIdle);
        return target;
    }

    /// <summary>
    /// 连接浏览器，并创建新的页面
    /// </summary>
    /// <param name="urls">访问的url地址列表组</param>
    /// <returns></returns>
    public static async Task<IPage[]> CreateManyPages(string[] urls)
    {
        var playwright = await Playwright.CreateAsync();
        var browser =
            await playwright.Chromium.ConnectOverCDPAsync(await GetChromeUrl());
        List<IPage> pages = new List<IPage>();
        IBrowserContext context = browser.Contexts.First();
        Console.WriteLine(urls.Length);
        for (int i = 0; i < urls.Length; i++)
        {
            IPage temp = await context.NewPageAsync();
            await temp.GotoAsync(urls[i]);
            await temp.WaitForLoadStateAsync(LoadState.NetworkIdle);
            pages.Add(temp);
        }

        return pages.ToArray();
    }


    /// <summary>
    /// 获取 http://127.0.0.1:9222/json/version 返回的远程连接地址
    /// </summary>
    /// <returns></returns>
    public static async Task<string> GetChromeUrl()
    {
        using (HttpClient client = new HttpClient())
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync("http://127.0.0.1:9222/json/version");
                response.EnsureSuccessStatusCode(); // 确保响应成功

                string responseBody = await response.Content.ReadAsStringAsync();

                dynamic json = JsonConvert.DeserializeObject(responseBody);
                string webSocketDebuggerUrl = json.webSocketDebuggerUrl;
                return webSocketDebuggerUrl;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"HTTP 请求错误: {e.Message}");
                return "";
            }
        }
    }
}