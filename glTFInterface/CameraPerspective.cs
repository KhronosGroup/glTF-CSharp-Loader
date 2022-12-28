using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace glTFInterface
{
    public class CameraPerspective
    {
        // Type:  number
        // The floating-point aspect ratio of the field of view.
        // Required: No
        public double aspectRatio { get; set; } = 1.0;

        // Type: number
        // The floating-point vertical field of view in radians.
        // This value SHOULD be less than π.
        // Required: Yes
        public double yfov { get; set; } = 1.0;

        // Type: number
        //  The floating-point horizontal magnification of the view.T
        //  his value MUST NOT be equal to zero.
        //  This value SHOULD NOT be negative.
        // Required: Yes
        public double xmag { get; set; } = 1.0;

        // Type: number
        // The floating-point distance to the far clipping plane.
        // Required: No
        public double zfar { get; set; } = double.MaxValue;

        // Type: number
        // The floating-point distance to the near clipping plane.
        // Required: Yes
        public double znear { get; set; } = double.MinValue;

        // Type: extension
        // JSON object with extension-specific objects.
        // Required: No
        public System.Collections.Generic.Dictionary<string, object>? extensions { get; set; } = null;

        // Type: extras
        // Application-specific data.
        // Required: No
        public Extra? extras { get; set; } = null;
        /*
         * **********************************************************
         */
        private bool isLocked = false;
        public void Lock()
        {
            isLocked = true;
        }
        public void Unlock()
        {
            isLocked = false;
        }
    }
}
