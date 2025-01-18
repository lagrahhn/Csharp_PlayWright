namespace DefaultNamespace;

public class MouseEvent
{
    /// <summary>
    /// 鼠标下滑操作
    /// </summary>
    /// <param name="page">目标网页</param>
    /// <param name="count">下滑次数，默认10</param>
    /// <param name="startX">起始位置，默认0</param>
    /// <param name="startY">终止位置，默认200</param>
    /// <param name="timeout">下滑间隔，默认350ms</param>
    public static async Task ScrollDownAsync(IPage page, int count = 10, int startX = 0, int startY = 200,
        int timeout = 350)
    {
        // NOTE:连续 {count} 次鼠标下滑事件，以加载下一页界面
        for (int i = 0; i < count; i++)
        {
            await page.Mouse.WheelAsync(startX, startY);
            Thread.Sleep(timeout);
        }
    }

    public static void ScrollDown(IPage page, int count = 10, int startX = 0, int startY = 200, int timeout = 350)
    {
        // NOTE:连续 {count} 次鼠标下滑事件，以加载下一页界面
        for (int i = 0; i < count; i++)
        {
            page.Mouse.WheelAsync(startX, startY);
            Thread.Sleep(timeout);
        }
    }
}
