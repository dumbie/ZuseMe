using ArnoldVinkCode;
using System;
using System.Diagnostics;
using System.Windows.Media.Imaging;
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
                //Check overlay setting
                if (!AVSettings.Load(null, "VolumeShowOverlay", typeof(bool)))
                {
                    return;
                }

                //Get current volume
                int currentVolume = AudioVolumeGet(false);
                bool currentMute = AudioMuteGetStatus(false);
                //Debug.WriteLine("Current volume: " + currentVolume + " / Previous volume: " + AppVariables.VolumeLevelPrevious);
                //Debug.WriteLine("Current mute: " + currentMute + " / Previous mute: " + AppVariables.VolumeMutePrevious);

                //Compare volume variable
                if (AppVariables.VolumeLevelPrevious != -1 && (currentVolume != AppVariables.VolumeLevelPrevious || currentMute != AppVariables.VolumeMutePrevious))
                {
                    ActionDispatcherInvoke(delegate
                    {
                        try
                        {
                            Debug.WriteLine("Volume has changed, showing overlay.");

                            //Show media overlay
                            AppVariables.WindowOverlay.ShowWindowDuration(3000);
                        }
                        catch { }
                    });
                }

                //Update overlay volume control
                ActionDispatcherInvoke(delegate
                {
                    try
                    {
                        AppVariables.WindowOverlay.textblock_ControlVolume.Text = currentVolume.ToString();
                        AppVariables.WindowOverlay.slider_ControlVolume.ValueSkipEvent(currentVolume, true);

                        if (currentVolume == 0 || currentMute)
                        {
                            AppVariables.WindowOverlay.image_ControlMute.Source = new BitmapImage(new Uri("pack://application:,,,/ZuseMe;component/Assets/VolumeMuteLight.png"));
                        }
                        else
                        {
                            AppVariables.WindowOverlay.image_ControlMute.Source = new BitmapImage(new Uri("pack://application:,,,/ZuseMe;component/Assets/VolumeUnmuteLight.png"));
                        }
                    }
                    catch { }
                });

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