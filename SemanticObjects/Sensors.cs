namespace Sensors
{
    public class Sensor
    {

    }
    public class Camera : Sensor
    {

    }
    public class Binocular : Sensor
    {
        public Binocular()
        {

        }
        public Binocular(Camera leftCamera, Camera rightCamera)
        {

        }
        public Camera LeftCamera { get; set; } = new Camera();
        public Camera RightCamera { get; set; } = new Camera();
    }

}
