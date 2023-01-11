using ArnoldVinkCode;
using System;
using System.Diagnostics;
using static ArnoldVinkCode.AVActions;
using static ArnoldVinkCode.AVAudioDevice;

namespace ZuseMe
{
    public partial class Media
    {
        public static void VolumeStatusCheck()
        {
            try
            {
                //Get current volume
                int currentVolume = AudioVolumeGet(false);
                bool currentMute = AudioMuteGetStatus(false);
                //Debug.WriteLine("Current volume: " + currentVolume + " / Previous volume: " + AppVariables.VolumeLevelPrevious);
                //Debug.WriteLine("Current mute: " + currentMute + " / Previous mute: " + AppVariables.VolumeMutePrevious);

                //Compare volume variable
                if (AppVariables.VolumeLevelPrevious != -1 && (currentVolume != AppVariables.VolumeLevelPrevious || currentMute != AppVariables.VolumeMutePrevious))
                {
                    if (AVSettings.Load(null, "VolumeShowOverlay", typeof(bool)))
                    {
                        ActionDispatcherInvoke(delegate
                        {
                            try
                            {
                                //Show media overlay
                                AppVariables.WindowOverlay.ShowWindowDuration(2500);
                            }
                            catch { }
                        });
                    }
                }

                //Update volume variable
                AppVariables.VolumeLevelPrevious = currentVolume;
                AppVariables.VolumeMutePrevious = currentMute;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to check volume: " + ex.Message);
            }
        }
    }
}