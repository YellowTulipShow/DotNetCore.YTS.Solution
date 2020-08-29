namespace YTS.Test
{
    public interface ITestOutput
    {
        void OnInit();

        void OnEnd();

        void Write(string msg);
        void WriteError(string msg);
        void WriteInfo(string msg);
        void WriteWarning(string msg);

        void WriteLine(string msg);
        void WriteLineError(string msg);
        void WriteLineInfo(string msg);
        void WriteLineWarning(string msg);
    }
}
