using static ArnoldVinkCode.AVActions;

namespace ZuseMe
{
    public partial class AppTasks
    {
        //Application Tasks
        public static AVTaskDetails vTask_MonitorVolume = new AVTaskDetails("MonitorVolume");
        public static AVTaskDetails vTask_MonitorMedia = new AVTaskDetails("MonitorMedia");
        public static AVTaskDetails vTask_MonitorPlayer = new AVTaskDetails("MonitorPlayer");
        public static AVTaskDetails vTask_LoginCheck = new AVTaskDetails("LoginCheck");
    }
}