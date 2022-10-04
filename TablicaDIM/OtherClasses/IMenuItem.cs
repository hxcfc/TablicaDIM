namespace TablicaDIM.OtherClasses
{
    public interface IMenuItem
    {
        public string Title { get; }
        public static string TittleToMenu { get; }
        public object InputsVMCon { get; }
    }
}
