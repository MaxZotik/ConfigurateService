using System.Windows.Controls;

namespace ConfigurateService.Class.Managment
{
    internal class Manager
    {
        internal static Frame Frame { get; set; } 

        static Manager()
        {
            Frame = new Frame();
        }
    }
}
