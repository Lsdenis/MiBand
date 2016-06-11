using Android.App;
using Android.OS;
using IMiBand = Com.Zhaoxiaodan.Miband.MiBand;

namespace MiBand.Droid
{
    [Activity(Label = "ScanActivity", MainLauncher = true)]
    public class ScanActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            var a = new IMiBand(this);
        }
    }
}