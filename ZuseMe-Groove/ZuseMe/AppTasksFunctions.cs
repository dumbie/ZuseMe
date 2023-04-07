using System.Threading.Tasks;
using static ArnoldVinkCode.AVActions;
using static ZuseMe.Media;

namespace ZuseMe
{
    public partial class AppTasks
    {
        public static async Task vTaskLoop_MonitorPlayer()
        {
            try
            {
                while (TaskCheckLoop(vTask_MonitorPlayer))
                {
                    await UpdateMediaPlayerSession();

                    //Delay the loop task
                    await TaskDelay(2000, vTask_MonitorPlayer);
                }
            }
            catch { }
        }

        public static async Task vTaskLoop_MonitorMedia()
        {
            try
            {
                while (TaskCheckLoop(vTask_MonitorMedia))
                {
                    await UpdateMediaInformation();

                    //Delay the loop task
                    await TaskDelay(1000, vTask_MonitorMedia);
                }
            }
            catch { }
        }

        public static async Task vTaskLoop_MonitorVolume()
        {
            try
            {
                while (TaskCheckLoop(vTask_MonitorVolume))
                {
                    VolumeStatusCheck();

                    //Delay the loop task
                    await TaskDelay(1000, vTask_MonitorVolume);
                }
            }
            catch { }
        }
    }
}