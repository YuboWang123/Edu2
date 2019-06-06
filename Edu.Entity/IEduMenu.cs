namespace Edu.Entity
{
    /// <summary>
    /// menu configs
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IEduMenu
    {
        string Name { get; set; }
        bool IsEnabled { get; set; }
        int OrderNo { get; set; }
        
    }
}
