namespace PhantomDanmaku.Runtime
{
    /// <summary>
    /// 可交互的对象
    /// </summary>
    public interface Interoperable
    {
        /// <summary>
        /// 显示交互提示
        /// </summary>
        void ShowTip();

        void HideTip();

        /// <summary>
        /// 具体交互逻辑
        /// </summary>
        void Interact();

        /// <summary>
        /// 可交互判断，一般是距离小于某个数值
        /// </summary>
        /// <returns></returns>
        bool CanInteract { get; }
    }
}