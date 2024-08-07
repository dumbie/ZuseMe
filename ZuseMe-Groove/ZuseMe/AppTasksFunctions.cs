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
                while (await TaskCheckLoop(vTask_MonitorPlayer, 2000))
                {
                    //Update media player session
                    await UpdateMediaPlayerSession();
                }
            }
            catch { }
        }

        public static async Task vTaskLoop_MonitorMedia()
        {
            try
            {
                while (await TaskCheckLoop(vTask_MonitorMedia, 1000))
                {
                    //Update media information
                    await UpdateMediaInformation();
                }
            }
            catch { }
        }

        public static async Task vTaskLoop_MonitorVolume()
        {
            try
            {
                while (await TaskCheckLoop(vTask_MonitorVolume, 1000))
                {
                    //Check volume status
                    VolumeStatusCheck();
                }
            }
            catch { }
        }
    }
}